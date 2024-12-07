using AradaAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class MenuItem
{
    public int Id { get; set; }
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;

    public string DescriptionTr { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionAr { get; set; } = string.Empty;

    public int PriceTr { get; set; }
    public int PriceEn { get; set; }
    public int PriceAr { get; set; }
    public int ItemOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool isVisible { get; set; }

    [ForeignKey("Category")]
    public int? categoryId { get; set; }
    public Categories Category { get; set; }


    public Videos Video { get; set; }
    public string? imageUrl { get; set; }

}
