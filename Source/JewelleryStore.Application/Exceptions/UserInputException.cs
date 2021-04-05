using System;

namespace JewelleryStore.Application.Exceptions
{
    public class UserInputException : Exception
    {
        public UserInputException() { }

        public UserInputException(string message) : base(message) { }
    }
}
