using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XingXpressionEvaluator
{
    class NullExpressionException : ArgumentException
    {
        public NullExpressionException(string message): base(message) {
        
        }
    }
}
