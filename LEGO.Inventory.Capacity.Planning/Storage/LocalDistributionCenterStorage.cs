using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Storage;

public class LocalDistributionCenterStorage : ILocalDistributionCenterStorage
{
    private static readonly List<LocalDistributionCenter> _localDistributionCenters =
  [
      new LocalDistributionCenter("Central Warehouse Europe", "LEGO European Distribution Center", "Lego - Harry Potter", 50, 20, 20),
        new LocalDistributionCenter("Eastern Warehouse Europe", "LEGO European Distribution Center", "Lego - Harry Potter", 60, 20, 20),
        new LocalDistributionCenter("Western Warehouse Europe", "LEGO European Distribution Center", "Lego - Harry Potter", 70, 20, 20)
  ];

    public Task<LocalDistributionCenter?> GetByNameAsync(string name)
    {
        var ldc = _localDistributionCenters.Find(x => x.Name == name);
        return Task.FromResult(ldc);
    }
    public Task UpdateAsync(LocalDistributionCenter updatedLdc)
    {
        var existingLdcIndex = _localDistributionCenters.FindIndex(x => x.Name == updatedLdc.Name);
        if (existingLdcIndex != -1)
        {
            _localDistributionCenters[existingLdcIndex] = updatedLdc;
        }
        return Task.CompletedTask;
    }
}
