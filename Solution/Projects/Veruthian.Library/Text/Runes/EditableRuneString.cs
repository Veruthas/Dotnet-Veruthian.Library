using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Library.Text.Runes
{

    public class EditableRuneString : IEditableText<Rune, RuneString>, IEditableText<Rune, EditableRuneString>, IEnumerable<Rune>
    {
        RuneString value = RuneString.Empty;


        public EditableRuneString() { }

        public EditableRuneString(RuneString value) => this.value = value ?? RuneString.Empty;


        public int Length => value.Length;

        public string Value => value;


        public void Append(Rune value) => this.value += value;

        public void Append(RuneString value) => this.value += value;

        public void Append(EditableRuneString value) => Append(value.value);


        public void Prepend(Rune value) => this.value = value + this.value;

        public void Prepend(RuneString value) => this.value = this.value + value;

        public void Prepend(EditableRuneString value) => Prepend(value.value);


        public void Insert(int position, Rune value) => this.value = RuneString.Insert(this.value, position, value);

        public void Insert(int position, RuneString value) => this.value = RuneString.Insert(this.value, position, value);

        public void Insert(int position, EditableRuneString value) => Insert(position, value.value);


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