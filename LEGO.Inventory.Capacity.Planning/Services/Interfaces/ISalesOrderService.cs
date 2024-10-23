using LEGO.Inventory.Capacity.Planning.Domain.Orders;

namespace LEGO.Inventory.Capacity.Planning.Services.Interfaces
{
    public interface ISalesOrderService
    {
        void CreateSalesOrder(SalesOrder salesOrder);
        List<SalesOrder> GetSalesOrders();
    }
}
