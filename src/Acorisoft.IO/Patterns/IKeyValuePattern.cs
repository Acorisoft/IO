using LiteDB;

namespace Acorisoft.Patterns
{
    public interface IKeyValuePattern : IPattern
    {
        BsonDocument MaintainDocument { get; }
    }
}