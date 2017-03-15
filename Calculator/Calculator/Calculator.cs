using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Calculator
    {
        private class Operand
        {
            public int Value { get; set;}
            public bool Eliminated { get; set;}

            public Operand()
            {
                Eliminated = false;
                Value = 0;
            }

            public Operand(int value)
            {
                Eliminated = false;
                Value = value;
            }
        }

        public enum Operator
        {
            Exponention,
            Multiplication,
            Division,
            Addition,
            Subtraction
        };

        // Expression evaluator class. 

        private string Expression_;

        public string Expression {
            get
            {
                return Expression_; 
            }
            set
            {
                Expression_ = value;
                ParseExpression(); // may throw
            }
        }

        private List<Operator> Operators;
        private List<Operand> Operands;

        private Calculator()
        {
            // no public constructor for this implementation, instantiation is internal.
        }

        private Calculator(string expression)
        {
            Expression = expression;
        }

        private static string[] parseableDigits = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        private static string[] parseableOperators = { "+", "-", "*", "/", "^" };

        private static int TakeValueOperand(ref string expressionRemaining)
        {
            string Digits = "01234567890";
            int operand = 0;
            bool haveOperand = false;
            bool negative = false;
            int length = expressionRemaining.Length;
            int parseIndex;
            try
            {
                for (parseIndex = 0; parseIndex < length; parseIndex++)
                {
                    if (parseIndex==0 && expressionRemaining[0]=='-')
                    {
                        negative = true;
                        continue;
                    }
                    else
                    {
                        int digitIndex = Digits.IndexOf(expressionRemaining[parseIndex]);
                        if (digitIndex >= 0)
                        {
                            operand = checked(operand * 10 + digitIndex);
                            haveOperand = true;
                            continue;
                        }
                        else break;
                    }
                
                }
                if (!haveOperand)
                {
                    throw new CalculatorParseException("Missing operand");
                }
                if (parseIndex < length)
                {
                    // if anything left
                    expressionRemaining = expressionRemaining.Substring(parseIndex);
                }
                else
                {
                    // no more, handle this way so Substring() doesn't throw out of bounds
                    expressionRemaining = "";
                }

                if (negative) operand = checked(-operand);
            }
            catch(OverflowException ex)
            {
                throw new CalculatorParseException("Operand out of range");
            }
            return operand;
        }

        private static Operator GetOperatorFromToken(string token)
        {
            switch (token)
            {
                case "+":
                    return Operator.Addition;
                case "-":
                    return Operator.Subtraction;
                case "*":
                    return Operator.Multiplication;
                case "/":
                    return Operator.Division;
                default:
                    throw new CalculatorParseException(string.Format("Unknown operator '{0}'", token));
            }
        }

        private static Operator TakeOperator(ref string expressionRemaining)
        {
            // Parse constant integer by taking successive digits until non-digit found or end of string.
            // If leading - seen, flag for negation.
            // If integer overflow during parse, throw CalculatorParseException


            int length = expressionRemaining.Length;
            string operatorToken = expressionRemaining.Substring(0, 1);

            Operator parsedOperator = GetOperatorFromToken(operatorToken);

            if (length < 2)
            {
                // not enough left for operand after operator!
                throw new CalculatorParseException(string.Format("missing operand after '{0}'", operatorToken));
            }

            expressionRemaining = expressionRemaining.Substring(1, length - 1);

            return parsedOperator;
        }

        private static string UnknownCharacters(string expression)
        {
            // Take out what we know can be valid - return what's left (s/b nothing)

            string[] allowed = parseableDigits.Union(parseableOperators).ToArray();
            foreach (string removethis in allowed)
            {
                expression = expression.Replace(removethis, "");
            }
            return expression;
        }

        private void ParseExpression()
        {
            // Parse expression into list of operators and list of operands.
            // Expression is presumed to start with an operand.
            // May have 0 to any following operator and expression combinations.

            Operands = new List<Operand>();
            Operators = new List<Operator>();

            // presume success upon return - any problems throw CalculatorParseException
            string remaining = Expression.Replace(" ", "");
            string unknown = UnknownCharacters(remaining);
            if (!string.IsNullOrEmpty(unknown))
            {
                // If anything left, we know parsing will fail eventually, so throw now.
                throw new CalculatorParseException(string.Format("Unknown characters {0}", unknown));
            }

            Operands.Add(new Operand(TakeValueOperand(ref remaining))); // throws if fail to parse and take operand
            while (!string.IsNullOrEmpty(remaining))
            {
                Operators.Add(TakeOperator(ref remaining)); //throws if fail to take a valid operator
                Operands.Add(new Operand(TakeValueOperand(ref remaining))); // throws if fails to take
            }

        }

        void PerformOperation(int leftIndex, int rightIndex, Operator operation)
        {
            // Perform operation against given operands.
            // Result is stored back to left operand value.
            // Right operand value is flagged as eliminated (used).

            Operand left = GetLeftOperand(leftIndex);
            Operand right = GetRightOperand(rightIndex);

            try
            {
                switch (operation)
                {
                    case Operator.Exponention:
                        throw new CalculatorEvaluateException("Exponentiation not implemented as integer operation");

                    case Operator.Multiplication:
                        left.Value = checked(left.Value * right.Value);
                        break;

                    case Operator.Division:
                        if (right.Value == 0)
                        {
                            throw new CalculatorEvaluateException("Division by zero.");
                        }
                        left.Value = left.Value / right.Value;
                        break;

                    case Operator.Addition:
                        left.Value = checked(left.Value + right.Value);
                        break;

                    case Operator.Subtraction:
                        left.Value = checked(left.Value - right.Value);
                        break;

                    default:
                        throw new CalculatorEvaluateException(string.Format("Unsupported operation {0}", operation.ToString()));
                
                }
            }
            catch (OverflowException ex)
            {
                throw new CalculatorEvaluateException("Overflow error");
            }
            right.Eliminated = true;
        }


        private Operand GetLeftOperand(int index)
        {
            // Return correct operand to use.
            // If specified operand has been eliminated, look at the preceding operand until find non-eliminated value.
            // If algorithm implementation is sound, should always find a value. Test anyway.

            for (int leftIndex = index; leftIndex >= 0; leftIndex--)
            {
                Operand leftOperand = Operands[leftIndex];
                if (leftOperand.Eliminated == false)
                {
                    return leftOperand;
                }
            }
            throw new CalculatorEvaluateException("Internal calculation error - missing left operand");
        }

        private Operand GetRightOperand(int index)
        {
            // Return correct operand to use.
            // If specified operand has been eliminated, look at the following operand until find non-eliminated value.
            // If algorithm implementation is sound, should always find a value. Test anyway.

            int operandMaxIndex = Operands.Count - 1;
            for (int rightIndex = index; rightIndex <= operandMaxIndex; rightIndex++)
            {
                Operand leftOperand = Operands[rightIndex];
                if (leftOperand.Eliminated == false)
                {
                    return leftOperand;
                }
            }
            throw new CalculatorEvaluateException("Internal calculation error - missing right operand");
        }

        private int Evaluate()
        {
            Operator[] firstpass = { Operator.Multiplication, Operator.Division };
            Operator[] secondpass = { Operator.Addition, Operator.Subtraction };
            Operator[][] passes = { firstpass, secondpass };
            int OperationCount = Operators.Count;
            int OperationsDone = 0;

            // Multiple passes done, one for each tier of precendence.
            // Within each precendence level, operations of that level are done left to right (0 to n index)
            // Given successful parsing, number of operand is number of operators plus one.
            // As operations are performed, result will have been stored back in the left operand value, with right operand flagged as used
            // Internal function to retrieve left/right operand moved further left/right to get value from correct operand object as values are used up.

            foreach (Operator[] operationsThisPass in passes)
            {
                for (int operatorIndex = 0; operatorIndex < OperationCount; operatorIndex++)
                {
                    Operator operation = Operators[operatorIndex];
                    if (operationsThisPass.Contains(operation))
                    {
                        PerformOperation(operatorIndex, operatorIndex + 1, operation);
                        OperationsDone++;
                    }
                }
            }
            if (OperationsDone != OperationCount)
            {
                throw new CalculatorEvaluateException(string.Format("Internal error - Operation count {0} does not match expected value {1}", OperationsDone, OperationCount));
            }
            return Operands[0].Value;
        }

        public static int Calculate(string expressionToEvaluate)
        {
            // Compute value, let caller catch exception
            Calculator myCalc = new Calculator(expressionToEvaluate); // throws CalculatorException if parsing error
            return myCalc.Evaluate(); // throws CalculatorError if overflow or divide by zero, or other detected internal failure
        }

        public static int CalculateNoThrow(string expressionToEvaluate, out bool succeeded, out string errorIfAny)
        {
            int result = 0;
            try
            {
                result =  Calculate(expressionToEvaluate);
                succeeded = true;
                errorIfAny = null;
            }
            catch (CalculatorEvaluateException ex) // something we accounted for
            {
                succeeded = false;
                errorIfAny = ex.Message;
            }
            catch (CalculatorParseException ex) // something we accounted for
            {
                succeeded = false;
                errorIfAny = ex.Message;
            }
            catch (Exception ex) // something we didn't account for
            {
                succeeded = false;
                errorIfAny = ex.Message;
            }

            return result;
        }
    }
}
