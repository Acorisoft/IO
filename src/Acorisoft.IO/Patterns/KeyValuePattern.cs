using Acorisoft.Patterns.LiteDB;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acorisoft.Patterns
{
    public abstract class KeyValuePattern : Pattern, IKeyValuePattern
    {
        protected KeyValuePattern(ISingletonPattern pattern, string targetDocumentName) : base(pattern.Database, pattern.CollectionName)
        {
            MaintainDocumentName = targetDocumentName;
            OnInitialize();
        }

        protected KeyValuePattern(ISingletonPattern pattern) : base(pattern.Database, pattern.CollectionName)
        {
            MaintainDocumentName = GetType().FullName;
            OnInitialize();
        }

        protected void OnInitialize()
        {
            if (!Collection.Exists(Query.EQ("_id", MaintainDocumentName)))
            {
                MaintainDocument = new BsonDocument();
                Collection.Insert(MaintainDocumentName, MaintainDocument);
            }
            else
            {
                MaintainDocument = Collection.FindOne(Query.EQ("_id", MaintainDocumentName));
            }
        }

        protected KeyValueProperty<T> Property<T>(string propertyName, T defaultValue)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new InvalidOperationException("属性名不能为空");
            }

            if (!MaintainDocument.ContainsKey(propertyName))
            {
                MaintainDocument.Add(propertyName, BsonMapper.Global.Serialize(defaultValue));
            }

            return new KeyValueProperty<T>(this, propertyName);
        }

        protected KeyValueProperty<T> Property<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new InvalidOperationException("属性名不能为空");
            }

            if (!MaintainDocument.ContainsKey(propertyName))
            {
                MaintainDocument.Add(propertyName, BsonMapper.Global.Serialize(default(T)));
            }

            return new KeyValueProperty<T>(this, propertyName);
        }

        internal void SetValue<T>(string propertyName, T value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new InvalidOperationException("属性名不能为空");
            }

            if (!MaintainDocument.ContainsKey(propertyName))
            {
                MaintainDocument.Add(propertyName, BsonMapper.Global.Serialize(value));
            }
            else
            {
                MaintainDocument[propertyName] = BsonMapper.Global.Serialize(value);
            }

            using (Start())
            {
                Collection.Upsert(MaintainDocument);
            }

            OnSetValue<T>(value, propertyName);
        }

        internal void UpdateValue(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new InvalidOperationException("属性名不能为空");
            }

            using (Start())
            {
                Collection.Upsert(MaintainDocument);
            }

            OnRaiseUpdate(propertyName);
        }

        internal T GetValue<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new InvalidOperationException("属性名不能为空");
            }
            if (!MaintainDocument.TryGetValue(propertyName, out var val))
            {
                val = default;

            }

            return BsonMapper.Global.Deserialize<T>(val);
        }

        protected virtual void OnSetValue<T>(T value,string propertyName)
        {
            OnRaiseUpdate(propertyName);
        }

        protected virtual void OnRaiseUpdate(string propertyName)
        {

        }


        protected internal string MaintainDocumentName { get; }

        public BsonDocument MaintainDocument { get; private set; }
    }
}
