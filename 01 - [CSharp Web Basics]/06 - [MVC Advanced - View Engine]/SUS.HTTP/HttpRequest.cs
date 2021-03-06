using System;
using System.Text;
using System.Linq;
using SUS.HTTP.Enums;
using System.Collections.Generic;

namespace SUS.HTTP
{
    public class HttpRequest
    {
        public HttpRequest(string requestString)
        {
            var lines = requestString.Split(new string[] { HttpConstants.NEW_LINE }, StringSplitOptions.None);

            var headerLine = lines[0];

            var headerLineParts = headerLine.Split();

            this.Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), headerLineParts[0], true);
            this.Path = headerLineParts[1];

            int lineIndex = 1;
            bool isInHeaders = true;

            var bodyBuilder = new StringBuilder();

            while (lineIndex < lines.Length)
            {
                var line = lines[lineIndex];

                lineIndex++;

                if (string.IsNullOrWhiteSpace(line))
                {
                    isInHeaders = false;

                    continue;
                }

                if (isInHeaders)
                {
                    this.Headers.Add(new Header(line));
                }
                else
                {
                    bodyBuilder.AppendLine(line);
                }

                if (this.Headers.Any(h => h.Name == HttpConstants.REQUEST_COOKIE_HEADER))
                {
                    var cookiesAsString = this.Headers.FirstOrDefault(h => h.Name == HttpConstants.REQUEST_COOKIE_HEADER).Value;

                    var cookies = cookiesAsString.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var cookie in cookies)
                    {
                        this.Cookies.Add(new Cookie(cookie));
                    }
                }

                this.Body = bodyBuilder.ToString();
            }
        }

        public string Path { get; set; }

        public HttpMethod Method { get; set; }

        public ICollection<Header> Headers { get; set; }
            = new List<Header>();

        public ICollection<Cookie> Cookies { get; set; }
            = new List<Cookie>();

        public string Body { get; set; }
    }
}