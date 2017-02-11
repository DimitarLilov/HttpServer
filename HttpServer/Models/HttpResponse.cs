namespace HttpServer.Models
{
    using System;
    using System.Text;
    using Enums;

    public class HttpResponse
    {
        public HttpResponse()
        {
            this.Header = new Header(HeaderType.HttpResponse);
            this.Content = new byte[] { };
        }

        public ResponseStatusCode StatusCode { get; set; }

        public string StatusMessage
        {
            get
            {
                return Enum.GetName(typeof(ResponseStatusCode), this.StatusCode);               
            }
        }

        public Header Header { get; set; }

        public byte[] Content { get; set; }

        public string ContentAsUTF8
        {
            set { this.Content = Encoding.UTF8.GetBytes(value); }
        }

        public override string ToString()
        {
            return $"HTTP/1.0 {(int) this.StatusCode} {this.StatusMessage}\r\n{this.Header}";
        }
    }
}
