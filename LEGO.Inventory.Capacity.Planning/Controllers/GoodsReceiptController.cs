using LEGO.Inventory.Capacity.Planning.Storage;
using Microsoft.AspNetCore.Mvc;
namespace LEGO.Inventory.Capacity.Planning.Controllers;

public class GoodsReceiptController(IStorage storage) : Controller
{
    [HttpGet("getAll")]
    public IActionResult GetAll()
    {
        return Ok(storage.GoodsReceipts);
    }
}
