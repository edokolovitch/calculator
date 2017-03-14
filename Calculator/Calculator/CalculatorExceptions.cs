using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    // Wrapppers for known exceptions that can be thrown by Calculator class

    [global::System.Serializable]
    public class CalculatorEvaluateException : Exception
    {
        public CalculatorEvaluateException() { }
        public CalculatorEvaluateException(string message) : base(message) { }
        public CalculatorEvaluateException(string message, Exception inner) : base(message, inner) { }
        protected CalculatorEvaluateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    public class CalculatorParseException : Exception
    {
        public CalculatorParseException() { }
        public CalculatorParseException(string message) : base(message) { }
        public CalculatorParseException(string message, Exception inner) : base(message, inner) { }
        protected CalculatorParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
