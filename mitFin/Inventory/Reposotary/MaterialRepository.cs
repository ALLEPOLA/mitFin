using MitFin_Api.DBAccess;
using MitFin_Api.Inventory.Interface;
using MitFin_Api.Models;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace MitFin_Api.Inventory.Reposatory
{
    // Repository implementation for committed materials data access
    public class MaterialRepository : MaterialInterface
    {
        private readonly DBConnection _db;
        public MaterialRepository(DBConnection db)
        {
            _db = db;

            // Fetch all committed materials
        }
        public async Task<IEnumerable<CommittedMaterial>> GetAllAsync()
        {
            var list = new List<CommittedMaterial>();

            const string sql = @"
            SELECT
               T1.MAT_CD,
               (
                  SELECT CASE WHEN lvl_no = 60 THEN parent_id ELSE grp_comp END
                  FROM glcompm
                  WHERE comp_id IN 
               (
                   SELECT comp_id FROM gldeptm WHERE dept_id = T1.dept_id
                 )
               ) 
            AS region,
               (
                  SELECT CASE WHEN lvl_no = 60 THEN comp_id ELSE parent_id END
                  FROM glcompm
                  WHERE comp_id IN 
               (
                   SELECT comp_id FROM gldeptm WHERE dept_id = T1.dept_id
                 )
               ) 
            AS province,
               (
                T1.dept_id || ' - ' ||
               (
               SELECT dept_nm FROM gldeptm WHERE dept_id = T1.dept_id)
               ) 

            AS dept_id,
               T2.MAT_NM,
               T2.unit_price,
               SUM(T1.QTY_ON_HAND) AS committed_cost,
               T1.UOM_CD
            FROM INWRHMTM T1
            JOIN INMATM T2 ON T2.MAT_CD = T1.MAT_CD
            WHERE
              T1.dept_id IN 
              
               (
                  SELECT dept_id
                  FROM gldeptm
                  WHERE comp_id IN 
               (
                  SELECT comp_id
                  FROM glcompm
                  WHERE status = 2
                      AND (
                        parent_id LIKE 'DISCO%' OR
                        grp_comp  LIKE 'DISCO%' OR
                        comp_id    LIKE 'DISCO%'
                      )
                 )
              )
              AND T1.MAT_CD LIKE 'D%'
              AND T1.GRADE_CD = 'NEW'
              AND T1.status   = 2
            GROUP BY
              T1.MAT_CD,
              T2.MAT_NM,
              T1.UOM_CD,
              T1.dept_id,
              T2.unit_price
            ORDER BY 1, 2, 3, 4";

            using var conn = _db.CreateConnection();
            using var cmd = new OracleCommand(sql, conn);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new CommittedMaterial
                {
                    MatCd = reader.GetString("MAT_CD"),
                    Region = reader.GetString("REGION"),
                    Province = reader.GetString("PROVINCE"),
                    DeptId = reader.GetString("DEPT_ID"),
                    MatNm = reader.GetString("MAT_NM"),
                    UnitPrice = reader.GetDecimal("UNIT_PRICE"),
                    CommittedCost = reader.GetDecimal("COMMITTED_COST"),
                    UomCd = reader.GetString("UOM_CD")
                });
            }

            return list;
        }
    }
}
