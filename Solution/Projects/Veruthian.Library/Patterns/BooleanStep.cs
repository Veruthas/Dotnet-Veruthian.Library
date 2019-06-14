namespace Veruthian.Library.Patterns
{
    public sealed class BooleanStep : BaseStep
    {
        private BooleanStep(bool? value) => this.Value = value;

        public override string Type => "Boolean";

        public override string Name => Value == true ? "True" : Value == false ? "False" : "Null";


        public bool? Value { get; }


        public static BooleanStep True = new BooleanStep(true);

        public static BooleanStep False = new BooleanStep(false);

        public static BooleanStep Null = new BooleanStep(null);
    }
}