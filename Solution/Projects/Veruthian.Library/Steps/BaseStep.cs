namespace Veruthian.Library.Steps
{
    public abstract class BaseStep : IStep
    {
        public abstract string Description { get; }


        IStep IStep.Shunt => GetShunt();

        IStep IStep.Step => GetDown();

        IStep IStep.Next => GetNext();


        protected virtual IStep GetShunt() => null;

        protected virtual IStep GetDown() => null;

        protected virtual IStep GetNext() => null;



        public override string ToString() => Description;
    }
}