using System;
using System.Text;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Operations.Analyzers
{
    public class RuleTable<TState>
    {
        DataLookup<string, ClassifiedOperation<TState>> rules = new DataLookup<string, ClassifiedOperation<TState>>();

        bool qualifyName;

        bool qualifyClasses;

        string classSeparator;


        public RuleTable(bool qualifyName = false, bool qualifyClasses = false, string classSeparator = ":")
        {
            this.qualifyClasses = qualifyClasses;

            this.classSeparator = classSeparator;
        }

        public IOperation<TState> this[string name]
        {
            get => GetRule(name, null);
            set => GetRule(name, null).Operation = value;
        }

        public IOperation<TState> this[string name, params string[] classes]
        {
            get => GetRule(name, classes);
            set => GetRule(name, classes).Operation = value;
        }

        public ILookup<string, ClassifiedOperation<TState>> Rules => rules;

        private ClassifiedOperation<TState> GetRule(string name, string[] classes)
        {
            if (!rules.TryGet(name, out var rule))
            {
                DataSet<string> classSet = new DataSet<string>();

                classSet.Add(AnalyzerClasses.Rule);

                classSet.Add(GetClass(name));

                foreach (var item in classes)
                    classSet.Add(GetClass(name, item));

                rule = new ClassifiedOperation<TState>();

                rules.Insert(name, rule);
            }

            return rule;
        }

        private string GetClass(string name, string item = null)
        {
            if (item == null)
                return qualifyName ? AnalyzerClasses.Rule + classSeparator + name : name;
            else
                return qualifyClasses ? AnalyzerClasses.Rule + classSeparator + name + classSeparator + item : item;
        }

        public bool Perform(string name, TState state, ITracer<TState> tracer = null)
            => GetRule(name, null).Perform(state, tracer);


        public string FormatRule(string name, int indentSize = 3, char indentChar = ' ')
        {
            var builder = new StringBuilder();

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

            return builder.ToString();
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