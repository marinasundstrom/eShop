namespace YourBrand.Catalog.Domain.Entities;

public class ProductGroupAttribute : Entity<Guid>
{
    public string ProductGroupId { get; set; } = null!;

    public ProductGroup ProductGroup { get; set; } = null!;

    //public ProductGroupAttribute InheritedFromId { get; set; } = null!;

    //public ProductGroupAttribute InheritedFrom { get; set; } = null!;

    public string AttributeId { get; set; } = null!;

    public Attribute Attribute { get; set; } = null!;
}
