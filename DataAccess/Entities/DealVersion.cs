using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Objects
{
  public class DealVersion
  {
	public int Id { get; set; }
	public int DealId { get; set; }
	public string CurrencyCode { get; set; }
	public decimal? ActualSize { get; set; }
	public decimal? MaturityTerm { get; set; }
	public DateTime? SettlementDate { get; set; }
	public DateTime? MaturityDate { get; set; }
	public decimal? MinimumDenomination { get; set; }
	public decimal? MultipleDenomination { get; set; }
	public decimal? MinimumOrderSize { get; set; }
	public string Moodys { get; set; }
	public string Sp { get; set; }
	public string Fitch { get; set; }
	//[JsonProperty("name")]
	public string IssuerName { get; set; }
	public string Name { get; set; }
	public string MidName { get; set; }
	public string MacroName { get; set; }
	public string PaymentType { get; set; }
	public string Frequency { get; set; }
	public string Index { get; set; }
	public DateTime? FirstCouponDate { get; set; }
	public double? CouponAmount { get; set; }
	[JsonIgnore]
	public double? Spread { get; set; }
	[JsonIgnore]
	public List<Tranche> tranches { get; set; }
  }

  public class Tranche
  {
	public decimal? MaturityTerm { get; set; }
	public DateTime? SettlementDate { get; set; }
	public DateTime? MaturityDate { get; set; }
	public decimal? MinimumDenomination { get; set; }
	public decimal? MultipleDenomination { get; set; }
	public decimal? MinimumOrderSize { get; set; }
	public Ratings Ratings { get; set; }
	public Issuer Issuer { get; set; }
	public Coupon Coupon { get; set; }

  }

  public class Ratings
  {
	public string Moodys { get; set; }
	public string Sp { get; set; }
	public string Fitch { get; set; }
  }
  public class Issuer
  {
	public string Name { get; set; }
	public Industry Industry { get; set; }
  }

  public class Industry
  {
	public string Name { get; set; }
	public string MidName { get; set; }
	public string MacroName { get; set; }
  }

  public class Coupon
  {
	public string PaymentType { get; set; }
	public string Frequency { get; set; }
	public string Index { get; set; }
	public DateTime? FirstCouponDate { get; set; }
	//[JsonProperty("coupon")]
	public double? CouponAmount { get; set; }
	public double? Spread { get; set; }
  }
}

