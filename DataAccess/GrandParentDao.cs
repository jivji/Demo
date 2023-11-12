using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Dapper;

namespace DataAccess.Objects
{
    public interface IGrandParentDao
    {
        GrandParent? Get(int id);
        IEnumerable<GrandParent> GetAll();
        int Add(GrandParent item);
        int Update(GrandParent item);
        int Delete(int itemId);
        int UpdateGrandParentWithPrimaryChild(int itemId, int childId);
    }

    public class GrandParentDao : IGrandParentDao
    {
            private readonly IDbConnection Connection;
            private HandledExceptions HandledExceptions = new HandledExceptions(); 

            public GrandParentDao(IDbConnection connection)
            {
                Connection = connection;
            }

            public GrandParent? Get(int id)
            {
                var exec = Connection.QueryFirstOrDefault<GrandParent>("SELECT * FROM GrandParents WHERE Id = @ID", new { ID = id });
                if (exec == null)
                {
                    throw new SystemException();
                }

                return exec;
            }

            public IEnumerable<GrandParent> GetAll()
            {
                return Connection.Query<GrandParent>("SELECT * FROM GrandParents");
            }

            public int Add(GrandParent item)
            {
                SystemException exception = new SystemException();
                int exec = 0;
                try
                {
                    exec = Connection.ExecuteScalar<int>("INSERT INTO GrandParents (Name, Description, PrimaryChild) VALUES (@Name, @Description, @PrimaryChild); SELECT CAST(SCOPE_IDENTITY() AS INT)", item);
                }
                catch (Exception ex)
                {
                    exception = HandledExceptions.CheckDuplicateNameException(ex, exception);

                    throw exception;
                }

                return exec;
            }
            
            public int Update(GrandParent item)
            {
                return Connection.Execute("UPDATE GrandParents SET Name = @Name, Description = @Description, PrimaryChild = @PrimaryChild WHERE Id = @Id;", new {Name = item.Name, Description = item.Description, PrimaryChild = item.PrimaryChild, Id=item.Id });
            }

            public int Delete(int itemId)
            {
                return Connection.Execute("DELETE FROM GrandParents WHERE Id=@Id", new {Id = itemId});
            }

            public int UpdateGrandParentWithPrimaryChild(int itemId, int childId)
            {
                return Connection.Execute("UPDATE GrandParents SET PrimaryChild = @PrimaryChild WHERE Id=@Id", new {Id = itemId, PrimaryChild = childId});
            }

    }
}