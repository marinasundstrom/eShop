namespace YourBrand.Catalog.Application.Options;

public static class Mapping
{
    public static OptionDto ToDto(this Domain.Entities.Option option)
    {
        return new OptionDto(option.Id, option.Name, option.Description, (Application.OptionType)option.OptionType, option.Group == null ? null : new OptionGroupDto(option.Group.Id, option.Group.Name, option.Group.Description, option.Group.Seq, option.Group.Min, option.Group.Max), option.ItemId, option.Price, option.IsSelected,
                option.Values.Select(x => new OptionValueDto(x.Id, x.Name, x.ItemId, x.Price, x.Seq)),
                option.DefaultValue == null ? null : new OptionValueDto(option.DefaultValue.Id, option.DefaultValue.Name, option.DefaultValue.ItemId, option.DefaultValue.Price, option.DefaultValue.Seq), option.MinNumericalValue, option.MaxNumericalValue, option.DefaultNumericalValue, option.TextValueMinLength, option.TextValueMaxLength, option.DefaultTextValue);
    }

    public static OptionValueDto ToDto(this Domain.Entities.OptionValue option)
    {
        return new OptionValueDto(option.Id, option.Name, option.ItemId, option.Price, option.Seq);
    }
}
