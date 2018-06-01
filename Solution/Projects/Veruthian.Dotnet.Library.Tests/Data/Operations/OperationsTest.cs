using System.Collections.Generic;
using Xunit;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class OperationsTest
    {
        static readonly IOperation<object> True = BooleanOperation<object>.True;
        static readonly IOperation<object> False = BooleanOperation<object>.False;

        static readonly IOperation<object> NotTrue = Invert(True);
        static readonly IOperation<object> NotFalse = Invert(False);

        


        private static IOperation<object> Invert(IOperation<object> operation) => new InvertedOperation<object>(operation);

        public static Dictionary<string, IOperation<object>> operations = new Dictionary<string, IOperation<object>>()
        {
            ["True"] = True,
            ["False"] = False,
            ["!True"] = NotTrue,
            ["!False"] = NotFalse,
        };


        [Theory]
        [InlineData("True", true)]
        [InlineData("False", false)]
        [InlineData("!True", false)]
        [InlineData("!False", true)]
        public void TestOperation(string operationKey, bool expectedResult)
        {
            IOperation<object> operation = operations[operationKey];

            bool result = operation.Perform(null);

            Assert.Equal(expectedResult, result);
        }
    }
}