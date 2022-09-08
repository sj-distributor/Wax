namespace Wax.Core.Entities.Customers
{
    public class Customer : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}