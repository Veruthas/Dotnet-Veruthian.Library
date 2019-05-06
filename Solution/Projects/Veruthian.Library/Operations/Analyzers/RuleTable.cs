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
            this.classSeparator = classSeparator;
        }

        public IOperation<TState> this[string name]
        {
            get => GetRule(name);
            set => GetRule(name).Operation = value;
        }

        public ILookup<string, ClassifiedOperation<TState>> Rules => rules;

        private ClassifiedOperation<TState> GetRule(string name)
        {
            if (!rules.TryGet(name, out var rule))
            {
                DataSet<string> classSet = new DataSet<string>();

                classSet.Add(AnalyzerClasses.Rule);

                classSet.Add(qualifyName ? AnalyzerClasses.Rule + classSeparator + name : name);

                rule = new ClassifiedOperation<TState>(classSet);

                rules.Insert(name, rule);
            }

            return rule;
        }


        public void Classify(string name, string additional)
        {
            var rule = GetRule(name);

            AddClass(rule, name, additional, false);
        }

        public void Classify(string name, params string[] additional)
        {
            var rule = GetRule(name);

            foreach (var item in additional)
                AddClass(rule, name, item, false);
        }

        public void ClassifyQualified(string name, string additional)
        {
            var rule = GetRule(name);

            AddClass(rule, name, additional, true);
        }

        public void ClassifyQualified(string name, params string[] additional)
        {
            var rule = GetRule(name);

            foreach (var item in additional)
                AddClass(rule, name, item, true);
        }

        private void AddClass(ClassifiedOperation<TState> rule, string name, string additional, bool qualify)
        {
            var newClass = GetClassName(name, additional, qualify);

            rule.Classes.Add(newClass);
        }

        private string GetClassName(string name, string additional, bool qualify)
        {
            var result = additional;

            if (qualify)
                result = name + classSeparator + result;

            if (qualifyName)
                result = AnalyzerClasses.Rule + classSeparator + result;

            return result;
        }


        public bool Perform(string name, TState state, ITracer<TState> tracer = null)
            => GetRule(name).Perform(state, tracer);


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