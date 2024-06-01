namespace SimpleOnlineStore.Api.Helper
{
    public class ProductHelper
    {
        public static decimal CalculateProductDiscount(decimal price, int discount) => price * (1 - discount / 100.0m);
    }
}
