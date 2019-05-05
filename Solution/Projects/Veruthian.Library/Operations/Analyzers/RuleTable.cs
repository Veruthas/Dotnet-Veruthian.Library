using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations.Analyzers
{
    public class RuleTable<TState>
    {
        public const string RuleClass = "rule";

        public const string SubMarker = ":";

        public DataLookup<string, ClassifiedOperation<TState>> Rules = new DataLookup<string, ClassifiedOperation<TState>>();

        public IOperation<TState> this[string rule]
        {
            get => GetRule(rule);
            set => GetRule(rule).Operation = value;
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


        private ClassifiedOperation<TState> Classify(params string[] classes)
            => new ClassifiedOperation<TState>(new DataSet<string>(classes));
    }
}