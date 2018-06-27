namespace Veruthian.Dotnet.Library.Numeric
{
    public struct Number : INumeric<Number>
    {
        readonly uint value;

        readonly uint[] values;


        #region Comparison

        public int CompareTo(Number other)
        {
            if (value < other.value)
            {
                return -1;
            }
            else if (value > other.value)
            {
                return 1;
            }
            else if (values == null)
            {
                if (other.values == null)
                    return 0;
                else
                    return -1;
            }
            else if (other.values == null)
            {
                return 1;
            }
            else if (values.Length < other.values.Length)
            {
                return -1;
            }
            else if (values.Length > other.values.Length)
            {
                return 1;
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                    if (values[i] < other.values[i])
                        return -1;
                    else if (values[i] > other.values[i])
                        return 1;
            }

            return 0;
        }

        public bool Equals(Number other)
        {
            if (value != other.value)
                return false;

            if (values != other.values)
            {
                if (values == null || other.values == null)
                    return false;

                if (values.Length != other.values.Length)
                    return false;

                for (int i = 0; i < values.Length; i++)
                    if (values[i] != other.values[i])
                        return false;
            }

            return true;
        }

        public bool Precedes(Number other) => CompareTo(other) < 0;

        public bool Follows(Number other) => CompareTo(other) > 0;

        


        #endregion


        public Number Add(Number value) => throw new System.NotImplementedException();

        public Number Subtract(Number value) => throw new System.NotImplementedException();

        public Number Delta(Number value) => throw new System.NotImplementedException();

        public Number Multiply(Number value) => throw new System.NotImplementedException();

        public Number Divide(Number value) => throw new System.NotImplementedException();

        public Number Divide(Number value, out Number remainder) => throw new System.NotImplementedException();

        public Number Modulus(Number value) => throw new System.NotImplementedException();
    }
}