using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess.Objects
{
    public interface IGrandParentsRepository
    {
        GrandParent? Get(int id);
        IEnumerable<GrandParent> GetAll();
        int Add(GrandParent item);
        int Update(GrandParent item);
        int Delete(int itemId);
        int UpdateGrandParentWithPrimaryChild(int itemId, int childId);
    }

    public class GrandParentsRepository : IGrandParentsRepository
    {
            private readonly IDbConnection _connection;
            private SqlValidationHelper _sqlValidationHelper = new(); 

            public GrandParentsRepository(IDbConnection connection)
            {
                _connection = connection;
            }

            public GrandParent? Get(int grandParentId)
            {
                return _connection.QueryFirstOrDefault<GrandParent>("SELECT * FROM GrandParents WHERE Id = @ID", new { ID = grandParentId });
            }

            public IEnumerable<GrandParent> GetAll()
            {
                return _connection.Query<GrandParent>("SELECT * FROM GrandParents");
            }

            public int Add(GrandParent grandParent)
            {
                int exec = 0;
                try
                {
                    exec = _connection.ExecuteScalar<int>("INSERT INTO GrandParents (Name, Description, PrimaryChildId) VALUES (@Name, @Description, @PrimaryChildId); SELECT CAST(SCOPE_IDENTITY() AS INT)", grandParent);
                }
                catch (SqlException ex)
                {
                    SqlValidationHelper.CheckDuplicateNameException(ex);
                }

                return exec;
            }
            
            public int Update(GrandParent grandParent)
            {
                return _connection.Execute("UPDATE GrandParents SET Name = @Name, Description = @Description, PrimaryChildId = @PrimaryChildId WHERE Id = @Id;", new {Name = grandParent.Name, Description = grandParent.Description, PrimaryChildId = grandParent.PrimaryChildId, Id=grandParent.Id });
            }

            public int Delete(int grandParentId)
            {
                return _connection.Execute("DELETE FROM GrandParents WHERE Id=@Id", new {Id = grandParentId});
            }

            public int UpdateGrandParentWithPrimaryChild(int grandParentId, int childId)
            {
                return _connection.Execute("UPDATE GrandParents SET PrimaryChildId = @PrimaryChildId WHERE Id=@Id", new {Id = grandParentId, PrimaryChildId = childId});
            }

    }
}