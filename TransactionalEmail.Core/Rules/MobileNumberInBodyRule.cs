using System;
using System.Linq;
using System.Text;
using Conditions;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Core.Rules
{
    public class MobileNumberInBodyRule : IForwardingRule
    {
        private const string Prefix = "Message sent in by ";
        private const string Postfix = " to ";

        public string RuleName => "Mobile Number In Body Rule";

        public RuleResult ApplyRule(Email email)
        {
            if (IsPhoneNumberInBody(email.PlainTextBody))
            {
                return new RuleResult
                {
                    RuleName = RuleName,
                    RuleApplied = true,
                    Email = ForwardEmail(email),
                };
            }

            return new RuleResult
            {
                RuleName = RuleName,
                RuleApplied = false,
                Email = email
            };
        }

        private static Email ForwardEmail(Email email)
        {
            var result = email;

            email.FromAddress.Email = email.FromAddress.Name = GetPhoneNumber(email.PlainTextBody, Prefix, Postfix);

            return result;
        }

        private static bool IsPhoneNumberInBody(string body)
        {
            return body.Contains(Prefix);
        }

        public static string GetPhoneNumber(string plainTextBody, string prefix, string postfix)
        {
            var startIndex = plainTextBody.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) + prefix.Length;
            var endIndex = plainTextBody.IndexOf(postfix, StringComparison.OrdinalIgnoreCase);

            var phoneNumber = plainTextBody.Substring(startIndex, (endIndex - startIndex)).Trim();

            return FormatPhoneNumber(phoneNumber);
        }

        private static string FormatPhoneNumber(string phoneNumber)
        {
            var formattedNumber = new StringBuilder();
            formattedNumber.Append("44");
            formattedNumber.Append(phoneNumber.TrimStart('0').Replace(" ", ""));

            return formattedNumber.ToString();
        }
    }
}
