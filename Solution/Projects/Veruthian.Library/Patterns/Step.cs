namespace Veruthian.Library.Patterns
{
    public abstract class Step : IStep
    {
        public virtual string Type => null;

        public virtual string Label => null;


        IStep IStep.Shunt => GetShunt();

        IStep IStep.Down => GetDown();

        IStep IStep.Next => GetNext();


        protected virtual IStep GetShunt() => null;

        protected virtual IStep GetDown() => null;

        protected virtual IStep GetNext() => null;



        public override string ToString() => (Type == null ? (Label == null ? "" : Label) : (Label == null ? Type : Label + ":" + Type));
    }
}