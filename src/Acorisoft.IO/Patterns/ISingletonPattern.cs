using LiteDB;
using System;

namespace Acorisoft.Patterns
{
    /// <summary>
    /// <see cref="ISingletonPattern"/> 表示一种单例模式，用于在数据库中维护全局唯一的类型实例。
    /// </summary>
    public interface ISingletonPattern : IPattern
    {
        /// <summary>
        /// 从数据库中获取指定类型的实例。
        /// </summary>
        /// <typeparam name="T">指定要获取的数据类型。</typeparam>
        /// <returns>如果要获取的实例存在于数据库当中，则返回该实例，否则返回 null。</returns>
        T GetInstance<T>() where T : class;


        /// <summary>
        /// 从数据库中获取指定类型的实例。
        /// </summary>
        /// <typeparam name="T">指定要获取的数据类型。</typeparam>
        /// <param name="factory">构建该类型实例的工厂方法。</param>
        /// <returns>如果要获取的实例存在于数据库当中，则返回该实例，否则返回 null。</returns>
        T GetInstance<T>(Func<T> factory) where T : class;

        /// <summary>
        /// 注册一个指定类型的实例，并存储到数据库当中。
        /// </summary>
        /// <typeparam name="T">指定要注册的数据类型。</typeparam>
        /// <param name="factory">获取该数据类型实例的工厂方法。</param>
        void RegisterInstance<T>(Func<T> factory) where T : class;


        /// <summary>
        /// 注册一个指定类型的实例，并存储到数据库当中。
        /// </summary>
        /// <typeparam name="T">指定要注册的数据类型。</typeparam>
        /// <param name="instance">指定要注册的数据类型实例。</param>
        void RegisterInstance<T>(T instance) where T : class;
    }
}