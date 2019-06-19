using Veruthian.Library.Collections;
using Veruthian.Library.Processing;

namespace Veruthian.Library.Steps.Actions
{
    public class SpeculateStep : BaseStep, IActionStep<ISpeculative>, IActionStep<ObjectTable>
    {
        public override string Type => "Speculate";

        public override string Name => null;

        public bool? Act(bool? state, bool completed, ISpeculative speculative)
        {
            if (!completed)
                speculative.Mark();
            else
                speculative.Rollback();

            return state;
        }


        public const string SpeculativeAddress = "SpeculateStep.Speculative";

        public bool? Act(bool? state, bool completed, ObjectTable table)
            => Act(state, completed, table.Get<ISpeculative>(SpeculativeAddress));
    }
}