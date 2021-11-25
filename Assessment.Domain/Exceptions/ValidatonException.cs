using System;

namespace Assessment.Domain.Exceptions
{
    public class TestletException : ApplicationException
    {
        public TestletException(string? message) : base(message)
        {
        }

        public TestletException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

    }

    public class TestletValidationException : TestletException
    {
        public TestletValidationException(string? message): base(message)
        {
        }
    }

    public class TestletOperationException : TestletException
    {
        public TestletOperationException(string? message) : base(message)
        {
        }

        public TestletOperationException(string? message, Exception? innerException): base(message, innerException)
        {
        }
    }

}
