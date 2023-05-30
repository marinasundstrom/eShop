﻿namespace YourBrand.Payments.Domain;

public static class Errors
{
    public static class ProductPriceLists
    {
        public static readonly Error ProductPriceListNotFound = new Error(nameof(ProductPriceListNotFound), "ProductPriceListNotFound not found", string.Empty);
    }

    public static class ProductPrice
    {
        public static readonly Error ProductPriceNotFound = new Error(nameof(ProductPriceNotFound), "ProductPrice not found", string.Empty);
    }

    public static class Receipts
    {
        public static readonly Error ReceiptNotFound = new Error(nameof(ReceiptNotFound), "Receipt not found", string.Empty);
    }

    public static class Users
    {
        public static readonly Error UserNotFound = new Error(nameof(UserNotFound), "User not found", string.Empty);
    }
}
