namespace Veruthian.Library.Patterns
{
    public class BooleanStep : Step
    {
        private BooleanStep(bool? value) : base(value == null ? "null" : value.ToString()) => Value = value;

        public bool? Value { get; }


        public static BooleanStep True = new BooleanStep(true);

        public static BooleanStep False = new BooleanStep(false);

        public static BooleanStep Null = new BooleanStep(null);
    }
}