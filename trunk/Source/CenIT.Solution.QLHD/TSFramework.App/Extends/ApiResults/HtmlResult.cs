using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace TSFramework.App.Extends.ApiResults
{
    public class HtmlResult : IHttpActionResult
    {
        public HtmlResult(HttpRequestMessage request, string content)
        {
            Request = request;
            Content = content;
        }

        public string Content { get; }
        public HttpRequestMessage Request { get; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(ExecuteResult());
        }

        public HttpResponseMessage ExecuteResult()
        {
            var response = new HttpResponseMessage();

            if (!string.IsNullOrWhiteSpace(Content))
                response.Content = new StringContent(Content, Encoding.UTF8, "text/html");

            response.RequestMessage = Request;
            return response;
        }
    }
}