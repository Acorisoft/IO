using System;
using System.Collections.Generic;
using System.Text;

namespace Acorisoft.Patterns
{
    public interface IKeyValueProperty<T>
    {
        T Value { get; set; }
    }
}
