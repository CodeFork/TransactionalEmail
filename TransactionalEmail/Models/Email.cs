using System.Collections.Generic;

namespace TransactionalEmail.Models
{
    public class Email
    {
        public string EmailReference { get; set; }
        public string ToAddresses { get; set; }
        public string FromAddress { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string PlainTextBody { get; set; }
        public string HtmlBody { get; set; }
        public List<Attachment> Attachments { get; set; }  
    }
}