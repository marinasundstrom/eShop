namespace YourBrand.Catalog.Domain;

public static class Errors
{
    public static class Products
    {
        public static readonly Error ProductNotFound = new Error(nameof(ProductNotFound), "Product not found", string.Empty);

        public static readonly Error ProductAlreadyHasSpecifiedSku = new Error(nameof(ProductAlreadyHasSpecifiedSku), "Product already has specified SKU", string.Empty);

        public static readonly Error ProductWithSkuAlreadyExists = new Error(nameof(ProductWithSkuAlreadyExists), "Product with SKU already exists", string.Empty);
    }

    public static class Todos
    {
        public static readonly Error TodoNotFound = new Error(nameof(TodoNotFound), "Todo not found", string.Empty);
    }
}
