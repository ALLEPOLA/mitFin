namespace MitFin_Api.Models
{

    public class CommittedMaterial
    {
        public string MatCd { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string DeptId { get; set; } = null!;
        public string MatNm { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public decimal CommittedCost { get; set; }
        public string UomCd { get; set; } = null!;
    }
}
