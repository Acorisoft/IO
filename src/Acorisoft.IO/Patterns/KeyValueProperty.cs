using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acorisoft.Patterns
{
    public class KeyValueProperty<T> : IKeyValueProperty<T>
    {
        private readonly string _propertyName;
        private readonly KeyValuePattern _pattern;

        internal KeyValueProperty(KeyValuePattern owner,string propertyName)
        {
            _propertyName = propertyName;
            _pattern = owner;
        }

        public void RaiseUpdate()
        {
            _pattern.UpdateValue(_propertyName);
        }

        protected virtual bool OnValueChanged(T newValue)
        {
            return true;
        }

        public T Value
        {
            get
            {
                return _pattern.GetValue<T>(_propertyName);
            }
            set
            {
                if (OnValueChanged(value))
                {
                    _pattern.SetValue(_propertyName, value);
                }
            }
        }
    }
}
