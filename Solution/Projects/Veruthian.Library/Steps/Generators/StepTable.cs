using System;
using System.Text;
using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Steps.Generators
{
    public delegate IExpandableContainer<string> LabelContainerGenerator(string name, string[] additionalLabels = null);

    public delegate IExpandableContainer<string> LabelContainerModifier(string name, IExpandableContainer<string> container);


    public class StepTable
    {
        IExpandableLookup<string, LabeledStep> items;

        IExpandableLookup<string, LabeledStep> subItems;

        LabelContainerGenerator generator;

        LabelContainerModifier modifier;


        // Constructors
        public StepTable()
            : this(null, DefaultGenerator) { }

        public StepTable(params string[] defaultLabels)
            : this(null, GetGenerator(null, defaultLabels)) { }

        public StepTable(Func<string, string> nameProcessor)
            : this(null, GetGenerator(nameProcessor, null)) { }

        public StepTable(Func<string, string> nameProcessor, params string[] defaultLabels)
            : this(null, GetGenerator(nameProcessor, defaultLabels)) { }

        public StepTable(LabelContainerGenerator generator)
            : this(null, generator) { }

        public StepTable(IExpandableLookup<string, LabeledStep> items, params string[] defaultLabels)
            : this(items, GetGenerator(null, defaultLabels)) { }

        public StepTable(IExpandableLookup<string, LabeledStep> items, Func<string, string> nameProcessor)
            : this(items, GetGenerator(nameProcessor, null)) { }

        public StepTable(IExpandableLookup<string, LabeledStep> items, Func<string, string> nameProcessor, params string[] defaultLabels)
            : this(items, GetGenerator(nameProcessor, defaultLabels)) { }

        public StepTable(IExpandableLookup<string, LabeledStep> items, LabelContainerGenerator generator)
        {
            this.items = items ?? new DataLookup<string, LabeledStep>();

            this.generator = generator ?? DefaultGenerator;
        }


        // SubTable
        public StepTable CreateSubTable(params string[] additionalDefault) 
            => CreateSubTable((name, container) =>
                             {
                                 if (additionalDefault != null)
                                     foreach (var label in additionalDefault)
                                         container.Add(label);
 
                                 return container;
                             });

        public StepTable CreateSubTable(LabelContainerModifier modifier)
        {
            var subTable = new StepTable(items, generator);

            subTable.subItems = new DataLookup<string, LabeledStep>();

            // If this is a new subtable
            if (this.modifier == null)
            {
                if (modifier == null)
                    subTable.modifier = (name, container) => container;
                else
                    subTable.modifier = modifier;
            }
            // If this is a nested subtable        
            else
            {
                if (modifier == null)
                    subTable.modifier = this.modifier;
                else
                    subTable.modifier = (name, container) => { modifier(name, container); this.modifier(name, container); return container; };
            }

            return subTable;
        }


        // Generators
        static LabelContainerGenerator DefaultGenerator { get; } = GetGenerator(null, null);

        static LabelContainerGenerator GetGenerator(Func<string, string> nameProcessor, string[] defaultLabels)
        {
            return ((name, additional) =>
            {
                var labels = new DataSet<string>();

                name = nameProcessor != null ? nameProcessor(name) : name;

                if (name != null)
                    labels.Add(name);

                if (defaultLabels != null)
                    foreach (var label in defaultLabels)
                        labels.Add(label);

                if (additional != null)
                    foreach (var label in additional)
                        labels.Add(label);

                return labels;
            });
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
                var labels = generator(name, additionalLabels);

                item = new LabeledStep(labels);

                items.Insert(name, item);
            }

            if (subItems != null && !subItems.HasAddress(name))
            {
                subItems.Insert(name, item);

                modifier(name, item.Labels);
            }

            if (additionalLabels != null)
            {
                foreach (var label in additionalLabels)
                    item.Labels.Add(label);
            }

            return item;
        }


        public ILookup<string, LabeledStep> Items => items;


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