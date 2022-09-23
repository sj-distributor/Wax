namespace Wax.Core.Domain.Customers
{
    public class Customer : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
    }
}