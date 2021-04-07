using JewelleryStore.Application.Exceptions;
using JewelleryStore.Application.User;
using JewelleryStore.Model.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;
using System.Threading.Tasks;

namespace JewelleryStore.UnitTest
{
    [TestClass]
    public class UserDetailsQueryHandlerTest : BaseValidationTest
    {
        [TestMethod]
        [ExpectedException(typeof(NotFoundException), "NotFound exception is not thrown even if user not found.")]
        public async Task UserNotFound_ShouldThrowNotFoundException()
        {
            //Arrange
            var userDataAcccessMock = CreateStrictMock<IUserDataAccess>();
            userDataAcccessMock.Setup(o => o.DetailsAsync(It.IsAny<int>())).ReturnsAsync((UserMessage)null);

            var testObject = new UserDetailsQueryHandler(userDataAcccessMock.Object);

            //Act //Assert
            await testObject.Handle(new UserDetailsQuery(), new CancellationToken());
        }

        [TestMethod]
        public async Task UserFound_VerifyExpectations()
        {
            //Arrange
            var testUserMessage = new UserMessage() { Rno = 1, Id = "TestUser", DiscountPercentage = 10, Type = UserType.PrivilegedUser };

            var userDataAcccessMock = CreateStrictMock<IUserDataAccess>();
            userDataAcccessMock.Setup(o => o.DetailsAsync(It.IsAny<int>())).ReturnsAsync(testUserMessage);

            var testObject = new UserDetailsQueryHandler(userDataAcccessMock.Object);

            //Act
            var result = await testObject.Handle(new UserDetailsQuery(), new CancellationToken());

            //Assert
            Assert.AreEqual(testUserMessage.Rno, result.Rno);
            Assert.AreEqual(testUserMessage.Id, result.Id);
            Assert.AreEqual(testUserMessage.DiscountPercentage, result.DiscountPercentage);
            Assert.AreEqual(testUserMessage.Type, result.Type);
        }
    }
}
