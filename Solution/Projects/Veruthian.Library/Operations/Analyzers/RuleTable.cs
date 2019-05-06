using System;
using System.Text;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations.Analyzers
{
    public class RuleTable<TState>
    {
        DataLookup<string, ClassifiedOperation<TState>> rules = new DataLookup<string, ClassifiedOperation<TState>>();

        bool qualifyName;

        string classSeparator;


        public RuleTable(bool qualifyName = false, string classSeparator = ":")
        {
            this.qualifyName = qualifyName;

            this.classSeparator = classSeparator;
        }


        // Access
        public IOperation<TState> this[string name]
        {
            get => GetRule(name);
            set => GetRule(name).Operation = value;
        }

        public IOperation<TState> this[string name, params string[] additional]
        {
            get => GetRule(name, additional);
            set => GetRule(name, additional).Operation = value;
        }

        public ILookup<string, ClassifiedOperation<TState>> Rules => rules;


        private ClassifiedOperation<TState> GetRule(string name, string[] additional = null)
        {
            if (!rules.TryGet(name, out var rule))
            {
                DataSet<string> classSet = new DataSet<string>();

                classSet.Add(AnalyzerClasses.Rule);

                classSet.Add(qualifyName ? AnalyzerClasses.Rule + classSeparator + name : name);

                rule = new ClassifiedOperation<TState>(classSet);

                rules.Insert(name, rule);
            }

            if (additional != null)
            {
                foreach (var item in additional)
                    rule.Classes.Add(item);
            }

            return rule;
        }


        // Classify
        public void Classify(string name, string additional)
        {
            var rule = GetRule(name);

            rule.Classes.Add(additional);
        }

        public void Classify(string name, params string[] additional)
        {
            var rule = GetRule(name);

            foreach (var item in additional)
                rule.Classes.Add(item);
        }


        // Perform
        public bool Perform(string name, TState state, ITracer<TState> tracer = null)
            => GetRule(name).Perform(state, tracer);


        // Formatting
        private const int IndentSize = 3;
        private const char IndentChar = ' ';


        public override string ToString() => ToString(IndentSize, IndentChar);

        public string ToString(int indentSize, char indentChar)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var name in rules.Keys)
            {
                FormatRule(builder, name, indentSize, indentChar);

                builder.AppendLine();
            }

            return builder.ToString();
        }

        public string FormatRule(string name, int indentSize = IndentSize, char indentChar = IndentChar)
        {
            var builder = new StringBuilder();

            FormatRule(builder, name, indentSize, indentChar);

            return builder.ToString();
        }

        private void FormatRule(StringBuilder builder, string name, int indentSize = IndentSize, char indentChar = IndentChar)
        {
            builder.Append($"rule:{name} :=");

            if (!rules.TryGet(name, out var rule) || rule.Operation == null)
            {
                builder.Append(" ()");
            }
            else
            {
                builder.AppendLine();

                FormatOperation(builder, 1, indentSize, indentChar, rule.Operation);
            }
        }

        private void FormatOperation(StringBuilder builder, int indent, int indentSize, char indentChar, IOperation<TState> operation)
        {
            builder.Append(new string(indentChar, indent * indentSize));

            builder.Append(operation.Description);

            builder.AppendLine();

            if (operation.SubOperations.Count > 0)
            {
                var possible = operation as ClassifiedOperation<TState>;

                if (possible == null || !possible.Classes.Contains(AnalyzerClasses.Rule))
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