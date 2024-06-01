namespace SimpleOnlineStore.Api.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public DateTime CreationDate { get; set; }
        public User Buyer { get; set; }
    }
}
