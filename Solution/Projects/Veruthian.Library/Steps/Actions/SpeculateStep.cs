using Veruthian.Library.Collections;
using Veruthian.Library.Processing;

namespace Veruthian.Library.Steps.Actions
{
    public class SpeculateStep : BaseNestedStep, IActionStep<ISpeculator>, IActionStep<ObjectTable>
    {
        public SpeculateStep(IStep step) : base(step) { }

        
        public override string Description => "Speculate";
        

        public bool? Act(bool? state, bool completed, ISpeculator speculative)
        {
            if (!completed)
                speculative.Mark();
            else
                speculative.Rollback();

            return state;
        }


        public const string SpeculativeAddress = "SpeculateStep.Speculative";        

        public bool? Act(bool? state, bool completed, ObjectTable table)
            => Act(state, completed, table.Get<ISpeculator>(SpeculativeAddress));
    }
}