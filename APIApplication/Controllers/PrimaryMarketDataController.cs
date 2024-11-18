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
	
	public PrimaryMarketDataController(IPrimaryMarketDataRepository primaryMarketDataRepository)
	{
	  _primaryMarketDataRepository = primaryMarketDataRepository;
	}


	[HttpGet(Name = "GetPrimaryMarketData")]
	public async Task<ActionResult<IEnumerable<Models.DealVersion>>> Get()
	{
	  var primaryMarketDataString = await _primaryMarketDataRepository.Get();
	  var primaryMarketData = ExtractDealVersions(primaryMarketDataString);
	  return Ok(primaryMarketData);
	}

	private List<Models.DealVersion> ExtractDealVersions(string data)
	{
		try
		{
		  var jsonObject = JObject.Parse(data);
		  var dealVersionsArray = jsonObject["values"]["dealVersions"].ToString();
		  var dealVersions = JsonConvert.DeserializeObject<List<Models.DealVersion>>(dealVersionsArray);
		  return dealVersions.Select(dv => new Models.DealVersion
		  {
			Id = dv.Id,
			DealId = dv.DealId,
			ActualSize = dv.ActualSize,
			CurrencyCode = dv.CurrencyCode,
			MaturityTerm = dv.tranches?.FirstOrDefault()?.MaturityTerm ?? 0,
			SettlementDate = dv.tranches?.FirstOrDefault().SettlementDate ?? null,
			MaturityDate = dv.tranches?.FirstOrDefault().MaturityDate ?? null,
			MinimumDenomination = dv.tranches?.FirstOrDefault().MinimumDenomination,
			MultipleDenomination = dv.tranches?.FirstOrDefault().MultipleDenomination,
			MinimumOrderSize = dv.tranches?.FirstOrDefault().MinimumOrderSize,
			Moodys = dv.tranches?.FirstOrDefault().Ratings?.Moodys,
			Sp = dv.tranches?.FirstOrDefault().Ratings?.Sp,
			Fitch = dv.tranches?.FirstOrDefault().Ratings?.Fitch,
			IssuerName = dv.tranches?.FirstOrDefault().Issuer?.Name,
			Name = dv.tranches?.FirstOrDefault().Issuer.Industry?.Name,
			MidName = dv.tranches?.FirstOrDefault().Issuer.Industry?.MidName,
			MacroName = dv.tranches?.FirstOrDefault().Issuer.Industry?.MacroName,
			PaymentType = dv.tranches?.FirstOrDefault().Coupon?.PaymentType,
			Frequency = dv.tranches?.FirstOrDefault().Coupon?.Frequency,
			Index = dv.tranches?.FirstOrDefault().Coupon?.Index,
			FirstCouponDate = dv.tranches?.FirstOrDefault().Coupon?.FirstCouponDate,
			CouponAmount = dv.tranches?.FirstOrDefault().Coupon?.CouponAmount,
			Spread = dv.tranches?.FirstOrDefault().Coupon?.Spread,
		  }).ToList();
		}
		catch (JsonException ex)
		{
		  // Log the exception or handle it as needed
		  Console.WriteLine($"JSON parsing error: {ex.Message}");
		  return new List<Models.DealVersion>(); // Return an empty list or handle the error appropriately
		}
	  }
	}
}
