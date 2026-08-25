using System;

namespace Cores.Major.Helper
{
    public static class StringHelper
    {
        public static string GenFKey()
        {
            long i = 1;
            foreach (var b in Guid.NewGuid().ToByteArray()) i *= b + 1;
            return $"{i - DateTime.Now.Ticks:x}".ToUpper();
        }
    }
}