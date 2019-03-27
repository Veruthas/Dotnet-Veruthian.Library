namespace Veruthian.Library.Text.Chars
{
    public class EditableString : IEditableText<char, string>
    {
        string value = string.Empty;


        public EditableString() { }

        public EditableString(string value) => this.value = value ?? string.Empty;

        
        public void Append(char value) => this.value += value;

        public void Append(string values) => this.value += value;


        public void Prepend(char value) => this.value = value + this.value;

        public void Prepend(string values) => this.value = value + this.value;


        public void Insert(int position, char value) => this.value = this.value.Insert(position, value.ToString());

        public void Insert(int position, string values) => this.value = value + this.value.Insert(position, value);


        public void Remove(int position, int amount) => this.value = this.value.Remove(position, amount);


        public static implicit operator string(EditableString value) => value ?? string.Empty;

        public static implicit operator EditableString(string value) => new EditableString(value);


        public override string ToString() => value;

        public override int GetHashCode() => value.GetHashCode();

        public override bool Equals(object obj) => value.Equals(obj);        
    }
}
