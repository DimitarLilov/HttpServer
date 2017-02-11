namespace HttpServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;
    using Enums;
    using Models;

    public class HttpProcessor
    {
        private IList<Route> Routes;
        private HttpRequest Request;
        private HttpResponse Response;

        public HttpProcessor(IEnumerable<Route> routes)
        {
            this.Routes = new List<Route>(routes);
        }

        public void HendleClient(TcpClient tcpClient)
        {
            using (var stream = tcpClient.GetStream())
            {
                this.Request = this.GetRequest(stream);
                this.Response = this.RouteRequest();
                StreamUtils.WriteResponse(stream, this.Response);
            }
        }

        private HttpRequest GetRequest(Stream inputStream)
        {
            var requestLine = StreamUtils.ReadLine(inputStream);
            var tokens = requestLine.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }

            RequestMethod method = (RequestMethod)Enum.Parse(typeof(RequestMethod), tokens[0].ToUpper());

            string url = tokens[1];
            string protocolVersion = tokens[2];

            Header header = new Header(HeaderType.HttpRequest);
            string line;
            while ((line = StreamUtils.ReadLine(inputStream)) != null)
            {
                if (line.Equals(""))
                {
                    break;
                }

                var separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }

                var name = line.Substring(0, separator);
                var pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++;
                }

                var value = line.Substring(pos, line.Length - pos);
                if (name == "Cookie")
                {
                    var cookieSaves = value.Split(';');
                    foreach (var cookieSave in cookieSaves)
                    {
                        var cookiePair = cookieSave.Split('=').Select(x => x.Trim()).ToArray();
                        var cookie = new Cookie(cookiePair[0], cookiePair[1]);
                        header.AddCookie(cookie);
                    }
                }
                else if (name == "Content-Length")
                {
                    header.ContentLength = value;
                }
                else
                {
                    header.OtherParameters.Add(name, value);
                }
            }

            string content = null;
            if (header.ContentLength != null)
            {
                var totalBytes = Convert.ToInt32(header.ContentLength);
                var bytesLeft = totalBytes;
                var bytes = new byte[totalBytes];

                while (bytesLeft > 0)
                {
                    var buffer = new byte[bytesLeft > 1024 ? 1024 : bytesLeft];
                    int n = inputStream.Read(buffer, 0, buffer.Length);
                    buffer.CopyTo(bytes, totalBytes - bytesLeft);

                    bytesLeft -= n;
                }

                content = Encoding.ASCII.GetString(bytes);
            }

            var request = new HttpRequest()
            {
                Method = method,
                Url = url,
                Header = header,
                Content = content
            };

            Console.WriteLine("-REQUEST-----------------------------");
            Console.WriteLine(request);
            Console.WriteLine("------------------------------");

            return request;
        }

        private HttpResponse RouteRequest()
        {
            var routes = this.Routes.Where(x => Regex.Match(this.Request.Url, x.UrlRegex).Success).ToList();

            if (!routes.Any())
            {
                return HttpResponseBuilder.NotFound();
            }

            var route = routes.SingleOrDefault(x => x.Method == this.Request.Method);

            if (route == null)
            {
                return new HttpResponse()
                {
                    StatusCode = ResponseStatusCode.MethodNotAllowed
                };
            }

            try
            {
                return route.Callable(this.Request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return HttpResponseBuilder.InternalServerError();
            }
        }
    }
}
