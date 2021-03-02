using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPProtocolDemo
{
    public class StartUp
    {
        const string NEW_LINE = "\r\n";

        private static Dictionary<string, int> SessionStorage = new Dictionary<string, int>();

        public static async Task Main(string[] args)
        {

            //await HttpRequest();

            var tcpListener = new TcpListener(IPAddress.Loopback, 80);

            tcpListener.Start();

            while (true)
            {
                var tcpClient = tcpListener.AcceptTcpClient();

                ProcessClientAsync(tcpClient);
            }
        }

        private static async Task ProcessClientAsync(TcpClient tcpClient)
        {
            var networkStream = tcpClient.GetStream();

            using (networkStream)
            {

                // TODO: Use buffer
                var requestBytes = new byte[1000000];

                var bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);

                var request = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);

                bool sessionSet = request.Contains("sid=");

                var match=Regex.Match(request,"sid=[^\n]*\r\n");

                var sid = new Guid().ToString();

                if (match.Success)
                {
                    sid = match.Value.Substring(4);
                }

                if (!SessionStorage.ContainsKey(sid))
                {
                    SessionStorage.Add(sid,0);
                }

                SessionStorage[sid]++;

                //var responseText = "<h1>Hello Header</h1>";

                string responseText = @"<form action='/Account/Login' method='post'> 
                                        <input type=text name='username' /> 
                                        <input type=password name='password' /> 
                                        <input type=date name='date' /> 
                                        <input type=submit value='login' /> 
                                        </form>" + DateTime.Now + $" {SessionStorage[sid]} time";

                var response = "HTTP/1.0 200 OK" +
                               NEW_LINE +
                               "Server: SoftUniServer/1.0" +
                               NEW_LINE +
                               "Content-Type: text/html" +
                               NEW_LINE +
                               $"Content-Length: {responseText.Length}" +
                               NEW_LINE +
                               (!sessionSet ?
                               ($"Set-Cookie: sid=ng54eb785renbv57re") : string.Empty) +
                               NEW_LINE +
                               NEW_LINE +
                               responseText;

                var responseBytes = Encoding.UTF8.GetBytes(response);

                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);

                Console.WriteLine(request);
                Console.WriteLine(new string('=', 60));
            }
        }

        private static async Task HttpRequest()
        {
            var client = new HttpClient();

            var response = await client.GetAsync("https://softuni.bg/");

            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine(result);
        }
    }
}
