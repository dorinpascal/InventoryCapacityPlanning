using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;

namespace LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

public interface ILocalDistributionCenterStorage
{
    Task<LocalDistributionCenter?> GetByNameAsync(string name);
    Task UpdateAsync(LocalDistributionCenter updatedLdc);
}
