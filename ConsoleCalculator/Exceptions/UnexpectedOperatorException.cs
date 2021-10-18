using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator.Exceptions
{
    public class UnexpectedOperatorException : Exception
    {
        public UnexpectedOperatorException(string message = "Unexpected operator") : base(message)
        {
        }
    }
}
