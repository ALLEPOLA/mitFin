using MitFin_Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MitFin_Api.Inventory.Interface
{
    public interface RegionMaterialInterface
    {
        Task<IEnumerable<RegionWiseMaterial>> GetRegionWiseMaterialsAsync(string matPrefix);
    }
}
