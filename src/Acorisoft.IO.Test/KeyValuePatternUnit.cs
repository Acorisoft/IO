using Microsoft.VisualStudio.TestTools.UnitTesting;
using LiteDB;
using Acorisoft.Patterns;
using LiteDB.Engine;
using System.IO;
using System.Collections.Generic;
using System;

namespace Acorisoft.IO.Test
{
    public class Setting : KeyValuePattern
    {
        private readonly KeyValueProperty<string> _name;
        private readonly KeyValueProperty<Dictionary<string,string>> _dictionary;

        public Setting(ILiteDatabase database) : base(database.GetSingletonPattern(), nameof(Setting))
        {
            _name = Property<string>(nameof(Name), nameof(Setting));
            _dictionary = Property(nameof(Dictionary), new Dictionary<string, string> { { "A", "1" }, { "B", "2" } });
        }

        public string Name
        {
            get => _name.Value;
            set => _name.Value = value;
        }

        public Dictionary<string, string> Dictionary
        {
            get => _dictionary.Value;
            set => _dictionary.Value = value;
        }
    }

    public class Setting1 : KeyValuePattern
    {
        private readonly KeyValueProperty<string> _name;
        private readonly KeyValueProperty<Dictionary<string,string>> _dictionary;

        public Setting1(ILiteDatabase database) : base(database.GetSingletonPattern(), nameof(Setting))
        {
            _name = Property<string>(nameof(Name), nameof(Setting));
            _dictionary = Property(nameof(Dictionary), new Dictionary<string, string> { { "A", "1" }, { "B", "2" } });
        }

        public string Name
        {
            get => _name.Value;
            set => _name.Value = value;
        }

        public Dictionary<string, string> Dictionary
        {
            get => _dictionary.Value;
            set => _dictionary.Value = value;
        }
    }

    [TestClass]
    public class KeyValuePatternUnit
    {
        /// <summary>
        /// 测试注册实例和获取实例
        /// </summary>
        [TestMethod]
        public void SetValue_GetValue()
        {
            var setting = new EngineSettings
            {
                DataStream = new MemoryStream(),
                InitialSize = 512 * 1024,

            };
            var engine = new LiteEngine(setting);
            var database = new LiteDatabase(engine);
            var setting1 = new Setting(database);
            var setting2 = new Setting1(database);

            Assert.AreEqual(setting1.Name, setting2.Name);
            Assert.AreEqual(setting1.Dictionary["A"], setting2.Dictionary["A"]);
            Assert.AreEqual(setting1.Dictionary["B"], setting2.Dictionary["B"]);
        }
    }
}
