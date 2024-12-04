namespace Wax.Core.Requests.Customers;

public record CustomerShortInfo : IBaseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}