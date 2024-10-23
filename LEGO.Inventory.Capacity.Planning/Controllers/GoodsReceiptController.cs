using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace LEGO.Inventory.Capacity.Planning.Controllers;

public class GoodsReceiptController(IRegionalDistributionCenterStorage storage) : Controller
{
    [HttpGet("getAll")]
    public IActionResult GetAll()
    {
        return Ok(storage.GoodsReceipts);
    }
}
