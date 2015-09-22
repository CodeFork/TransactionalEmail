using System;
using NUnit.Framework;
using SimpleInjector;
using TransactionalEmail.Infrastructure.DependencyInjection;

namespace TransactionalEmail.Tests.UnitTests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class RegistryConfig_TestFixture
    {
        [Test]
        public void Container_Is_CorrectlyConfigured()
        {
            //arrange

            //act
            try
            {
                var container = new Container();
                new Registry().RegisterServices(container);
            }
            catch (Exception ex)
            { 
                Assert.Fail("There is a problem with the Simple Injector configuration");
            }
        }
    }
}
