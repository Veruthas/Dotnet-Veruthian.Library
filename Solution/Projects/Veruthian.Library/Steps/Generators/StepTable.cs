using System;
using System.Text;
using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Steps.Generators
{
    public delegate void Labeler(LabeledStep step, string[] additional = null);

    public class StepTable
    {
        DataLookup<string, LabeledStep> items;

        DataLookup<string, LabeledStep> subItems;

        Labeler labeler;

        Labeler subLabler;


        // Constructors
        public StepTable()
            : this((Labeler)null) { }

        public StepTable(params string[] defaultLabels)
            : this(WithDefaultLabels(false, defaultLabels)) { }

        public StepTable(bool labelWithName, params string[] defaultLabels)
            : this(WithDefaultLabels(labelWithName, defaultLabels)) { }

        public StepTable(Labeler labeler)
            : this(null, labeler) { }

        public StepTable(StepTable parent, params string[] defaultLabels)
            : this(parent, WithDefaultLabels(false, defaultLabels)) { }

        public StepTable(StepTable parent, bool labelWithName, params string[] defaultLabels)
            : this(parent, WithDefaultLabels(labelWithName, defaultLabels)) { }

        public StepTable(StepTable parent, Labeler labeler)
        {
            if (parent == null)
            {
                this.items = new DataLookup<string, LabeledStep>();

                this.labeler = labeler ?? DefaultLabeler;
            }
            else
            {
                this.subLabler = labeler ?? DefaultLabeler;

                this.subItems = new DataLookup<string, LabeledStep>();

                this.items = parent.items;

                this.labeler = labeler == null ? parent.labeler : ChainedLabeler(parent.labeler, labeler);
            }
        }


        // SubTables
        public StepTable CreateSubTable(bool labelWithName) => new StepTable(this, labelWithName);

        public StepTable CreateSubTable(params string[] defaultLabels) => new StepTable(this, defaultLabels);

        public StepTable CreateSubTable(bool labelWithName, params string[] defaultLabels) => new StepTable(this, labelWithName, defaultLabels);

        public StepTable CreateSubTable(Labeler labeler) => new StepTable(this, labeler);


        // Labelers
        private static readonly Labeler DefaultLabeler = (step, additional) => { if (additional != null) step.Labels.AddRange(additional); };

        private static Labeler ChainedLabeler(Labeler labeler, Labeler sublabeler) => (step, additional) => { labeler(step, additional); sublabeler(step, additional); };

        private static Labeler WithDefaultLabels(bool labelWithName, params string[] defaultLabels)
        {
            if (labelWithName)
            {
                if (defaultLabels == null || defaultLabels.Length == 0)
                {
                    return (step, additional) =>
                    {
                        if (labelWithName)
                            step.Labels.Add(step.Name);

                        if (additional != null)
                            step.Labels.AddRange(additional);
                    };
                }
                else
                {
                    return (step, additional) =>
                    {
                        if (labelWithName)
                            step.Labels.Add(step.Name);

                        if (defaultLabels != null)
                            step.Labels.AddRange(defaultLabels);

                        if (additional != null)
                            step.Labels.AddRange(additional);
                    };
                }
            }
            else
            {
                if (defaultLabels == null || defaultLabels.Length == 0)
                {
                    return DefaultLabeler;
                }
                else
                {
                    return (step, additional) =>
                    {
                        if (defaultLabels != null)
                            step.Labels.AddRange(defaultLabels);

                        if (additional != null)
                            step.Labels.AddRange(additional);
                    };
                }
            }
        }


        // Access
        public IStep this[string name]
        {
            get => GetItem(name);
            set => GetItem(name).Step = value;
        }

        public IStep this[string name, params string[] additionalLabels]
        {
            get => GetItem(name, additionalLabels);
            set => GetItem(name, additionalLabels).Step = value;
        }

        private LabeledStep GetItem(string name, string[] additionalLabels = null)
        {
            if (!items.TryGet(name, out var item))
            {
                item = new LabeledStep(name, new DataSet<string>());

                labeler(item, additionalLabels);

                items.Insert(name, item);
            }

            if (subItems != null && !subItems.IsValidAddress(name))
            {
                subItems.Insert(name, item);

                labeler(item, additionalLabels);
            }

            if (additionalLabels != null)
            {
                foreach (var label in additionalLabels)
                    item.Labels.Add(label);
            }

            return item;
        }


        public FixedLookup<string, LabeledStep> Items => new FixedLookup<string, LabeledStep>(items);


        // Label
        public void Label(string name, string additional)
        {
            var rule = GetItem(name);

            rule.Labels.Add(additional);
        }

        public void Label(string name, params string[] additional)
        {
            var rule = GetItem(name);

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

            foreach (var name in items.Addresses)
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
            builder.Append($"{name}");

            if (!items.TryGet(name, out var item))
            {
                builder.AppendLine(";");
            }
            else
            {
                if (item.Labels != null)
                {
                    builder.Append(item.Labels.ToListString("<", ">", ", "));
                }

                builder.AppendLine(" => ");

                if (item.Step != null)
                {
                    FormatStep(builder, 1, indentSize, indentChar, item.Step);
                }
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