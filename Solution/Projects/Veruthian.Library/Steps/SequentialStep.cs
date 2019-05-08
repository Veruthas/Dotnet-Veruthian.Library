using System.Collections.Generic;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public class SequentialStep : BaseStep
    {
        IIndex<IStep> sequence;


        public SequentialStep() : this((IIndex<IStep>)null) { }

        public SequentialStep(IIndex<IStep> sequence) => Sequence = sequence;

        public SequentialStep(IEnumerable<IStep> sequence) => Sequence = DataIndex<IStep>.Extract(sequence);

        public SequentialStep(params IStep[] sequence) => Sequence = DataIndex<IStep>.From(sequence);


        public IIndex<IStep> Sequence
        {
            get => sequence;
            set => this.sequence = value ?? new DataIndex<IStep>();
        }

        public override string Description => "seqeunce";

        protected override int SubStepCount => sequence == null ? 0 : sequence.Count;

        protected override IStep GetSubStep(int verifiedIndex) => sequence[verifiedIndex];
    }
}