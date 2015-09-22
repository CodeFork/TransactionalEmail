using System;

namespace TransactionalEmail.Core.Objects
{
    public class EmailAddress
    {
        public int EmailAddressId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public EmailAddressType Type { get; set; }
        public DateTime DateCreated { get; set; }

        public EmailAddress()
        {
            Name = string.Empty;
            Email = string.Empty;
            Type = EmailAddressType.Unknown;
            DateCreated = DateTime.UtcNow;
        }
    }
}
