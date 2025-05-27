using MitFin_Api.DBAccess;
using MitFin_Api.Models;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace MitFin_Api.Inventory
{
    public class MaterialInfoRepository : MaterialInfoInterface
    {
        private readonly DBConnection _db;

        public MaterialInfoRepository(DBConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<MaterialInfo>> GetMaterialsAsync()
        {
            var list = new List<MaterialInfo>();

            const string sql = @"
                SELECT mat_cd, mat_nm 
                FROM inmatm 
                WHERE status = 2";

            using var conn = _db.CreateConnection();
            using var cmd = new OracleCommand(sql, conn);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new MaterialInfo
                {
                    MatCd = reader.GetString("mat_cd"),
                    MatNm = reader.GetString("mat_nm")
                });
            }

            return list;
        }
    }
}
