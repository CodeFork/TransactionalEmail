using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Core.Interfaces
{
    public interface IForwardService
    {
        ForwardResult ProcessEmail(Email email);
    }
}
