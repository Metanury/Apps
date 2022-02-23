namespace Metanury.Apps.MVC
{
    public class MailData
    {
        public string ReceiverName { get; set; } = string.Empty;

        public string ReceiverMail { get; set; } = string.Empty;

        public string SenderName { get; set; } = string.Empty;

        public string SenderMail { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public string SmtpAddress
        {
            get
            {
                return this.Service.SmtpAddress;
            }
        }

        public int SmtpPort
        {
            get
            {
                return this.Service.SmtpPort;
            }
        }

        public string AccountID
        {
            get
            {
                return this.Service.AccountID;
            }
        }

        public string AccountPW
        {
            get
            {
                return this.Service.AccountPW;
            }
        }

        protected MailService Service { get; set; }

        public MailData(MailService service)
        {
            this.Service = service;
        }
    }

    public class MailService
    {
        public string SmtpAddress { get; set; } = string.Empty;

        public int SmtpPort { get; set; } = 587;

        public string AccountID { get; set; } = string.Empty;

        public string AccountPW { get; set; } = string.Empty;

        public MailService()
        {
        }
    }
}
