using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    static public class clsGarageDataAccessLayer
    {
        static public bool FindGarageByID(ref int GarageID,ref string GarageLocation, ref decimal Size , ref byte Number,
            ref int EstateFeatureID)
        {

            bool res = false;

            string query = "select * from vw_Garage where GarageID = @GarageID";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@GarageID",SqlDbType.Int).Value = GarageID;

                    connection.Open();  

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        res = true;

                        GarageID = reader["GarageID"] != DBNull.Value ? (int)reader["GarageID"] : -1;

                        EstateFeatureID = reader["EstateFeatureID"] != DBNull.Value ? (int)reader["EstateFeatureID"] : -1;

                        GarageLocation = reader["GarageLocation"]?.ToString() ?? "";

                        Size = reader["Size"] != DBNull.Value ? (decimal)reader["Size"] : 0;

                        Number = reader["Number"] != DBNull.Value ? (byte)reader["Number"] :(byte)0;
                    }

                }


            }
            catch (Exception ex) { res = false; Console.WriteLine(ex.Message); }

            return res;
        }



        static public int AddNewGarageToDB(decimal Size ,string GarageLocation, byte Number,int EstateFeatureID)
        {

            int id = -1;
            string query = @"insert into Garage(Size,GarageLocation,Number,EstateFeatureID)
                            values
                            (@Size,@GarageLocation,@Number,@EstateFeatureID);
                            SELECT SCOPE_IDENTITY();";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@Size",SqlDbType.Decimal).Value = Size;
                    command.Parameters.Add("@GarageLocation", SqlDbType.NVarChar).Value = GarageLocation;
                    command.Parameters.Add("@Number",SqlDbType.TinyInt).Value = Number;
                    command.Parameters.Add("@EstateFeatureID", SqlDbType.Int).Value = EstateFeatureID;
                  
                    conn.Open();    

                    object res = command.ExecuteScalar();

                    if (res != DBNull.Value && int.TryParse(res.ToString(), out int resID))
                    {
                        id= resID;
                    }


                }



            }catch  (Exception ex) { id = -1; Console.WriteLine(ex.Message); }  

            return id;


        }

        static public bool UpdateGarageDataInDB( int GarageID,  decimal Size,  string GarageLocation,  byte Number,
             int EstateFeatureID)
        {
            int rowsAffected = -1;

            string query = @"update Garage set GarageLocation = @GarageLocation 
                                            ,Size = @Size
                                            ,Number=@Number 
                                            ,EstateFeatureID = @EstateFeatureID 
                                             where GarageID = @GarageID";

            try
            {


                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    command.Parameters.Add("@GarageID", SqlDbType.Int).Value = GarageID;
                    command.Parameters.Add("@Size", SqlDbType.Decimal).Value = Size;
                    command.Parameters.Add("@GarageLocation", SqlDbType.NVarChar).Value = GarageLocation;
                    command.Parameters.Add("@Number", SqlDbType.TinyInt).Value = Number;
                    command.Parameters.Add("@EstateFeatureID", SqlDbType.Int).Value = EstateFeatureID;

                    conn.Open();


                    rowsAffected = command.ExecuteNonQuery();   


                } 

            }
            catch (Exception ex) { rowsAffected = -1; Console.WriteLine(ex.Message); }


            return rowsAffected > 0;

        }


        public static bool DeleteMultiGaragesFromDB(List<int> ids)
        {

            bool res = false;

            if (ids == null || ids.Count == 0) return false;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.DeleteGarages", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable tvp = new DataTable();
                    tvp.Columns.Add("ID", typeof(int));

                    foreach (int id in ids)
                    {
                        tvp.Rows.Add(id);
                    }

                    SqlParameter p = cmd.Parameters.Add("@GaragesIds", SqlDbType.Structured);
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


        public static bool DoesGarageExsist(int GarageID)
        {
            if (GarageID <= 0) return false;

            string query = "select COUNT(1) from Garage where GarageID = @GarageID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@GarageID", SqlDbType.Int).Value = GarageID;

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

        public static DataTable getAllGaragesFormDB()
        {
            DataTable dt = new DataTable();
            string query = @"select * from Garage";

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




            }
            catch (Exception ex)
            {
                dt = null;
            }


            return dt;
        }

        public static DataTable getAllGaragesWithSizeFromDB(decimal Size)
        {

            DataTable dt = new DataTable();

            string query = @"select * from Garage where Size = @Size";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {


                    cmd.Parameters.Add("@Size", SqlDbType.Decimal).Value = Size;

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


        public static DataTable getAllGaragesWithEstateFeatureID(int EstateFeatureID)
        {

            DataTable dt = new DataTable();

            string query = @"select * from Garage where EstateFeatureID = @EstateFeatureID";

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
