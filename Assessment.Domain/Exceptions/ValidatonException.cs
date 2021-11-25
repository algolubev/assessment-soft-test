using System;

namespace Assessment.Domain.Exceptions
{
    public class TestletException : ApplicationException
    {
        public TestletException(string? message) : base(message)
        {
        }

    }

    public class TestletValidationException : TestletException
    {
        public TestletValidationException(string? message): base(message)
        {
        }
    }


}
