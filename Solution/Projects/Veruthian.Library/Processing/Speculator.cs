using System.Collections.Generic;

namespace Veruthian.Library.Processing
{
    public class Speculator : ISpeculator
    {
        IEnumerable<ISpeculator> speculators;

        int count;


        public Speculator(IEnumerable<ISpeculator> speculators)
            => this.speculators = speculators;

        public Speculator(params ISpeculator[] speculators)
            => this.speculators = speculators;


        public bool IsSpeculating => count > 0;


        public void Mark()
        {
            if (speculators != null)
                foreach (var speculator in speculators)
                    speculator.Mark();

            count++;
        }

        public void Commit()
        {
            if (speculators != null)
                foreach (var speculator in speculators)
                    speculator.Commit();

            count--;
        }

        public void Rollback()
        {
            if (speculators != null)
                foreach (var speculator in speculators)
                    speculator.Rollback();

            count--;
        }
    }
}