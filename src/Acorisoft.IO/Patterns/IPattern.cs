using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acorisoft.Patterns
{
    public interface IPattern
    {
        public ILiteDatabase Database { get; }
        public ILiteCollection<BsonDocument> Collection { get; }
        public string CollectionName { get; }
    }
}
