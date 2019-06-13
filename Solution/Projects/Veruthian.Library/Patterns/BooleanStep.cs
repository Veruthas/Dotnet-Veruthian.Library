namespace Veruthian.Library.Patterns
{
    public class BooleanStep : IStep
    {
        private BooleanStep(bool value) => Value = value;

        public bool Value { get; }


        public IStep Shunt => null;

        public IStep Down => null;

        public IStep Next => null;


        public static BooleanStep True = new BooleanStep(true);

        public static BooleanStep False = new BooleanStep(false);
    }
}