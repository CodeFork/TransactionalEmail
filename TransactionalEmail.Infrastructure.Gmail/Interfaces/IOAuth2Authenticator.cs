namespace TransactionalEmail.Infrastructure.Gmail.Interfaces
{
    public interface IOAuth2Authenticator
    {
        string GetOAuth2AccessToken(string emailAddress);
    }
}
