namespace DemoAPIApplication.Models
{
  using Newtonsoft.Json;
  using System;
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
	[JsonProperty("name")]
	public string IssuerName { get; set; }
	public string Name { get; set; }
	public string MidName { get; set; }
	public string MacroName { get; set; }
	public string PaymentType { get; set; }
	public string Frequency { get; set; }
	public string Index { get; set; }
	public DateTime? FirstCouponDate { get; set; }
	public double? CouponAmount { get; set; }
	public double? Spread { get; set; }
  }
}
