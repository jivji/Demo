using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using DataAccess.Objects;

namespace DataAccess
{
    public interface IChildDao
    {
        public Child Get(int itemId);
        public IEnumerable<Child> GetAll();
        public int Add(Child item);
        public int Update(Child item);
        public int Delete(int itemId);
    }
    
    public class ChildDao : IChildDao
    {
        private readonly IDbConnection Connection;
        private HandledExceptions HandledExceptions = new(); 

        public ChildDao(IDbConnection connection)
        {
            Connection = connection;
        }

        public Child Get(int itemId)
        {
            var exec = Connection.QueryFirstOrDefault<Child?>("SELECT * FROM Children WHERE Id = @ID", new {ID = itemId});
            if ( exec == null)
            {
                throw new SystemException();
            }

            return exec;
        }

        public IEnumerable<Child> GetAll()
        {
            return Connection.Query<Child>("SELECT * FROM Children");
        }

        public int Add(Child item)
        {
            SystemException exception = new();
            var exec = 0; 
            try
            {
               exec =  Connection.ExecuteScalar<int>("INSERT INTO Children (Name, Description) VALUES (@Name, @Description); SELECT CAST(SCOPE_IDENTITY() AS INT)", item);
            }
            catch (Exception ex)
            {
                exception = HandledExceptions.CheckDuplicateNameException(ex, exception);
                throw exception;
            }

            return exec;
        }

        public int Update(Child item)
        {
            return Connection.ExecuteScalar<int>("UPDATE Children SET Name = @Name, Description = @Description WHERE id=@Id; SELECT CAST(SCOPE_IDENTITY() AS INT", new { Name = item.Name, Description = item.Description, id= item.Id});
        }
        
        public int Delete(int itemId)
        {
            return Connection.Execute("DELETE FROM Children WHERE id=@Id", new { id= itemId});
        }
    }
}