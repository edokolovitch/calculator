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
                        operand = operand * 10 + digitIndex;
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

            if (negative) operand = -operand;
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
            Operands = new List<Operand>();
            Operators = new List<Operator>();

            // presume success upon return - any problems throw CalculatorException
            string remaining = Expression.Replace(" ", "");
            string unknown = UnknownCharacters(remaining);
            if (!string.IsNullOrEmpty(unknown))
            {
                // If anything left, we know parsing will fail eventually, so throw now.
                throw new CalculatorParseException(string.Format("Unknown characters {0}", unknown));
            }

            Operands.Add(new Operand(TakeValueOperand(ref remaining)));
            while (!string.IsNullOrEmpty(remaining))
            {
                Operators.Add(TakeOperator(ref remaining));
                Operands.Add(new Operand(TakeValueOperand(ref remaining)));
            }

        }

        void PerformOperation(int leftIndex, int rightIndex, Operator operation)
        {
            Operand left = GetLeftOperand(leftIndex);
            Operand right = GetRightOperand(rightIndex);

            switch (operation)
            {
                case Operator.Exponention:
                    throw new CalculatorEvaluateException("Exponentiation not implemented as integer operation");

                case Operator.Multiplication:
                    left.Value = left.Value * right.Value;
                    break;

                case Operator.Division:
                    left.Value = left.Value / right.Value;
                    break;

                case Operator.Addition:
                    left.Value = left.Value + right.Value;
                    break;

                case Operator.Subtraction:
                    left.Value = left.Value - right.Value;
                    break;

                default:
                    throw new CalculatorEvaluateException(string.Format("Unsupported operation {0}", operation.ToString()));
                
            }

            right.Eliminated = true;
        }


        private Operand GetLeftOperand(int index)
        {
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
            return myCalc.Evaluate(); // throws CalculatorError if overflow or divide by zero
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
