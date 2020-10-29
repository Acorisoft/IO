using Acorisoft.Patterns.LiteDB;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acorisoft.Patterns
{
    public static class SingletonPatternMixins
    {
        /// <summary>
        /// 使用指定的数据库创建单例模式
        /// </summary>
        /// <param name="database">创建单例模式使用的数据库，不能为空。</param>
        /// <returns>返回一个单例模式实例。</returns>
        public static ISingletonPattern GetSingletonPattern(this ILiteDatabase database)
        {
            if(database == null)
            {
                throw new InvalidOperationException("无法使用指定的数据库实例创建单例模式");
            }
            return new LiteDbSinletonPatternImpl(database);
        }

        /// <summary>
        /// 从数据库中获取指定类型的实例。
        /// </summary>
        /// <typeparam name="T">指定要获取的数据类型。</typeparam>
        /// <returns>如果要获取的实例存在于数据库当中，则返回该实例，否则返回 null。</returns>
        public static T GetInstance<T>(this ILiteDatabase database) where T : class
        {
            if (database == null)
            {
                throw new InvalidOperationException("无法使用指定的数据库实例创建单例模式");
            }
            return database.GetSingletonPattern().GetInstance<T>(null);
        }

        /// <summary>
        /// 从数据库中获取指定类型的实例。
        /// </summary>
        /// <typeparam name="T">指定要获取的数据类型。</typeparam>
        /// <param name="factory">构建该类型实例的工厂方法。</param>
        /// <returns>如果要获取的实例存在于数据库当中，则返回该实例，否则返回 null。</returns>
        public static T GetInstance<T>(this ILiteDatabase database, Func<T> factory) where T : class
        {
            if (database == null)
            {
                throw new InvalidOperationException("无法使用指定的数据库实例创建单例模式");
            }
            return database.GetSingletonPattern().GetInstance<T>(factory);
        }

        /// <summary>
        /// 注册一个指定类型的实例，并存储到数据库当中。
        /// </summary>
        /// <typeparam name="T">指定要注册的数据类型。</typeparam>
        /// <param name="factory">获取该数据类型实例的工厂方法。</param>
        public static void RegisterInstance<T>(this ILiteDatabase database, Func<T> factory) where T : class
        {
            if (database == null)
            {
                throw new InvalidOperationException("无法使用指定的数据库实例创建单例模式");
            }
            database.GetSingletonPattern().RegisterInstance<T>(factory);
        }

        /// <summary>
        /// 注册一个指定类型的实例，并存储到数据库当中。
        /// </summary>
        /// <typeparam name="T">指定要注册的数据类型。</typeparam>
        /// <param name="instance">指定要注册的数据类型实例。</param>
        public static void RegisterInstance<T>(this ILiteDatabase database, T instance) where T : class
        {
            if (database == null)
            {
                throw new InvalidOperationException("无法使用指定的数据库实例创建单例模式");
            }

            if (instance == null)
            {
                return;
            }

            database.GetSingletonPattern().RegisterInstance<T>(instance);
        }

        private const string IdField = "_id";

        internal static bool Exists<T>(this ILiteCollection<BsonDocument> collection, out string name)
        {
            name = typeof(T).FullName;
            return collection.Exists(Query.EQ(IdField, name));
        }
    }
}
