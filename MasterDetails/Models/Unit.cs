
namespace MasterDetails.Models;

public class Unit
{
    public int Id { get; set; }
    public string UnitName { get; set; }
    public virtual List<PurchaseProduct> PurchaseProducts { get; set; } = new List<PurchaseProduct>();

}
