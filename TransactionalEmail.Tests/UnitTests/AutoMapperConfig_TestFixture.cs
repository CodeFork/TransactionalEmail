using System;
using NUnit.Framework;

namespace TransactionalEmail.Tests.UnitTests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class AutoMapperConfig_TestFixture
    {
        [Test]
        public void Automapper_Is_CorrectlyConfigured()
        {
            //act
            try
            {
                AutoMapperConfig.Bootstrap();
            }
            catch (Exception)
            {
                Assert.Fail("There is a problem with the automapper configuration");
            }
        }
    }
}
