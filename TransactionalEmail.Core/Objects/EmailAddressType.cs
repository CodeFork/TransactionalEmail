namespace TransactionalEmail.Core.Objects
{
    public enum EmailAddressType
    {
        Unknown =0,
        To = 1,
        From = 2,
        CarbonCopy = 3,
        BlindCarbonCopy = 4
    }
}