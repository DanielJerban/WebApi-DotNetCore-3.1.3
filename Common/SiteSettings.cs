namespace Common
{
    public class SiteSettings
    {
        public ElmahSettings ElmahSettings { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public SwaggerSettings SwaggerSettings { get; set; }
    }

    public class ElmahSettings
    {
        public string Path { get; set; }
        public string ConnectionStringName { get; set; }
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int NotBeforeMinutes { get; set; }
        public int ExpirationMinutes { get; set; }
    }

    public class SwaggerSettings
    {

    }
}