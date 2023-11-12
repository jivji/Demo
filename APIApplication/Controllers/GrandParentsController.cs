using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AutoMapper;
using DataAccess;
using DataAccess.Objects;
using Microsoft.AspNetCore.Mvc;
using Parent = DemoAPIApplication.Models.Parent;

namespace DemoAPIApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GrandParentsController : Controller
    {
        private readonly IGrandParentsRepository _grandParentsRepository;
        private readonly IChildrenRepository _childrenRepository;
        private readonly IMapper _mapper;

        public GrandParentsController(IGrandParentsRepository grandParentsRepository , IChildrenRepository childrenRepository, IMapper mapper)
        {
            _grandParentsRepository = grandParentsRepository ;
            _childrenRepository = childrenRepository ;
            _mapper = mapper;
        }
        
        [HttpGet(Name = "GetGrandParents")]
        public ActionResult<IEnumerable<Models.GrandParent>> Get(int id)
        {
            try
            {
                if (id == 0) return Ok(_grandParentsRepository.GetAll());
                var item = _grandParentsRepository.Get(id);
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
            var itemToAdd = _mapper.Map<GrandParent>(item);

            if (itemToAdd.PrimaryChildId != 0)
            {
                try
                {
                    Ok(_childrenRepository.Get(itemToAdd.PrimaryChildId));
                }
                catch (Exception)
                {
                    return BadRequest("Primary Child does not exist.");
                }

                try
                {
                    return Ok(_grandParentsRepository.Add(itemToAdd));
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
            var itemToUpdate = _mapper.Map<GrandParent>(item);
            itemToUpdate.Id = id;

            if (itemToUpdate.PrimaryChildId != 0)
            {
                try
                {
                    Ok(_childrenRepository.Get(itemToUpdate.PrimaryChildId));
                }
                catch (Exception)
                {
                    return BadRequest($"Primary Child : {itemToUpdate.PrimaryChildId} does not exist.");
                }
            }
            try
            {
                if (itemToUpdate.Name == null && itemToUpdate.Description == null)
                {
                    return Ok(_grandParentsRepository.UpdateGrandParentWithPrimaryChild(itemToUpdate.Id,
                        itemToUpdate.PrimaryChildId));
                }
                else
                {
                    return Ok(_grandParentsRepository.Update(itemToUpdate));
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
                return Ok(_grandParentsRepository.Delete(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}