using SQLCommon.Models;

namespace SQLCommon.Logic
{
    public interface IDbClient
    {
        IDbClient Initialize();
        void Save(SQLProduct product);
        void FlushProducts(int commitStep = 0);
        void PostMigration();
    }
}
