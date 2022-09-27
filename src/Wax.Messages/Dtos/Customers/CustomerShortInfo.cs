namespace Wax.Messages.Dtos.Customers;

public record CustomerShortInfo
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}