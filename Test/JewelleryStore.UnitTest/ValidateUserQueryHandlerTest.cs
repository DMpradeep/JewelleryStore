using FluentValidation;
using FluentValidation.Results;
using JewelleryStore.Application.Common;
using JewelleryStore.Application.Exceptions;
using JewelleryStore.Application.User;
using JewelleryStore.Model.Common;
using JewelleryStore.Model.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JewelleryStore.UnitTest
{
    [TestClass]
    public class ValidateUserQueryHandlerTest : BaseValidationTest
    {
        [TestMethod]
        [ExpectedException(typeof(ValidationException), "Validation exception is not thrown even if validation failed.")]
        public async Task ValidationFails_ShouldThrowValidationException()
        {
            //Arrange
            var tokenGeneratorMock = CreateStrictMock<ITokenGenerator>();
            var userDataAcccessMock = CreateStrictMock<IUserDataAccess>();
            var validatorMock = CreateStrictMock<AbstractValidator<UserAuthenticationMessage>>();
            validatorMock.Setup(o => o.ValidateAsync(It.IsAny<ValidationContext<UserAuthenticationMessage>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>() { new ValidationFailure(nameof(UserAuthenticationMessage.Id), "userId is required") }));

            var testObject = new ValidateUserQueryHandler(validatorMock.Object, userDataAcccessMock.Object, tokenGeneratorMock.Object);

            //Act //Assert
            await testObject.Handle(new ValidateUserQuery(), new CancellationToken());
        }

        [TestMethod]
        [ExpectedException(typeof(UserInputException), "UserInput exception is not thrown even if user not found / invalid user.")]
        public async Task UserNotFound_ShouldThrowUserInputException()
        {
            //Arrange
            var tokenGeneratorMock = CreateStrictMock<ITokenGenerator>();
            var userDataAcccessMock = CreateStrictMock<IUserDataAccess>();
            userDataAcccessMock.Setup(o => o.GetUserAuthenticationMessageAsync(It.IsAny<string>())).ReturnsAsync((UserAuthenticationMessage)null);
            var validatorMock = CreateStrictMock<AbstractValidator<UserAuthenticationMessage>>();
            validatorMock.Setup(o => o.ValidateAsync(It.IsAny<ValidationContext<UserAuthenticationMessage>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var testObject = new ValidateUserQueryHandler(validatorMock.Object, userDataAcccessMock.Object, tokenGeneratorMock.Object);

            //Act //Assert
            await testObject.Handle(new ValidateUserQuery() { Id = "InvalidUser", Password = "TestPassword" }, new CancellationToken());
        }

        [TestMethod]
        [ExpectedException(typeof(UserInputException), "UserInput exception is not thrown even if invalid user password.")]
        public async Task InvalidPassword_ShouldThrowUserInputException()
        {
            //Arrange
            var testUserId = "ValidUserId";
            var testPassword = "InvalidPassword";
            var tokenGeneratorMock = CreateStrictMock<ITokenGenerator>();
            var userDataAcccessMock = CreateStrictMock<IUserDataAccess>();
            userDataAcccessMock.Setup(o => o.GetUserAuthenticationMessageAsync(It.IsAny<string>()))
                .ReturnsAsync(new UserAuthenticationMessage() { Rno = 1, Id = testUserId, Password = "123" });
            var validatorMock = CreateStrictMock<AbstractValidator<UserAuthenticationMessage>>();
            validatorMock.Setup(o => o.ValidateAsync(It.IsAny<ValidationContext<UserAuthenticationMessage>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var request = new ValidateUserQuery() { Id = testUserId, Password = testPassword };
            var testObject = new ValidateUserQueryHandler(validatorMock.Object, userDataAcccessMock.Object, tokenGeneratorMock.Object);

            //Act //Assert
            await testObject.Handle(request, new CancellationToken());
        }

        [TestMethod]
        public async Task ValidUserIdPassword_VerifyExpectations()
        {
            //Arrange
            var testUserRno = 1;
            var testToken = "DummyToken";
            var testUserId = "ValidUserId";
            var testPassword = "ValidPassword";
            var testTokenMessage = new TokenMessage() { Token = testToken, UserRno = testUserRno };

            var tokenGeneratorMock = CreateStrictMock<ITokenGenerator>();
            tokenGeneratorMock.Setup(o => o.GenerateToken(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(testTokenMessage);
            var userDataAcccessMock = CreateStrictMock<IUserDataAccess>();
            userDataAcccessMock.Setup(o => o.GetUserAuthenticationMessageAsync(It.IsAny<string>()))
                .ReturnsAsync(new UserAuthenticationMessage() { Rno = testUserRno, Id = testUserId, Password = testPassword });
            var validatorMock = CreateStrictMock<AbstractValidator<UserAuthenticationMessage>>();
            validatorMock.Setup(o => o.ValidateAsync(It.IsAny<ValidationContext<UserAuthenticationMessage>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var request = new ValidateUserQuery() { Id = testUserId, Password = testPassword };
            var testObject = new ValidateUserQueryHandler(validatorMock.Object, userDataAcccessMock.Object, tokenGeneratorMock.Object);

            //Act
            var result = await testObject.Handle(request, new CancellationToken());

            //Assert
            Assert.AreEqual(testTokenMessage.Token, result.Token);
            Assert.AreEqual(testTokenMessage.UserRno, result.UserRno);
        }
    }
}
