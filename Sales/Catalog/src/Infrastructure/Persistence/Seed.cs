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
        context.Stores.Add(new Store("Joes", "joes"));

        await context.SaveChangesAsync();

        context.Brands.Add(new Brand("myBrand", "MyBrand"));

        await context.SaveChangesAsync();

        context.ProductGroups.Add(new ProductGroup("clothes", "Clothes")
        {
            Description = null,
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        });

        context.ProductGroups.Add(new ProductGroup("food", "Food")
        {
            Description = null,
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
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

    public static async Task CreateShirt2(ApplicationDbContext context)
    {
        var sizeAttribute = new Domain.Entities.Attribute("Size");

        context.Attributes.Add(sizeAttribute);
        var valueSmall = new AttributeValue("Small");

        sizeAttribute.Values.Add(valueSmall);
        var valueMedium = new AttributeValue("Medium");

        sizeAttribute.Values.Add(valueMedium);
        var valueLarge = new AttributeValue("Large");

        sizeAttribute.Values.Add(valueLarge);
        context.Attributes.Add(sizeAttribute);

        var colorAttribute = new Domain.Entities.Attribute("Color");
        context.Attributes.Add(colorAttribute);

        var valueBlue = new AttributeValue("Blue");
        colorAttribute.Values.Add(valueBlue);

        var valueRed = new AttributeValue("Red");
        colorAttribute.Values.Add(valueRed);

        var item = new Product("tshirt", "T-shirt")
        {
            Description = "T-shirt i olika färger",
            HasVariants = true,
            Group = await context.ProductGroups.FirstAsync(x => x.Name == "Clothes"),
            Visibility = YourBrand.Catalog.Domain.Enums.ProductVisibility.Listed,
            Brand = await context.Brands.FirstAsync(x => x.Id == "myBrand"),
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        context.Products.Add(item);

        item.ProductAttributes.Add(new ProductAttribute {
            ForVariant = true,
            IsMainAttribute = false,
            Attribute = colorAttribute,
            Value = null
        });

        item.ProductAttributes.Add(new ProductAttribute
        {
            ForVariant = true,
            IsMainAttribute = true,
            Attribute = sizeAttribute,
            Value = null
        });

        ///*

        var variantBlueSmall = new Product("tshirt-blue-small", "Blue S")
        {
            GTIN = "4345547457457",
            Price = 120,
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        variantBlueSmall.ProductAttributes.Add(new ProductAttribute
        {
            Attribute = sizeAttribute,
            Value = valueSmall,
            ForVariant = true
        });

        variantBlueSmall.ProductAttributes.Add(new ProductAttribute
        {
            Attribute = colorAttribute,
            Value = valueBlue,
            ForVariant = true,
            IsMainAttribute = true
        });

        item.Variants.Add(variantBlueSmall);

        //*/

        var variantBlueMedium = new Product("tshirt-blue-medium", "Blue M")
        {
            GTIN = "543453454567",
            Price = 120,
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        variantBlueMedium.ProductAttributes.Add(new ProductAttribute
        {
            Attribute = sizeAttribute,
            Value = valueMedium,
            ForVariant = true
        });

        variantBlueMedium.ProductAttributes.Add(new ProductAttribute
        {
            Attribute = colorAttribute,
            Value = valueBlue,
            ForVariant = true,
            IsMainAttribute = true
        });

        item.Variants.Add(variantBlueMedium);

        var variantBlueLarge = new Product("tshirt-blue-large", "Blue L")
        {
            GTIN = "6876345345345",
            Price = 60,
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        variantBlueLarge.ProductAttributes.Add(new ProductAttribute
        {
            Attribute = sizeAttribute,
            Value = valueLarge,  
            ForVariant = true
        });

        variantBlueLarge.ProductAttributes.Add(new ProductAttribute
        {
            Attribute = colorAttribute,
            Value = valueBlue,
            ForVariant = true,
            IsMainAttribute = true
        });

        item.Variants.Add(variantBlueLarge);

        /////

        var variantRedSmall = new Product("tshirt-red-small", "Red S")
        {
            GTIN = "4345547457457",
            Price = 120,
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        variantRedSmall.ProductAttributes.Add(new ProductAttribute
        {
            Attribute = sizeAttribute,
            Value = valueSmall,
            ForVariant = true
        });

        variantRedSmall.ProductAttributes.Add(new ProductAttribute
        {
            Attribute = colorAttribute,
            Value = valueRed,
            ForVariant = true,
            IsMainAttribute = true
        });

        item.Variants.Add(variantRedSmall);

        var variantRedMedium = new Product("tshirt-red-medium", "Red M")
        {
            GTIN = "543453454567",
            Price = 120,
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        variantRedMedium.ProductAttributes.Add(new ProductAttribute
        {
            Attribute = sizeAttribute,
            Value = valueMedium,     
            ForVariant = true,
        });

        variantRedMedium.ProductAttributes.Add(new ProductAttribute
        {
            Attribute = colorAttribute,
            Value = valueRed,
            ForVariant = true,
            IsMainAttribute = true
        });

        item.Variants.Add(variantRedMedium);

        var variantRedLarge = new Product("tshirt-red-large", "Red L")
        {
            GTIN = "6876345345345",
            Price = 120,
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        variantRedLarge.ProductAttributes.Add(new ProductAttribute
        {
            Attribute = sizeAttribute,
            Value = valueLarge,
            ForVariant = true
        });

        variantRedLarge.ProductAttributes.Add(new ProductAttribute
        {
            Attribute = colorAttribute,
            Value = valueRed,
            ForVariant = true,
            IsMainAttribute = true
        });

        item.Variants.Add(variantRedLarge);

        var textOption = new Domain.Entities.Option("Custom text")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.TextValue
        };

        item.Options.Add(textOption);
    }

    public static async Task CreateKebabPlate(ApplicationDbContext context)
    {
        var item = new Product("kebabtallrik", "Kebabtallrik")
        {
            Description = "Dönnerkebab, nyfriterad pommes frites, sallad, och sås",
            Price = 89,
            Group = await context.ProductGroups.FirstAsync(x => x.Name == "Food"),
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        context.Products.Add(item);

        var option = new Option("Sås");
        item.Options.Add(option);

        await context.SaveChangesAsync();

        var valueSmall = new OptionValue("Mild sås");

        option.Values.Add(valueSmall);

        var valueMedium = new OptionValue("Stark sås");

        option.Values.Add(valueMedium);

        var valueLarge = new OptionValue("Blandad sås");

        option.DefaultValue = valueSmall;

        option.Values.Add(valueLarge);
    }

    public static async Task CreateHerrgardsStek(ApplicationDbContext context)
    {
        var item = new Product("herrgardsstek", "Herrgårdsstek")
        {
            Description = "Vår fina stek med pommes och vår hemlagade bearnaise sås",
            Price = 179,
            Group = await context.ProductGroups.FirstAsync(x => x.Name == "Food"),
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        context.Products.Add(item);

        await context.SaveChangesAsync();

        var optionDoneness = new Option("Stekning");

        item.Options.Add(optionDoneness);

        await context.SaveChangesAsync();

        optionDoneness.Values.Add(new OptionValue("Rare")
        {
            Seq = 1
        });

        var optionMediumRare = new OptionValue("Medium Rare")
        {
            Seq = 2
        };

        optionDoneness.Values.Add(optionMediumRare);

        optionDoneness.Values.Add(new OptionValue("Well Done")
        {
            Seq = 3
        });

        optionDoneness.DefaultValue = optionMediumRare;

        var optionSize = new Option("Extra stor - 50 g mer")
        {
            Price = 15
        };

        item.Options.Add(optionSize);

        await context.SaveChangesAsync();
    }

    public static async Task CreateKorg(ApplicationDbContext context)
    {
        var item = new Product("korg", "Korg")
        {
            Description = "En korg med smårätter",
            Price = 179,
            Group = await context.ProductGroups.FirstAsync(x => x.Name == "Food"),
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        context.Products.Add(item);

        await context.SaveChangesAsync();

        var ratterGroup = new OptionGroup("Rätter")
        {
            Max = 7
        };

        item.OptionGroups.Add(ratterGroup);

        var optionFalafel = new Option("Falafel")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.NumericalValue,
            Group = ratterGroup
        };

        item.Options.Add(optionFalafel);

        var optionChickenWing = new Option("Spicy Chicken Wing")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.NumericalValue,
            Group = ratterGroup
        };

        item.Options.Add(optionChickenWing);

        var optionRib = new Option("Rib")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.NumericalValue,
            Group = ratterGroup
        };

        item.Options.Add(optionRib);

        await context.SaveChangesAsync();


        var extraGroup = new OptionGroup("Extra");

        item.OptionGroups.Add(extraGroup);

        await context.SaveChangesAsync();

        var optionSauce = new Option("Sås")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
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
        var item = new Product("pizza", "Pizza")
        {
            Description = "Custom pizza",
            Price = 40,
            Group = await context.ProductGroups.FirstAsync(x => x.Name == "Food"),
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        context.Products.Add(item);

        await context.SaveChangesAsync();

        var breadGroup = new OptionGroup("Meat")
        {
            Seq = 1,
        };

        item.OptionGroups.Add(breadGroup);

        var meatGroup = new OptionGroup("Meat")
        {
            Seq = 2,
            Max = 2
        };

        item.OptionGroups.Add(meatGroup);

        var nonMeatGroup = new OptionGroup("Non-Meat")
        {
            Seq = 3
        };

        item.OptionGroups.Add(nonMeatGroup);

        var sauceGroup = new OptionGroup("Sauce")
        {
            Seq = 4
        };

        item.OptionGroups.Add(sauceGroup);

        var toppingsGroup = new OptionGroup("Toppings")
        {
            Seq = 5
        };

        item.OptionGroups.Add(toppingsGroup);

        await context.SaveChangesAsync();

        var optionStyle = new Option("Style");

        item.Options.Add(optionStyle);

        await context.SaveChangesAsync();

        var valueItalian = new OptionValue("Italian");

        optionStyle.Values.Add(valueItalian);

        var valueAmerican = new OptionValue("American");

        optionStyle.DefaultValue = valueAmerican;

        optionStyle.Values.Add(valueAmerican);

        var optionHam = new Option("Ham")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = meatGroup,
            Price = 15
        };

        item.Options.Add(optionHam);

        var optionKebab = new Option("Kebab")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = meatGroup,
            Price = 10,
            IsSelected = true
        };

        item.Options.Add(optionKebab);

        var optionChicken = new Option("Chicken")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = meatGroup,
            Price = 10
        };

        item.Options.Add(optionChicken);

        var optionExtraCheese = new Option("Extra cheese")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = toppingsGroup,
            Price = 5
        };

        item.Options.Add(optionExtraCheese);

        var optionGreenOlives = new Option("Green Olives")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = toppingsGroup,
            Price = 5
        };

        item.Options.Add(optionGreenOlives);
    }

    public static async Task CreateSalad(ApplicationDbContext context)
    {
        var item = new Product("sallad", "Sallad")
        {
            Description = "Din egna sallad",
            Price = 52,
            Group = await context.ProductGroups.FirstAsync(x => x.Name == "Food"),
            Visibility = YourBrand.Catalog.Domain.Enums.ProductVisibility.Listed,
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        context.Products.Add(item);

        var baseGroup = new OptionGroup("Bas")
        {
            Seq = 1,
        };

        item.OptionGroups.Add(baseGroup);

        var proteinGroup = new OptionGroup("Välj protein")
        {
            Seq = 2,
            Max = 1
        };

        item.OptionGroups.Add(proteinGroup);

        var additionalGroup = new OptionGroup("Välj tillbehör")
        {
            Seq = 4,
            Max = 3
        };

        item.OptionGroups.Add(additionalGroup);

        var dressingGroup = new OptionGroup("Välj dressing")
        {
            Seq = 5,
            Max = 1
        };

        item.OptionGroups.Add(dressingGroup);

        await context.SaveChangesAsync();

        var optionBase = new Option("Bas")
        {
            Group = baseGroup
        };

        item.Options.Add(optionBase);

        await context.SaveChangesAsync();

        var valueSallad = new OptionValue("Sallad");

        optionBase.Values.Add(valueSallad);

        var valueSalladPasta = new OptionValue("Sallad med pasta");

        optionBase.DefaultValue = valueSalladPasta;

        optionBase.Values.Add(valueSalladPasta);

        var valueSalladQuinoa = new OptionValue("Sallad med quinoa");

        optionBase.Values.Add(valueSalladQuinoa);

        var valueSalladNudlar = new OptionValue("Sallad med glasnudlar");

        optionBase.Values.Add(valueSalladNudlar);

        var optionChicken = new Option("Kycklingfilé")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = proteinGroup
        };

        item.Options.Add(optionChicken);

        var optionSmokedTurkey = new Option("Rökt kalkonfilé")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = proteinGroup
        };

        item.Options.Add(optionSmokedTurkey);

        var optionBeanMix = new Option("Marinerad bönmix")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = proteinGroup
        };

        item.Options.Add(optionBeanMix);

        var optionVegMe = new Option("VegMe")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = proteinGroup
        };

        item.Options.Add(optionVegMe);

        var optionChevre = new Option("Chevré")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = proteinGroup
        };

        item.Options.Add(optionChevre);

        var optionSmokedSalmon = new Option("Varmrökt lax")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = proteinGroup
        };

        item.Options.Add(optionSmokedSalmon);

        var optionPrawns = new Option("Handskalade räkor")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = proteinGroup
        };

        item.Options.Add(optionPrawns);

        var optionCheese = new Option("Parmesanost")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = additionalGroup
        };

        item.Options.Add(optionCheese);

        var optionGreenOlives = new Option("Gröna oliver")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = additionalGroup
        };

        item.Options.Add(optionGreenOlives);

        var optionSoltorkadTomat = new Option("Soltorkade tomater")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = additionalGroup
        };

        item.Options.Add(optionSoltorkadTomat);

        var optionInlagdRödlök = new Option("Inlagd rödlök")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = additionalGroup
        };

        item.Options.Add(optionInlagdRödlök);

        var optionRostadAioli = new Option("Rostad aioli")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = dressingGroup
        };

        item.Options.Add(optionRostadAioli);

        var optionPesto = new Option("Pesto")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = dressingGroup
        };

        item.Options.Add(optionPesto);

        var optionOrtvinagret = new Option("Örtvinägrett")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = dressingGroup
        };

        item.Options.Add(optionOrtvinagret);

        var optionSoyavinagret = new Option("Soyavinägrett")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = dressingGroup
        };

        item.Options.Add(optionSoyavinagret);

        var optionRhodeIsland = new Option("Rhode Island")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = dressingGroup
        };

        item.Options.Add(optionRhodeIsland);

        var optionKimchimayo = new Option("Kimchimayo")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = dressingGroup
        };

        item.Options.Add(optionKimchimayo);

        var optionCaesar = new Option("Caesar")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = dressingGroup
        };

        item.Options.Add(optionCaesar);

        var optionCitronLime = new Option("Citronlime")
        {
            OptionType = YourBrand.Catalog.Domain.Enums.OptionType.YesOrNo,
            Group = dressingGroup
        };

        item.Options.Add(optionCitronLime);
    }
}