using AutoMapper;
using DataAccess;
using DataAccess.Objects;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPIApplication.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class PrimaryMarketDataController : Controller
  {

	private readonly IPrimaryMarketDataRepository _primaryMarketDataRepository;
	
	public PrimaryMarketDataController(IPrimaryMarketDataRepository primaryMarketDataRepository)
	{
	  _primaryMarketDataRepository = primaryMarketDataRepository;
	}

	[HttpGet(Name = "GetPrimaryMarketData")]
	public async Task<ActionResult<string>> Get()
	{	  
	  return await _primaryMarketDataRepository.Get();
	}
  }
}
