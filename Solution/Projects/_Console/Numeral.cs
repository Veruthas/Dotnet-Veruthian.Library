namespace Veruthian.Library.Numeric
{
    public struct Numeral
    {
        string numerals;

        int index;


        public Numeral(string numerals, int index)
        {
            this.numerals = numerals;

            this.index = index % numerals.Length;
        }

        public int Base => numerals.Length;

        public int Max => Base - 1;


        public static Numeral operator >(Numeral a, Numeral b) => a.index > b.index ? a : b;

        public static Numeral operator <(Numeral a, Numeral b) => a.index < b.index ? a : b;


        public static Numeral operator ~(Numeral v) => new Numeral(v.numerals, v.Max - v.index);


        public static Numeral operator ++(Numeral v) => new Numeral(v.numerals, v.index + 1);

        public static Numeral operator --(Numeral v) => new Numeral(v.numerals, v.Max + v.index);


        public override string ToString() => numerals[index].ToString();
    }
}