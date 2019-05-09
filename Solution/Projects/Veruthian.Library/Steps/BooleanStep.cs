namespace Veruthian.Library.Steps
{
    public class BooleanStep : SimpleStep
    {
        bool value;

        private BooleanStep(bool value) => this.value = value;


        public bool Value => value;
        

        public override string Description => value.ToString();


        public static readonly BooleanStep True = new BooleanStep(true);

        public static readonly BooleanStep False = new BooleanStep(false);


        public static BooleanStep From(bool value) => value;
        

        public static implicit operator BooleanStep(bool value) => value ? True : False;

        public static implicit operator bool(BooleanStep value) => value.value;
    }
}