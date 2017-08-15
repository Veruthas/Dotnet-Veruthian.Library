using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Data.Processors
{
    public class SequenceMatcher<TState> : IProcessor<TState>
    {
        IEnumerable<IProcessor<TState>> processors;
        
        public bool Process(TState state)
        {
            foreach(var processor in processors)
                if (!processor.Process(state))
                    return false;

            return true;
        }
    }
}