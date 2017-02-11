namespace HttpServer.Interfaces
{
    using System.Collections.Generic;
    using Models;

    public interface ICookieCollection : IEnumerable<Cookie>
    {
        int Count { get; }

        Cookie this[string key] { get; set; }

        void Add(Cookie cookie);

        bool Contains(string key);
    }
}
