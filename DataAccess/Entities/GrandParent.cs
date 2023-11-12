namespace DataAccess.Objects
{
    public class GrandParent
    {
        public int Id { get; set; }
        public int PrimaryChildId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}