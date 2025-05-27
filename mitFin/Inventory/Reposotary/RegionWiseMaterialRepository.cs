using MitFin_Api.DBAccess;
using MitFin_Api.Inventory.Interface;
using MitFin_Api.Models;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace MitFin_Api.Inventory.Reposatory
{
    public class RegionWiseMaterialRepository : RegionMaterialInterface
    {
        private readonly DBConnection _db;

        public RegionWiseMaterialRepository(DBConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<RegionWiseMaterial>> GetRegionWiseMaterialsAsync(string matPrefix)
        {
            var list = new List<RegionWiseMaterial>();

            const string sql = @"
SELECT
  CASE WHEN c.lvl_no = 60 THEN c.parent_id ELSE c.grp_comp END AS Region,
  i.mat_cd,
  SUM(i.qty_on_hand) AS qty_on_hand
FROM inwrhmtm i
JOIN gldeptm d ON i.dept_id = d.dept_id
JOIN glcompm c ON d.comp_id = c.comp_id
WHERE
  i.mat_cd LIKE :prefix
  AND i.status = 2
  AND i.grade_cd = 'NEW'
  AND (
    c.parent_id LIKE 'DISCO%' OR
    c.grp_comp   LIKE 'DISCO%' OR
    c.comp_id    LIKE 'DISCO%'
  )
GROUP BY
  CASE WHEN c.lvl_no = 60 THEN c.parent_id ELSE c.grp_comp END,
  i.mat_cd
ORDER BY 1, 2";

            using var conn = _db.CreateConnection();
            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("prefix", matPrefix + "%"));

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new RegionWiseMaterial
                {
                    Region = reader.GetString(0),
                    MatCd = reader.GetString(1),
                    QtyOnHand = reader.GetDecimal(2)
                });
            }

            return list;
        }
    }
}
