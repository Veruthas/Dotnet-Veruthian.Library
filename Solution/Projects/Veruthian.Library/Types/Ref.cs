namespace Veruthian.Library.Types
{
    public class Ref<T>
    {
        public Ref() { }

        public Ref(T value) => this.Value = value;


        public T Value { get; set; }


        public static implicit operator Ref<T>(T value) => new Ref<T>(value);

        public static implicit operator T(Ref<T> value) => value.Value;
        

        public override string ToString() => Value.ToString();
    }
}