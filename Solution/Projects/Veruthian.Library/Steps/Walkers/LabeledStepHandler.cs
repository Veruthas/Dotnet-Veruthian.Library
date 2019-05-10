namespace Veruthian.Library.Steps.Walkers
{
    public class LabeledStepHandler<TState> : IStepHandler<TState>
    {
        public bool? Handle(IStep step, IStepWalker<TState> walker, TState state)
        {
            switch(step)
            {
                case LabeledStep labeled:
                    return HandleLabeledStep(labeled, walker, state);

                default:
                    return null;
            }
        }


        protected virtual bool HandleLabeledStep(LabeledStep labeled, IStepWalker<TState> walker, TState state)
        {
            return walker.Walk(labeled.Step, state);
        }
    }
}