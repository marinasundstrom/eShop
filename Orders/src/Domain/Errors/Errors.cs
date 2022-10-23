namespace YourBrand.Orders.Domain;

public static class Errors
{
    public static class Orders
    {
        public static readonly Error OrderNotFound = new Error(nameof(OrderNotFound), "Order not found", string.Empty);
    }

    public static class Users
    {
        public static readonly Error UserNotFound = new Error(nameof(UserNotFound), "User not found", string.Empty);
    }
}
