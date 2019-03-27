using System.Collections.Generic;

namespace Veruthian.Library.Text
{
    public interface IEditableText<in U, in S>
        where S : IEnumerable<U>
    {
        void Append(U value);

        void Append(S values);


        void Prepend(U value);

        void Prepend(S values);


        void Insert(int position, U value);

        void Insert(int position, S values);


        void Remove(int position, int amount);
    }
}