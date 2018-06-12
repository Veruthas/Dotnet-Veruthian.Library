namespace Veruthian.Dotnet.Library.Data
{
    public interface HasType<T>
    {
        void Get(out T value);
    }
    
    public class TypeSet<T0> : HasType<T0>
    {
        T0 t0;

        public TypeSet(T0 t0) => this.t0 = t0;

        public void Get(out T0 value) => value = t0;
    }

    public class TypeSet<T0, T1> : TypeSet<T0>, HasType<T1>
    {
        T1 t1;

        public TypeSet(T0 t0, T1 t1) : base(t0) => this.t1 = t1;

        public void Get(out T1 value) => value = t1;
    }

    public class TypeSet<T0, T1, T2> : TypeSet<T0, T1>, HasType<T2>
    {
        T2 t2;

        public TypeSet(T0 t0, T1 t1, T2 t2) : base(t0, t1) => this.t2 = t2;

        public void Get(out T2 value) => value = t2;
    }

    public class TypeSet<T0, T1, T2, T3> : TypeSet<T0, T1, T2>, HasType<T3>
    {
        T3 t3;

        public TypeSet(T0 t0, T1 t1, T2 t2, T3 t3) : base(t0, t1, t2) => this.t3 = t3;

        public void Get(out T3 value) => value = t3;
    }

    public class TypeSet<T0, T1, T2, T3, T4> : TypeSet<T0, T1, T2, T3>, HasType<T4>
    {
        T4 t4;

        public TypeSet(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4) : base(t0, t1, t2, t3) => this.t4 = t4;

        public void Get(out T4 value) => value = t4;
    }

    public class TypeSet<T0, T1, T2, T3, T4, T5> : TypeSet<T0, T1, T2, T3, T4>, HasType<T5>
    {
        T5 t5;

        public TypeSet(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) : base(t0, t1, t2, t3, t4) => this.t5 = t5;

        public void Get(out T5 value) => value = t5;
    }

    public class TypeSet<T0, T1, T2, T3, T4, T5, T6> : TypeSet<T0, T1, T2, T3, T4, T5>, HasType<T6>
    {
        T6 t6;

        public TypeSet(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) : base(t0, t1, t2, t3, t4, t5) => this.t6 = t6;

        public void Get(out T6 value) => value = t6;
    }

    public class TypeSet<T0, T1, T2, T3, T4, T5, T6, T7> : TypeSet<T0, T1, T2, T3, T4, T5, T6>, HasType<T7>
    {
        T7 t7;

        public TypeSet(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) : base(t0, t1, t2, t3, t4, t5, t6) => this.t7 = t7;

        public void Get(out T7 value) => value = t7;
    }

    public class TypeSet<T0, T1, T2, T3, T4, T5, T6, T7, T8> : TypeSet<T0, T1, T2, T3, T4, T5, T6, T7>, HasType<T8>
    {
        T8 t8;

        public TypeSet(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) : base(t0, t1, t2, t3, t4, t5, t6, t7) => this.t8 = t8;

        public void Get(out T8 value) => value = t8;
    }

    public class TypeSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : TypeSet<T0, T1, T2, T3, T4, T5, T6, T7, T8>, HasType<T9>
    {
        T9 t9;

        public TypeSet(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) : base(t0, t1, t2, t3, t4, t5, t6, t7, t8) => this.t9 = t9;

        public void Get(out T9 value) => value = t9;
    }

}