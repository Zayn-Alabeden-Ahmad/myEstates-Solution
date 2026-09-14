using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    public static class clsBuildingFeaturesDataAccessLayer
    {

        public static bool FindBuildingFeatureBy(ref int BuildingFeaturesID, ref byte NumberOfBlocks, 
            ref byte NumberOfFloors, ref bool HasAlivator,ref int BuildingID)
        {
            bool res = false;

            string query = "select * from BuildingFeatures where BuildingFeaturesID = @BuildingFeaturesID";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) { 
                    
                    command.Parameters.Add("@BuildingFeaturesID",SqlDbType.Int).Value = BuildingFeaturesID;

                    connection.Open();
                

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read()) {
                        
                        res = true;

                        BuildingFeaturesID = reader["BuildingFeaturesID"] != DBNull.Value ? (int)reader["BuildingFeaturesID"] : -1;
                        BuildingID = reader["BuildingID"] != DBNull.Value ? (int)reader["BuildingID"] : -1;
                        NumberOfBlocks = reader["NumberOfBlocks"] != DBNull.Value ? (byte)reader["NumberOfBlocks"] : (byte)0;
                        NumberOfFloors = reader["NumberOfFloors"] != DBNull.Value ? (byte)reader["NumberOfFloors"] : (byte)0;
                        HasAlivator = reader["HasAlivator"] != DBNull.Value ? (bool)reader["HasAlivator"] : false; 


                    }
                
                
                }


            }
            catch (Exception ex) {
                res = false;
                Console.WriteLine(ex.Message);  
            }

            return res;

        }
    
    
        public static int AddNewFeatureOfBuidling(byte NumberOfBlocks, byte NumberOfFloors, bool HasAlivator,int BuildingID)
        {
            int FeatureID = -1;

            string query = @"insert into BuildingFeatures(NumberOfBlocks ,NumberOfFloors ,HasAlivator,BuildingID) values
                            (@NumberOfBlocks,@NumberOfFloors,@HasAlivator,@BuildingID);SELECT SCOPE_IDENTITY();";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                 
                    cmd.Parameters.Add("@NumberOfBlocks",SqlDbType.TinyInt).Value = NumberOfBlocks;
                    cmd.Parameters.Add("@NumberOfFloors", SqlDbType.TinyInt).Value = NumberOfFloors;
                    cmd.Parameters.Add("@HasAlivator", SqlDbType.Bit).Value = HasAlivator;
                    cmd.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;

                    conn.Open();    
                
                    object res = cmd.ExecuteScalar();

                    if (res != DBNull.Value && int.TryParse(res.ToString(), out int resID))
                    {
                        FeatureID = resID;
                    }
              

                }

            }
            catch (Exception ex) {
                FeatureID = -1;
                Console.WriteLine(ex.Message);
            }
        
        
            return FeatureID;
        }

        public static bool UpdateTheBuildingFeature(int BuildingFeaturesID,
            byte NumberOfBlocks, byte NumberOfFloors, bool HasAlivator,int BuildingID)
        {

            int rowsAffected = 0;

            string query = @"update BuildingFeatures set
                        NumberOfBlocks = @NumberOfBlocks ,
                        NumberOfFloors = @NumberOfFloors ,
                        HasAlivator = @HasAlivator,
                        BuildingID = @BuildingID  where
                        BuildingFeaturesID = @BuildingFeaturesID;";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@BuildingFeaturesID", SqlDbType.Int).Value = BuildingFeaturesID;
                    cmd.Parameters.Add("@NumberOfBlocks", SqlDbType.TinyInt).Value = NumberOfBlocks;
                    cmd.Parameters.Add("@NumberOfFloors", SqlDbType.TinyInt).Value = NumberOfFloors;
                    cmd.Parameters.Add("@HasAlivator", SqlDbType.Bit).Value = HasAlivator;
                    cmd.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;

                    connection.Open();
                
                
                    rowsAffected = cmd.ExecuteNonQuery();



                }
            }
            catch (Exception ex)
            {

                rowsAffected = -1;
                Console.WriteLine(ex.Message);
            }

            return rowsAffected > 0;

        }
    
        public static bool DeleteMultiBuidligFeatureFromDB(List<int> FeaturesID)
        {
            int rowsAffected = 0;

            if (FeaturesID == null || FeaturesID.Count == 0) return false;

            var parameters = FeaturesID.Select((id, index) => "@id" + index).ToArray();

            string query = $"Delete from BuildingFeatures where BuildingFeaturesID in ({string.Join(",", parameters)})";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn)) {

                    for (int i = 0; i < parameters.Length; i++) {
                        cmd.Parameters.Add(parameters[i],SqlDbType.Int).Value = FeaturesID[i];
                      }
                    conn.Open();
                
                    rowsAffected = cmd.ExecuteNonQuery();
                
                }


            }
            catch (Exception ex) {
                rowsAffected = -1;
                Console.WriteLine(ex.Message);
            }


            return (rowsAffected > 0);

        }

        public static bool DeleteSingleBuidligFeatureFromDB(int BuildingFeaturesID) { 
            
            int rowsAffected = 0;

            string query = "Delete from BuildingFeatures where BuildingFeaturesID = @BuildingFeaturesID";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn)) {

                    cmd.Parameters.Add("@BuildingFeaturesID", SqlDbType.Int).Value = BuildingFeaturesID;
                    conn.Open();    
                
                    rowsAffected = cmd.ExecuteNonQuery();
                
                }         
    
            }catch(Exception ex) 
            { 
                rowsAffected = -1; 
                Console.WriteLine(ex.Message); 
            }

            return (rowsAffected > 0);

        }


        public static bool DoesBuildingFeatureExsist(int BuildingFeaturesID) {

            if (BuildingFeaturesID <= 0) return false;

            string query = "select COUNT(1) from BuildingFeatures where BuildingFeaturesID = @BuildingFeaturesID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@BuildingFeaturesID", SqlDbType.Int).Value = BuildingFeaturesID;

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


        public static byte getNumberOfBlocksForBuilding(int BuildingID)
        {

            byte res = 0;
            string query = @"select  NumberOfBlocks from BuildingFeatures where BuildingID =@BuildingID";
            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;


                    conn.Open();    


                    object obj = cmd.ExecuteScalar();

           
                    if (obj != null && obj != DBNull.Value)
                    {
                        res = Convert.ToByte(obj);  
                    }


                }  

            }
            catch (Exception ex) {
                res = 0;
                Console.WriteLine(ex.Message);  
            }
            return res;
        
        }

        public static byte getNumberOfFloorsForBuilding(int BuildingID)
        {

            byte res = 0;
            string query = @"select  NumberOfFloors from BuildingFeatures where BuildingID =@BuildingID";
            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;


                    conn.Open();


                    object obj = cmd.ExecuteScalar();


                    if (obj != null && obj != DBNull.Value)
                    {
                        res = Convert.ToByte(obj);
                    }


                }

            }
            catch (Exception ex)
            {
                res = 0;
                Console.WriteLine(ex.Message);
            }
            return res;

        }

        public static bool checkAlivatorInBuilding(int BuildingID) { 
        

            string query = @"select Result = 1 from BuildingFeatures 
                               where BuildingID = @BuildingID and HasAlivator = 1";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand( query, conn))
                {

                    cmd.Parameters.Add("@BuildingID",SqlDbType.Int).Value = BuildingID;

                    conn.Open();    

                    object obj = cmd.ExecuteScalar();

                    return obj != null && obj != DBNull.Value;

                }



            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);  
                return false;
         
            }
               

        }
    }
}
