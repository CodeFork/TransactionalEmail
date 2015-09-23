using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TransactionalEmail.Core.Objects;
using TransactionalEmail.Extensions;

namespace TransactionalEmail.Tests.UnitTests.Extensions
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class EmailAddressListExtensions_TestFixture
    {
        [Test]
        public void Can_Convert_Email_Address_To_String()
        {
            //Arrange
            var email = new EmailAddress {Name = "Kristian Wilson", Email = "kristian.wilson@gmail.com"};
            
            //Act
            var result = email.ToAddressString();

            //Assert
            result.Should().Be("Kristian Wilson <kristian.wilson@gmail.com>");
        }

        [Test]
        public void Can_Convert_Email_Address_With_No_Name_To_String()
        {
            //Arrange
            var email = new EmailAddress { Email = "kristian.wilson@gmail.com" };

            //Act
            var result = email.ToAddressString();

            //Assert
            result.Should().Be("<kristian.wilson@gmail.com>");
        }

        [Test]
        public void Can_Convert_Email_Address_List_To_String()
        {
            //Arrange
            var emails = new List<EmailAddress>
            {
                new EmailAddress {Name = "Kristian Wilson", Email = "kristian.wilson@gmail.com"},
                new EmailAddress {Name = "John Smith", Email = "john.smith@gmail.com"},
            };

            //Act
            var result = emails.ToAddressString();

            //Assert
            result.Should().Be("Kristian Wilson <kristian.wilson@gmail.com>; John Smith <john.smith@gmail.com>");
        }
    }
}