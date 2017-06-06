namespace Soedeum.Dotnet.Library.Collections
{
    public abstract class SimpleScannerBase<T, S> : ScannerBase<T, S>
        where S : ScannerBase<T, S>
    {
        T item;

        protected override T RawPeek(int lookahead = 0) => item;

        protected override void Initialize() => MoveToNext();

        protected override void MoveToNext()
        {
            bool success = GetNext(out T next);

            if (!success)
                SetEnd(Position + 1, next);

            item = next;
        }

        protected override void VerifyLookahead(int lookahead = 0) { }
    }
}