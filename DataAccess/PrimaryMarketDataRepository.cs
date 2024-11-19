using DataAccess.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccess.Objects
{
  public interface IPrimaryMarketDataRepository
  {
	public Task<List<DealVersion>> Get();
  }
  public class PrimaryMarketDataRepository: IPrimaryMarketDataRepository
  {
	private static readonly string ApiUrl = "https://deals.uat.ia.ipreo.com/v2/fixed-income/deal-versions?isLatestDealVersion=TRUE";
	private static readonly string PredefinedToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjM4MkVCQjdBMkMwOUFCMEE1QkFCOTFFQUE0MzQyRDhBRjQzQUQxNTUiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJPQzY3ZWl3SnF3cGJxNUhxcERRdGl2UTYwVlUifQ.eyJuYmYiOjE3MzE1OTIzNjksImV4cCI6MTczMTYyMTE2OSwiaXNzIjoiaHR0cHM6Ly9pZGVudGl0eS51YXQuaWEuaXByZW8uY29tIiwiY2xpZW50X2lkIjoiYnNwLmNsaWVudCIsImF1ZCI6Imh0dHBzOi8vaWRlbnRpdHkudWF0LmlhLmlwcmVvLmNvbS9yZXNvdXJjZXMiLCJzdWIiOiI0MmRiNjhiNS1lZjE5LTQzZWMtYjJhZi1lZTAyNWI4NmNkODEiLCJzY29wZSI6WyJpYWRlYWwiLCJucy1vcmRlcnMiXX0.EijcrTjdQ68Deg0FImm7d19LYfjt76yiaiiONbVgPd5Mwp4B0Re-BayoWYBkDw6dW1pl7PPPFHJPYLseJxPlWGv-D2ffRcdgC9pV-iHVE-M4rvfR6wH8tk7DesfeKSusnzZnh9_YdKFSEfr87hhZfQdDkwPC2XomynCWXvQc6EywCieeztrr61GP2vHTmfhZPWhDYqU1kS06uluFRgsFsc-YYadx_h3082GVM_P7ickl_amxePiXwEiXq8UI3j7j27a_no9MGsqPeL1tfM846RrL8o-xuVSpDUJcN_2d5PdAHOetIP9NFOIGLvVD0v42bXe0SRFP-QoiQcnbJvZy6Q"; // Replace with your actual token
	private static readonly string ApiKey = "mqAxMrZppFD9iRlwJt0I2e2CRe5XKEM2HGY1dNt0";
	private static readonly string Username = "api.test@simcorp.com";
	private static readonly string Password = "$vvPhurx8";
	private static readonly string AuthUrl = "https://deals.uat.ia.ipreo.com/fixed-income/v1/token";
	private static string? Token;
	private static DateTime TokenExpiry;
	private static int ExpiresIn = 28800; // 8 hours in seconds

	public PrimaryMarketDataRepository()
	{
	}

	public async Task<List<DealVersion>> Get()
	{
	  if (string.IsNullOrEmpty(Token) || DateTime.UtcNow >= TokenExpiry)
	  {
		await RefreshTokenAsync();
	  }

	  using (HttpClient client = new HttpClient())
	  {
		// Set up the authorization header with the predefined token
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
		client.DefaultRequestHeaders.Add("x-api-key", ApiKey);

		try
		{
		  // Make the GET request
		  HttpResponseMessage response = await client.GetAsync(ApiUrl);		  
		  response.EnsureSuccessStatusCode(); // Throws if the response is not successful

		  // Read and return the response content as a string
		  string responseData = await response.Content.ReadAsStringAsync();

		  // Extract the DealVersions from the response data
		  List<DealVersion> dealVersions = ExtractDealVersions(responseData);
		  return dealVersions;
		}
		catch (HttpRequestException e)
		{
		  // Handle the error as needed; for now, return an error message
		  throw new Exception($"Request error: {e.Message}");
		}
	  }
	}

	private static async Task RefreshTokenAsync()
	{

	  using (HttpClient client = new HttpClient())
	  {
		var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Username}:{Password}"));
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

		// Add the API key as a header for the token request
		client.DefaultRequestHeaders.Add("x-api-key", ApiKey);

		try
		{
		  // Log the attempt to obtain a token
		  Console.WriteLine("Requesting new token...");

		  HttpResponseMessage response = await client.PostAsync(AuthUrl, null);
		  response.EnsureSuccessStatusCode();

		  var responseBody = await response.Content.ReadAsStringAsync();
		  Console.WriteLine("Token response received: " + responseBody);

		  Token = responseBody;
		  TokenExpiry = DateTime.UtcNow.AddSeconds(ExpiresIn).AddMinutes(-5); // Add buffer

		  Console.WriteLine($"New token acquired. Expires at: {TokenExpiry}");
		}
		catch (HttpRequestException e)
		{
		  Console.WriteLine($"Token request error: {e.Message}");
		  throw;
		}
	  }
	}	

	private List<DealVersion> ExtractDealVersions(string data)
	{
	  try
	  {
		var jsonObject = JObject.Parse(data);
		var dealVersions = jsonObject["values"]["dealVersions"].ToString();
		var dealVersionsArray = JsonConvert.DeserializeObject<List<DataAccess.Objects.DealVersion>>(dealVersions);
		return dealVersionsArray.Select(dv => new DealVersion
		{
		  Id = dv.Id,
		  DealId = dv.DealId,
		  ActualSize = dv.ActualSize,
		  CurrencyCode = dv.CurrencyCode,
		  MaturityTerm = dv.Tranches?.FirstOrDefault()?.MaturityTerm ?? 0,
		  SettlementDate = dv.Tranches?.FirstOrDefault().SettlementDate ?? null,
		  MaturityDate = dv.Tranches?.FirstOrDefault().MaturityDate ?? null,
		  MinimumDenomination = dv.Tranches?.FirstOrDefault().MinimumDenomination,
		  MultipleDenomination = dv.Tranches?.FirstOrDefault().MultipleDenomination,
		  MinimumOrderSize = dv.Tranches?.FirstOrDefault().MinimumOrderSize,
		  Moodys = dv.Tranches?.FirstOrDefault().Ratings?.Moodys,
		  Sp = dv.Tranches?.FirstOrDefault().Ratings?.Sp,
		  Fitch = dv.Tranches?.FirstOrDefault().Ratings?.Fitch,
		  IssuerName = dv.Tranches?.FirstOrDefault().Issuer?.Name,
		  Name = dv.Tranches?.FirstOrDefault().Issuer.Industry?.Name,
		  MidName = dv.Tranches?.FirstOrDefault().Issuer.Industry?.MidName,
		  MacroName = dv.Tranches?.FirstOrDefault().Issuer.Industry?.MacroName,
		  PaymentType = dv.Tranches?.FirstOrDefault().Coupon?.PaymentType,
		  Frequency = dv.Tranches?.FirstOrDefault().Coupon?.Frequency,
		  Index = dv.Tranches?.FirstOrDefault().Coupon?.Index,
		  FirstCouponDate = dv.Tranches?.FirstOrDefault().Coupon?.FirstCouponDate,
		  CouponAmount = dv.Tranches?.FirstOrDefault().Coupon?.CouponAmount,
		  Spread = dv.Tranches?.FirstOrDefault().Coupon?.Spread,
		  IPT = dv.Tranches.FirstOrDefault().Pricing?.IPT,
		  Guidance = dv.Tranches.FirstOrDefault().Pricing?.Guidance,
		  ISIN = dv.Tranches.FirstOrDefault().Securities?.FirstOrDefault().ISIN,
		  CUSIP = dv.Tranches.FirstOrDefault().Securities?.FirstOrDefault().CUSIP,
		  FIGI = dv.Tranches.FirstOrDefault().Securities?.FirstOrDefault().FIGI,
		}).ToList();
	  }
	  catch (Newtonsoft.Json.JsonException ex)
	  {
		// Log the exception or handle it as needed
		Console.WriteLine($"JSON parsing error: {ex.Message}");
		return new List<DealVersion>(); // Return an empty list or handle the error appropriately
	  }
	}
  }
}

