using System.Collections.Generic;
using System.Linq;
using System.Text;
using Conditions;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Extensions
{
    public static class EmailAddressListExtensions
    {
        //Email Address String Example: "Kristian Wilson <kristian.wilson@amigo.me>; Julian Inwood <julian.inwood@gmail.com>"

        public static string ToAddressString(this List<EmailAddress> emailAddresses)
        {
            if (emailAddresses.Count == 0)
                return string.Empty;

            var builder = new StringBuilder();

            for(var i = 0; i < emailAddresses.Count(); i++)
            {
                if (i > 0)
                {
                    builder.Append("; ");
                }

                builder.AppendFormat(emailAddresses[i].ToAddressString());
            }

            return builder.ToString();
        }

        //Email Address Example: "Kristian Wilson <kristian.wilson@amigo.me>"

        public static string ToAddressString(this EmailAddress emailAddress)
        {
            if (emailAddress.IsNull())
                return string.Empty;

            var seperator = string.IsNullOrEmpty(emailAddress.Name) ? string.Empty : " "; //we only want a space if both parts are present
            
            return $"{emailAddress.Name}{seperator}<{emailAddress.Email}>";
        }
    }
}