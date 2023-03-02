namespace YourBrand.Catalog;

using System;

using YourBrand.Catalog.Features.Options;

public record class ApiCreateProduct(string Name, string Handle, bool HasVariants, string? Description, string? GroupId, string? Sku, decimal? Price, decimal? CompareAtPrice, ProductVisibility? Visibility);

public record class ApiUpdateProductDetails(string Name, string? Description, string? Id, string? Image, decimal? Price, decimal? CompareAtPrice, string? GroupId);

public record class ApiCreateProductGroup(string? Id, string Name, string? Description, string? ParentGroupId);

public record class ApiUpdateProductGroup(string Name, string? Description, string? ParentGroupId);

public record class ApiCreateProductOption(string Name, string? Description, OptionType OptionType, OptionGroupDto? Group, bool? IsSelected, string? SKU, decimal? Price, string? GroupId, IEnumerable<ApiCreateProductOptionValue> Values, 
    string? DefaultOptionValueId, int? MinNumericalValue, int? MaxNumericalValue, int? DefaultNumericalValue, int? TextValueMinLength, int? TextValueMaxLength, string? DefaultTextValue);

public record class ApiAddProductAttribute(string Name, string? Description, bool ForVariant, bool IsMainAttribute, string? GroupId, IEnumerable<ApiCreateProductAttributeValue> Values);

public record class ApiCreateProductOptionValue(string Name, string? SKU, decimal? Price);

public record class ApiCreateProductAttributeValue(string Name);

public record class ApiUpdateProductOption(string Name, string? Description, bool? IsSelected, string? SKU, decimal? Price, string? GroupId, IEnumerable<ApiUpdateProductOptionValue> Values,
    string? DefaultOptionValueId, int? MinNumericalValue, int? MaxNumericalValue, int? DefaultNumericalValue, int? TextValueMinLength, int? TextValueMaxLength, string? DefaultTextValue);

public record class ApiUpdateProductAttribute(string Name, string? Description, bool ForVariant, bool IsMainAttribute, string? GroupId, IEnumerable<ApiUpdateProductAttributeValue> Values);

public record class ApiUpdateProductOptionValue(string? Id, string Name, string? SKU, decimal? Price);

public record class ApiUpdateProductAttributeValue(string? Id, string Name);


public record class ApiCreateProductOptionGroup(string Name, string? Description, int? Min, int? Max);

public record class ApiUpdateProductOptionGroup(string Name, string? Description, int? Min, int? Max);

public record class ProductVariantAttributeDto(string Id, string Name, string? Value, string? ValueId, bool IsMainAttribute);


public record class ApiCreateProductAttributeGroup(string Name, string? Description);

public record class ApiUpdateProductAttributeGroup(string Name, string? Description);



public record class ApiCreateProductVariant(string Name, string Handle, string? Description, string Id, decimal Price, decimal? CompareAtPrice, IEnumerable<ApiCreateProductVariantAttribute> Attributes);

public record class ApiCreateProductVariantAttribute(string AttributeId, string ValueId);


public record class ApiUpdateProductVariant(string Name, string? Description, string Id, decimal Price, decimal? CompareAtPrice, IEnumerable<ApiUpdateProductVariantAttribute> Attributes);

public record class ApiUpdateProductVariantAttribute(int? Id, string AttributeId, string ValueId);


public class VariantAlreadyExistsException : Exception
{
    public VariantAlreadyExistsException(string message) : base(message) { }
}


public enum OptionType { YesOrNo, Choice, NumericalValue, TextValue }

public enum ProductVisibility { Unlisted, Listed }