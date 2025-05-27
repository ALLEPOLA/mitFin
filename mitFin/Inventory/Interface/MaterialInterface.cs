using MitFin_Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MitFin_Api.Inventory.Interface
{
    public interface MaterialInterface
    {
        // Method to get all committed materials
        Task<IEnumerable<CommittedMaterial>> GetAllAsync();
    }
}
