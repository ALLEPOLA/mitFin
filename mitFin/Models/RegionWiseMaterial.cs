
namespace MitFin_Api.Models
{
    public class RegionWiseMaterial
    {
        public string Region { get; set; } = null!;
        public string MatCd { get; set; } = null!;
        public decimal QtyOnHand { get; set; }
    }
}
