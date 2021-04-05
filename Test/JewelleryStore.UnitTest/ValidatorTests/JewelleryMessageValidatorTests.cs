using FluentValidation.Validators;
using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using JewelleryStore.Application.Price.Validator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JewelleryStore.UnitTest.ValidatorTests
{
    [TestClass]
    public class JewelleryMessageValidatorTests : BaseValidationTest
    {
        private static JewelleryMessageValidator JewelleryMessageValidator => new JewelleryMessageValidator();

        [TestMethod]
        public void PriceRule()
        {
            JewelleryMessageValidator.ShouldHaveRules(
                 x => x.Price,
                 BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<GreaterThanOrEqualValidator>(0)
                .Create());
        }

        [TestMethod]
        public void WeightRule()
        {
            JewelleryMessageValidator.ShouldHaveRules(
                 x => x.Weight,
                 BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<GreaterThanOrEqualValidator>(0)
                .Create());
        }
    }
}
