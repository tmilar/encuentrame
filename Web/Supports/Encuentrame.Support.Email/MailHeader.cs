namespace Encuentrame.Support.Email
{
    public class MailHeader
    {
        public MailHeader()
        {
            IsHtml = true;
        }

        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public bool IsHtml { get; set; }
    }
}