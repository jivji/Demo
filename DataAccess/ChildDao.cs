using System.Collections.Generic;
using System.Data;
using Dapper;
using DataAccess.Objects;

namespace DataAccess
{
    public class ChildDao
    {
        private readonly IDbConnection _connection;

        public ChildDao(IDbConnection connection)
        {
            _connection = connection;
        }

        public Child? Get(int itemId)
        {
            return _connection.QueryFirstOrDefault<Child>("SELECT * FROM Children WHERE Id = @ID", new { ID = itemId });
        }

        public IEnumerable<Child> GetAll()
        {
            return _connection.Query<Child>("SELECT * FROM Children");
        }

        public int Add(Child item)
        {
            return _connection.Execute("INSERT INTO Children (Name, Description) VALUES (@Name, @Description)", item);
        }

        public int Update(Child item)
        {
            return _connection.Execute("UPDATE Children SET Name = @Name, Description = @Description WHERE id=@Id", new { Name = item.Name, Description = item.Description, id= item.Id});
        }
        
        public int Delete(int itemId)
        {
            return _connection.Execute("DELETE FROM Children WHERE id=@Id", new { id= itemId});
        }
    }
}