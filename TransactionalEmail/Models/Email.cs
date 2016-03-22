using System;
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
        public DateTime? EmailDate { get; set; }

        public Email()
        {
            EmailReference = string.Empty;
            ToAddresses = string.Empty;
            FromAddress = string.Empty;
            Cc = string.Empty;
            Bcc = string.Empty;
            Subject = string.Empty;
            PlainTextBody = string.Empty;
            HtmlBody = string.Empty;
            Attachments = new List<Attachment>();
            EmailDate = null;
        }
    }
}