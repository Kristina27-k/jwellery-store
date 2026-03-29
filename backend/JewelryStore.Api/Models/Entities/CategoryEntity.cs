using System.ComponentModel.DataAnnotations;

namespace JewelryStore.Api.Models.Entities;

public class CategoryEntity
{
    public int Id { get; set; }
    public string cat_name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
