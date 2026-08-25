using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace TSFramework.Core.Members.Log
{
    public class LogWriter
    {
        private static LogWriter _instance;
        private static Queue<LogModel> _logQueue;
        private static readonly string logPath = ConfigurationManager.AppSettings["LogPath"] ?? "Logs";
        private static readonly string logFile = ConfigurationManager.AppSettings["LogFile"] ?? "LogsFile.log";
        private static readonly int flushAtAge = int.Parse(ConfigurationManager.AppSettings["FlushAtAge"] ?? "500");
        private static readonly int flushAtQty = int.Parse(ConfigurationManager.AppSettings["FlushAtQty"] ?? "1");
        private static DateTime _flushedAt;
        private static readonly object fileLock = new object();

        private LogWriter()
        {
            _logQueue = new Queue<LogModel>();
        }

        public static LogWriter Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new LogWriter();
                lock (_logQueue)
                {
                    _logQueue = new Queue<LogModel>();
                }

                _flushedAt = DateTime.Now;
                return _instance;
            }
        }

        public void WriteToLog(string message)
        {
            var log = new LogModel(message);
            _logQueue.Enqueue(log);
            if (_logQueue.Count >= flushAtQty || CheckTimeToFlush()) FlushLogToFileAsync().ConfigureAwait(false);
        }

        public void WriteToLog(Exception e)
        {
            var msg = new LogModel(e.Source?.Trim() + " " + e.Message?.Trim());
            var stack = new LogModel("Stack: " + e.StackTrace?.Trim());
            _logQueue.Enqueue(msg);
            _logQueue.Enqueue(stack);
            if (_logQueue.Count >= flushAtQty || CheckTimeToFlush()) FlushLogToFileAsync().ConfigureAwait(false);
        }

        public static void ForceFlush()
        {
            FlushLogToFileAsync().ConfigureAwait(false);
        }

        private static bool CheckTimeToFlush()
        {
            var time = DateTime.Now - _flushedAt;
            if (!(time.TotalSeconds >= flushAtAge)) return false;
            _flushedAt = DateTime.Now;
            return true;
        }

        private static async Task FlushLogToFileAsync()
        {
            await Task.Run(() =>
            {
                lock (fileLock)
                {
                    while (_logQueue.Count > 0)
                    {
                        var dir = HttpRuntime.AppDomainAppPath;
                        var entry = _logQueue.Dequeue();

                        var datedFolder = Path.Combine(dir, logPath);
                        if (!Directory.Exists(datedFolder))
                            Directory.CreateDirectory(datedFolder);

                        var path = Path.Combine(datedFolder, $"{entry.GetDate()}_{logFile}");

                        try
                        {
                            using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
                            using (var writer = new StreamWriter(stream))
                            {
                                writer.WriteLine($"{entry.GetTime()} {entry.GetMessage()}");
                            }
                        }
                        catch (IOException)
                        {
                            // Optional: Log exception elsewhere if needed
                        }
                    }
                }
            });
        }
    }
}