using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Variants;

public static class Mappings
{
    public static ItemVariantAttributeDto ToDto(this ItemAttributeValue x)
    {
        return new ItemVariantAttributeDto(x.Attribute.Id, x.Attribute.Name, x.Value.Name, x.Value?.Id, x.Attribute.IsMainAttribute);
    }
}