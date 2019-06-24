using Veruthian.Library.Collections;
using Veruthian.Library.Readers;

namespace Veruthian.Library.Steps.Reader
{
    public abstract class ReaderStep<T> : BaseStep, IActionStep<IReader<T>>, IActionStep<ObjectTable>
    {
        protected abstract bool Act(IReader<T> reader);

        public bool? Act(bool? state, bool completed, IReader<T> reader)
        {
            if (!completed && state == true)
                return Act(reader);
            else
                return state;
        }


        public const string ReaderAddress = "ReaderStep.Reader";

        public bool? Act(bool? state, bool completed, ObjectTable table)
            => Act(state, completed, table.Get<IReader<T>>(ReaderAddress));

        public static void PrepareTable(ObjectTable table, IReader<T> reader)
            => table.GetOrInsert(ReaderAddress, reader);
    }
}