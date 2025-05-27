using Microsoft.AspNetCore.Mvc;
using MitFin_Api.Inventory;
using MitFin_Api.Inventory.Interface;
using MitFin_Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MitFin_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialController : ControllerBase
    {
        private readonly MaterialInterface _committedRepo;
        private readonly RegionMaterialInterface _regionRepo;
        private readonly MaterialInfoInterface _infoRepo;

        public MaterialController(
            MaterialInterface committedRepo,
            RegionMaterialInterface regionRepo,
            MaterialInfoInterface infoRepo)
        {
            _committedRepo = committedRepo;
            _regionRepo = regionRepo;
            _infoRepo = infoRepo;
        }

        // GET: api/Material
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommittedMaterial>>> GetAllCommittedMaterials()
        {
            var items = await _committedRepo.GetAllAsync();
            if (items == null || !items.Any())
                return NotFound("No committed material data found.");
            return Ok(items);
        }

        // GET: api/Material/region-wise?prefix=ABC
        [HttpGet("region-wise")]
        public async Task<ActionResult<IEnumerable<RegionWiseMaterial>>> GetRegionWiseMaterials([FromQuery] string prefix)
        {
            var data = await _regionRepo.GetRegionWiseMaterialsAsync(prefix);
            if (data == null || !data.Any())
                return NotFound($"No region‑wise material data found for prefix '{prefix}'.");
            return Ok(data);
        }

        // GET: api/Material/info
        [HttpGet("info")]
        public async Task<ActionResult<IEnumerable<MaterialInfo>>> GetMaterialInfo()
        {
            var list = await _infoRepo.GetMaterialsAsync();
            if (list == null || !list.Any())
                return NotFound("No material info found with status=2.");
            return Ok(list);
        }
    }
}
