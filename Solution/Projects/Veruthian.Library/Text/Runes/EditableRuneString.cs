using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Text.Runes
{

    public class EditableRuneString : IEditableText<Rune, RuneString>, IEditableText<Rune, EditableRuneString>, IEditableText<Rune, IEnumerable<Rune>>, IEnumerable<Rune>
    {
        RuneString value = RuneString.Empty;


        public EditableRuneString() { }

        public EditableRuneString(RuneString value) => this.value = value ?? RuneString.Empty;


        public Number Length => (int)value.Length;

        public RuneString Value => value;


        public void Append(Rune value) => this.value += value;

        public void Append(RuneString value) => this.value += value;

        public void Append(EditableRuneString value) => this.value += value.value;

        public void Append(IEnumerable<Rune> values) => this.value += new RuneString(values);



        public void Prepend(Rune value) => this.value = value + this.value;

        public void Prepend(RuneString value) => this.value = this.value + value;

        public void Prepend(EditableRuneString value) => Prepend(value.value);

        public void Prepend(IEnumerable<Rune> values) => this.value = new RuneString(values) + this.value;


        public void Insert(int position, Rune value) => this.value = RuneString.Insert(this.value, position, value);

        public void Insert(int position, RuneString value) => this.value = RuneString.Insert(this.value, position, value);

        public void Insert(int position, EditableRuneString value) => this.value = RuneString.Insert(this.value, position, value.value);

        public void Insert(int position, IEnumerable<Rune> values) => this.value = RuneString.Insert(this.value, position, new RuneString(values));


        public void Remove(int position, int amount) => this.value = RuneString.Remove(this.value, position, amount);


        public void Clear() => this.value = string.Empty;


        public static implicit operator RuneString(EditableRuneString value) => value.value;

        public static implicit operator EditableRuneString(RuneString value) => new EditableRuneString(value);


        public override string ToString() => value;

        public override int GetHashCode() => value.GetHashCode();

        public override bool Equals(object obj) => value.Equals(obj);

        IEnumerator<Rune> IEnumerable<Rune>.GetEnumerator() => ((IEnumerable<Rune>)value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<char>)value).GetEnumerator();
    }
}