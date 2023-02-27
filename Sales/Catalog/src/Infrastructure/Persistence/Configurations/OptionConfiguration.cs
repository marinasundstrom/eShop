using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Infrastructure.Persistence.Configurations;

public class OptionConfiguration : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> builder)
    {
        builder.ToTable("Options");

        builder.HasDiscriminator(x => x.OptionType)
            .HasValue(typeof(SelectableOption), Domain.Enums.OptionType.YesOrNo)
            .HasValue(typeof(ChoiceOption), Domain.Enums.OptionType.Choice)
            .HasValue(typeof(NumericalValueOption), Domain.Enums.OptionType.NumericalValue)
            .HasValue(typeof(TextValueOption), Domain.Enums.OptionType.TextValue);
    }
}


public class ChoiceOptionConfiguration : IEntityTypeConfiguration<ChoiceOption>
{
    public void Configure(EntityTypeBuilder<ChoiceOption> builder)
    {
        builder
            .HasMany(p => p.Values)
            .WithOne(p => p.Option);

        builder.HasOne(p => p.DefaultValue);
    }
}
