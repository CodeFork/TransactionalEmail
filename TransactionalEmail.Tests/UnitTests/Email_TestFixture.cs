using FluentAssertions;
using Moq;
using NUnit.Framework;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Tests.UnitTests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class Email_TestFixture
    {
        [Test]
        public void CreateReference_Sets_NoteReference()
        {
            //arrange
            const string reference = "ABCDE12345";
            var mockReferenceGenerator = new Mock<IReferenceGenerator>();
            var email = new Email();

            mockReferenceGenerator.Setup(x => x.CreateReference(It.IsAny<int>())).Returns(reference);

            //act
            email.CreateReference(mockReferenceGenerator.Object);

            //assert
            email.EmailReference.Should().NotBeNullOrWhiteSpace();
            email.EmailReference.Should().Be(reference);

            mockReferenceGenerator.Verify(x => x.CreateReference(It.IsAny<int>()), Times.Once);
        }
    }
}
