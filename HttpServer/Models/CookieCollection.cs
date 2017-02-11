namespace HttpServer.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using Interfaces;

    public class CookieCollection : ICookieCollection
    {
        public CookieCollection()
        {
            this.Cookies = new Dictionary<string, Cookie>();
        }

        public IDictionary<string, Cookie> Cookies { get; private set; }

        public int Count
        {
            get { return this.Cookies.Count; }
        }

        public Cookie this[string key]
        {
            get
            {
                return this.Cookies[key];
            }

            set
            {
                this.Add(value);
            }
        }

        public void Add(Cookie cookie)
        {
            if (!this.Cookies.ContainsKey(cookie.Name))
            {
                this.Cookies.Add(cookie.Name, new Cookie());
            }

            this.Cookies[cookie.Name] = cookie;
        }

        public bool Contains(string key)
        {
            return this.Cookies.ContainsKey(key);
        }

        public override string ToString()
        {
            return string.Join("; ", this.Cookies.Values);
        }

        public IEnumerator<Cookie> GetEnumerator()
        {
            return this.Cookies.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
