using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acorisoft.Patterns.LiteDB
{
    /// <summary>
    /// <see cref="LiteDbSinletonPatternImpl"/> 实现了LiteDB数据库单例模式。
    /// </summary>
    internal class LiteDbSinletonPatternImpl : Pattern, ISingletonPattern
    {
        private const string CollectionName = "Singletion";

        internal LiteDbSinletonPatternImpl(ILiteDatabase database) : base(database, CollectionName)
        {

        }

        internal LiteDbSinletonPatternImpl(ILiteDatabase database, string collectionName) : base(database, collectionName)
        {

        }

        /// <summary>
        /// 从数据库中获取指定类型的实例。
        /// </summary>
        /// <typeparam name="T">指定要获取的数据类型。</typeparam>
        /// <returns>如果要获取的实例存在于数据库当中，则返回该实例，否则返回 null。</returns>
        public T GetInstance<T>() where T : class
        {
            return GetInstance<T>(null);
        }

        public T GetInstance<T>(Func<T> factory) where T : class
        {
            T instance = default;

            if (Collection.Exists<T>(out string instanceKey))
            {
                //
                // 注意：
                // 该方法可能返回为空。
                var document = Collection.FindById(instanceKey);
                return BsonMapper.Global.Deserialize<T>(document);
            }
            else if (factory != null)
            {
                instance = factory();
                RegisterInstance(instance, instanceKey);
            }

            return instance;
        }

        public void RegisterInstance<T>(Func<T> factory) where T : class
        {
            if (factory == null)
            {
                throw new InvalidOperationException("实例的工厂方法不能为空");
            }

            RegisterInstance(factory());
        }

        public void RegisterInstance<T>(T instance) where T : class
        {
            if (instance == null)
            {
                return;
            }

            RegisterInstance<T>(instance, typeof(T).FullName);
        }

        private void RegisterInstance<T>(T instance, string key) where T : class
        {
            using (Start())
            {
                Collection.Upsert(key, BsonMapper.Global.ToDocument(instance));
            }
        }
    }
}
