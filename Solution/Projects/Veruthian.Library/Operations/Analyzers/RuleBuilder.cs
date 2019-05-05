using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations.Analyzers
{
    public class RuleBuilder<TState> : OperationBuilder<TState>
    {
        public const string RuleClass = "rule";

        public const string SubMarker = ":";

        public DataLookup<string, ClassifiedOperation<TState>> Rules = new DataLookup<string, ClassifiedOperation<TState>>();

        public IOperation<TState> this[string rule]
        {
            get
            {
                return GetRule(rule);
            }
            set
            {
                GetRule(rule).Operation = value;
            }
        }

        private ClassifiedOperation<TState> GetRule(string name)
        {
            if (!Rules.TryGet(name, out var rule))
            {
                rule = Classify(RuleClass, RuleClass + SubMarker + name);

                Rules.Insert(name, rule);
            }

            return rule;
        }
    }
}