namespace ActioBP.General
{
    public class EmailConfig
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        
        public string LocalDomain { get; set; }

        public string MailServerAddress { get; set; }
        public int MailServerPort { get; set; }

        public string UserId { get; set; }
        public string UserPassword { get; set; }

        public bool EnableSsl { get; set; }
    }
}
