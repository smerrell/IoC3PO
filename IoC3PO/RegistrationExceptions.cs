using System;

namespace IoC3PO
{
    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(string message) : base(message)
        {
        }
    }

    public class TypeNotAssignableToContractException : Exception
    {
        public TypeNotAssignableToContractException(string message) : base(message)
        {
        }
    }
}