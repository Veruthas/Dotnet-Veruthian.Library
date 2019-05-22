using System;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Text.Runes
{
    public class RuneBuffer : BaseResizableVector<Rune, RuneBuffer>, IEditableText<Rune>
    {
        protected override RuneBuffer This => this;

        void IEditableText<Rune>.Append(Rune value) => Append(value);

        void IEditableText<Rune>.Clear() => Clear();

        void IEditableText<Rune>.Insert(int position, Rune value) => Insert(position, value);

        void IEditableText<Rune>.Prepend(Rune value) => Prepend(value);

        void IEditableText<Rune>.Remove(int position, int amount) => RemoveBy(position, amount);
    }
}