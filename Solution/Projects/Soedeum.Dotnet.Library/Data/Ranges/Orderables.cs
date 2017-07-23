using System;

namespace Soedeum.Dotnet.Library.Data.Ranges
{
    public struct OrderableByte : IOrderable<OrderableByte>
    {
        sbyte value;

        public OrderableByte(sbyte value)
        {
            this.value = value;
        }

        public static implicit operator sbyte(OrderableByte value) => value.value;

        public static implicit operator OrderableByte(sbyte value) => new OrderableByte(value);

        public OrderableByte Default => default(sbyte);

        public OrderableByte MinValue => sbyte.MinValue;

        public OrderableByte MaxValue => sbyte.MaxValue;


        public int CompareTo(OrderableByte other) => this.value.CompareTo(other.value);

        public bool Equals(OrderableByte other) => this.value.Equals(other.value);


        public OrderableByte Next() => new OrderableByte((sbyte)(this.value + 1));

        public OrderableByte Previous() => new OrderableByte((sbyte)(this.value - 1));
    }

    public struct OrderableShort : IOrderable<OrderableShort>
    {
        short value;

        public OrderableShort(short value)
        {
            this.value = value;
        }

        public static implicit operator short(OrderableShort value) => value.value;

        public static implicit operator OrderableShort(short value) => new OrderableShort(value);

        public OrderableShort Default => default(short);

        public OrderableShort MinValue => short.MinValue;

        public OrderableShort MaxValue => short.MaxValue;


        public int CompareTo(OrderableShort other) => this.value.CompareTo(other.value);

        public bool Equals(OrderableShort other) => this.value.Equals(other.value);


        public OrderableShort Next() => new OrderableShort((short)(this.value + 1));

        public OrderableShort Previous() => new OrderableShort((short)(this.value - 1));
    }

    public struct OrderableInt : IOrderable<OrderableInt>
    {
        int value;

        public OrderableInt(int value)
        {
            this.value = value;
        }

        public static implicit operator int(OrderableInt value) => value.value;

        public static implicit operator OrderableInt(int value) => new OrderableInt(value);

        public OrderableInt Default => default(int);

        public OrderableInt MinValue => int.MinValue;

        public OrderableInt MaxValue => int.MaxValue;


        public int CompareTo(OrderableInt other) => this.value.CompareTo(other.value);

        public bool Equals(OrderableInt other) => this.value.Equals(other.value);


        public OrderableInt Next() => new OrderableInt((int)(this.value + 1));

        public OrderableInt Previous() => new OrderableInt((int)(this.value - 1));
    }

    public struct OrderableLong : IOrderable<OrderableLong>
    {
        long value;

        public OrderableLong(long value)
        {
            this.value = value;
        }

        public static implicit operator long(OrderableLong value) => value.value;

        public static implicit operator OrderableLong(long value) => new OrderableLong(value);

        public OrderableLong Default => default(long);

        public OrderableLong MinValue => long.MinValue;

        public OrderableLong MaxValue => long.MaxValue;


        public int CompareTo(OrderableLong other) => this.value.CompareTo(other.value);

        public bool Equals(OrderableLong other) => this.value.Equals(other.value);


        public OrderableLong Next() => new OrderableLong((long)(this.value + 1));

        public OrderableLong Previous() => new OrderableLong((long)(this.value - 1));
    }

    public struct OrderableUByte : IOrderable<OrderableUByte>
    {
        byte value;

        public OrderableUByte(byte value)
        {
            this.value = value;
        }

        public static implicit operator byte(OrderableUByte value) => value.value;

        public static implicit operator OrderableUByte(byte value) => new OrderableUByte(value);

        public OrderableUByte Default => default(byte);

        public OrderableUByte MinValue => byte.MinValue;

        public OrderableUByte MaxValue => byte.MaxValue;


        public int CompareTo(OrderableUByte other) => this.value.CompareTo(other.value);

        public bool Equals(OrderableUByte other) => this.value.Equals(other.value);


        public OrderableUByte Next() => new OrderableUByte((byte)(this.value + 1));

        public OrderableUByte Previous() => new OrderableUByte((byte)(this.value - 1));
    }

    public struct OrderableUShort : IOrderable<OrderableUShort>
    {
        ushort value;

        public OrderableUShort(ushort value)
        {
            this.value = value;
        }

        public static implicit operator ushort(OrderableUShort value) => value.value;

        public static implicit operator OrderableUShort(ushort value) => new OrderableUShort(value);

        public OrderableUShort Default => default(ushort);

        public OrderableUShort MinValue => ushort.MinValue;

        public OrderableUShort MaxValue => ushort.MaxValue;


        public int CompareTo(OrderableUShort other) => this.value.CompareTo(other.value);

        public bool Equals(OrderableUShort other) => this.value.Equals(other.value);


        public OrderableUShort Next() => new OrderableUShort((ushort)(this.value + 1));

        public OrderableUShort Previous() => new OrderableUShort((ushort)(this.value - 1));
    }

    public struct OrderableUInt : IOrderable<OrderableUInt>
    {
        uint value;

        public OrderableUInt(uint value)
        {
            this.value = value;
        }

        public static implicit operator uint(OrderableUInt value) => value.value;

        public static implicit operator OrderableUInt(uint value) => new OrderableUInt(value);

        public OrderableUInt Default => default(uint);

        public OrderableUInt MinValue => uint.MinValue;

        public OrderableUInt MaxValue => uint.MaxValue;


        public int CompareTo(OrderableUInt other) => this.value.CompareTo(other.value);

        public bool Equals(OrderableUInt other) => this.value.Equals(other.value);


        public OrderableUInt Next() => new OrderableUInt((uint)(this.value + 1));

        public OrderableUInt Previous() => new OrderableUInt((uint)(this.value - 1));
    }

    public struct OrderableULong : IOrderable<OrderableULong>
    {
        ulong value;

        public OrderableULong(ulong value)
        {
            this.value = value;
        }

        public static implicit operator ulong(OrderableULong value) => value.value;

        public static implicit operator OrderableULong(ulong value) => new OrderableULong(value);

        public OrderableULong Default => default(ulong);

        public OrderableULong MinValue => ulong.MinValue;

        public OrderableULong MaxValue => ulong.MaxValue;


        public int CompareTo(OrderableULong other) => this.value.CompareTo(other.value);

        public bool Equals(OrderableULong other) => this.value.Equals(other.value);


        public OrderableULong Next() => new OrderableULong((ulong)(this.value + 1));

        public OrderableULong Previous() => new OrderableULong((ulong)(this.value - 1));
    }

    public struct OrderableChar : IOrderable<OrderableChar>
    {
        char value;

        public OrderableChar(char value)
        {
            this.value = value;
        }

        public static implicit operator char(OrderableChar value) => value.value;

        public static implicit operator OrderableChar(char value) => new OrderableChar(value);

        public OrderableChar Default => default(char);

        public OrderableChar MinValue => char.MinValue;

        public OrderableChar MaxValue => char.MaxValue;


        public int CompareTo(OrderableChar other) => this.value.CompareTo(other.value);

        public bool Equals(OrderableChar other) => this.value.Equals(other.value);


        public OrderableChar Next() => new OrderableChar((char)(this.value + 1));

        public OrderableChar Previous() => new OrderableChar((char)(this.value - 1));
    }
}