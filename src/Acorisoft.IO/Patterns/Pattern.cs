using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Transactions;

namespace Acorisoft.Patterns
{
    public abstract class Pattern : IPattern
    {
        private readonly TransactionImpl _transaction;
        private readonly ILiteDatabase _database;
        private readonly string _scopeName;
        private readonly ILiteCollection<BsonDocument> _collection;

        protected Pattern(ILiteDatabase database, string scopeName)
        {
            _database = database ?? throw new ArgumentNullException();
            _scopeName = scopeName;
            _collection = database.GetCollection<BsonDocument>(scopeName);
            _transaction = new TransactionImpl(database);
        }

        protected Pattern(Pattern pattern) : this(pattern._database, pattern._scopeName)
        {

        }

        protected internal IDisposable Start()
        {
            _transaction.Start();
            return _transaction;
        }

        protected ILiteCollection<BsonDocument> Collection => _collection;

        ILiteDatabase IPattern.Database => _database;
        string IPattern.CollectionName => _scopeName;
        ILiteCollection<BsonDocument> IPattern.Collection => _collection;

        private class TransactionImpl : IDisposable
        {
            private readonly ILiteDatabase _database;

            public TransactionImpl(ILiteDatabase database)
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
}
