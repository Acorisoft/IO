# Acorisoft.IO
`Acorisoft.IO` 库用于为用户提供便捷的IO操作。

## 单例模式

单例模式，指的是一个类型在同一个进程中只有一个对象实例的设计模式即为单例模式。`Acorisoft.IO`提供了一种数据库单例模式支持，用户可以更快捷的创建数据库单例实例。

## 使用场景

`Acorisoft.IO`为上游项目提供IO操作的支持，除去上游项目之外的应用场景即：应用设置等场景，亦或者用户存在需要在数据库中维护多个单例实例。

## 如何使用

### SinletonPattern

 `ISinletionPattern` 接口用于为用户提供数据库单例模式支持，目前我们暂时只提供了`LiteDB`数据库支持。

``` C#
using Acorisoft.Patterns;
namespace Demo
{
  class Program
  {
      public static void Main(string[] args)
      {
          // 可以使用用户自己的数据库构造模式
          var database = new LiteDatabase("test");

          // 使用指定的数据库创建数据库单例模式。
          var singleton = database.GetSingletonPattern();
          
          // 第一次使用时请注册。
          singleton.RegisterInstance<string>(); 
          
          // 获取指定类型实例
          var value = singleton.GetInstance<string>();

          // 当然也可以通过这种方式获取实例，但是并不推荐，因为这会生成多余的对象
          var fastGetInstace = database.GetInstance<IPAddress>();

      }
  }
}
```

### KeyValuePattern

`KeyValuePattern` 类型为用户提供了基于键值对的存储模式用以简化对`BsonDocument`的访问，目前该模式我们以`LiteDB`作为实现。

``` C#
using Acorisoft.Patterns;
namespace Demo
{
    public class Setting : KeyValuePattern
    {
        private readonly KeyValueProperty<string> _name;

        public Setting(ILiteDatabase database) : base(database.GetSingletonPattern())
        {
            _name = Property<string>(nameof(Name), nameof(Setting));
        }

        public string Name
        {
            get => _name.Value;
            set => _name.Value = value;
        }
    }
}
```

使用前先继承 `KeyValuePattern` 类型。我们通过`KeyValueProperty<T>` 类型封装了对`BsonDocument`的访问，在继承 `KeyValuePattern`之后用户将获得对于`Property<T>(string,T defalutValue)` 方法的访问权。这时候需要在构造函数中创建对应类型的`KeyValueProperty<T>`。即可完成操作。

> 注意：
> 用户可能需要使得该类型能够通知属性的更改，因为目前暂时没有考虑与其他框架诸如：ReactiveUI的整合，所以用户需要实现`INotifyPropertyChanged` 接口并且重写`OnSetValue<T>`方法来获得属性变更通知的支持。