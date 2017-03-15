using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;


namespace CalculatorTest
{
    [TestClass]
    public class UnitTest1
    {
        class TestData
        {
            public string Description { get; set; }
            public string Expression { get; set; }
            public int ExpectedResult { get; set; }
            public string ExpectedErrorMessage { get; set; }

            public TestData(string description, string expression, int result, string errormessage)
            {
                Description = description;
                Expression = expression;
                ExpectedResult = result;
                ExpectedErrorMessage = errormessage;
            }
        }

        [TestMethod]
        public void TestCalculateNoThrow()
        {
            TestData[] Examples = 
            {
                new TestData("Given example 1",         "5+5+32",    42, ""),
                new TestData("Given example 2",         "3 * 4",     12, ""),

                new TestData("Multiply over add",       "2+3*4",     14, ""),
                new TestData("Divide over add",         "30+10/5",   32, ""),
                new TestData("Multiply over subtract",  "2-3*4",    -10, ""),
                new TestData("Divide over subtract",    "30-10/5",   28, ""),
                new TestData("negative single value",   "-5",        -5, ""),
                new TestData("positive single value",   "77",        77, ""),
                new TestData("to a pos, add a negative","10+-2",      8, ""),
                new TestData("multiply pos by negative","10*-5",    -50, ""),
                new TestData("divide negative by negative","-71/-7", 10,  ""),
                new TestData("divide negative by pos",   "-71/7",   -10,  ""),
                new TestData("divide pos by negative",   "71/-7",   -10,  ""),
                new TestData("Misc #1",                  "3*2+8/4",   8,  ""),
                new TestData("Misc #2",                  "-4+10-1",   5,  ""),

                // tests expecting specific trapped failures
                new TestData("Divide by zero handling", "10/0",     0,  "Division by zero."),
                new TestData("Missing left operand",    "+2",       0,  "Missing operand"),
                new TestData("Missing right operand",   "1+",       0,  "missing operand after '+'"),
                new TestData("Unknown characters",      "1A+2BC",   0,  "Unknown characters ABC"),
                new TestData("Addition overflow",       "2000000000+2000000000", 0, "Overflow error"),
                new TestData("Subtraction overflow",    "-2000000000-2000000000", 0, "Overflow error"),
                new TestData("Multiplication overflow", "2000000000+2000000000", 0, "Overflow error"),
                new TestData("Out of range operand",    "4000000000", 0, "Overflow error"),
                new TestData("Out of range -operand",    "-4000000000", 0, "Overflow error"),
                
            };

            bool success;
            string message;
            foreach (TestData testCase in Examples)
            {
                success = false;
                message = null;
                int result;
                result = Calculator.Calculator.CalculateNoThrow(testCase.Expression, out success, out message);
                Assert.AreEqual(testCase.ExpectedResult, result, testCase.Description);
                if (!string.IsNullOrEmpty(testCase.ExpectedErrorMessage))
                {
                    Assert.AreEqual(testCase.ExpectedErrorMessage, message, testCase.Description);
                }
            }
        }
    }
}
