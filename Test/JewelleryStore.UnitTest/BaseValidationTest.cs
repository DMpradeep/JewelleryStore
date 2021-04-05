using Moq;

namespace JewelleryStore.UnitTest
{
    public abstract class BaseValidationTest
    {
        protected static Mock<T> CreateStrictMock<T>() where T : class
        {
            return new Mock<T>(MockBehavior.Strict);
        }
    }
}