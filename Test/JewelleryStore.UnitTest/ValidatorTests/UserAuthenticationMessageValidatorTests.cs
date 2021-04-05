using FluentValidation.Validators;
using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using JewelleryStore.Application.User.Validator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JewelleryStore.UnitTest.ValidatorTests
{
    [TestClass]
    public class UserAuthenticationMessageValidatorTests : BaseValidationTest
    {
        private static UserAuthenticationMessageValidator UserAuthenticationMessageValidator => new UserAuthenticationMessageValidator();

        [TestMethod]
        public void UserIdRule()
        {
            UserAuthenticationMessageValidator.ShouldHaveRules(
                 x => x.Id,
                 BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator>()
                .Create());
        }

        [TestMethod]
        public void PasswordRule()
        {
            UserAuthenticationMessageValidator.ShouldHaveRules(
                 x => x.Password,
                 BaseVerifiersSetComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator>()
                .Create());
        }
    }
}
