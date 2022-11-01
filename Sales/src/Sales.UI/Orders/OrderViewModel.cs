using System.ComponentModel.DataAnnotations;

using YourBrand.Sales.Client;

namespace YourBrand.Sales.Orders;

public class OrderViewModel
{
    public int Id { get; set; }

    [Required]
    public DateTime? Date { get; set; }

    [Required]
    public TimeSpan? Time { get; set; }

    [Required]
    public OrderStatusDto Status { get; set; }

    public string? Reference { get; set; }

    public string? Notes { get; set; }

    public DateTime? DueDate { get; set; }

    public bool VatIncluded { get; set; } = true;

    public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();

    public decimal SubTotal => Items.Select(i => !VatIncluded ? i.LineTotal : i.LineTotal.GetSubTotal(i.VatRate)).Sum();

    public decimal Vat => Items.Select(i => VatIncluded ? i.LineTotal.GetVatFromTotal(i.VatRate) : i.LineTotal.AddVat(i.VatRate)).Sum();

    public decimal Total 
    {
        get 
        {
            var total = Items.Select(i => VatIncluded ? i.LineTotal: i.LineTotal.AddVat(i.VatRate)).Sum();
            return total;
        }
    }

    public decimal? Paid { get; set; }
}
