using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DataAccess.Objects;

namespace DataAccess
{
    public interface IChildrenRepository
    {
        public Child? Get(int itemId);
        public IEnumerable<Child> GetAll();
        public int Add(Child item);
        public int Update(Child item);
        public int Delete(int itemId);
    }
    
    public class ChildrenRepository : IChildrenRepository
    {
        private readonly IDbConnection _connection;
        private SqlValidationHelper _sqlValidationHelper = new(); 

        public ChildrenRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public Child? Get(int childId)
        {
            return _connection.QueryFirstOrDefault<Child?>("SELECT * FROM Children WHERE Id = @ID", new {ID = childId});
        }

        public IEnumerable<Child> GetAll()
        {
            return _connection.Query<Child>("SELECT * FROM Children");
        }

        public int Add(Child child)
        {
            var exec = 0; 
            try
            {
               exec =  _connection.ExecuteScalar<int>("INSERT INTO Children (Name, Description) VALUES (@Name, @Description); SELECT CAST(SCOPE_IDENTITY() AS INT)", child);
            }
            catch (SqlException ex)
            {
                SqlValidationHelper.CheckDuplicateNameException(ex);
            }

            return exec;
        }

        public int Update(Child child)
        {
            return _connection.ExecuteScalar<int>("UPDATE Children SET Name = @Name, Description = @Description WHERE id=@Id;", new { Name = child.Name, Description = child.Description, id= child.Id});
        }
        
        public int Delete(int childId)
        {
            return _connection.Execute("DELETE FROM Children WHERE id=@Id", new { id= childId});
        }
    }
}