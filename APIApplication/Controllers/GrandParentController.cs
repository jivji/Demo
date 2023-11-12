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
    [Route("[controller]")]
    public class GrandParentController : Controller
    {
        private readonly IGrandParentDao GrandParentDao;
        private readonly IChildDao ChildDao;
        private readonly Utility Utility = new();

        public GrandParentController()
        {
            GrandParentDao = new GrandParentDao(new SqlConnection(Configuration.GetConnectionString()));
            ChildDao = new ChildDao(new SqlConnection(Configuration.GetConnectionString()));
        }

        public GrandParentController(IGrandParentDao grandParentDao , IChildDao childDao)
        {
            GrandParentDao = grandParentDao ;
            ChildDao = childDao ;
        }
        
        [HttpGet(Name = "GetGrandParents")]
        public ActionResult<IEnumerable<Models.GrandParent>> Get(int id)
        {
            try
            {
                if (id == 0) return Ok(GrandParentDao.GetAll());
                var item = GrandParentDao.Get(id);
                if (item == null)
                {
                    return NotFound();
                }

                Models.GrandParent grandParent = new()
                {
                    Name = item.Name,
                    Description = item.Description
                };

                return new List<Models.GrandParent> {grandParent};
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost(Name = "AddGrandParent")]
        public ActionResult<Models.GrandParent> Add(Models.GrandParent item)
        {
            var itemToAdd = Utility.GetAutoMapper().Map<GrandParent>(item);

            if (itemToAdd.PrimaryChild != 0)
            {
                try
                {
                    Ok(ChildDao.Get(itemToAdd.PrimaryChild));
                }
                catch (Exception)
                {
                    return BadRequest("Primary Child does not exist.");
                }

                try
                {
                    return Ok(GrandParentDao.Add(itemToAdd));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest("When adding a GrandParent, you must specify a Primary Child.");
            }
        }

        [HttpPut(Name = "UpdateGrandParent")]
        public ActionResult<Models.GrandParent> Update(Models.GrandParent item, int id)
        {
            var itemToUpdate = Utility.GetAutoMapper().Map<GrandParent>(item);
            itemToUpdate.Id = id;

            if (itemToUpdate.PrimaryChild != 0)
            {
                try
                {
                    Ok(ChildDao.Get(itemToUpdate.PrimaryChild));
                }
                catch (Exception)
                {
                    return BadRequest($"Primary Child : {itemToUpdate.PrimaryChild} does not exist.");
                }
            }
            try
            {
                if (itemToUpdate.Name == null && itemToUpdate.Description == null)
                {
                    return Ok(GrandParentDao.UpdateGrandParentWithPrimaryChild(itemToUpdate.Id,
                        itemToUpdate.PrimaryChild));
                }
                else
                {
                    return Ok(GrandParentDao.Update(itemToUpdate));
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete(Name = "DeleteGrandParent")]
        public ActionResult<Models.GrandParent> Delete(int id)
        {
            try
            {
                return Ok(GrandParentDao.Delete(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}