using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccess.Objects
{
    public interface IParentsRepository
    {
        Parent? Get(int itemId);
        IEnumerable<Parent> GetAll();
        int Add(Parent item);
        int Update(Parent item);
        int UpdateParentWithGrandParent(Parent item);
        int[] AddChildrenToParent(int itemId, int[] children);
        void AddChildToParent(int itemId, int childId, IDbTransaction transaction);
        int Delete(int itemId);
    }

    public class ParentsRepository : IParentsRepository
    {
        private readonly IDbConnection _connection;
        private readonly IChildrenRepository _childrenRepository;
        private SqlValidationHelper _sqlValidationHelper = new();

        public ParentsRepository(IDbConnection connection, IChildrenRepository childrenRepository)
        {
            _connection = connection;
            _childrenRepository = childrenRepository;
        }

        public Parent? Get(int itemId)
        {
            var exec = _connection.QueryFirstOrDefault<Parent>("SELECT * FROM Parents WHERE Id = @ID",
                new {ID = itemId});
            if (exec == null)
            {
                throw new SystemException();
            }

            return exec;
        }

        public IEnumerable<Parent> GetAll()
        {
            return _connection.Query<Parent>("SELECT * FROM Parents");
        }

        public int Add(Parent item)
        {
            Exception exception = new();
            var exec = 0;
            try
            {
                exec = _connection.ExecuteScalar<int>(
                    "INSERT INTO Parents (Name, Description, GrandParentId) VALUES (@Name, @Description, @GrandParentId); SELECT CAST(SCOPE_IDENTITY() AS INT)",
                    item);
            }
            catch (SqlException ex)
            {
                SqlValidationHelper.CheckDuplicateNameException(ex);
            }

            return exec;
        }

        public int Update(Parent item)
        {
            Exception exception = new();
            var exec = 0;
            try
            {
                exec = _connection.ExecuteScalar<int>(
                    "UPDATE Parents SET Name = @Name, Description = @Description, GrandParentId = @GrandParentId WHERE Id = @Id; SELECT CAST(SCOPE_IDENTITY() AS INT)",
                    new
                    {
                        Name = item.Name, Description = item.Description, GrandParentId = item.GrandParentId,
                        Id = item.Id
                    });
            }
            catch (SqlException ex)
            {
                SqlValidationHelper.CheckItemNotFound(ex);
            }

            return exec;
        }

        public int UpdateParentWithGrandParent(Parent item)
        {
            Exception exception = new();
            var exec = 0;
            try
            {
                exec = _connection.ExecuteScalar<int>(
                    "UPDATE Parents SET GrandParentId = @GrandParentId WHERE Id = @Id; SELECT CAST(SCOPE_IDENTITY() AS INT)",
                    new {GrandParentId = item.GrandParentId, Id = item.Id});
            }
            catch (SqlException ex)
            {
                SqlValidationHelper.CheckItemNotFound(ex);
            }

            return exec;
        }

        public int[] AddChildrenToParent(int itemId, int[] children)
        {
            foreach (var childId in children)
            {
                try
                {
                    _childrenRepository.Get(childId);
                }
                catch (Exception)
                {
                    throw new Exception($"Child not found {childId}");
                }
            }
            
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
            catch
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

        public int Delete(int itemId)
        {
            _connection.Open();

            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    _connection.Execute("DELETE FROM Parents WHERE Id = @Id", new {Id = itemId}, transaction);
                    _connection.Execute("DELETE FROM ParentsChildren WHERE ParentId = @ParentId",
                        new {ParentId = itemId}, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            _connection.Close();
            return itemId;
        }
    }
}