namespace Veruthian.Library.Steps.Walkers
{
    public class ConditionalStepHandler<TState> : IStepHandler<TState>
    {
        public bool? Handle(IStep step, IStepWalker<TState> walker, TState state)
        {
            switch (step)
            {
                case ConditionalStep conditional:
                    return HandleConditionalStep(conditional, walker, state);
                default:
                    return null;
            }
        }

        protected virtual bool HandleConditionalStep(ConditionalStep conditional, IStepWalker<TState> walker, TState state)
        {
            var result = HandleSpeculation(conditional.Condition, walker, state);

            if (result == conditional.Expecting)
            {
                if (conditional.HasThenStep)
                    return walker.Walk(conditional.Then, state);
                else
                    return true;
            }
            else
            {
                if (conditional.HasElseStep)
                    return walker.Walk(conditional.Else, state);
                else
                    return false;
            }

        }

        protected virtual bool HandleSpeculation(IStep speculation, IStepWalker<TState> walker, TState state)
        {
            OnSpeculationStarted(speculation, walker, state);

            var result = walker.Walk(speculation, state);

            OnSpeculationCompleted(speculation, walker, state, result);

            return result;
        }


        protected virtual void OnSpeculationStarted(IStep speculation, IStepWalker<TState> walker, TState state) { }

        protected virtual void OnSpeculationCompleted(IStep speculation, IStepWalker<TState> walker, TState state, bool result) { }
    }
}