using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;
using TransactionalEmail.Infrastructure.Data;

namespace TransactionalEmail.Tests.UnitTests.Repository
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class EmailRepository_TestFixture
    {
        private readonly Mock<DbSet<Email>> _mockDbSet = new Mock<DbSet<Email>>();
        private readonly Mock<IEmailContext> _mockContext = new Mock<IEmailContext>();
        private readonly Mock<IDbSettings> _mockDbSettings = new Mock<IDbSettings>();
        private IEmailRepository _emailRepository;

        [SetUp]
        public void Setup()
        {
            _mockDbSettings.Setup(x => x.ConnectionString).Returns("TestConnectionString");
            _mockContext.Setup(x => x.Emails).Returns(_mockDbSet.Object);
            _mockContext.Setup(x => x.SaveChanges()).Returns(1);

            _emailRepository = new EmailRepository(_mockContext.Object);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void NoteRepository_NullNoteContext_ThrowsException()
        {
            //act
            var sut = new EmailRepository(null);
        }

        [Test]
        public void GetEmailByReference_Must_GetEmailWithMatchingReference()
        {
            //arrange
            const string reference = "ABCD1234";

            SetupDataForTest(new List<Email>
            {
                new Email {EmailReference = reference},
                new Email {EmailReference = "12345678"},
            }.AsQueryable());

            //act
            var result = _emailRepository.GetEmailByReference(reference);

            //assert
            result.EmailReference.Should().Be(reference);
        }

        [Test]
        public void CreateEmail_EmptyEmailReference_Returns_False()
        {
            //act
            var result = _emailRepository.CreateEmail(new Email { EmailReference = string.Empty });

            //assert
            result.Should().BeFalse();
        }

        [Test]
        public void CreateEmail_SavesEmailToContext_ReturnsTrue()
        {
            //arrange
            _mockContext.ResetCalls();

            //act
            var result = _emailRepository.CreateEmail(new Email { EmailReference= "ABCD1234" });

            //assert
            result.Should().BeTrue();
            _mockDbSet.Verify(x => x.Add(It.IsAny<Email>()), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void UpdateStatus_EmailDoesNotExist_ReturnsFalse()
        {
            //arrange
            const string reference = "ABCD1234";

            SetupDataForTest(new List<Email>
            {
                new Email {EmailReference = "12345678"},
            }.AsQueryable());

            //act
            var result = _emailRepository.UpdateStatus(reference, Status.Success);

            //assert
            result.Should().BeFalse();
        }

        [Test]
        public void UpdateStatus_EmailDoesExist_ReturnsTrue()
        {
            //arrange
            const string reference = "ABCD1234";

            _mockContext.ResetCalls();

            SetupDataForTest(new List<Email>
            {
                new Email {EmailReference = reference},
            }.AsQueryable());

            //act
            var result = _emailRepository.UpdateStatus(reference, Status.Success);

            //assert
            result.Should().BeTrue();
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void UpdateAppliedRules_EmailDoesNotExist_ReturnsFalse()
        {
            //arrange
            const string reference = "ABCD1234";

            SetupDataForTest(new List<Email>
            {
                new Email {EmailReference = "12345678"},
            }.AsQueryable());

            //act
            var result = _emailRepository.UpdateAppliedRules(reference, "Test Rule");

            //assert
            result.Should().BeFalse();
        }

        [Test]
        public void UpdateAppliedRules_EmailDoesExist_ReturnsTrue()
        {
            //arrange
            const string reference = "ABCD1234";

            _mockContext.ResetCalls();

            SetupDataForTest(new List<Email>
            {
                new Email {EmailReference = reference},
            }.AsQueryable());

            //act
            var result = _emailRepository.UpdateAppliedRules(reference, "Test Rule");

            //assert
            result.Should().BeTrue();
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        private void SetupDataForTest(IQueryable<Email> data)
        {
            _mockDbSet.As<IQueryable<Email>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockDbSet.As<IQueryable<Email>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockDbSet.As<IQueryable<Email>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockDbSet.As<IQueryable<Email>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }
    }
}