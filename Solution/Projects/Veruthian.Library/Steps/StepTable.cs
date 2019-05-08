using System.Text;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public class StepTable
    {
        DataLookup<string, LabeledStep> items = new DataLookup<string, LabeledStep>();

        string autoLabel;

        bool qualifyName;

        string qualifierSeparator;


        public StepTable() { }

        public StepTable(string autoLabel, bool qualifyName = false, string qualifierSeparator = ":")
        {
            this.autoLabel = autoLabel;

            this.qualifyName = qualifyName;

            this.qualifierSeparator = qualifierSeparator;
        }


        // Access
        public IStep this[string name]
        {
            get => Get(name);
            set => Get(name).Step = value;
        }

        public IStep this[string name, params string[] additional]
        {
            get => Get(name, additional);
            set => Get(name, additional).Step = value;
        }

        public ILookup<string, LabeledStep> Items => items;


        private LabeledStep Get(string name, string[] additional = null)
        {
            if (!items.TryGet(name, out var rule))
            {
                DataSet<string> labels = new DataSet<string>();

                if (autoLabel != null)
                    labels.Add(autoLabel);

                labels.Add(qualifyName ? $"{autoLabel}{qualifierSeparator}{name}" : name);

                rule = new LabeledStep(labels);

                items.Insert(name, rule);
            }

            if (additional != null)
            {
                foreach (var item in additional)
                    rule.Labels.Add(item);
            }

            return rule;
        }


        // Label
        public void Label(string name, string additional)
        {
            var rule = Get(name);

            rule.Labels.Add(additional);
        }

        public void Label(string name, params string[] additional)
        {
            var rule = Get(name);

            foreach (var item in additional)
                rule.Labels.Add(item);
        }



        // Formatting
        private const int IndentSize = 3;

        private const char IndentChar = ' ';


        public override string ToString() => ToString(IndentSize, IndentChar);

        public string ToString(int indentSize, char indentChar)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var name in items.Keys)
            {
                FormatItem(builder, name, indentSize, indentChar);

                builder.AppendLine();
            }

            return builder.ToString();
        }

        public string FormatItem(string name, int indentSize = IndentSize, char indentChar = IndentChar)
        {
            var builder = new StringBuilder();

            FormatItem(builder, name, indentSize, indentChar);

            return builder.ToString();
        }

        private void FormatItem(StringBuilder builder, string name, int indentSize = IndentSize, char indentChar = IndentChar)
        {
            builder.Append($"{(autoLabel != null && qualifyName ? $"{autoLabel}{qualifierSeparator}" : "")}{name} :=");

            if (!items.TryGet(name, out var rule) || rule.Step == null)
            {
                builder.Append(" ()");
            }
            else
            {
                builder.AppendLine();

                FormatStep(builder, 1, indentSize, indentChar, rule.Step);
            }
        }

        private void FormatStep(StringBuilder builder, int indent, int indentSize, char indentChar, IStep step)
        {
            builder.Append(new string(indentChar, indent * indentSize));

            builder.Append(step.Description);

            builder.AppendLine();

            if (step.SubSteps.Count > 0)
            {
                var labeledStep = step as LabeledStep;

                if (labeledStep == null || !items.Contains(labeledStep))
                {
                    foreach (var subStep in step.SubSteps)
                    {
                        FormatStep(builder, indent + 1, indentSize, indentChar, subStep);
                    }
                }
            }
        }
    }
}