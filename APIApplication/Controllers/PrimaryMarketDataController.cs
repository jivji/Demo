using AutoMapper;
using DataAccess;
using DataAccess.Objects;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DemoAPIApplication.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class PrimaryMarketDataController : Controller
  {
	private readonly IPrimaryMarketDataRepository _primaryMarketDataRepository;
	private readonly IMapper _mapper;

	public PrimaryMarketDataController(IPrimaryMarketDataRepository primaryMarketDataRepository, IMapper mapper)
	{
	  _primaryMarketDataRepository = primaryMarketDataRepository;
	  _mapper = mapper;
	}


	[HttpGet(Name = "GetPrimaryMarketData")]
	public async Task<ActionResult<IEnumerable<Models.DealVersion>>> Get()
	{
	  var primaryMarketData = await _primaryMarketDataRepository.Get();
	  return Ok(primaryMarketData);
	}
  }
}
