using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DataAccess.Objects
{
    public class ParentDao
    {
        private readonly IDbConnection _connection;

        public ParentDao(IDbConnection connection)
        {
            _connection = connection;
        }

        public Parent? GetParent(int itemId)
        {
            return _connection.QueryFirstOrDefault<Parent>("SELECT * FROM Parents WHERE Id = @ID", new {ID = itemId});
        }

        public IEnumerable<GrandParent> GetAllParents()
        {
            return _connection.Query<GrandParent>("SELECT * FROM Parents");
        }

        public int AddParent(Parent item)
        {
            return _connection.Query<int>("INSERT INTO Parents (Name, Description) VALUES (@Name, @Description); SELECT CAST(SCOPE_IDENTITY() AS INT)", item).Single();
        }
        
        public void UpdateParent(Parent item)
        {
            _connection.Execute("UPDATE Parents SET Name = @Name, Description = @Description WHERE Id = @Id", new {Name = item.Name, Description = item.Description, Id = item.Id});
        }

        public int[] AddChildrenToParent(int itemId, int[] children)
        {
            _connection.Open();

            using var transaction = _connection.BeginTransaction();
            try
            {
                foreach (var childId in children)
                {
                    AddChildToParent(itemId, childId, transaction);
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            _connection.Close();
            return children;
        }

        public void AddChildToParent(int itemId, int childId, IDbTransaction transaction)
        {
            _connection.Execute("INSERT INTO ParentsChildren (ParentId, ChildId) VALUES (@ParentId, @ChildId)", new {ParentId = itemId, ChildId = childId}, transaction);
        }
        
        public void DeleteParent(int itemId)
        {
            _connection.Open();
    
            using var transaction = _connection.BeginTransaction();
            try
            {
                _connection.Execute("DELETE FROM Parents WHERE Id = @Id", new {Id = itemId}, transaction);
                _connection.Execute("DELETE FROM ParentsChildren WHERE ParentId = @ParentId", new {ParentId = itemId},transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            _connection.Close();
        }
        
        public void DeleteChildFromParent(int itemId, int childId=0)
        {
            switch (childId)
            {
                case 0:
                    _connection.Execute("DELETE FROM ParentsChildren WHERE Parentd = @ParentId", new {Parentd = itemId});
                    break;
                default:
                    _connection.Execute("DELETE FROM ParentsChildren WHERE ParentId = @ParentId AND ChildId = @ChildId", new {ParentId = itemId, ChildId = @childId});
                    break;
            }
        }
    }
}