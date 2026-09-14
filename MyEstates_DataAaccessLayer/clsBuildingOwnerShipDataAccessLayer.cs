using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    public static class clsBuildingOwnerShipDataAccessLayer
    {

        public static bool FindBuildingOwnerShipWithID(ref int BuildingOwnerShip, ref decimal OwnerShipPercentage,
            ref int BuildingID, ref int OwnerID)
        {

            bool res = false;
            string query = @"select BuildingOwnerShip,OwnerShipPercentage ,BuildingID ,OwnerID from BuildingOwnerShip
                                where BuildingOwnerShip = @BuildingOwnerShip";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@BuildingOwnerShip", SqlDbType.Int).Value = BuildingOwnerShip;

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {

                        if (reader.Read()) {

                            res = true;

                            BuildingOwnerShip = reader["BuildingOwnerShip"] != DBNull.Value ? (int)reader["BuildingOwnerShip"] : -1;
                            OwnerShipPercentage = reader["OwnerShipPercentage"] != DBNull.Value ? (decimal)reader["OwnerShipPercentage"] : -1;
                            BuildingID = reader["BuildingID"] != DBNull.Value ? (int)reader["BuildingID"] : -1;
                            OwnerID = reader["OwnerID"] != DBNull.Value ? (int)reader["OwnerID"] : -1;

                          
                        
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



        public static int AddNewBuildingOwnerShip( decimal OwnerShipPercentage , int BuildingID, int OwnerID)
        {
            int ID = -1;

            string query = @"insert into BuildingOwnerShip (OwnerShipPercentage,BuildingID,OwnerID) values
                            (@OwnerShipPercentage,@BuildingID,@OwnerID);
                            SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection)) { 
                        
                    cmd.Parameters.Add("@OwnerShipPercentage",SqlDbType.Decimal).Value = OwnerShipPercentage;
                    cmd.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;
                    cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = OwnerID;
                

                    connection.Open();

                    object obj = cmd.ExecuteScalar();
              
                    if (obj != DBNull.Value && int.TryParse(obj.ToString(), out int resID)){
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

        public static bool UpdateBuildingOwnerShip(int BuildingOwnerShipID, decimal OwnerShipPercentage, int BuildingID, int OwnerID)
        {
            int rowsAffected = 0;

            string query = @"
                               Update BuildingOwnerShip Set 
                                 OwnerShipPercentage = @OwnerShipPercentage,
                                 BuildingID= @BuildingID,
                                 OwnerID= @OwnerID
                                 where BuildingOwnerShipID = @BuildingOwnerShipID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@BuildingOwnerShipID", SqlDbType.Int).Value = BuildingOwnerShipID;
                    cmd.Parameters.Add("@OwnerShipPercentage", SqlDbType.Decimal).Value = OwnerShipPercentage;
                    cmd.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;
                    cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = OwnerID;


                    connection.Open();

                    rowsAffected = cmd.ExecuteNonQuery();

                }


            }
            catch (Exception ex) { 
                rowsAffected = -1;
                Console.WriteLine(ex.Message);
            }

            return (rowsAffected > 0);
        }


        public static bool DeleteGroupOfBuildingOwnerShip(List<int> ids)
        {
            if (ids == null || ids.Count == 0) return false;

            int rowsAffected = 0;

            var parameters = ids.Select((id,index)=>"@id"+index).ToArray();

            string query = $@"delete from BuildingOwnerShip where BuildingOwnerShip in ({string.Join(",", parameters)})";

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        command.Parameters.Add(parameters[i],SqlDbType.Int).Value = ids[i];
                    }

                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();

                }



            }catch(Exception ex)
            {
                rowsAffected = -1;
                Console.WriteLine(ex.Message);   
            } 

            return (rowsAffected > 0);

        }


        public static bool DeleteSingleOfBuildingOwnerShip(int id)
        {
            int rowsAffected = 0;

            string query = @"delete from BuildingOwnerShip where BuildingOwnerShip = @id";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@id",SqlDbType.Int).Value = id; 

                    connection.Open();  

                    rowsAffected = cmd.ExecuteNonQuery();   




                }

            }
            catch (Exception ex) {

                rowsAffected = -1;
                Console.WriteLine(ex.Message);
            }

            return (rowsAffected > 0);
        }


        public static DataTable getOwnersWithPercentage(decimal OwnerShipPercentage)
        {
            DataTable dt = new DataTable();

            string query = @"SELECT O.* ,p.FirstName,p.LastName,p.FatherName,p.MotherName,p.CivilRegistry,p.Kaidinfo,BOS.OwnerShipPercentage ,BOS.BuildingID 
                                FROM owner O
                                inner join BuildingOwnerShip BOS
                                    ON O.OwnerID = BOS.OwnerID
                                inner join Person p on O.PersonID = p.PersonID
                                WHERE BOS.OwnerShipPercentage = @OwnerShipPercentage";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using(SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@OwnerShipPercentage",SqlDbType.Decimal).Value = OwnerShipPercentage;   
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {

                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    
                    }

                }

            }
            catch (Exception ex) {
                dt = null;
                Console.WriteLine(ex.Message);  
            }

            return dt;
        }


        public static DataTable getOwnersWithPercentageBiggerThan(decimal OwnerShipPercentage) { 
            
            DataTable dt = new DataTable();

            string query = @"SELECT O.* ,p.FirstName,p.LastName,p.FatherName,p.MotherName,p.CivilRegistry,p.Kaidinfo,BOS.OwnerShipPercentage ,BOS.BuildingID 
                                FROM owner O
                                inner join BuildingOwnerShip BOS
                                    ON O.OwnerID = BOS.OwnerID
                                inner join Person p on O.PersonID = p.PersonID
                                WHERE BOS.OwnerShipPercentage > @OwnerShipPercentage";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@OwnerShipPercentage", SqlDbType.Decimal).Value = OwnerShipPercentage;
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                dt = null;
                Console.WriteLine(ex.Message);
            }

            return dt;

        }


        public static DataTable getOwnersWithPercentageLessThan(decimal OwnerShipPercentage) {
            DataTable dt = new DataTable();

            string query = @"SELECT O.* ,p.FirstName,p.LastName,p.FatherName,p.MotherName,p.CivilRegistry,p.Kaidinfo,BOS.OwnerShipPercentage ,BOS.BuildingID 
                                FROM owner O
                                inner join BuildingOwnerShip BOS
                                    ON O.OwnerID = BOS.OwnerID
                                inner join Person p on O.PersonID = p.PersonID
                                WHERE BOS.OwnerShipPercentage < @OwnerShipPercentage";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@OwnerShipPercentage", SqlDbType.Decimal).Value = OwnerShipPercentage;
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                dt = null;
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        
        public static DataTable getAllOwnersOfThisBuildingWith(int BuildingID)
        {
            DataTable dt = new DataTable();
            string query = @"
                               SELECT O.* ,p.FirstName,p.LastName,p.FatherName,p.MotherName,
                                p.CivilRegistry,p.Kaidinfo,BOS.OwnerShipPercentage ,BOS.BuildingID 
                                FROM owner O
                                inner join BuildingOwnerShip BOS
                                    ON O.OwnerID = BOS.OwnerID
                                inner join Person p on O.PersonID = p.PersonID
                                inner join Building b on b.BuildingID = BOS.BuildingID
                                where b.BuildingID = @BuildingID";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) { 

                    if (reader.HasRows)
                    {
                        dt.Load(reader);

                    }
                }
            }

            }catch(Exception ex)
            {
                dt = null;
                Console.WriteLine(ex.Message);
            }

            return dt;

        }

        public static DataTable getAllBuildingsOfOwnerWith(int OwnerID) { 
        
            DataTable dt = new DataTable();

            string query = @"
                                SELECT  b.Name, b.BuildingNumber, BOS.OwnerShipPercentage ,BOS.BuildingID ,O.OwnerID
                                FROM owner O
                                inner join BuildingOwnerShip BOS
                                    ON O.OwnerID = BOS.OwnerID
                                inner join Building b on b.BuildingID = BOS.BuildingID
                                where O.OwnerID = @OwnerID";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = OwnerID;

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            dt.Load(reader);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                dt = null;
                Console.WriteLine(ex.Message);
            }

            return dt;

        }


    }


}
