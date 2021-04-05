using System;

namespace JewelleryStore.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key) : base($"Entity [{name}] ({key}) was not found.") { }
    }
}
