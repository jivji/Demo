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
        private readonly IParentDao ParentDao;
        private readonly IGrandParentDao GrandParentDao;
        private readonly Utility Utility = new();

        public ParentController(IParentDao parentDao, IGrandParentDao grandParentDao)
        {
            ParentDao = parentDao;
            GrandParentDao = grandParentDao;
        }
        
        // public ParentController()
        // {
        //     GrandParentDao = new GrandParentDao(new SqlConnection(Configuration.GetConnectionString()));
        //     ParentDao = new ParentDao(new SqlConnection(Configuration.GetConnectionString()));
        // }

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
        public ActionResult<int> Update(Parent item, int grandParentId)
        {
            var itemToUpdate = Utility.GetAutoMapper().Map<DataAccess.Objects.Parent>(item);
            itemToUpdate.Id = grandParentId;
            try
            {
                if (itemToUpdate.Name == null && itemToUpdate.Description == null)
                {
                    return Ok(ParentDao.UpdateParentWithGrandParent(itemToUpdate));
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
