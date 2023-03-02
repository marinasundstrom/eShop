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

        var item = new Product("T-shirt", "tshirt")
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
            IsMainAttribute = true,
            Attribute = colorAttribute,
            Value = null
        });

        item.ProductAttributes.Add(new ProductAttribute
        {
            ForVariant = true,
            IsMainAttribute = false,
            Attribute = sizeAttribute,
            Value = null
        });

        ///*

        var variantBlueSmall = new Product("Blue S", "tshirt-blue-small")
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

        var variantBlueMedium = new Product("Blue M", "tshirt-blue-medium")
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

        var variantBlueLarge = new Product("Blue L", "tshirt-blue-large")
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

        var variantRedSmall = new Product("Red S", "tshirt-red-small")
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

        var variantRedMedium = new Product("Red M", "tshirt-red-medium")
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

        var variantRedLarge = new Product("Red L", "tshirt-red-large")
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

        var textOption = new Domain.Entities.TextValueOption("Custom text");

        item.Options.Add(textOption);
    }

    public static async Task CreateKebabPlate(ApplicationDbContext context)
    {
        var item = new Product("Kebabtallrik", "kebabtallrik")
        {
            Description = "Dönnerkebab, nyfriterad pommes frites, sallad, och sås",
            Price = 89,
            Group = await context.ProductGroups.FirstAsync(x => x.Name == "Food"),
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        context.Products.Add(item);

        var option = new ChoiceOption("Sås");
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
        var item = new Product("Herrgårdsstek", "herrgardsstek")
        {
            Description = "Vår fina stek med pommes och vår hemlagade bearnaise sås",
            Price = 179,
            Group = await context.ProductGroups.FirstAsync(x => x.Name == "Food"),
            Store = await context.Stores.FirstAsync(x => x.Handle == "joes")
        };

        context.Products.Add(item);

        await context.SaveChangesAsync();

        var optionDoneness = new ChoiceOption("Stekning");

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

        var optionSize = new SelectableOption("Extra stor - 50 g mer")
        {
            Price = 15
        };

        item.Options.Add(optionSize);

        await context.SaveChangesAsync();
    }

    public static async Task CreateKorg(ApplicationDbContext context)
    {
        var item = new Product("Korg", "korg")
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

        var optionFalafel = new NumericalValueOption("Falafel")
        {
            Group = ratterGroup
        };

        item.Options.Add(optionFalafel);

        var optionChickenWing = new NumericalValueOption("Spicy Chicken Wing")
        {
            Group = ratterGroup
        };

        item.Options.Add(optionChickenWing);

        var optionRib = new NumericalValueOption("Rib")
        {
            Group = ratterGroup
        };

        item.Options.Add(optionRib);

        await context.SaveChangesAsync();


        var extraGroup = new OptionGroup("Extra");

        item.OptionGroups.Add(extraGroup);

        await context.SaveChangesAsync();

        var optionSauce = new SelectableOption("Sås")
        {
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
        var item = new Product("Pizza", "pizza")
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

        var optionStyle = new ChoiceOption("Style");

        item.Options.Add(optionStyle);

        await context.SaveChangesAsync();

        var valueItalian = new OptionValue("Italian");

        optionStyle.Values.Add(valueItalian);

        var valueAmerican = new OptionValue("American");

        optionStyle.DefaultValue = valueAmerican;

        optionStyle.Values.Add(valueAmerican);

        var optionHam = new SelectableOption("Ham")
        {
            Group = meatGroup,
            Price = 15
        };

        item.Options.Add(optionHam);

        var optionKebab = new SelectableOption("Kebab")
        {
            Group = meatGroup,
            Price = 10,
            IsSelected = true
        };

        item.Options.Add(optionKebab);

        var optionChicken = new SelectableOption("Chicken")
        {
            Group = meatGroup,
            Price = 10
        };

        item.Options.Add(optionChicken);

        var optionExtraCheese = new SelectableOption("Extra cheese")
        {
            Group = toppingsGroup,
            Price = 5
        };

        item.Options.Add(optionExtraCheese);

        var optionGreenOlives = new SelectableOption("Green Olives")
        {
            Group = toppingsGroup,
            Price = 5
        };

        item.Options.Add(optionGreenOlives);
    }

    public static async Task CreateSalad(ApplicationDbContext context)
    {
        var item = new Product("Sallad", "sallad")
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

        var optionBase = new ChoiceOption("Bas")
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

        var optionChicken = new SelectableOption("Kycklingfilé")
        {
            Group = proteinGroup
        };

        item.Options.Add(optionChicken);

        var optionSmokedTurkey = new SelectableOption("Rökt kalkonfilé")
        {
            Group = proteinGroup
        };

        item.Options.Add(optionSmokedTurkey);

        var optionBeanMix = new SelectableOption("Marinerad bönmix")
        {
            Group = proteinGroup
        };

        item.Options.Add(optionBeanMix);

        var optionVegMe = new SelectableOption("VegMe")
        {
            Group = proteinGroup
        };

        item.Options.Add(optionVegMe);

        var optionChevre = new SelectableOption("Chevré")
        {
            Group = proteinGroup
        };

        item.Options.Add(optionChevre);

        var optionSmokedSalmon = new SelectableOption("Varmrökt lax")
        {
            Group = proteinGroup
        };

        item.Options.Add(optionSmokedSalmon);

        var optionPrawns = new SelectableOption("Handskalade räkor")
        {
            Group = proteinGroup
        };

        item.Options.Add(optionPrawns);

        var optionCheese = new SelectableOption("Parmesanost")
        {
            Group = additionalGroup
        };

        item.Options.Add(optionCheese);

        var optionGreenOlives = new SelectableOption("Gröna oliver")
        {
            Group = additionalGroup
        };

        item.Options.Add(optionGreenOlives);

        var optionSoltorkadTomat = new SelectableOption("Soltorkade tomater")
        {
            Group = additionalGroup
        };

        item.Options.Add(optionSoltorkadTomat);

        var optionInlagdRödlök = new SelectableOption("Inlagd rödlök")
        {
            Group = additionalGroup
        };

        item.Options.Add(optionInlagdRödlök);

        var optionRostadAioli = new SelectableOption("Rostad aioli")
        {
            Group = dressingGroup
        };

        item.Options.Add(optionRostadAioli);

        var optionPesto = new SelectableOption("Pesto")
        {
            Group = dressingGroup
        };

        item.Options.Add(optionPesto);

        var optionOrtvinagret = new SelectableOption("Örtvinägrett")
        {
            Group = dressingGroup
        };

        item.Options.Add(optionOrtvinagret);

        var optionSoyavinagret = new SelectableOption("Soyavinägrett")
        {
            Group = dressingGroup
        };

        item.Options.Add(optionSoyavinagret);

        var optionRhodeIsland = new SelectableOption("Rhode Island")
        {
            Group = dressingGroup
        };

        item.Options.Add(optionRhodeIsland);

        var optionKimchimayo = new SelectableOption("Kimchimayo")
        {
            Group = dressingGroup
        };

        item.Options.Add(optionKimchimayo);

        var optionCaesar = new SelectableOption("Caesar")
        {
            Group = dressingGroup
        };

        item.Options.Add(optionCaesar);

        var optionCitronLime = new SelectableOption("Citronlime")
        {
            Group = dressingGroup
        };

        item.Options.Add(optionCitronLime);
    }
}