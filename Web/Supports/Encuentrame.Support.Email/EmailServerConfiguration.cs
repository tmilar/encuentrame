namespace Encuentrame.Support.Email
{
    public class EmailServerConfiguration
    {
        public string From { get; set; }        
        public int Port { get; set; }        
        public string User { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
    }
}