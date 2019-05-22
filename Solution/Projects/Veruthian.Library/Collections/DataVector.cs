using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Collections
{
    public class DataVector<T> : BaseMutableVector<T, DataVector<T>>
    {
        protected override DataVector<T> This => this;

        public static DataVector<T> Default { get; } = new DataVector<T>();
    }
}