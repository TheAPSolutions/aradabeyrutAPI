using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AradaAPI.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }

        public string NameEn { get; set; }

        public string NameTr { get; set; }

        public string NameAr { get; set; }

        public int CategoryOrder { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CategoryImage { get; set; }

        public bool IsVisible { get; set; }

        public ICollection<MenuItem>? MenuItems { get; set; }
    }
}
