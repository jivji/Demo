using DataAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Objects
{
  public interface IPrimaryMarketDataRepository
  {
	public Task<string> Get();
  }
  public class PrimaryMarketDataRepository: IPrimaryMarketDataRepository
  {
	private static readonly string ApiUrl = "https://deals.uat.ia.ipreo.com/v2/fixed-income/deal-versions?isLatestDealVersion=TRUE";
	private static readonly string PredefinedToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjM4MkVCQjdBMkMwOUFCMEE1QkFCOTFFQUE0MzQyRDhBRjQzQUQxNTUiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJPQzY3ZWl3SnF3cGJxNUhxcERRdGl2UTYwVlUifQ.eyJuYmYiOjE3MzE1OTIzNjksImV4cCI6MTczMTYyMTE2OSwiaXNzIjoiaHR0cHM6Ly9pZGVudGl0eS51YXQuaWEuaXByZW8uY29tIiwiY2xpZW50X2lkIjoiYnNwLmNsaWVudCIsImF1ZCI6Imh0dHBzOi8vaWRlbnRpdHkudWF0LmlhLmlwcmVvLmNvbS9yZXNvdXJjZXMiLCJzdWIiOiI0MmRiNjhiNS1lZjE5LTQzZWMtYjJhZi1lZTAyNWI4NmNkODEiLCJzY29wZSI6WyJpYWRlYWwiLCJucy1vcmRlcnMiXX0.EijcrTjdQ68Deg0FImm7d19LYfjt76yiaiiONbVgPd5Mwp4B0Re-BayoWYBkDw6dW1pl7PPPFHJPYLseJxPlWGv-D2ffRcdgC9pV-iHVE-M4rvfR6wH8tk7DesfeKSusnzZnh9_YdKFSEfr87hhZfQdDkwPC2XomynCWXvQc6EywCieeztrr61GP2vHTmfhZPWhDYqU1kS06uluFRgsFsc-YYadx_h3082GVM_P7ickl_amxePiXwEiXq8UI3j7j27a_no9MGsqPeL1tfM846RrL8o-xuVSpDUJcN_2d5PdAHOetIP9NFOIGLvVD0v42bXe0SRFP-QoiQcnbJvZy6Q"; // Replace with your actual token
	private static readonly string ApiKey = "mqAxMrZppFD9iRlwJt0I2e2CRe5XKEM2HGY1dNt0";

	public PrimaryMarketDataRepository()
	{
	}

	public async Task<string> Get()
	{
	  using (HttpClient client = new HttpClient())
	  {
		// Set up the authorization header with the predefined token
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PredefinedToken);
		client.DefaultRequestHeaders.Add("x-api-key", ApiKey);

		try
		{
		  // Make the GET request
		  HttpResponseMessage response = await client.GetAsync(ApiUrl);
		  response.EnsureSuccessStatusCode(); // Throws if the response is not successful

		  // Read and return the response content as a string
		  return await response.Content.ReadAsStringAsync();
		}
		catch (HttpRequestException e)
		{
		  // Handle the error as needed; for now, return an error message
		  return $"Request error: {e.Message}";
		}
	  }
	}

  }
}
