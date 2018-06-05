using System;
using System.Collections.Generic;
using Xunit;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class OperationsTest
    {
        private static readonly IOperation<object> True = BooleanOperation<object>.True;
        private static readonly IOperation<object> False = BooleanOperation<object>.False;

        private static IOperation<object> Invert(IOperation<object> operation) => new InvertedOperation<object>(operation);

        private static IOperation<object> Maybe(IOperation<object> operation) => new OptionalOperation<object>(operation);

        private static IOperation<object> AllOf(params IOperation<object>[] operations) => new VariableSequentialOperation<object>(SequenceType.AllOf, operations);

        private static IOperation<object> AnyOf(params IOperation<object>[] operations) => new FixedSequentialOperation<object>(SequenceType.AnyOf, operations);

        private static IOperation<object> Repeat(IOperation<object> operation, int min = 0, int max = 0) => new RepeatedOperation<object>(operation, min, max);


        private static IOperation<object> Increment(int until)
        {
            return new ActionOperation<object>(
                (o) =>
                {
                    var data = (Boxed<int>)o;

                    if (data.Value == until)
                        return false;
                    else
                    {
                        data.Value++;
                        return true;
                    }
                }
            );
        }


        private class Boxed<T>
        {
            public T Value;

            public Boxed() { }

            public Boxed(T val) => Value = val;


            public override string ToString() => Value != null ? Value.ToString() : string.Empty;
        }



        public static Dictionary<string, IOperation<object>> operations = new Dictionary<string, IOperation<object>>()
        {
            ["True"] = True,
            ["False"] = False,
            ["!True"] = Invert(True),
            ["!False"] = Invert(False),
            ["True?"] = Maybe(True),
            ["False?"] = Maybe(False),
            ["(True True)"] = AllOf(True, True),
            ["(True False)"] = AllOf(True, False),
            ["(False True)"] = AllOf(False, True),
            ["(False False)"] = AllOf(False, True),
            ["(True|True)"] = AnyOf(True, True),
            ["(True|False)"] = AnyOf(True, False),
            ["(False|True)"] = AnyOf(False, True),
            ["(False|False)"] = AnyOf(False, False),
            ["{False}"] = Repeat(False),
            ["{True:0:5}"] = Repeat(True, 0, 5),
            ["{Inc(0-5)}"] = Repeat(Increment(5)),
            ["{Inc(0-5):0:5}"] = Repeat(Increment(5), 0, 5),
            ["{Inc(0-4):0:5}"] = Repeat(Increment(4), 0, 5),
            ["{Inc(0-3):3:5}"] = Repeat(Increment(3), 3, 5),
            ["{Inc(0-2):3:5}"] = Repeat(Increment(2), 3, 5),
        };

        public static Dictionary<string, Func<object, object>> constructors = new Dictionary<string, Func<object, object>>()
        {
            ["int"] = (o) => { var i = (int?)o; return new Boxed<int>(i.GetValueOrDefault()); }
        };


        [Theory]
        [InlineData("True", true)]
        [InlineData("False", false)]
        [InlineData("!True", false)]
        [InlineData("!False", true)]
        [InlineData("True?", true)]
        [InlineData("False?", true)]
        [InlineData("(True True)", true)]
        [InlineData("(True False)", false)]
        [InlineData("(False True)", false)]
        [InlineData("(False False)", false)]
        [InlineData("(True|True)", true)]
        [InlineData("(True|False)", true)]
        [InlineData("(False|True)", true)]
        [InlineData("{False}", true)]
        [InlineData("{True:0:5}", true)]
        [InlineData("{Inc(0-5)}", true, "int")]
        [InlineData("{Inc(0-5):0:5}", true, "int")]
        [InlineData("{Inc(0-4):0:5}", true, "int")]
        [InlineData("{Inc(0-3):3:5}", true, "int")]
        [InlineData("{Inc(0-2):3:5}", false, "int")]
        public void TestSimpleOperation(string operationKey, bool expectedResult, string constructor = null, object args = null, string expectedState = null)
        {
            IOperation<object> operation = operations[operationKey];

            object data = null;

            if (constructor != null)
                data = constructors[constructor](args);

            bool result = operation.Perform(data);

            Assert.Equal(expectedResult, result);

            if (expectedState != null)
                Assert.Equal(expectedState, data?.ToString());
        }
    }
}