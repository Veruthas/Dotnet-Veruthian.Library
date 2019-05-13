using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public class SequentialStep : Step
    {
        IVector<IStep> sequence;


        public SequentialStep() : this((IVector<IStep>)null) { }

        public SequentialStep(IVector<IStep> sequence) => Sequence = sequence;

        public SequentialStep(IEnumerable<IStep> sequence) => Sequence = DataVector<IStep>.Extract(sequence);

        public SequentialStep(params IStep[] sequence) => Sequence = DataVector<IStep>.From(sequence);


        public IVector<IStep> Sequence
        {
            get => this.sequence;
            set => this.sequence = value ?? new DataVector<IStep>();
        }

        public override string Description => "seqeunce";

        protected override int SubStepCount => sequence == null ? 0 : sequence.Count;

        protected override IStep GetSubStep(int verifiedAddress) => sequence[verifiedAddress];
    }
}