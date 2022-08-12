using Assets.Scripts.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts.WebApi
{
    public class JsonHttpRequestHandler<TContent> : HttpRequestHandler<TContent>
    {
        public class ResponseParser : IHttpResponseParser<TContent>
        {
            async Task<TContent> IHttpResponseParser<TContent>.ParseAsync(HttpResponseMessage response, CancellationToken cancellationToken) {
                string content = await response.Content.ReadAsStringAsync();
                cancellationToken.ThrowIfCancellationRequested();

                TContent parsedContent = JsonConvert.DeserializeObject<TContent>(content);
                return parsedContent;
            }
        }

        public class ResponseErrorParser : IHttpResponseErrorParser
        {
            async Task<ForbidError> IHttpResponseErrorParser.ParseAsync(HttpResponseMessage response, CancellationToken cancellationToken) {
                string content = await response.Content.ReadAsStringAsync();
                cancellationToken.ThrowIfCancellationRequested();

                ForbidError parsedContent = JsonConvert.DeserializeObject<ForbidError>(content);
                return parsedContent;
            }
        }

        public JsonHttpRequestHandler(HttpClient httpClientProxy) : base(httpClientProxy, new ResponseParser(), new ResponseErrorParser()) { }
    }
}
