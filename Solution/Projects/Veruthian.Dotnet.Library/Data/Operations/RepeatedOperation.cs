using System;
using System.Text;

namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class RepeatedOperation<TState> : IOperation<TState>
    {
        IOperation<TState> operation;

        int minimum, maximum;

        public RepeatedOperation(IOperation<TState> operation, int minimum = 0, int maximum = 0)
        {
            if (operation == null)
                throw new ArgumentNullException("Operation cannot be null!");

            this.operation = operation;


            if (minimum < 0 || maximum < 0 || (maximum != 0 && minimum > maximum))
                throw new ArgumentException("Minimum cannot be less that maximum");

            this.minimum = minimum;

            this.maximum = maximum;
        }

        public bool Perform(TState state)
        {
            int count = 0;

            while (true)
            {
                var result = operation.Perform(state);

                if (result)
                {
                    count++;

                    if (maximum != 0 && count == maximum)
                        return true;
                }
                else
                {
                    return count >= minimum;
                }
            }
        }

        public bool Perform(TState state, IOperationTracer<TState> tracer)
        {
            tracer.StartingOperation(this, state);

            bool finalResult = true;

            int count = 0;

            while (true)
            {
                var result = operation.Perform(state, tracer);

                if (result)
                {
                    count++;

                    if (maximum != 0 && count == maximum)
                    {
                        finalResult = true;
                        break;
                    }
                }
                else
                {
                    finalResult = count >= minimum;
                    break;
                }
            }

            tracer.FinishingOperation(this, state, finalResult);

            return finalResult;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append('{').Append(operation.ToString());

            if (maximum != 0)
            {
                builder.Append(" : ").Append(minimum);

                builder.Append(" to ").Append(maximum);
            }
            else if (minimum != 0)
            {
                builder.Append(" : ").Append(minimum);
            }

            builder.Append('}');

            return builder.ToString();
        }

    }
}