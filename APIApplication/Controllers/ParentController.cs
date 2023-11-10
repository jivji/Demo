using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DataAccess;
using DataAccess.Objects;
using Microsoft.AspNetCore.Mvc;
using Parent = DemoAPIApplication.Models.Parent;

namespace DemoAPIApplication.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ParentController : Controller
    {
        private readonly ParentDao ParentDao = new(new SqlConnection(Configuration.GetConnectionString()));
        private readonly ChildDao ChildDao = new(new SqlConnection(Configuration.GetConnectionString()));
        private readonly GrandParentDao GrandParentDao = new(new SqlConnection(Configuration.GetConnectionString()));
        private readonly Utility Utility = new();

        [HttpGet(Name = "GetParents")]
        public ActionResult<IEnumerable<Parent>> Get(int id)
        {
            try
            {
                if (id == 0) return Ok(ParentDao.GetAll());
                var item = ParentDao.Get(id);
                if (item == null)
                {
                    return NotFound();
                }

                Parent parent = new()
                {
                    Name = item.Name,
                    Description = item.Description
                };

                return new List<Parent> {parent};
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost(Name = "AddParent")]
        public ActionResult<Parent> Add(Parent item)
        {
            var itemToAdd = Utility.GetAutoMapper().Map<DataAccess.Objects.Parent>(item);

            try
            {
                if (itemToAdd.GrandParentId == 0)
                {
                    return BadRequest("GrandParentId is required.");
                }

                GrandParentDao.Get(itemToAdd.GrandParentId);
            }
            catch (Exception)
            {
                return BadRequest("GrandParent does not exist.");
            }

            try
            {
                return Ok(ParentDao.Add(itemToAdd));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut(Name = "UpdateParent")]
        public ActionResult<Parent> Update(Parent item, int id)
        {
            var itemToUpdate = Utility.GetAutoMapper().Map<DataAccess.Objects.Parent>(item);
            itemToUpdate.Id = id;
            try
            {
                if (itemToUpdate.Name == null && itemToUpdate.Description == null)
                {
                    return Ok(ParentDao.UpdateParentGrandParent(itemToUpdate));
                }

                return Ok(ParentDao.Update(itemToUpdate));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost(Name = "AddChildrenToParent")]
        public ActionResult<int[]> AddChildrenToParent(int itemId, int[] childrenId)
        {
            try
            {
                return Ok(ParentDao.AddChildrenToParent(itemId, childrenId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete(Name = "DeleteParent")]
        public ActionResult<Parent> Delete(int id)
        {
            try
            {
                return Ok(ParentDao.Delete(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
