using System;
using System.Text;
using SUS.HTTP.Enums;
using System.Collections.Generic;

namespace SUS.HTTP
{
    public class HttpResponse
    {
        public HttpResponse(string contentType, byte[] body, HttpStatusCode statusCode = HttpStatusCode.Ok)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            this.StatusCode = statusCode;
            this.Body = body;

            this.Headers.Add(new Header("Content-Type", contentType));
            this.Headers.Add(new Header("Content-Length", body.Length.ToString()));
        } 

        public HttpStatusCode StatusCode { get; set; }

        public byte[] Body { get; set; }

        public ICollection<Header> Headers { get; set; }
            = new List<Header>();

        public ICollection<Cookie> Cookies { get; set; }
            = new List<Cookie>();

        public override string ToString()
        {
            var responseBuilder = new StringBuilder();

            responseBuilder.Append($"HTTP/1.1 {(int)this.StatusCode} {this.StatusCode}" + HttpConstants.NEW_LINE);

            foreach (var header in this.Headers)
            {
                responseBuilder.Append($"{header.ToString()}" + HttpConstants.NEW_LINE);
            }

            foreach (var cookie in this.Cookies)
            {
                responseBuilder.Append($"Set-Cookie: {cookie.ToString()}{HttpConstants.NEW_LINE}");
            }

            responseBuilder.Append(HttpConstants.NEW_LINE);

            return responseBuilder.ToString();
        }
    }
}