﻿namespace TransactionalEmail.Core.Objects
{
    public class Attachment
    {
        public string AttachmentId { get; set; }
        public string AttachmentName { get; set; }
        public string MimeType { get; set; }
        public byte[] ByteArray { get; set; }

        public Attachment()
        {
            AttachmentName = string.Empty;
            MimeType = string.Empty;
            ByteArray = new byte[] {};
        }
    }
}