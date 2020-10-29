using Microsoft.VisualStudio.TestTools.UnitTesting;
using LiteDB;
using Acorisoft.Patterns;
using LiteDB.Engine;
using System.IO;
using System.Collections.Generic;
using System;

namespace Acorisoft.IO.Test
{
    public class ComplexModel
    {
        public string Name { get; set; } = nameof(ComplexModel);
        public List<string> ComplexModels { get; set; } = new List<string>
        {
           "asd",
           "asd1"
        };
        public int? Number { get; set; }
    }

    [TestClass]
    public class SingletonPatternUnit
    {
        /// <summary>
        /// 测试注册实例和获取实例
        /// </summary>
        [TestMethod]
        public void GetInstance_RegisterInstace()
        {
            var setting = new EngineSettings
            {
                DataStream = new MemoryStream(),
                InitialSize = 512 * 1024,

            };
            var engine = new LiteEngine(setting);
            var database = new LiteDatabase(engine);
            var singleton = database.GetSingletonPattern();
            var register =  new ComplexModel();
            singleton.RegisterInstance(register);
            var instance = singleton.GetInstance<ComplexModel>();

            Assert.AreEqual(register.Name, instance.Name);
            Assert.AreEqual(register.Number, instance.Number);
            Assert.AreEqual(register.ComplexModels[0], instance.ComplexModels[0]);
            Assert.AreEqual(register.ComplexModels[1], instance.ComplexModels[1]);
        }
    }
}
