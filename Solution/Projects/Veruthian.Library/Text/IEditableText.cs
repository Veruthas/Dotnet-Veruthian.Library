using System.Collections.Generic;

namespace Veruthian.Library.Text
{
    public interface IEditableText<in U>
    {
        void Append(U value);

        void Prepend(U value);

        void Insert(int position, U value);

        void Remove(int position, int amount);


        void Clear();
    }

    public interface IEditableText<in U, in S> : IEditableText<U>
        where S : IEnumerable<U>
    {
        void Append(S values);

        void Prepend(S values);

        void Insert(int position, S values);
    }
}