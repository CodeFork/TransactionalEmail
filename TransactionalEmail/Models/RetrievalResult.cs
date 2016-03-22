namespace TransactionalEmail.Models
{
    public class RetrievalResult
    {
        public string EmailReference { get; set; }
        public bool RetrievedOk { get; set; }

        public RetrievalResult()
        {
            EmailReference = string.Empty;
            RetrievedOk = false;
        }
    }
}