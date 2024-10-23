using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;

namespace LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

public interface IRegionalDistributionCenterStorage
{
    Task<RegionalDistributionCenter> GetAllAsync();
}
