using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TransactionalEmail.Core.Objects;
using TransactionalEmail.Extensions;

namespace TransactionalEmail.Tests.UnitTests.Extensions
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class EmailAddressStringExtensions_TestFixture
    {
        [Test]
        public void If_String_Is_Null_Or_Empty_Returns_Empty_List()
        {
            //Arrange
            var testString = string.Empty;

            //Act
            var result = testString.ToEmailAddressList(EmailAddressType.From);

            //Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void If_String_Is_Incorrect_Format_Or_Empty_Returns_Empty_List()
        {
            //Arrange
            const string testString = "This is a random string that does not match the format";

            //Act
            var result = testString.ToEmailAddressList(EmailAddressType.From);

            //Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void If_String_Is_Correct_Format_Returns_Email_Address_List()
        {
            //Arrange
            const string testString = "Kristian Wilson <kristian.wilson@gmail.com>";

            //Act
            var result = testString.ToEmailAddressList(EmailAddressType.From);

            //Assert
            result.Should().NotBeEmpty();
            result.Count().Should().Be(1);
            result.First().Name.Should().Be("Kristian Wilson");
            result.First().Email.Should().Be("kristian.wilson@gmail.com");
        }

        [Test]
        public void If_String_Includes_No_Name_Returns_Email_With_Email_Address_Only()
        {
            //Arrange
            const string testString = "<kristian.wilson@gmail.com>";

            //Act
            var result = testString.ToEmailAddressList(EmailAddressType.From);

            //Assert
            result.Should().NotBeEmpty();
            result.Count().Should().Be(1);
            result.First().Name.Should().Be(string.Empty);
            result.First().Email.Should().Be("kristian.wilson@gmail.com");
        }

        [Test]
        public void If_String_Includes_Errant_Seperator_Returns_Single_Email()
        {
            //Arrange
            const string testString = "Kristian Wilson <kristian.wilson@gmail.com>|";

            //Act
            var result = testString.ToEmailAddressList(EmailAddressType.From);

            //Assert
            result.Should().NotBeEmpty();
            result.Count().Should().Be(1);
            result.First().Name.Should().Be("Kristian Wilson");
            result.First().Email.Should().Be("kristian.wilson@gmail.com");
        }
    }
}