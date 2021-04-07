using FluentValidation;
using FluentValidation.Results;
using JewelleryStore.Application.Common;
using JewelleryStore.Application.Price;
using JewelleryStore.Application.User;
using JewelleryStore.Model.Jewellery;
using JewelleryStore.Model.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JewelleryStore.UnitTest
{
    [TestClass]
    public class CalculateJewelleryPriceQueryHandlerTest : BaseValidationTest
    {
        [TestMethod]
        [ExpectedException(typeof(ValidationException), "Validation exception is not thrown even if validation failed.")]
        public async Task ValidationFails_ShouldThrowValidationException()
        {
            //Arrange
            var userContextMock = CreateStrictMock<IUserContext>();
            var userDataAcccessMock = CreateStrictMock<IUserDataAccess>();
            var validatorMock = CreateStrictMock<AbstractValidator<JewelleryMessage>>();
            validatorMock.Setup(o => o.ValidateAsync(It.IsAny<ValidationContext<JewelleryMessage>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>() { new ValidationFailure(nameof(JewelleryMessage.Price), "invalid price") }));

            var testObject = new CalculateJewelleryPriceQueryHandler(userContextMock.Object, userDataAcccessMock.Object, validatorMock.Object);

            //Act //Assert
            await testObject.Handle(new CalculateJewelleryPriceQuery(), new CancellationToken());
        }

        [TestMethod]
        public async Task NormalUser_GetTotalPriceWithoutDicount_VerifyExpectations()
        {
            //Arrange
            var userContextMock = CreateStrictMock<IUserContext>();
            userContextMock.Setup(o => o.UserRno).Returns(1);
            var userDataAcccessMock = CreateStrictMock<IUserDataAccess>();
            userDataAcccessMock.Setup(o => o.DetailsAsync(It.IsAny<int>())).ReturnsAsync(new UserMessage() { Rno = 1, Id = "TestNormalUser", DiscountPercentage = 10, Type = UserType.NormalUser });
            var validatorMock = CreateStrictMock<AbstractValidator<JewelleryMessage>>();
            validatorMock.Setup(o => o.ValidateAsync(It.IsAny<ValidationContext<JewelleryMessage>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            var request = new CalculateJewelleryPriceQuery() { Price = 10, Weight = 10 };
            var testObject = new CalculateJewelleryPriceQueryHandler(userContextMock.Object, userDataAcccessMock.Object, validatorMock.Object);

            //Act
            var result = await testObject.Handle(request, new CancellationToken());

            //Assert
            var expectedTotalPrice = request.Price * request.Weight;
            Assert.AreEqual(expectedTotalPrice, result);
        }

        [TestMethod]
        public async Task PriviledgedUser_GetTotalPriceWithDicount_VerifyExpectations()
        {
            //Arrange
            var discount = 10;
            var userContextMock = CreateStrictMock<IUserContext>();
            userContextMock.Setup(o => o.UserRno).Returns(1);
            var userDataAcccessMock = CreateStrictMock<IUserDataAccess>();
            userDataAcccessMock.Setup(o => o.DetailsAsync(It.IsAny<int>())).ReturnsAsync(new UserMessage() { Rno = 1, Id = "TestPriviledgedUser", DiscountPercentage = discount, Type = UserType.PrivilegedUser });
            var validatorMock = CreateStrictMock<AbstractValidator<JewelleryMessage>>();
            validatorMock.Setup(o => o.ValidateAsync(It.IsAny<ValidationContext<JewelleryMessage>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            var request = new CalculateJewelleryPriceQuery() { Price = 10, Weight = 10 };
            var testObject = new CalculateJewelleryPriceQueryHandler(userContextMock.Object, userDataAcccessMock.Object, validatorMock.Object);

            //Act
            var result = await testObject.Handle(request, new CancellationToken());

            //Assert
            var totalPrice = (request.Price * request.Weight);
            var discountPrice = (request.Price * request.Weight) * ((double)discount / 100);
            var expectedTotalPrice = totalPrice - discountPrice;

            Assert.AreEqual(expectedTotalPrice, result);
        }
    }
}
