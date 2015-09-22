using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TransactionalEmail.Infrastructure;

namespace TransactionalEmail.Tests.UnitTests
{
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    public class CryptographicReferenceGenerator_TestFixture
    {
        [Test]
        public void CreateReference_Returns_CorrectLengthString()
        {
            //arrange
            const int size = 10;
            var referenceGenerator = new CryptographicReferenceGenerator();

            //act
            var reference = referenceGenerator.CreateReference(size);

            //assert
            reference.Should().NotBeNullOrWhiteSpace();
            reference.Length.Should().Be(size);
        }

        [Test]
        public void CreateReference_CalledTwice_Returns_DifferentReference()
        {
            //arrange
            const int size = 10;
            var referenceGenerator = new CryptographicReferenceGenerator();

            //act
            var reference1 = referenceGenerator.CreateReference(size);
            var reference2 = referenceGenerator.CreateReference(size);

            //assert
            reference1.Should().NotMatch(reference2);
        }

        [Test]
        public void CreateReference_CalledMillionTimes_NoCollisions()
        {
            //arrange
            const int size = 10;
            var referenceGenerator = new CryptographicReferenceGenerator();
            var ids = new List<string>();

            //act
            for (var i = 0; i < 1000000; i++)
            {
                ids.Add(referenceGenerator.CreateReference(size));
            }

            //assert
            var uniqueIds = ids.Distinct();
            uniqueIds.Count().Should().Be(ids.Count);

        }
    }
}
