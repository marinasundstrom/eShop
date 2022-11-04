namespace YourBrand.Catalog.Application;

using System;
using YourBrand.Catalog.Application.Options;

public record class ApiCreateItem(string Name, bool HasVariants, string? Description, string? GroupId, string? Id, decimal? Price, decimal? CompareAtPrice, ItemVisibility? Visibility);

public record class ApiUpdateItemDetails(string Name, string? Description, string? Id, string? Image, decimal? Price, decimal? CompareAtPrice, string? GroupId);

public record class ApiCreateItemGroup(string? Id, string Name, string? Description, string? ParentGroupId);

public record class ApiUpdateItemGroup(string Name, string? Description, string? ParentGroupId);

public record class ApiCreateItemOption(string Name, string? Description, OptionType OptionType, OptionGroupDto? Group, bool IsSelected, string? ItemId, decimal? Price, string? GroupId, IEnumerable<ApiCreateItemOptionValue> Values, string? DefaultOptionValueId);

public record class ApiCreateItemAttribute(string Name, string? Description, bool ForVariant, bool IsMainAttribute, string? GroupId, IEnumerable<ApiCreateItemAttributeValue> Values);


public record class ApiCreateItemOptionValue(string Name, string? ItemId, decimal? Price);

public record class ApiCreateItemAttributeValue(string Name);

public record class ApiUpdateItemOption(string Name, string? Description, OptionType OptionType, bool IsSelected, string? ItemId, decimal? Price, string? GroupId, IEnumerable<ApiUpdateItemOptionValue> Values, string? DefaultOptionValueId);

public record class ApiUpdateItemAttribute(string Name, string? Description, bool ForVariant, bool IsMainAttribute, string? GroupId, IEnumerable<ApiUpdateItemAttributeValue> Values);

public record class ApiUpdateItemOptionValue(string? Id, string Name, string? ItemId, decimal? Price);

public record class ApiUpdateItemAttributeValue(string? Id, string Name);


public record class ApiCreateItemOptionGroup(string Name, string? Description, int? Min, int? Max);

public record class ApiUpdateItemOptionGroup(string Name, string? Description, int? Min, int? Max);

public record class ItemVariantAttributeDto(string Id, string Name, string Value, string? ValueId, bool IsMainAttribute);


public record class ApiCreateItemAttributeGroup(string Name, string? Description);

public record class ApiUpdateItemAttributeGroup(string Name, string? Description);



public record class ApiCreateItemVariant(string Name, string? Description, string Id, decimal Price, decimal? CompareAtPrice, IEnumerable<ApiCreateItemVariantAttribute> Attributes);

public record class ApiCreateItemVariantAttribute(string OptionId, string ValueId);


public record class ApiUpdateItemVariant(string Name, string? Description, string Id, decimal Price, decimal? CompareAtPrice, IEnumerable<ApiUpdateItemVariantAttribute> Attributes);

public record class ApiUpdateItemVariantAttribute(int? Id, string AttributeId, string ValueId);


public class VariantAlreadyExistsException : Exception
{
    public VariantAlreadyExistsException(string message) : base(message) { }
}


public enum OptionType { YesOrNo, Choice, NumericalValue, TextValue }

public enum ItemVisibility { Unlisted, Listed }

