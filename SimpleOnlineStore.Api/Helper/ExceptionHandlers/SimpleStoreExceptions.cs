namespace SimpleOnlineStore.Api.Helper.ExceptionHandlers
{
    public class UserNotFundException : Exception
    {
        public UserNotFundException(int userId)
            : base($"User with ID {userId} not found.")
        {
        }
    }

    public class ProductNotFundException : Exception
    {
        public ProductNotFundException()
            : base($"product not fund.")
        {
        }
    }
}
