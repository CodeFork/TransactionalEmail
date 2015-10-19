using System.Collections.Generic;
using System.Linq;
using Conditions;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Extensions
{
    public static class EmailAddressStringExtensions
    {
        //Email Address String Example: "Kristian Wilson <kristian.wilson@amigo.me>; Julian Inwood <julian.inwood@gmail.com>"
        public static IEnumerable<EmailAddress> ToEmailAddressList(this string addressString, EmailAddressType type)
        {
            if (addressString.IsNotNullOrEmpty() && addressString.Contains('<') && addressString.Contains('>'))
            {
                var addresses = addressString.Split('|');

                return
                    addresses.Where(address => address.IsNotNullOrEmpty())
                        .Select(address => address.Split('<'))
                        .Select(parts => new EmailAddress
                        {
                            Name = parts[0].Trim(),
                            Email = parts[1].TrimEnd('>').Trim(), //get rid of the trailing angle bracket
                            Type = type,
                        }).ToList();
            }

            return new List<EmailAddress>();
        } 
    }
}