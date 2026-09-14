using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    static public class clsPoolDataAccessLayer
    {

        static public bool FindPool(ref int PoolID, ref decimal Volume, ref byte Number,ref int EstateFeatureID)
        {
            bool res =false;

            string query = "select * from Pool where where PoolID = @PoolID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn)) {

                    cmd.Parameters.Add("@PoolID",SqlDbType.Int).Value = PoolID;
                    cmd.Parameters.Add("@Volume", SqlDbType.Decimal).Value = Volume;
                    cmd.Parameters.Add("@Number", SqlDbType.TinyInt).Value = Number;
                    cmd.Parameters.Add("@EstateFeatureID", SqlDbType.Int).Value = EstateFeatureID;

                    conn.Open();


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            res = true;

                            PoolID = reader["PoolID"] != DBNull.Value ? (int)reader["PoolID"] : -1;
                            EstateFeatureID = reader["EstateFeatureID"] != DBNull.Value ? (int)reader["EstateFeatureID"] : -1;
                            Volume = reader["Volume"] != DBNull.Value ? (decimal)reader["Volume"] : 0;
                            Number = reader["Number"] != DBNull.Value ? (byte)reader["Number"] : (byte)0;


                        }
                    }

                }

            }
            catch (Exception ex) { 
                res = false;
                Console.WriteLine(ex.Message);
            }

            return res;


        }

        static public int AddNewPoolToDB(decimal Volume,  byte Number,int EstateFeatureID)
        {
            int ID = -1;

            string query = @"insert into Pool (Volume,Number,EstateFeatureID)
                            values (@Volume,@Number,@EstateFeatureID);
                             SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@Volume", SqlDbType.Decimal).Value = Volume;
                    cmd.Parameters.Add("@Number", SqlDbType.TinyInt).Value = Number;
                    cmd.Parameters.Add("@EstateFeatureID", SqlDbType.Int).Value = EstateFeatureID;
                    
                    conn.Open();

                    object res = cmd.ExecuteScalar();

                    if(res != DBNull.Value && int.TryParse(res.ToString(),out int resID))
                    {
                        ID = resID;
                    }


                }


            }
            catch (Exception ex) {

                ID = -1;
                Console.WriteLine(ex.Message);
            }

            return ID;

        }

        static public bool UpdatePoolInfo(int PoolID,decimal Volume, byte Number, int EstateFeatureID)
        {
            int rowsAffected = 0;

            string query = @"update Pool set Volume =@Volume ,Number = @Number , EstateFeatureID = @EstateFeatureID
                        where PoolID = @PoolID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@PoolID", SqlDbType.Int).Value = PoolID;
                    cmd.Parameters.Add("@Volume", SqlDbType.Decimal).Value = Volume;
                    cmd.Parameters.Add("@Number", SqlDbType.TinyInt).Value = Number;
                    cmd.Parameters.Add("@EstateFeatureID", SqlDbType.Int).Value = EstateFeatureID;

                    conn.Open();


                    rowsAffected = cmd.ExecuteNonQuery();


                }


            }
            catch (Exception ex)
            {

                rowsAffected = 0;
                Console.WriteLine(ex.Message);
            }

            return rowsAffected > 0;

        }


        static public bool DeleteMultiPoolsFromDB(List<int> PoolsIDs) {

            bool res = false;

            if (PoolsIDs == null || PoolsIDs.Count == 0) return false;


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.DeletePools", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable tvp = new DataTable();
                    tvp.Columns.Add("ID", typeof(int));

                    foreach (int id in PoolsIDs)
                    {
                        tvp.Rows.Add(id);
                    }

                    SqlParameter p = cmd.Parameters.Add("@PoolsIds", SqlDbType.Structured);
                    p.TypeName = "dbo.IntIdList";
                    p.Value = tvp;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    res = true;

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return res;

        }


        public static bool DoesPoolExsist(int PoolID)
        {
            if (PoolID <= 0) return false;

            string query = "select COUNT(1) from Pool where PoolID = @PoolID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@PoolID", SqlDbType.Int).Value = PoolID;

                    connection.Open();

                    int count = Convert.ToInt32(cmd.ExecuteScalar());


                    return count > 0;

                }


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }


        }

        public static DataTable getAllPoolsFormDB()
        {
            DataTable dt = new DataTable();
            string query = @"select * from Pool";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {


                    connection.Open();


                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }


                }




            }catch  (Exception ex)
            {
                dt = null;
            }


            return dt;
        }

        public static DataTable getAllPoolsWithVolumeFromDB(decimal Volume)
        {

            DataTable dt = new DataTable();

            string query = @"select * from Pool where Volume = @Volume";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {


                    cmd.Parameters.Add("@Volume", SqlDbType.Decimal).Value = Volume;

                    connection.Open();


                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }


                }

            }
            catch (Exception ex)
            {
                dt = null;
            }


            return dt;

        }


        public static DataTable getAllPoolWithEstateFeatureID(int EstateFeatureID)
        {

            DataTable dt = new DataTable();

            string query = @"select * from Pool where EstateFeatureID = @EstateFeatureID";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {


                    cmd.Parameters.Add("@EstateFeatureID", SqlDbType.Int).Value = EstateFeatureID;

                    connection.Open();


                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }


                }

            }
            catch (Exception ex)
            {
                dt = null;
            }


            return dt;

        }
    }
}
