namespace Hello
{
    using System.Collections.Generic;
    using System.IO;
    using HttpServer;
    using HttpServer.Enums;
    using HttpServer.Models;

    public class Hello
    {
        public static void Main()
        {
            var routes = new List<Route>()
            {
                new Route
                {
                    Name = "Hello",
                    UrlRegex = @"^/hello$",
                    Method = RequestMethod.GET,
                    Callable = (HttpRequest request) =>
                    {
                        return new HttpResponse()
                        {
                            StatusCode = ResponseStatusCode.Ok,
                            ContentAsUTF8 = File.ReadAllText("../../content/hello.html")
                        };
                    }
                }
            };

            var httpServer = new Server(8081, routes);
            httpServer.Listen();
        }
    }
}
