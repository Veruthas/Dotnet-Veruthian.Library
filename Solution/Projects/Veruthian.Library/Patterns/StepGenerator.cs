namespace Veruthian.Library.Patterns
{
    public class StepGenerator
    {
        public IStep Sequence(params IStep[] steps)
        {
            Step first = new Step("Sequence");

            Step current = null;

            foreach (var step in steps)
            {
                if (current == null)
                {
                    current = new Step();

                    first.Down = current;
                }
                else
                {
                    var next = new Step();

                    current.Next = next;

                    current = next;
                }

                current.Down = step;
            }

            return first;
        }

        public IStep IfThenElse(IStep condition, IStep thenStep, IStep elseStep)
        {
            return new Step("IfThenElse")
            {
                Shunt = condition,
                Down = thenStep,
                Next = elseStep
            };
        }

        public IStep UnlessThenElse(IStep condition, IStep thenStep, IStep elseStep)
        {
            return new Step("UnlessThenElse")
            {
                Shunt = condition,
                Down = elseStep,
                Next = thenStep
            };
        }
    }
}