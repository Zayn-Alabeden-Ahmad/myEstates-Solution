using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    static public class clsAddressesDataAccessLayer
    {

        public static bool FindAddress(ref int AddressID, ref string City, ref string Region, ref string Street)
        {

            bool res = false;

            string query = "select * from Addresses where AddressID = @AddressID";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@AddressID", SqlDbType.Int).Value = AddressID;
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
                    cmd.Parameters.Add("@Region", SqlDbType.NVarChar).Value = Region;
                    cmd.Parameters.Add("@Street", SqlDbType.NVarChar).Value = Street;

                    conn.Open();


                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.Read())
                    {

                        res = true;

                        AddressID = reader["AddressID"] != DBNull.Value ? (int)reader["AddressID"] : -1;
                        City = reader["City"]?.ToString() ?? "";
                        Region = reader["Region"]?.ToString() ?? "";
                        Street = reader["Street"]?.ToString() ?? "";


                    }

                }





            }
            catch (Exception ex)
            {

                res = false;
                Console.WriteLine(ex.Message);
            }

            return res;

        }



        public static int AddNewAddressToDB(string City, string Region, string Street)
        {

            int AdddressId = -1;

            string query = @"insert into Addresses(City,Region,Street) 
                        values (@City,@Region,@Street);
                        SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
                    cmd.Parameters.Add("@Region", SqlDbType.NVarChar).Value = Region;
                    cmd.Parameters.Add("@Street", SqlDbType.NVarChar).Value = Street;

                    conn.Open();

                    object res = cmd.ExecuteScalar();

                    if (res != DBNull.Value && int.TryParse(res.ToString(), out int resId))
                    {
                        AdddressId = resId;
                    }

                }

            }
            catch (Exception ex)
            {

                AdddressId = -1;
                Console.WriteLine(ex.Message);
            }


            return AdddressId;
        }


        public static bool UpdateAddressInDB(int AddressID, string City, string Region, string Street)
        {

            int rowsAffected = 0;

            string query =
                @"update Addresses set City=@City ,Region=@Region,
                Street=@Street 
                where AddressID = @AddressID";



            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
                    cmd.Parameters.Add("@Region", SqlDbType.NVarChar).Value = Region;
                    cmd.Parameters.Add("@Street", SqlDbType.NVarChar).Value = Street;

                    conn.Open();


                    rowsAffected = cmd.ExecuteNonQuery();   


                }
            
            
            
            }
            catch (Exception ex)
            {

                rowsAffected = 0;
                Console.WriteLine(ex.Message);
            }



            return (rowsAffected > 0);
        }



        public static bool DeleteAddressesAndUnlinkRefsFromDB(List<int> addressIds)
        {
            if (addressIds == null || addressIds.Count == 0)
                return false;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.DeleteAddressesAndUnlinkRefs", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // TVP = Table-Valued Parameter.
                    // it means you pass a table (multiple rows) as one SQL parameter from C# to a stored procedure.

                    DataTable tvp = new DataTable();

                    tvp.Columns.Add("ID", typeof(int));


                    foreach (int id in addressIds)
                    {
                        tvp.Rows.Add(id);
                    }

                    SqlParameter p = cmd.Parameters.Add("@AddressIds", SqlDbType.Structured);

                    p.TypeName = "dbo.IntIdList";

                    p.Value = tvp; // passing it as one value

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public static bool DoesAddressExsist(int AddressID)
        {

       
            if(AddressID <= 0) return false;

            string query = "select COUNT(1) from Addresses where AddressID = @AddressID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using(SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@AddressID", SqlDbType.Int).Value = AddressID;

                    connection.Open();  
                        
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                   
                    return count > 0;

                }


            }
            catch (Exception ex) { 
            
                Console.WriteLine(ex.Message);
                return false ;
            }


        }

    }
}
