using System;
using System.Linq;
using NUnit.Framework;
using Checkout.Challenge.BankApi;

namespace Checkout.Challenge.BankApi.Tests
{
    [TestFixture]
    public class MappingsShould
    {
        
        [Test]
        public void BeValid()
        {
            var profiles = typeof(MappingProfile).Assembly.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x));
            var configuration = new MapperConfiguration(cfg =>
                                                        {

                                                            foreach(var profile in profiles)
                                                            {
                                                                cfg.AddProfile(
                                                                    Activator.CreateInstance(profile) as
                                                                        Profile);
                                                            }

                                                        });
            configuration.AssertConfigurationIsValid();
        }
    }

}
