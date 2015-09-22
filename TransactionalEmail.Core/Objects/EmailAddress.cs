namespace TransactionalEmail.Core.Objects
{
    public class EmailAddress
    {
        public int EmailAddressId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public EmailAddressType Type { get; set; }
    }
}
