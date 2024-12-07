using System.ComponentModel.DataAnnotations;

namespace AradaAPI.Models
{
    public class Videos
    {
        [Key]
        public int videoID { get; set; }
        public MenuItem menuItem { get; set; }
        public string videoUrlTr { get; set; } = string.Empty;
        public string videoUrlAr { get; set; } = string.Empty;
        public string videoUrlEn { get; set; } = string.Empty;
    }
}
