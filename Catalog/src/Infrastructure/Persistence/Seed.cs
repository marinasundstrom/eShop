using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YourBrand.Catalog.Domain.Entities;
using YourBrand.Catalog.Domain.Enums;

namespace YourBrand.Catalog.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        context.ItemGroups.Add(new ItemGroup()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Items",
            Description = null
        });

        context.ItemGroups.Add(new ItemGroup()
        {
            Id = "clothes",
            Name = "Clothes",
            Description = null
        });

        context.ItemGroups.Add(new ItemGroup()
        {
            Id = "food",
            Name = "Food",
            Description = null
        });

        await context.SaveChangesAsync();

        await CreateShirt2(context);

        await CreateKebabPlate(context);

        await CreateHerrgardsStek(context);

        await CreateKorg(context);

        await CreatePizza(context);

        await CreateSalad(context);

        await context.SaveChangesAsync();
    }

    public static async Task CreateShirt(ApplicationDbContext context)
    {
        var item = new Item()
        {
            Id = "t-shirt-randig",
            Name = "Randing t-shirt",
            Description = "Stilren t-shirt med randigt mönster",
            HasVariants = true,
            Group = await context.ItemGroups.FirstAsync(x => x.Name == "Clothes")
        };

        context.Items.Add(item);

        var option = new Domain.Entities.Attribute()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Size",
            ForVariant = true
        };

        item.Attributes.Add(option);

        var valueSmall = new AttributeValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Small"
        };

        option.Values.Add(valueSmall);

        var valueMedium = new AttributeValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Medium"
        };

        option.Values.Add(valueMedium);

        var valueLarge = new AttributeValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Large"
        };

        option.Values.Add(valueLarge);

        item.Attributes.Add(option);

        var variantSmall = new Item()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Small",
            GTIN = "4345547457457",
        };

        variantSmall.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = option,
            Value = valueSmall
        });

        item.Variants.Add(variantSmall);

        var variantMedium = new Item()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Medium",
            GTIN = "543453454567",
        };

        variantMedium.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = option,
            Value = valueMedium
        });

        item.Variants.Add(variantMedium);

        var variantLarge = new Item()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Large",
            GTIN = "6876345345345",
        };

        variantLarge.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = option,
            Value = valueLarge
        });

        item.Variants.Add(variantLarge);
    }

    public static async Task CreateShirt2(ApplicationDbContext context)
    {
        var item = new Item()
        {
            Id = "tshirt",
            Name = "T-shirt",
            Description = "T-shirt i olika färger",
            HasVariants = true,
            Group = await context.ItemGroups.FirstAsync(x => x.Name == "Clothes"),
            Visibility = ItemVisibility.Listed
        };

        context.Items.Add(item);

        var attr = new Domain.Entities.Attribute()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Size",
            ForVariant = true
        };

        item.Attributes.Add(attr);

        var valueSmall = new AttributeValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Small"
        };

        attr.Values.Add(valueSmall);

        var valueMedium = new AttributeValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Medium"
        };

        attr.Values.Add(valueMedium);

        var valueLarge = new AttributeValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Large"
        };

        attr.Values.Add(valueLarge);

        item.Attributes.Add(attr);

        var option2 = new Domain.Entities.Attribute()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Color",
            ForVariant = true,
            IsMainAttribute = true
        };

        item.Attributes.Add(option2);

        var valueBlue = new AttributeValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Blue"
        };

        option2.Values.Add(valueBlue);

        var valueRed = new AttributeValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Red"
        };

        option2.Values.Add(valueRed);

        ///*

        var variantBlueSmall = new Item()
        {
            Id = "tshirt-blue-small",
            Name = "Blue S",
            GTIN = "4345547457457",
            Price = 120,
        };

        variantBlueSmall.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = attr,
            Value = valueSmall
        });

        variantBlueSmall.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = option2,
            Value = valueBlue
        });

        item.Variants.Add(variantBlueSmall);

        //*/

        var variantBlueMedium = new Item()
        {
            Id = "tshirt-blue-medium",
            Name = "Blue M",
            GTIN = "543453454567",
            Price = 120
        };

        variantBlueMedium.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = attr,
            Value = valueMedium
        });

        variantBlueMedium.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = option2,
            Value = valueBlue
        });

        item.Variants.Add(variantBlueMedium);

        var variantBlueLarge = new Item()
        {
            Id = "tshirt-blue-large",
            Name = "Blue L",
            GTIN = "6876345345345",
            Price = 60,
        };

        variantBlueLarge.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = attr,
            Value = valueLarge
        });

        variantBlueLarge.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = option2,
            Value = valueBlue
        });

        item.Variants.Add(variantBlueLarge);

        /////

        var variantRedSmall = new Item()
        {
            Id = "tshirt-red-small",
            Name = "Red S",
            GTIN = "4345547457457",
            Price = 120,
        };

        variantRedSmall.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = attr,
            Value = valueSmall
        });

        variantRedSmall.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = option2,
            Value = valueRed
        });

        item.Variants.Add(variantRedSmall);

        var variantRedMedium = new Item()
        {
            Id = "tshirt-red-medium",
            Name = "Red M",
            GTIN = "543453454567",
            Price = 120,
        };

        variantRedMedium.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = attr,
            Value = valueMedium
        });

        variantRedMedium.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = option2,
            Value = valueRed
        });

        item.Variants.Add(variantRedMedium);

        var variantRedLarge = new Item()
        {
            Id = "tshirt-red-large",
            Name = "Red L",
            GTIN = "6876345345345",
            Price = 120,
        };

        variantRedLarge.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = attr,
            Value = valueLarge
        });

        variantRedLarge.AttributeValues.Add(new ItemAttributeValue()
        {
            Attribute = option2,
            Value = valueRed
        });

        item.Variants.Add(variantRedLarge);

        var textOption = new Domain.Entities.Option()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Custom text",
            OptionType = OptionType.TextValue
        };

        item.Options.Add(textOption);
    }

    public static async Task CreateKebabPlate(ApplicationDbContext context)
    {
        var item = new Item()
        {
            Id = "kebabtallrik",
            Name = "Kebabtallrik",
            Description = "Dönnerkebab, nyfriterad pommes frites, sallad, och sås",
            Price = 89,
            Group = await context.ItemGroups.FirstAsync(x => x.Name == "Food")
        };

        context.Items.Add(item);

        var option = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Sås"
        };

        item.Options.Add(option);

        await context.SaveChangesAsync();

        var valueSmall = new OptionValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Mild sås"
        };

        option.Values.Add(valueSmall);

        var valueMedium = new OptionValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Stark sås"
        };

        option.Values.Add(valueMedium);

        var valueLarge = new OptionValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Blandad sås"
        };

        option.DefaultValue = valueSmall;

        option.Values.Add(valueLarge);
    }

    public static async Task CreateHerrgardsStek(ApplicationDbContext context)
    {
        var item = new Item()
        {
            Id = "herrgardsstek",
            Name = "Herrgårdsstek",
            Description = "Vår fina stek med pommes och vår hemlagade bearnaise sås",
            Price = 179,
            Group = await context.ItemGroups.FirstAsync(x => x.Name == "Food")
        };

        context.Items.Add(item);

        await context.SaveChangesAsync();

        var optionDoneness = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Stekning"
        };

        item.Options.Add(optionDoneness);

        await context.SaveChangesAsync();

        optionDoneness.Values.Add(new OptionValue()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Rare",
            Seq = 1
        });

        var optionMediumRare = new OptionValue()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Medium Rare",
            Seq = 2
        };

        optionDoneness.Values.Add(optionMediumRare);

        optionDoneness.Values.Add(new OptionValue()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Well Done",
            Seq = 3
        });

        optionDoneness.DefaultValue = optionMediumRare;

        var optionSize = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Extra stor - 50 g mer",
            Price = 15
        };

        item.Options.Add(optionSize);

        await context.SaveChangesAsync();
    }

    public static async Task CreateKorg(ApplicationDbContext context)
    {
        var item = new Item()
        {
            Id = "korg",
            Name = "Korg",
            Description = "En korg med smårätter",
            Price = 179,
            Group = await context.ItemGroups.FirstAsync(x => x.Name == "Food")
        };

        context.Items.Add(item);

        await context.SaveChangesAsync();

        var ratterGroup = new OptionGroup()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Rätter",
            Max = 7
        };

        item.OptionGroups.Add(ratterGroup);

        var optionFalafel = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Falafel",
            OptionType = OptionType.NumericalValue,
            Group = ratterGroup
        };

        item.Options.Add(optionFalafel);

        var optionChickenWing = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Spicy Chicken Wing",
            OptionType = OptionType.NumericalValue,
            Group = ratterGroup
        };

        item.Options.Add(optionChickenWing);

        var optionRib = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Rib",
            OptionType = OptionType.NumericalValue,
            Group = ratterGroup
        };

        item.Options.Add(optionRib);

        await context.SaveChangesAsync();


        var extraGroup = new OptionGroup()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Extra"
        };

        item.OptionGroups.Add(extraGroup);

        await context.SaveChangesAsync();

        var optionSauce = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Sås",
            OptionType = OptionType.YesOrNo,
            Price = 10,
            Group = extraGroup
        };

        item.Options.Add(optionSauce);

        /*
        optionSauce.Values.Add(new OptionValue() {
            Name = "Favoritsås", 
        });
        */

        await context.SaveChangesAsync();
    }

    public static async Task CreatePizza(ApplicationDbContext context)
    {
        var item = new Item()
        {
            Id = "pizza",
            Name = "Pizza",
            Description = "Custom pizza",
            Price = 40,
            Group = await context.ItemGroups.FirstAsync(x => x.Name == "Food")
        };

        context.Items.Add(item);

        await context.SaveChangesAsync();

        var breadGroup = new OptionGroup()
        {
            Id = Guid.NewGuid().ToString(),
            Seq = 1,
            Name = "Bread"
        };

        item.OptionGroups.Add(breadGroup);

        var meatGroup = new OptionGroup()
        {
            Id = Guid.NewGuid().ToString(),
            Seq = 2,
            Name = "Meat",
            Max = 2
        };

        item.OptionGroups.Add(meatGroup);

        var nonMeatGroup = new OptionGroup()
        {
            Id = Guid.NewGuid().ToString(),
            Seq = 3,
            Name = "Non-Meat"
        };

        item.OptionGroups.Add(nonMeatGroup);

        var sauceGroup = new OptionGroup()
        {
            Id = Guid.NewGuid().ToString(),
            Seq = 4,
            Name = "Sauce"
        };

        item.OptionGroups.Add(sauceGroup);

        var toppingsGroup = new OptionGroup()
        {
            Id = Guid.NewGuid().ToString(),
            Seq = 5,
            Name = "Toppings"
        };

        item.OptionGroups.Add(toppingsGroup);

        await context.SaveChangesAsync();

        var optionStyle = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Style"
        };

        item.Options.Add(optionStyle);

        await context.SaveChangesAsync();

        var valueItalian = new OptionValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Italian"
        };

        optionStyle.Values.Add(valueItalian);

        var valueAmerican = new OptionValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "American"
        };

        optionStyle.DefaultValue = valueAmerican;

        optionStyle.Values.Add(valueAmerican);

        var optionHam = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Ham",
            Group = meatGroup,
            Price = 15
        };

        item.Options.Add(optionHam);

        var optionKebab = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Kebab",
            Group = meatGroup,
            Price = 10,
            IsSelected = true
        };

        item.Options.Add(optionKebab);

        var optionChicken = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Chicken",
            Group = meatGroup,
            Price = 10
        };

        item.Options.Add(optionChicken);

        var optionExtraCheese = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Extra cheese",
            Group = toppingsGroup,
            Price = 5
        };

        item.Options.Add(optionExtraCheese);

        var optionGreenOlives = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Green Olives",
            Group = toppingsGroup,
            Price = 5
        };

        item.Options.Add(optionGreenOlives);
    }

    public static async Task CreateSalad(ApplicationDbContext context)
    {
        var item = new Item()
        {
            Id = "sallad",
            Name = "Sallad",
            Description = "Din egna sallad",
            Price = 52,
            Group = await context.ItemGroups.FirstAsync(x => x.Name == "Food"),
            Visibility = ItemVisibility.Listed
        };

        context.Items.Add(item);

        var baseGroup = new OptionGroup()
        {
            Id = Guid.NewGuid().ToString(),
            Seq = 1,
            Name = "Bas"
        };

        item.OptionGroups.Add(baseGroup);

        var proteinGroup = new OptionGroup()
        {
            Id = Guid.NewGuid().ToString(),
            Seq = 2,
            Name = "Välj protein",
            Max = 1
        };

        item.OptionGroups.Add(proteinGroup);

        var additionalGroup = new OptionGroup()
        {
            Id = Guid.NewGuid().ToString(),
            Seq = 4,
            Name = "Välj tillbehör",
            Max = 3
        };

        item.OptionGroups.Add(additionalGroup);

        var dressingGroup = new OptionGroup()
        {
            Id = Guid.NewGuid().ToString(),
            Seq = 5,
            Name = "Välj dressing",
            Max = 1
        };

        item.OptionGroups.Add(dressingGroup);

        await context.SaveChangesAsync();

        var optionBase = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Bas",
            Group = baseGroup
        };

        item.Options.Add(optionBase);

        await context.SaveChangesAsync();

        var valueSallad = new OptionValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Sallad",
        };

        optionBase.Values.Add(valueSallad);

        var valueSalladPasta = new OptionValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Sallad med pasta"
        };

        optionBase.DefaultValue = valueSalladPasta;

        optionBase.Values.Add(valueSalladPasta);

        var valueSalladQuinoa = new OptionValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Sallad med quinoa",
        };

        optionBase.Values.Add(valueSalladQuinoa);

        var valueSalladNudlar = new OptionValue
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Sallad med glasnudlar",
        };

        optionBase.Values.Add(valueSalladNudlar);

        var optionChicken = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Kycklingfilé",
            Group = proteinGroup
        };

        item.Options.Add(optionChicken);

        var optionSmokedTurkey = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Rökt kalkonfilé",
            Group = proteinGroup
        };

        item.Options.Add(optionSmokedTurkey);

        var optionBeanMix = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Marinerad bönmix",
            Group = proteinGroup
        };

        item.Options.Add(optionBeanMix);

        var optionVegMe = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "VegMe",
            Group = proteinGroup
        };

        item.Options.Add(optionVegMe);

        var optionChevre = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Chevré",
            Group = proteinGroup
        };

        item.Options.Add(optionChevre);

        var optionSmokedSalmon = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Varmrökt lax",
            Group = proteinGroup
        };

        item.Options.Add(optionSmokedSalmon);

        var optionPrawns = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Handskalade räkor",
            Group = proteinGroup
        };

        item.Options.Add(optionPrawns);

        var optionCheese = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Parmesanost",
            Group = additionalGroup
        };

        item.Options.Add(optionCheese);

        var optionGreenOlives = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Gröna oliver",
            Group = additionalGroup
        };

        item.Options.Add(optionGreenOlives);

        var optionSoltorkadTomat = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Soltorkade tomater",
            Group = additionalGroup
        };

        item.Options.Add(optionSoltorkadTomat);

        var optionInlagdRödlök = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Inlagd rödlök",
            Group = additionalGroup
        };

        item.Options.Add(optionInlagdRödlök);

        var optionRostadAioli = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Rostad aioli",
            Group = dressingGroup
        };

        item.Options.Add(optionRostadAioli);

        var optionPesto = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Pesto",
            Group = dressingGroup
        };

        item.Options.Add(optionPesto);

        var optionOrtvinagret = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Örtvinägrett",
            Group = dressingGroup
        };

        item.Options.Add(optionOrtvinagret);

        var optionSoyavinagret = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Soyavinägrett",
            Group = dressingGroup
        };

        item.Options.Add(optionSoyavinagret);

        var optionRhodeIsland = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Rhode Island",
            Group = dressingGroup
        };

        item.Options.Add(optionRhodeIsland);

        var optionKimchimayo = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Kimchimayo",
            Group = dressingGroup
        };

        item.Options.Add(optionKimchimayo);

        var optionCaesar = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Caesar",
            Group = dressingGroup
        };

        item.Options.Add(optionCaesar);

        var optionCitronLime = new Option()
        {
            Id = Guid.NewGuid().ToString(),
            OptionType = OptionType.YesOrNo,
            Name = "Citronlime",
            Group = dressingGroup
        };

        item.Options.Add(optionCitronLime);
    }
}