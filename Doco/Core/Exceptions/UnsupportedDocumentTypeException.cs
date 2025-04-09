using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doco.Core.Exceptions
{
    internal class UnsupportedDocumentTypeException : Exception
    {
        public UnsupportedDocumentTypeException(string message) : base(message)
        {
        }
        public UnsupportedDocumentTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
