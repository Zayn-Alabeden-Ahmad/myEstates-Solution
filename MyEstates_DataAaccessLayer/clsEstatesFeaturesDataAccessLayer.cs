using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    static public class clsEstatesFeaturesDataAccessLayer
    {
        static public bool FindFeatureByID(ref int EstateFeatureID, ref decimal EstateSpace, ref short FloorNumber
            , ref byte NumberOfRooms, ref string Description, ref short BitwiseFeatures
            , ref bool HasGarden)
        {

            bool res = false;

            string query = "select * from EstatesFeatures where EstateFeatureID = @EstateFeatureID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@EstateFeatureID", SqlDbType.Int).Value = EstateFeatureID;

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        res = true;

                        EstateFeatureID = reader["EstateFeatureID"] != DBNull.Value ? (int)reader["EstateFeatureID"] : 0;
                        EstateSpace = reader["EstateSpace"] != DBNull.Value ? (decimal)reader["EstateSpace"] : 0;
                        FloorNumber = reader["FloorNumber"] != DBNull.Value ? Convert.ToInt16(reader["FloorNumber"]) : Convert.ToInt16(0);
                        NumberOfRooms = reader["NumberOfRooms"] != DBNull.Value ? (byte)reader["NumberOfRooms"] : (byte)0;
                        Description = reader["Description"]?.ToString() ?? "";
                        BitwiseFeatures = reader["BitwiseFeatures"] != DBNull.Value ? Convert.ToInt16(reader["BitwiseFeatures"]) : Convert.ToInt16(0);
                        HasGarden = reader["HasGarden"] != DBNull.Value ? (bool)reader["HasGarden"] : false;



                    }


                }


            }
            catch (Exception ex)
            {
                res = false;
                Console.Write(ex.Message);

            }


            return res;



        }


        static public DataTable GetGaragesRelatedToEstateWith(int EstateFeatureID)
        {

            DataTable dataTable = new DataTable();

            string query = "select * from vw_Garage where EstateFeatureID = @EstateFeatureID";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    command.Parameters.Add("@EstateFeatureID", SqlDbType.Int).Value = EstateFeatureID;

                    conn.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {

                        dataTable.Load(reader);

                    }

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return dataTable;

        }

        static public DataTable GetPoolsRelatedToEstateWith(int EstateFeatureID)
        {
            DataTable dataTable = new DataTable();

            string query = "select * from vw_Pool where EstateFeatureID = @EstateFeatureID";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    command.Parameters.Add("@EstateFeatureID", SqlDbType.Int).Value = EstateFeatureID;

                    conn.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {

                        dataTable.Load(reader);

                    }

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return dataTable;

        }

        static public int AddNewEstateFeatureToDB(decimal EstateSpace, short FloorNumber
            , byte NumberOfRooms, string Description, short BitwiseFeatures
            , bool HasGarden)
        {

            int ID = -1;

            string query = @"insert into EstatesFeatures
                    (EstateSpace,FloorNumber,NumberOfRooms,Description,BitwiseFeatures,HasGarden)
                    Values(@EstateSpace,@FloorNumber,@NumberOfRooms,@Description,@BitwiseFeatures,@HasGarden);
                    SELECT SCOPE_IDENTITY();";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@EstateSpace",SqlDbType.Decimal).Value = EstateSpace;
                    command.Parameters.Add("@FloorNumber", SqlDbType.SmallInt).Value = FloorNumber;
                    command.Parameters.Add("@NumberOfRooms", SqlDbType.TinyInt).Value = NumberOfRooms;
                    command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
                    command.Parameters.Add("@BitwiseFeatures", SqlDbType.SmallInt).Value = BitwiseFeatures;
                    command.Parameters.Add("@HasGarden", SqlDbType.Bit).Value = HasGarden;

                    connection.Open();


                    object obj = command.ExecuteScalar();

                    if(obj != DBNull.Value && int.TryParse(obj.ToString(),out int resID)) {
                        ID = resID;
                    }

                }

            }
            catch (Exception ex)
            {
                ID = -1;
                Console.WriteLine(ex.Message);
            }

            return ID;

        }



        static public bool UpdateEstateFeatureInDB(int EstateFeatureID, decimal EstateSpace, short FloorNumber
            , byte NumberOfRooms, string Description, short BitwiseFeatures
            , bool HasGarden)
        {

            int rowsAffected = 0;


            string query = @"update EstatesFeatures set EstateSpace = @EstateSpace , FloorNumber = @FloorNumber
                NumberOfRooms = @NumberOfRooms, Description = @Description , BitwiseFeatures = @BitwiseFeatures
                HasGarden = @HasGarden where EstateFeatureID = @EstateFeatureID";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn)) {


                    command.Parameters.Add("@EstateFeatureID", SqlDbType.Int).Value = EstateFeatureID;
                    command.Parameters.Add("@EstateSpace", SqlDbType.Decimal).Value = EstateSpace;
                    command.Parameters.Add("@FloorNumber", SqlDbType.SmallInt).Value = FloorNumber;
                    command.Parameters.Add("@NumberOfRooms", SqlDbType.TinyInt).Value = NumberOfRooms;
                    command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
                    command.Parameters.Add("@BitwiseFeatures", SqlDbType.SmallInt).Value = BitwiseFeatures;
                    command.Parameters.Add("@HasGarden", SqlDbType.Bit).Value = HasGarden;
                    
                    conn.Open();
                
                    
                    rowsAffected = command.ExecuteNonQuery();


                }




            } catch (Exception ex)
            {
                rowsAffected = 0;
                Console.WriteLine(ex.Message);   
            }


            return (rowsAffected > 0);
        }

    
        static public bool DeleteEstateFeaturesFromDB(List<int>estateFeaturesIds)
        {
                
            bool res = false;

            if(estateFeaturesIds == null || estateFeaturesIds.Count == 0) return false;



            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.DeleteEstatesFeatures", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable tvp = new DataTable();
                    tvp.Columns.Add("ID", typeof(int));

                    foreach (int id in estateFeaturesIds)
                    {
                        tvp.Rows.Add(id);
                    }

                    SqlParameter p = cmd.Parameters.Add("@EstatesFeaturesIds", SqlDbType.Structured);
                    p.TypeName = "dbo.IntIdList";
                    p.Value = tvp;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    res = true;

                }
            }

            catch (Exception ex) {  
                Console.WriteLine(ex.Message);
                return false; 
            }  

            return res;

        }


        public static bool DoesEstateFeaturExsist(int EstateFeatureID)
        {

            if (EstateFeatureID <= 0) return false;

            string query = "select COUNT(1) from EstatesFeatures where EstateFeatureID = @EstateFeatureID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@EstateFeatureID", SqlDbType.Int).Value = EstateFeatureID;

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
    }
}
