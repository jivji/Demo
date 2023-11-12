namespace DataAccess
{
  public abstract class Configuration
  {
    public static string GetConnectionString()
    {
      return "Server=DK01WP6649\\SQLEXPRESS;Database=DEMO;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;";
    }
  }
}