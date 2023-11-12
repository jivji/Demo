using System;
using System.Collections.Generic;
using DataAccess.Objects;
using Microsoft.AspNetCore.Mvc;
using Parent = DemoAPIApplication.Models.Parent;

namespace DemoAPIApplication.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ParentsController : Controller
    {
        private readonly IParentsRepository _parentsRepository;
        private readonly IGrandParentsRepository _grandParentsRepository;
        private readonly Utility _utility = new();

        public ParentsController(IParentsRepository parentsRepository, IGrandParentsRepository grandParentsRepository)
        {
            _parentsRepository = parentsRepository;
            _grandParentsRepository = grandParentsRepository;
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
                if (id == 0) return Ok(_parentsRepository.GetAll());
                var item = _parentsRepository.Get(id);
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
            var itemToAdd = _utility.GetAutoMapper().Map<DataAccess.Objects.Parent>(item);

            try
            {
                if (itemToAdd.GrandParentId == 0)
                {
                    return BadRequest("GrandParentId is required.");
                }

                _grandParentsRepository.Get(itemToAdd.GrandParentId);
            }
            catch (Exception)
            {
                return BadRequest("GrandParent does not exist.");
            }

            try
            {
                return Ok(_parentsRepository.Add(itemToAdd));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut(Name = "UpdateParent")]
        public ActionResult<int> Update(Parent item, int grandParentId)
        {
            var itemToUpdate = _utility.GetAutoMapper().Map<DataAccess.Objects.Parent>(item);
            itemToUpdate.Id = grandParentId;
            try
            {
                if (itemToUpdate.Name == null && itemToUpdate.Description == null)
                {
                    return Ok(_parentsRepository.UpdateParentWithGrandParent(itemToUpdate));
                }

                return Ok(_parentsRepository.Update(itemToUpdate));
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
                return Ok(_parentsRepository.AddChildrenToParent(itemId, childrenId));
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
                return Ok(_parentsRepository.Delete(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
