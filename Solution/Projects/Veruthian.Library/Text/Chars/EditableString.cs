using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Library.Text.Chars
{
    public class EditableString : IEditableText<char, string>, IEditableText<char, EditableString>, IEditableText<char, IEnumerable<char>>, IEnumerable<char>
    {
        string value = string.Empty;


        public EditableString() { }

        public EditableString(string value) => this.value = value ?? string.Empty;


        public int Length => value.Length;

        public string Value => value;


        public void Append(char value) => this.value += value;

        public void Append(string value) => this.value += value;

        public void Append(EditableString value) => this.value += value.value;

        public void Append(IEnumerable<char> values) => this.value += new string(values.ToArray());


        public void Prepend(char value) => this.value = value + this.value;

        public void Prepend(string value) => this.value = this.value + value;

        public void Prepend(EditableString value) => this.value = value.value + this.value;

        public void Prepend(IEnumerable<char> values) => this.value = new string(values.ToArray()) + this.value;


        public void Insert(int position, char value) => this.value = this.value.Insert(position, value.ToString());

        public void Insert(int position, string value) => this.value = this.value.Insert(position, value);

        public void Insert(int position, EditableString value) => this.value = this.value.Insert(position, value.value);

        public void Insert(int position, IEnumerable<char> values) => this.value = this.value.Insert(position, new string(values.ToArray()));


        public void Remove(int position, int amount) => this.value = this.value.Remove(position, amount);


        public void Clear() => this.value = string.Empty;


        public static implicit operator string(EditableString value) => value.value;

        public static implicit operator EditableString(string value) => new EditableString(value);


        public override string ToString() => value;

        public override int GetHashCode() => value.GetHashCode();

        public override bool Equals(object obj) => value.Equals(obj);

        IEnumerator<char> IEnumerable<char>.GetEnumerator() => ((IEnumerable<char>)value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<char>)value).GetEnumerator();            
    }
}
