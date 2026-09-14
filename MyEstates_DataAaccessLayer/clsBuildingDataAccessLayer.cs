using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyEstates_DataAaccessLayer
{
    static public class clsBuildingDataAccessLayer
    {
     
        static public short FindBuildingWithID(ref int BuildingID, ref string Name, ref string
            BuildingNumber, ref string City, ref string Region, ref string Street, ref int AddressID)
        {

            short res = -1;
            string query = @"select v.*, b.AddressID
                             from vw_BuildingView v
                             inner join Building b on b.BuildingID = v.BuildingID
                             where v.BuildingID = @BuildingID";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;

                    conn.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        res = 1;

                        BuildingID = reader["BuildingID"] != DBNull.Value ? (int)reader["BuildingID"] : -1;
                        AddressID = reader["AddressID"] != DBNull.Value ? (int)reader["AddressID"] : -1;
                   

                        Name = reader["Name"]?.ToString() ?? "";
                        BuildingNumber = reader["BuildingNumber"]?.ToString() ?? "";
                        City = reader["City"]?.ToString() ?? "";
                        Region = reader["Region"]?.ToString() ?? "";
                        Street = reader["Street"]?.ToString() ?? "";

                    }


                }

            }
            catch (Exception ex)
            {
                res = -1;
                Console.WriteLine(ex.Message);
            }

            return res;

        }

        static public DataTable GetEstateInBuilding(int BuildingID)
        {

            DataTable estatesInBuilding = new DataTable();

            string query = "select * from Estates where BuildingID = @BuildingID";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {


                    command.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;

                    conn.Open();


                    SqlDataReader reader = command.ExecuteReader();


                    if (reader.HasRows)
                    {

                        estatesInBuilding.Load(reader);

                    }

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }

            return estatesInBuilding;
        }

        static public int AddNewBuilding(string Name
                , string BuildingNumber, int AddressID)
        {

            int ID = -1;
            string query = @"insert into Building(Name,BuildingNumber,AddressID) 
                        values(@Name,@BuildingNumber,@AddressID);
                        SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                    command.Parameters.Add("@BuildingNumber", SqlDbType.NVarChar).Value = BuildingNumber;
                    command.Parameters.Add("@AddressID", SqlDbType.Int).Value = AddressID;
       

                    conn.Open();


                    object res = command.ExecuteScalar();

                    if (res != DBNull.Value && int.TryParse(res.ToString(), out int resID))
                    {
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

        static public bool UpdateBuildingData(int BuildingID, string Name
                , string BuildingNumber, int AddressID)
        {

            int rowsAffected = 0;

            string query = @"update Building set
                                        Name=@Name,
                                        BuildingNumber=@BuildingNumber,
                                        AddressID=@AddressID
                                        where BuildingID = @BuildingID;";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn)) {

                    command.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                    command.Parameters.Add("@BuildingNumber", SqlDbType.NVarChar).Value = BuildingNumber;
                    command.Parameters.Add("@AddressID", SqlDbType.Int).Value = AddressID;



                    conn.Open();

                    rowsAffected = command.ExecuteNonQuery();



                }


            }
            catch (Exception ex)
            {
                rowsAffected = 0;
                Console.WriteLine(ex.Message);
            }

            return (rowsAffected > 0);
        }

        static public bool DeleteMultiBuildingsFromDB(List<int> ids)
        {
            if(ids == null || ids.Count == 0) return false;

            int rowsAffected = 0;
                    
            var parameters = ids.Select((id,index) => "@id"+index).ToArray();

            string query = $@"delete from Building where BuildingID in ({string.Join(",",parameters)})";


            try
            {
                using(SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    for (int i = 0; i < parameters.Length; i++) {

                        command.Parameters.Add(parameters[i],SqlDbType.Int).Value = ids[i];
                        
                    }

                    conn.Open();    

                    rowsAffected = command.ExecuteNonQuery();


                }



            }catch(Exception ex)
            {

                rowsAffected = 0;
                Console.WriteLine(ex.Message);   

            }



            return (rowsAffected > 0);

        }

        static public bool DeleteSingleBuildingsFromDB(int BuildingID)
        {


            int rowsAffected = 0;

            string query = @"delete from Building where BuildingID = @BuildingID";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                 
                    command.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;

                    conn.Open();

                    rowsAffected = command.ExecuteNonQuery();


                }

            }
            catch (Exception ex)
            {

                rowsAffected = 0;
                Console.WriteLine(ex.Message);

            }

            return (rowsAffected > 0);

        }


        public static DataTable getAllBuildingsFromDB()
        {
           DataTable dataTable = new DataTable();

            string query = @"select * from Building";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }
                    }
                }

            }
            catch (Exception ex) { dataTable = null; }

            return dataTable;


        }

        public static bool DoesBuildingExsist(int BuildingID) {

            if (BuildingID <= 0) return false;

            string query = "select COUNT(1) from Building where BuildingID = @BuildingID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;

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

        public static string getBuildingNumberFromDB(int BuildingID)
        { 
                
            string res = string.Empty;

            string query = @"select BuildingNumber From Building where BuildingID =@BuildingID";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;

                    conn.Open();


                    object ob = cmd.ExecuteScalar();

                    if(ob != null && ob != DBNull.Value)
                    {
                        res = Convert.ToString(ob);
                    }

                }
            }
            catch (Exception ex) {
                res = string.Empty;
                Console.WriteLine(ex.Message);
            
            }

            return res;
        
        }

        public static DataTable getBuildingsWithFeatureFromDB(int BuildingFeatureID)
        {

            DataTable dt = new DataTable();

            string query = @"
                            select * from Building b inner join BuildingFeatures bf
                            on b.BuildingID = bf.BuildingID
                            where bf.BuildingFeaturesID = @BuildingFeatureID";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using(SqlCommand cmd =new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@BuildingFeatureID",SqlDbType.Int).Value =BuildingFeatureID;

                    conn.Open();


                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows) { 
                    
                        dt.Load(reader);    
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

        public static DataTable GetBuildingsWithNumOfEstatesForSell(long numOfEstatesForSellNumber)
        {
            DataTable dt = new DataTable();

            string query = @"
                            select
                            b.BuildingID,
                            b.BuildingNumber,
                            ad.City,
                            ad.Region,
                            ad.Street
                                from Estates e
                                inner join Building b on e.BuildingID = b.BuildingID
                                inner join Addresses ad on b.AddressID = ad.AddressID
                                where e.SetFor = 1
                            group by
                                b.BuildingID,
                                b.BuildingNumber,
                                ad.City,
                                ad.Region,
                                ad.Street
                           having count(*)  = @numOfEstatesForSellNumber;";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@numOfEstatesForSellNumber", SqlDbType.BigInt).Value = numOfEstatesForSellNumber;

                    conn.Open();


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

        public static DataTable GetBuildingsWithAnyEstatesForSell() {

            DataTable dt = new DataTable();

            string query = @"select
                            b.BuildingID,
                            b.BuildingNumber,
                            ad.City,
                            ad.Region,
                            ad.Street
                            from Estates e
                                inner join Building b on e.BuildingID = b.BuildingID
                                inner join Addresses ad on b.AddressID = ad.AddressID
                                where e.SetFor = 1
                             group by
                                b.BuildingID,
                                b.BuildingNumber,
                                ad.City,
                                ad.Region,
                                ad.Street;";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    conn.Open();


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

        public static DataTable getBuildingsInAddress(int AddressID)
        {
            DataTable dt = new DataTable();
            string query = "select * from Building where AddressID = @AddressID";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString)) 
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@AddressID", SqlDbType.Int).Value = AddressID;

                     conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows) {

                        dt.Load(reader);
                    
                    }
                }
        



            }catch(Exception ex)
            {
                dt =null;   
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public static DataTable getAllBuildingsWithFloorNumber(int NumberOfFloors) {

            DataTable dt = new DataTable();
            string query = @"select * from vw_BuildingsAndFeatures where NumberOfFloors = @NumberOfFloors";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@NumberOfFloors", SqlDbType.TinyInt).Value = NumberOfFloors;

                    conn.Open();

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
                Console.WriteLine(ex.Message);
            }

            return dt;

        }
        public static DataTable getAllBuildingsWithBlocksrNumber(int NumberOfBlocks)
        {


            DataTable dt = new DataTable();
            string query = @"select * from vw_BuildingsAndFeatures where NumberOfBlocks = @NumberOfBlocks";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@NumberOfBlocks", SqlDbType.TinyInt).Value = NumberOfBlocks;

                    conn.Open();

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
                Console.WriteLine(ex.Message);
            }

            return dt;


        }

        public static DataTable getBuildingsWithAverageEstatesPrice(decimal amount)
        {

            DataTable dt = new DataTable();

            // يرجع المباني التي متوسط أسعار العقارات فيها يساوي amount

            string query = @"
                            SELECT
                                b.BuildingID,
                                b.Name,
                                b.BuildingNumber,
                                ad.City,
                                ad.Region,
                                ad.Street
                            FROM Estates e
                            JOIN Building b ON e.BuildingID = b.BuildingID
                            JOIN Addresses ad ON b.AddressID = ad.AddressID
                            GROUP BY
                                b.BuildingID, b.BuildingNumber, ad.City, ad.Region, ad.Street ,  b.Name
                            HAVING ABS(AVG(CAST(e.Price AS DECIMAL(18,2))) - @amount) < 0.01;";


            //  (يعني نقبل النتيجة إذا الفرق أقل من 0.01 (سماحية بسيطة جدًا
            //   نقول: إذا الفرق أقل من قرش واحد، اعتبره مطابق 

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    SqlParameter p = new SqlParameter("@amount", SqlDbType.Decimal);
                    p.Precision = 18;
                    p.Scale = 2;
                    p.Value = amount;
                    cmd.Parameters.Add(p);


                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                            dt.Load(reader);
                    }
                }




            }
            catch (Exception ex) { dt = null; Console.WriteLine(ex.Message); }

            return dt;

        }
    }

}

