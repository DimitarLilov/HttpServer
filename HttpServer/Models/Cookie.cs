namespace HttpServer.Models
{
    public class Cookie
    {
        public Cookie(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public Cookie()
        {
            this.Name = null;
            this.Value = null;
        }

        public string Name { get; private set; }

        public string Value { get; private set; }

        public override string ToString()
        {
            return $"{this.Name}={this.Value}";
        }
    }
}
