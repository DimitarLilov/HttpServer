namespace HttpServer
{
    using System.IO;
    using Enums;
    using Models;

    public static class HttpResponseBuilder
    {
        public static HttpResponse InternalServerError()
        {
            var content = File.ReadAllText("Resources/Pages/500.html");

            return new HttpResponse()
            {
                StatusCode = ResponseStatusCode.InternalServerError,
                ContentAsUTF8 = content
            };
        }

        public static HttpResponse NotFound()
        {
            var content = File.ReadAllText("Resources/Pages/404.html");
            return new HttpResponse()
            {
                StatusCode = ResponseStatusCode.NotFound,
                ContentAsUTF8 = content
            };
        }
    }
}
