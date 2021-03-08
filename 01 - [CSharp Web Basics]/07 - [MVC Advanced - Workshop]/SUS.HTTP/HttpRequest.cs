using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace SUS.HTTP
{
    public class HttpRequest
    {
        public static IDictionary<string, Dictionary<string, string>>
            Sessions = new Dictionary<string, Dictionary<string, string>>();

        public HttpRequest(string requestString)
        {
            var lines = requestString.Split(new string[] { HttpConstants.NEW_LINE },
                StringSplitOptions.None);

            var headerLine = lines[0];
            var headerLineParts = headerLine.Split(' ');

            this.Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), headerLineParts[0], true);

            this.Path = headerLineParts[1];

            int lineIndex = 1;

            bool isInHeaders = true;

            StringBuilder bodyBuilder = new StringBuilder();
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
            }

            if (this.Headers.Any(x => x.Name == HttpConstants.REQUEST_COOKIE_HEADER))
            {
                var cookiesAsString = this.Headers.FirstOrDefault(x =>
                    x.Name == HttpConstants.REQUEST_COOKIE_HEADER).Value;

                var cookies = cookiesAsString.Split(new string[] { "; " },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var cookieAsString in cookies)
                {
                    this.Cookies.Add(new Cookie(cookieAsString));
                }
            }

            var sessionCookie = this.Cookies.FirstOrDefault(x => x.Name == HttpConstants.SESSION_COOKIE_NAME);

            if (sessionCookie == null)
            {
                var sessionId = Guid.NewGuid().ToString();

                this.Session = new Dictionary<string, string>();

                Sessions.Add(sessionId, this.Session);

                this.Cookies.Add(new Cookie(HttpConstants.SESSION_COOKIE_NAME, sessionId));
            }
            else if (!Sessions.ContainsKey(sessionCookie.Value))
            {
                this.Session = new Dictionary<string, string>();

                Sessions.Add(sessionCookie.Value, this.Session);
            }
            else
            {
                this.Session = Sessions[sessionCookie.Value];
            }

            if (this.Path.Contains("?"))
            {
                var pathParts = this.Path.Split(new char[] { '?' }, 2);

                this.Path = pathParts[0];
                this.QueryString = pathParts[1];
            }
            else
            {
                this.QueryString = string.Empty;
            }

            this.Body = bodyBuilder
                .ToString()
                .TrimEnd('\n', '\r');

            SplitParameters(this.Body, this.FormData);
            SplitParameters(this.QueryString, this.QueryData);
        }

        public string Path { get; set; }

        public string QueryString { get; set; }

        public HttpMethod Method { get; set; }

        public ICollection<Header> Headers { get; set; }
            = new List<Header>();

        public ICollection<Cookie> Cookies { get; set; }
            = new List<Cookie>();

        public IDictionary<string, string> FormData { get; set; }
            = new Dictionary<string, string>();

        public IDictionary<string, string> QueryData { get; set; }
            = new Dictionary<string, string>();

        public Dictionary<string, string> Session { get; set; }
            = new Dictionary<string, string>();

        public string Body { get; set; }

        private static void SplitParameters(string parametersAsString, IDictionary<string, string> output)
        {

            var parameters = parametersAsString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var parameter in parameters)
            {
                var parameterParts = parameter.Split(new[] { '=' }, 2);

                var name = parameterParts[0];
                var value = WebUtility.UrlDecode(parameterParts[1]);

                if (!output.ContainsKey(name))
                {
                    output.Add(name, value);
                }
            }
        }
    }
}