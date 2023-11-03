using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Dapper;

namespace DataAccess.Objects
{
    public class GrandParentDao
    {
            private readonly IDbConnection _connection;

            public GrandParentDao(IDbConnection connection)
            {
                _connection = connection;
            }

            public GrandParent? Get(int id)
            {
                return _connection.QueryFirstOrDefault<GrandParent>("SELECT * FROM GrandParents WHERE Id = @ID", new { ID = id });
            }

            public IEnumerable<GrandParent> GetAll()
            {
                return _connection.Query<GrandParent>("SELECT * FROM GrandParents");
            }

            public int Add(GrandParent item)
            {
                return _connection.Execute("INSERT INTO GrandParents (Name, Description, PrimaryChild) VALUES (@Name, @Description, @PrimaryChild)", item);
            }
            
            public int Update(GrandParent item)
            {
                return _connection.Execute("UPDATE GrandParents SET Name = @Name, Description = @Description WHERE Id = @Id)", new {Name = item.Name, Description = item.Description, Id=item.Description });
            }

            public int Delete(int itemId)
            {
                return _connection.Execute("DELETE FROM GrandParents WHERE Id=@Id", new {Id = itemId});
            }

            public int UpdateGrandParentWithPrimaryChild(int itemId, int childId)
            {
                return _connection.Execute("UPDATE GrandParents SET PrimaryChild = @PrimaryChild WHERE Id=@Id", new {Id = itemId, PrimaryChild = childId});
            }

    }
}