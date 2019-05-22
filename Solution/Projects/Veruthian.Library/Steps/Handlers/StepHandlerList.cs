using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps.Handlers
{
    public class StepHandlerList : StepHandler< IStep>
    {
        IResizableVector<IStepHandler> handlers;


        public StepHandlerList()
            : this(DataList<IStepHandler>.New()) { }


        public StepHandlerList(params IStepHandler[] handlers)
            : this(DataList<IStepHandler>.From(handlers)) { }

        public StepHandlerList(IEnumerable<IStepHandler> handlers)
            : this(DataList<IStepHandler>.Extract(handlers)) { }


        public StepHandlerList(IResizableVector<IStepHandler> handlers)
            => this.handlers = handlers ?? DataList<IStepHandler>.New();


        public IResizableVector<IStepHandler> Handlers => handlers;


        public override bool? Handle(IStep step, StateTable state, IStepHandler root = null)
        {
            if (root == null)
                root = this;

            bool? result = OnStepStarted(step, state);

            bool handled = result != null;

            if (!handled)
            {
                foreach (var handler in handlers)
                {
                    result = handler.Handle(step, state, root);

                    if (result != null)
                        break;
                }                         
            }

            OnStepCompleted(step, state, result, handled);


            return result;
        }


        protected override bool? HandleStep(IStep step, StateTable state, IStepHandler root) => null;
    }
}