using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acorisoft.Patterns
{
    public class Transaction : IDisposable
    {
        private readonly ILiteDatabase _database;

        public Transaction(ILiteDatabase database)
        {
            _database = database;
        }

        public void Start()
        {
            if (!_database.BeginTrans())
            {
                throw new InvalidOperationException("不能开启事务");
            }
        }

        public void Dispose()
        {
            if (!_database.Commit())
            {
                throw new InvalidOperationException("不能提交事务");
            }
        }
    }
}
