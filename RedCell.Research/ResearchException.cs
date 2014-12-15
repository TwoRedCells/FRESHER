using System;

namespace RedCell.Research
{
    public class ResearchException : Exception
    {
        public ResearchException(string message = null, Exception innerException = null) : base(message, innerException)
        {

        }
    }
}
