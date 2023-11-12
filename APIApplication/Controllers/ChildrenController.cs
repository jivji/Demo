using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AutoMapper;
using DataAccess;
using DataAccess.Objects;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPIApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChildrenController : Controller
    {
        private readonly IChildrenRepository _childrenRepository;
        private readonly IMapper _mapper;

        public ChildrenController(IChildrenRepository childrenRepository, IMapper mapper)
        {
            _childrenRepository = childrenRepository ;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetChildren")]
        public ActionResult<IEnumerable<Models.Child>> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    var children = _childrenRepository.GetAll();

                    var childrenToReturn = _mapper.Map<IEnumerable<Models.Child>>(children);
                    return Ok(childrenToReturn);
                }
                
                var item = _childrenRepository.Get(id);
                if (item == null)
                {
                    return NotFound();
                }

                var child = _mapper.Map<Models.Child>(item);
                
                return new List<Models.Child> {child};
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost(Name = "AddChild")]
        public ActionResult<Models.Child> Add(Models.Child item)
        {
            var itemToAdd = _mapper.Map<Child>(item);
            
            try
            {
                return Ok(_childrenRepository.Add(itemToAdd));
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut(Name = "UpdateChild")]
        public ActionResult<int> Update(Models.Child item, int id)
        {
            var itemToUpdate = _mapper.Map<Child>(item);

            try
            {
                return Ok(_childrenRepository.Update(itemToUpdate));
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete(Name = "DeleteChild")]
        public ActionResult<int> Delete(int id)
        {
            try
            {
                return Ok(_childrenRepository.Delete(id));
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}