using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    public static class clsEstatePictureDataAccessLayer
    {

        public static int FindEstatePicture(ref int PictureID, ref  string PictureName, ref string PictureURL , ref int EstateID)
        {
            int res = -1;
            string query = @"select * from EstatePictures where PictureID = @PictureID";

            try
            {

                using(SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PictureID",SqlDbType.Int).Value = PictureID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) {

                            res = 1;

                            PictureID = reader["PictureID"] != DBNull.Value ? (int)reader["PictureID"] : -1;
                            PictureName = reader["PictureName"]?.ToString() ?? "";
                            PictureURL = reader["PictureURL"]?.ToString() ?? "";
                            EstateID = reader["EstateID"] != DBNull.Value ? (int)reader["EstateID"] : -1;
                        
                        
                        }
                    }
                }   
            }
            catch (Exception ex) { res = -1; }

            return res;
        }


        public static int AddNewEstatePictureToDB(string PictureName,  string PictureURL,  int EstateID)
        {
            int id = -1;
            string query = @"insert into EstatePictures(PictureName,PictureURL,EstateID) values (@PictureName,@PictureURL,@EstateID);
                            SELECT SCOPE_IDENTITY();";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PictureName",SqlDbType.NVarChar).Value  = PictureName;
                    command.Parameters.Add("@PictureURL", SqlDbType.NVarChar).Value  = PictureURL;
                    command.Parameters.Add("@EstateID", SqlDbType.Int).Value  = EstateID;
                
                    object obj = command.ExecuteScalar();

                    if (obj != DBNull.Value && int.TryParse(obj.ToString(), out int resID))
                    {
                        id = resID;
                    }
                
                }

            }
            catch (Exception ex) { id = -1; }

            return id;
        }

        public  static bool UpdateEstatePictureInDB(int PictureID , string PictureName, string PictureURL, int EstateID)
        {
            int rowsAffected = -1;

            string query = @"update EstatePictures set 
                            PictureName = @PictureName ,PictureURL = @PictureURL , EstateID = @EstateID
                            where  PictureID = @PictureID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@PictureID", SqlDbType.Int).Value = PictureID;
                    cmd.Parameters.Add("@PictureName", SqlDbType.NVarChar).Value = PictureName;
                    cmd.Parameters.Add("@PictureURL", SqlDbType.NVarChar).Value = PictureURL;
                    cmd.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID;


                    connection.Open();


                    rowsAffected = cmd.ExecuteNonQuery();

                }


            }
            catch (Exception ex)
            {
                rowsAffected = -1;
            }

            return (rowsAffected > 0);


        }

        public static bool DeleteSingleEstatePicture(int PictureID)
        {
            int rowsAffected = -1;

            string query = @"delete from EstatePictures where PictureID = @PictureID";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {


                    cmd.Parameters.Add("@PictureID", SqlDbType.Int).Value = PictureID;


                    conn.Open();

                    rowsAffected = cmd.ExecuteNonQuery();

                }


            }
            catch (Exception ex)
            {
                rowsAffected = -1;

            }

            return (rowsAffected > 0);
        }


        public static bool DeleteMultiEstatePictures(List<int> ids) {

            int rowsAffected = -1;

            if (ids == null || ids.Count == 0) return false;

            var parameters = ids.Select((id, index) => "@id" + index).ToArray();

            string query = $@"delete from EstatePictures where PictureID in({string.Join(",", parameters)})";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i], SqlDbType.Int).Value = ids[i];
                    }

                    conn.Open();

                    rowsAffected = cmd.ExecuteNonQuery();

                }


            }
            catch (Exception ex)
            {
                rowsAffected = -1;

            }

            return (rowsAffected > 0);

        }
    

        public static DataTable getPicturesOfEstateWithEstateID(int EstateID)
        {

            DataTable dt = new DataTable();

            string query = @"select PictureName, PictureURL from EstatePictures where EstateID = @EstateID";


            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@EstateID",SqlDbType.Int).Value = EstateID; 

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) { 
                            dt.Load(reader);    
                        }
                    }
                } 


            }
            catch (Exception ex) { dt = null; }

            return dt;

        }
 

    }

}
