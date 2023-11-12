using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DataAccess;
using DataAccess.Objects;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPIApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChildController : Controller
    {
        private readonly IChildDao ChildDao;

        public ChildController()
        {
            ChildDao = new ChildDao(new SqlConnection(Configuration.GetConnectionString()));
        }

        public ChildController(IChildDao childDao) 
        {
            this.ChildDao = childDao ;
        }

        [HttpGet(Name = "GetChildren")]
        public ActionResult<IEnumerable<Models.Child>> Get(int id)
        {
            try
            {
                if (id == 0) return Ok(ChildDao.GetAll());
                var item = ChildDao.Get(id);
                if (item == null)
                {
                    return NotFound();
                }

                Models.Child child = new()
                {
                    Name = item.Name,
                    Description = item.Description
                };

                return new List<Models.Child> {child};
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost(Name = "AddChild")]
        public ActionResult<Models.Child> Add(Models.Child item)
        {
            var itemToAdd = new Child()
            {
                Name = item.Name,
                Description = item.Description
            };
            
            try
            {
                return Ok(ChildDao.Add(itemToAdd));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut(Name = "UpdateChild")]
        public ActionResult<int> Update(Models.Child item, int id)
        {
            var itemToUpdate = new Child()
            {
                Id = id,
                Name = item.Name,
                Description = item.Description
            };
            
            try
            {
                return Ok(ChildDao.Update(itemToUpdate));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete(Name = "DeleteChild")]
        public ActionResult<int> Delete(int id)
        {
            try
            {
                return Ok(ChildDao.Delete(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}