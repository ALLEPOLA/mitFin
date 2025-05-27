using MitFin_Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MitFin_Api.Inventory
{
    public interface MaterialInfoInterface
    {
        Task<IEnumerable<MaterialInfo>> GetMaterialsAsync();
    }
}
