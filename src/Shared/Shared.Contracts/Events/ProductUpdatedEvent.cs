namespace Shared.Contracts.Events;

public class ProductUpdatedEvent
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime OccurredAtUtc { get; set; }
}