using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace DataAccess.Objects
{
    public class ParentDao
    {
        private readonly IDbConnection Connection;
        private ChildDao ChildDao;
        private HandledExceptions HandledExceptions = new(); 

        public ParentDao(IDbConnection connection)
        {
            Connection = connection;
            ChildDao = new ChildDao(Connection);
        }

        public Parent? Get(int itemId)
        {
            var exec = Connection.QueryFirstOrDefault<Parent>("SELECT * FROM Parents WHERE Id = @ID", new {ID = itemId});
            if (exec == null)
            {
                throw new SystemException();
            }

            return exec;
        }

        public IEnumerable<Parent> GetAll()
        {
            return Connection.Query<Parent>("SELECT * FROM Parents");
        }

        public int Add(Parent item)
        {
            SystemException exception = new();
            var exec = 0; 
            try
            {
               exec =  Connection.ExecuteScalar<int>("INSERT INTO Parents (Name, Description, GrandParentId) VALUES (@Name, @Description, @GrandParentId); SELECT CAST(SCOPE_IDENTITY() AS INT)", item);
            }
            catch (Exception ex)
            {
                exception = HandledExceptions.CheckDuplicateNameException(ex, exception);
                throw exception;
            }

            return exec;
        }
        
        public int Update(Parent item)
        {
            SystemException exception = new();
            var exec = 0;
            try
            {
                exec = Connection.ExecuteScalar<int>("UPDATE Parents SET Name = @Name, Description = @Description, GrandParentId = @GrandParentId WHERE Id = @Id; SELECT CAST(SCOPE_IDENTITY() AS INT)", new {Name = item.Name, Description = item.Description, GrandParentId = item.GrandParentId, Id = item.Id});
            }
            catch (Exception ex)
            {
                exception = HandledExceptions.CheckItemNotFound(ex, exception);
                throw exception;
            }

            return exec;
        }
        
        public int UpdateParentGrandParent(Parent item)
        {
            SystemException exception = new();
            var exec = 0;
            try
            {
                exec = Connection.ExecuteScalar<int>("UPDATE Parents SET GrandParentId = @GrandParentId WHERE Id = @Id; SELECT CAST(SCOPE_IDENTITY() AS INT)", new { GrandParentId = item.GrandParentId, Id = item.Id});
            }
            catch (Exception ex)
            {
                exception = HandledExceptions.CheckItemNotFound(ex, exception);
                throw exception;
            }

            return exec;
        }

        public int[] AddChildrenToParent(int itemId, int[] children)
        {
            Connection.Open();

            using var transaction = Connection.BeginTransaction();
            try
            {
                foreach (var childId in children)
                {
                    try
                    {
                        ChildDao.Get(childId);
                    }   
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw new SystemException($"Child not found {childId}");
                    }

                    AddChildToParent(itemId, childId, transaction);
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }

            Connection.Close();
            return children;
        }

        public void AddChildToParent(int itemId, int childId, IDbTransaction transaction)
        {
            Connection.Execute("INSERT INTO ParentsChildren (ParentId, ChildId) VALUES (@ParentId, @ChildId)", new {ParentId = itemId, ChildId = childId}, transaction);
        }
        
        public int Delete(int itemId)
        {
            Connection.Open();
    
            using var transaction = Connection.BeginTransaction();
            try
            {
                Connection.Execute("DELETE FROM Parents WHERE Id = @Id", new {Id = itemId}, transaction);
                Connection.Execute("DELETE FROM ParentsChildren WHERE ParentId = @ParentId", new {ParentId = itemId},transaction);
                transaction.Commit();
                
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            Connection.Close();
            return itemId;
        }
        
        public void DeleteChildFromParent(int itemId, int childId=0)
        {
            switch (childId)
            {
                case 0:
                    Connection.Execute("DELETE FROM ParentsChildren WHERE Parentd = @ParentId", new {Parentd = itemId});
                    break;
                default:
                    Connection.Execute("DELETE FROM ParentsChildren WHERE ParentId = @ParentId AND ChildId = @ChildId", new {ParentId = itemId, ChildId = @childId});
                    break;
            }
        }
    }
}