using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps.Handlers
{
    public class StepHandlerList<TState> : StepHandler<TState, IStep>
    {
        IExpandableIndex<IStepHandler<TState>> handlers;


        public StepHandlerList()
            : this(DataList<IStepHandler<TState>>.New()) { }


        public StepHandlerList(params IStepHandler<TState>[] handlers)
            : this(DataList<IStepHandler<TState>>.From(handlers)) { }

        public StepHandlerList(IEnumerable<IStepHandler<TState>> handlers)
            : this(DataList<IStepHandler<TState>>.Extract(handlers)) { }


        public StepHandlerList(IExpandableIndex<IStepHandler<TState>> handlers)
            => this.handlers = handlers ?? DataList<IStepHandler<TState>>.New();


        public IExpandableIndex<IStepHandler<TState>> Handlers => handlers;


        public override bool? Handle(IStep step, TState state, IStepHandler<TState> root = null)
        {
            if (root == null)
                root = this;

            bool? result = OnStepStarted(step, state);

            if (result == null)
            {
                foreach (var handler in handlers)
                {
                    result = handler.Handle(step, state, root);

                    if (result != null)
                        break;
                }
            }

            OnStepCompleted(step, state, result);

            return result;
        }


        protected override bool? HandleStep(IStep step, TState state, IStepHandler<TState> root) => null;
    }
}