namespace TransactionalEmail.Core.Interfaces
{
    public interface IEmailAddressValidator
    {
        bool IsValidEmail(string emailAddress);
    }
}
