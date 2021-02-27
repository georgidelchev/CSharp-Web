using System;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace HTTPProtocolDemo
{
    public class StartUp
    {
        public static async Task Main(string[] args)
        {
            const string NEW_LINE = "\r\n";

            //await HttpRequest();

            var tcpListener = new TcpListener(IPAddress.Loopback, 1234);

            tcpListener.Start();

            while (true)
            {
                var tcpClient = tcpListener.AcceptTcpClient();

                var networkStream = tcpClient.GetStream();
                using (networkStream)
                {

                    // TODO: Use buffer
                    var requestBytes = new byte[1000000];

                    var bytesRead = networkStream.Read(requestBytes, 0, requestBytes.Length);

                    var request = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);

                    //var responseText = "<h1>Hello Header</h1>";

                    string responseText = @"<form action='/Account/Login' method='post'> 
                                        <input type=text name='username' /> 
                                        <input type=password name='password' /> 
                                        <input type=date name='date' /> 
                                        <input type=submit value='login' /> 
                                        </form>";

                    var response = "HTTP/1.0 200 OK" +
                                   NEW_LINE +
                                   "Server: SoftUniServer/1.0" +
                                   NEW_LINE +
                                   "Content-Type: text/html" +
                                   NEW_LINE+
                                   $"Content-Length: {responseText.Length}" +
                                   NEW_LINE +
                                   NEW_LINE +
                                   responseText;

                    var responseBytes = Encoding.UTF8.GetBytes(response);

                    networkStream.Write(responseBytes, 0, responseBytes.Length);

                    Console.WriteLine(request);
                    Console.WriteLine(new string('=', 60));
                }
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
