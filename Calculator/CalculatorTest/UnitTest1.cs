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
        public void TestGivenExample1()
        {
            RunSingleTestDataItem(new TestData("Given example 1", "5+5+32", 42, ""));
        }

        [TestMethod]
        public void TestGivenExample2()
        {
            RunSingleTestDataItem(new TestData("Given example 2",         "3 * 4",     12, ""));
        }

        [TestMethod]
        public void TestMultiplyOverAdd()
        {
            RunSingleTestDataItem(new TestData("Multiply over add",       "2+3*4",     14, ""));
        }

        [TestMethod]
        public void TestDivideOverAdd()
        {
            RunSingleTestDataItem(new TestData("Divide over add", "30+10/5", 32, ""));
        }

        [TestMethod]
        public void TestMultiplyOverSubtract()
        {
            RunSingleTestDataItem(new TestData("Multiply over subtract", "2-3*4", -10, ""));
        }

        [TestMethod]
        public void TestDivideOverSubtract()
        {
            RunSingleTestDataItem(new TestData("Divide over subtract", "30-10/5", 28, ""));
        }

        [TestMethod]
        public void TestNegativeSingleValue()
        {
            RunSingleTestDataItem(new TestData("negative single value", "-5", -5, ""));
        }

        [TestMethod]
        public void TestPositiveSingleValue()
        {
            RunSingleTestDataItem(new TestData("positive single value", "77", 77, ""));
        }

        [TestMethod]
        public void TestAddPositiveToNegative()
        {
            RunSingleTestDataItem(new TestData("to a pos, add a negative", "10+-2", 8, ""));
        }

        [TestMethod]
        public void TestMultiplyPositiveByNegative()
        {
            RunSingleTestDataItem(new TestData("multiply pos by negative", "10*-5", -50, ""));
        }

        [TestMethod]
        public void TestDivideNegativeByNegative()
        {
            RunSingleTestDataItem(new TestData("divide negative by negative", "-71/-7", 10, ""));
        }

        [TestMethod]
        public void TestDivideNegativeByPositive()
        {
            RunSingleTestDataItem(new TestData("divide negative by pos", "-71/7", -10, ""));
        }

        [TestMethod]
        public void TestDividePositiveByNegative()
        {
            RunSingleTestDataItem(new TestData("divide pos by negative", "71/-7", -10, ""));
        }

        [TestMethod]
        public void TestMiscellaneous1()
        {
            RunSingleTestDataItem(new TestData("Misc #1", "3*2+8/4", 8, ""));
        }

        [TestMethod]
        public void TestMiscellaneous2()
        {
            RunSingleTestDataItem(new TestData("Misc #2", "-4+10-1", 5, ""));
        }

        [TestMethod]
        public void TestDivideByZero()
        {
            RunSingleTestDataItem(new TestData("Divide by zero handling", "10/0",     0,  "Division by zero."));
        }

        [TestMethod]
        public void TestMissingLeftOperand()
        {
            RunSingleTestDataItem(new TestData("Missing left operand", "+2", 0, "Missing operand"));
        }

        [TestMethod]
        public void TestMissingRightOperand()
        {
            RunSingleTestDataItem(new TestData("Missing right operand", "1+", 0, "missing operand after '+'"));
        }

        [TestMethod]
        public void TestUnknownCharacters()
        {
            RunSingleTestDataItem(new TestData("Unknown characters", "1A+2BC", 0, "Unknown characters ABC"));
        }

        [TestMethod]
        public void TestOverflowOnAddition()
        {
            RunSingleTestDataItem(new TestData("Addition overflow", "2000000000+2000000000", 0, "Overflow error"));
        }

        [TestMethod]
        public void TestOverflowOnSubtraction()
        {
            RunSingleTestDataItem(new TestData("Subtraction overflow", "-2000000000-2000000000", 0, "Overflow error"));
        }

        [TestMethod]
        public void TestOverflowOnMultiplication()
        {
            RunSingleTestDataItem(new TestData("Multiplication overflow", "2000000000+2000000000", 0, "Overflow error"));
        }

        [TestMethod]
        public void TestOutOfRangeOperand()
        {
            RunSingleTestDataItem(new TestData("Out of range operand", "4000000000", 0, "Operand out of range"));
        }

        [TestMethod]
        public void TestOutOfRangeNegativeOperand()
        {
            RunSingleTestDataItem(new TestData("Out of range negative operand", "-4000000000", 0, "Operand out of range"));
        }

        private static void RunSingleTestDataItem(TestData testCase)
        {
            bool success = false;
            string message = null;
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
