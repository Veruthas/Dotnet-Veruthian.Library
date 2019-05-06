using System;
using System.Text;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations.Analyzers
{
    public class RuleTable<TState>
    {
        public const string RuleClass = "rule";

        public const string SubMarker = ":";

        public DataLookup<string, ClassifiedOperation<TState>> Rules = new DataLookup<string, ClassifiedOperation<TState>>();

        public IOperation<TState> this[string name]
        {
            get => GetRule(name);
            set => GetRule(name).Operation = value;
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


        public bool Perform(string name, TState state, ITracer<TState> tracer = null)
            => GetRule(name).Perform(state, tracer);


        public string FormatRule(string name, int indentSize = 3, char indentChar = ' ')
        {
            var builder = new StringBuilder();

            builder.Append($"rule:{name} :=");

            if (!Rules.TryGet(name, out var rule) || rule.Operation == null)
            {
                builder.Append(" ()");
            }
            else
            {
                builder.AppendLine();                

                FormatOperation(builder, 1, indentSize, indentChar, rule.Operation);
            }

            return builder.ToString();
        }

        private void FormatOperation(StringBuilder builder, int indent, int indentSize, char indentChar, IOperation<TState> operation)
        {
            builder.Append(new string(indentChar, indent * indentSize));

            builder.Append(operation.Description);

            builder.AppendLine();

            if (operation.SubOperations.Count > 0 )
            {
                var possible = operation as ClassifiedOperation<TState>;

                if (possible == null || !possible.Classes.Contains(RuleClass))
                {
                    foreach (var subOperation in operation.SubOperations)
                    {
                        FormatOperation(builder, indent + 1, indentSize, indentChar, subOperation);
                    }
                }
            }
        }
    }
}