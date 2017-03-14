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
            public TestData(string description, string expression, int result)
            {
                Description = description;
                Expression = expression;
                ExpectedResult = result;
            }
        }

        [TestMethod]
        public void TestCalculateNoThrow()
        {
            TestData[] Examples = 
            {
                new TestData("Given example 1",     "5+5+32",   42),
                new TestData("Given example 2",     "3 * 4",    12),

                new TestData("Multiply over add",   "2+3*4",    14),
                new TestData("Divide over add",     "30+10/5",  32),
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
            }
        }
    }
}
