using System.Data.SqlClient;

namespace UpdateBomNW
{
    public class Generate
    {
        SqlList sqlList=new SqlList();

        /// <summary>
        /// 更新BOM 净重
        /// </summary>
        /// <param name="fmaterialidlist"></param>
        public void UpdateBomNw(string fmaterialidlist)
        {
            var sqllist = sqlList.GetUpdate(fmaterialidlist);
            Generdt(sqllist);
        }

        /// <summary>
        /// 按照指定的SQL语句执行记录
        /// </summary>
        private void Generdt(string sqlscript)
        {
            using (var sql = GetCloudConn())
            {
                sql.Open();
                var sqlCommand = new SqlCommand(sqlscript, sql);
                sqlCommand.ExecuteNonQuery();
                sql.Close();
            }
        }

        /// <summary>
        /// 获取K3-Cloud连接
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetCloudConn()
        {
            var sqlcon = new SqlConnection(GetConnectionString());
            return sqlcon;
        }


        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            var strcon = @"Data Source='192.168.1.228';Initial Catalog='AIS20181204095717';Persist Security Info=True;User ID='sa'; Password='kingdee';
                       Pooling=true;Max Pool Size=40000;Min Pool Size=0";
            return strcon;
        }
    }
}
