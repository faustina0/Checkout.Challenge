using Checkout.Challenge.Api.Attributes;
using FluentAssertions;
using NUnit.Framework;

namespace Checkout.Challenge.Api.Tests.Attributes
{
    [TestFixture]
    public class CardExpiryDateShould
    {
        [SetUp]
        public void SetUp()
        {
            _target = new CardExpiryDate();
        }

        [TestCase("feb/21", false)]
        [TestCase("01/21", false)]
        [TestCase("08/21", true)]
        public void ReturnInvalidFormat(string input, bool expected)
        {
            var result = _target.IsValid(input);
            result.Should()
                  .Be(expected);
        }

        private CardExpiryDate _target;
    }
}