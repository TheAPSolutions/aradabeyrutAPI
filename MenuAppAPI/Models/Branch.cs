namespace AradaAPI.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string imageUrl { get; set; } = string.Empty;
        public string phoneNumber { get; set; }
        public string locationUrl { get; set; } = string.Empty;
    }
}
