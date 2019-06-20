namespace Veruthian.Library.Steps
{
    public abstract class BaseStep : IStep
    {
        public abstract string Type { get; }

        public abstract string Name { get; }


        IStep IStep.Shunt => GetShunt();

        IStep IStep.Down => GetDown();

        IStep IStep.Next => GetNext();


        protected virtual IStep GetShunt() => null;

        protected virtual IStep GetDown() => null;

        protected virtual IStep GetNext() => null;



        public override string ToString() => StepString(Type, Name);

        protected string StepString(string Type, string Name)
            => (Type == null ? (Name == null ? "" : "Step:" + Name) : (Name == null ? Type : Type + ":" + Name));
    }
}