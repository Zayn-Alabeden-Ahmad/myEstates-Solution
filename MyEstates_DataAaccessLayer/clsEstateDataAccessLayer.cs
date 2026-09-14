using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    static public class clsEstateDataAccessLayer
    {

        static public int getEstateFeaturIDFormEstateTable(int EstateID)
        {

            int ID = -1;

            string query = "select FeaturesID from Estates where EstateID = @EstateID";


            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    command.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID;

                    conn.Open();


                    // note 
                    /*
                     * *While this works, ExecuteScalar returns DBNull.Value (not a C# null) if the column in the database contains a NULL value.
                     * It returns C# null only if the query returns no rows. A cleaner, more "standard" way to handle this in ADO.NET is:
                     * 
                     *    object res = command.ExecuteScalar();
                            if (res != null && res != DBNull.Value)
                            {
                                ID = Convert.ToInt32(res);
                            }
                     */

                    object res = command.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        ID = Convert.ToInt32(res);
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
        static public int getBuildingIDofEstate(int EstateID)
        {

            int ID = -1;

            string query = "select BuildingID from Estates where EstateID = @EstateID";


            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    command.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID;

                    conn.Open();


                    // note 
                    /*
                     * *While this works, ExecuteScalar returns DBNull.Value (not a C# null) if the column in the database contains a NULL value.
                     * It returns C# null only if the query returns no rows. A cleaner, more "standard" way to handle this in ADO.NET is:
                     * 
                     *    object res = command.ExecuteScalar();
                            if (res != null && res != DBNull.Value)
                            {
                                ID = Convert.ToInt32(res);
                            }
                     */

                    object res = command.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        ID = Convert.ToInt32(res);
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

        public static bool FindEstateWithEstateID(ref int EstateID,
                    ref string EstateName, ref string EstateNumber, ref decimal Price, ref byte EstateType, ref byte SellingEstate
                        , ref byte EstateStatus, ref byte SetFor, ref bool HasKeys,
                    ref DateTime BuiltDate, ref int FeaturesID, ref int BuildingID)
        {

            bool res = false;

            string query = "select * from Estates where EstateID = @EstateID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID;
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        res = true;

                        EstateID = reader["EstateID"] != DBNull.Value ? (int)reader["EstateID"] : -1;
                        FeaturesID = reader["FeaturesID"] != DBNull.Value ? (int)reader["FeaturesID"] : -1;
                        BuildingID = reader["BuildingID"] != DBNull.Value ? (int)reader["BuildingID"] : -1;

                        EstateName = reader["EstateName"]?.ToString() ?? "";
                        EstateNumber = reader["EstateNumber"]?.ToString() ?? "";

                        Price = reader["Price"] != DBNull.Value ? (decimal)reader["Price"] : 0;

                        EstateType = reader["EstateType"] != DBNull.Value ? (byte)reader["EstateType"] : (byte)0;
                        SellingEstate = reader["SellingEstate"] != DBNull.Value ? (byte)reader["SellingEstate"] : (byte)0;
                        EstateStatus = reader["EstateStatus"] != DBNull.Value ? (byte)reader["EstateStatus"] : (byte)0;
                        SetFor = reader["SetFor"] != DBNull.Value ? (byte)reader["SetFor"] : (byte)0;

                        HasKeys = reader["HasKeys"] != DBNull.Value ? (bool)reader["HasKeys"] : false;

                        BuiltDate = reader["BuiltDate"] != DBNull.Value ? (DateTime)reader["BuiltDate"] : DateTime.Now;


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



        public static int AddNewEstateToDB(string EstateName, string EstateNumber, decimal Price,
                   byte EstateType, byte SellingEstate, byte EstateStatus, byte SetFor, bool HasKeys, DateTime BuiltDate,
                  int FeaturesID, int BuildingID)
        {

            int ID = -1;

            string query = @"insert into Estates (EstateName , EstateNumber, Price,
                   EstateType,SellingEstate, EstateStatus, SetFor, HasKeys, BuiltDate,
                   FeaturesID, BuildingID) 
                   values
                  (@EstateName,  @EstateNumber,  @Price, @EstateType,  @SellingEstate,  @EstateStatus,  
                  @SetFor,  @HasKeys, @BuiltDate,@FeaturesID,  @BuildingID);
                  SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@EstateName", SqlDbType.NVarChar).Value = EstateName;
                    cmd.Parameters.Add("@EstateNumber", SqlDbType.NVarChar).Value = EstateNumber;
                    cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = Price;
                    cmd.Parameters.Add("@EstateType", SqlDbType.TinyInt).Value = EstateType;
                    cmd.Parameters.Add("@SellingEstate", SqlDbType.TinyInt).Value = SellingEstate;
                    cmd.Parameters.Add("@EstateStatus", SqlDbType.TinyInt).Value = EstateStatus;
                    cmd.Parameters.Add("@SetFor", SqlDbType.TinyInt).Value = SetFor;
                    cmd.Parameters.Add("@HasKeys", SqlDbType.Bit).Value = HasKeys;
                    cmd.Parameters.Add("@BuiltDate", SqlDbType.Date).Value = BuiltDate;
                    cmd.Parameters.Add("@FeaturesID", SqlDbType.Int).Value = FeaturesID;
                    cmd.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;

                    conn.Open();


                    object obj = cmd.ExecuteScalar();

                    if (obj != DBNull.Value && int.TryParse(obj.ToString(), out int resID))
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


        public static bool UpdateEstateInDB(int EstateID, string EstateName, string EstateNumber, decimal Price,
                   byte EstateType, byte SellingEstate, byte EstateStatus, byte SetFor, bool HasKeys, DateTime BuiltDate,
                  int FeaturesID, int BuildingID)
        {

            int rowsAffected = 0;


            string query = @"Update Estates set
                   EstateName = @EstateName ,
                   EstateNumber = @EstateNumber,
                   Price = @Price,
                   EstateType = @EstateType  ,
                   SellingEstate = @SellingEstate, 
                   EstateStatus = @EstateStatus,
                   SetFor = @SetFor,
                   HasKeys = @HasKeys, 
                   BuiltDate = @BuiltDate,
                   FeaturesID = @FeaturesID, 
                   BuildingID = @BuildingID where EstateID=@EstateID";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID;
                    cmd.Parameters.Add("@EstateName", SqlDbType.NVarChar).Value = EstateName;
                    cmd.Parameters.Add("@EstateNumber", SqlDbType.NVarChar).Value = EstateNumber;
                    cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = Price;
                    cmd.Parameters.Add("@EstateType", SqlDbType.TinyInt).Value = EstateType;
                    cmd.Parameters.Add("@SellingEstate", SqlDbType.TinyInt).Value = SellingEstate;
                    cmd.Parameters.Add("@SetFor", SqlDbType.TinyInt).Value = SetFor;
                    cmd.Parameters.Add("@EstateStatus", SqlDbType.TinyInt).Value = EstateStatus;
                    cmd.Parameters.Add("@HasKeys", SqlDbType.Bit).Value = HasKeys;
                    cmd.Parameters.Add("@BuiltDate", SqlDbType.Date).Value = BuiltDate;
                    cmd.Parameters.Add("@FeaturesID", SqlDbType.Int).Value = FeaturesID;
                    cmd.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;

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


        public static bool DeleteEstatesFromDB(List<int> estateIds)
        {

            bool res = false;

            if (estateIds == null || estateIds.Count == 0) return false;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.DeleteEstatesOnly", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable tvp = new DataTable();

                    tvp.Columns.Add("ID", typeof(int));

                    foreach (int id in estateIds)
                    {
                        tvp.Rows.Add(id);
                    }

                    SqlParameter p = cmd.Parameters.Add("@EstateIds", SqlDbType.Structured);
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


        public static bool DoesEstateExsist(int EstateID)
        {
            if (EstateID <= 0) return false;

            string query = "select COUNT(1) from Estates where EstateID = @EstateID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID;

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


        public static DataTable getAllEstatesInfosFromDB()
        {
            DataTable dt = new DataTable();

            string query = "select * from vw_AllEstatesInformations";

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
                Console.WriteLine(ex.Message);
            }


            return dt;

        }

        public static byte ExecuteGetSellingEstateStatus(int? EstateID, string EstateNumber)
        {

            object result = null;
            // here we will use ternary with mixed types
            // so we need  a common type to cast to => and its Object   

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.GetSellingEstateStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@EstateID", SqlDbType.Int).Value =
                        EstateID.HasValue ? (object)EstateID.Value : DBNull.Value;

                    // string is already a reference type and can upcast to object (object)EstateNumber

                    cmd.Parameters.Add("@EstateNumber", SqlDbType.NVarChar, 50).Value =
                        EstateNumber != null ? (object)EstateNumber : DBNull.Value;

                    conn.Open();

                    result = cmd.ExecuteScalar();

                }
            }
            catch (Exception ex)
            {
                result = null;
                Console.WriteLine(ex.Message);
            }

            return (result == null || result == DBNull.Value) ? (byte)0 : Convert.ToByte(result);
        }


        public static byte getEstateTypeFor(int? EstateID, string EstateNumber)
        {


            object res = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.GetEstateType", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID.HasValue ? (object)EstateID.Value : DBNull.Value;
                    cmd.Parameters.Add("@EstateNumber", SqlDbType.NVarChar).Value = EstateNumber != null ? (object)EstateNumber : DBNull.Value;


                    conn.Open();

                    res = cmd.ExecuteScalar();

                }
            }
            catch (Exception ex) { res = null; Console.WriteLine(ex.Message); }

            return (res == null || res == DBNull.Value) ? (byte)0 : Convert.ToByte(res);

        }

        // named parameter
        public static DataTable getAllEstatesWithFilter(byte? EstateType = null, byte? SellingEstate = null, byte? EstateStatus = null)
        {

            DataTable dataTable = new DataTable();


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.GetEstatesWith", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@EstateType", SqlDbType.TinyInt).Value = EstateType.HasValue ? (object)EstateType.Value : DBNull.Value;
                    cmd.Parameters.Add("@SellingEstate", SqlDbType.TinyInt).Value = SellingEstate.HasValue ? (object)SellingEstate.Value : DBNull.Value;
                    cmd.Parameters.Add("@EstateStatus", SqlDbType.TinyInt).Value = EstateStatus.HasValue ? (object)EstateStatus.Value : DBNull.Value;


                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        dataTable.Load(reader);
                    }
                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return null; }

            return dataTable;

        }


        public static decimal GetEstatePriceFor(int? EstateID, string EstateNumber)
        {

            object res = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.GetPriceForEstate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID.HasValue ? (object)EstateID.Value : DBNull.Value;
                    cmd.Parameters.Add("@EstateNumber", SqlDbType.NVarChar).Value = EstateNumber != null ? (object)EstateNumber : DBNull.Value;

                    conn.Open();

                    res = cmd.ExecuteScalar();

                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); res = null; }

            return (res == null || res == DBNull.Value) ? -1 : Convert.ToDecimal(res);


        }


        public static byte GetEstateStatusFor(int? EstateID, string EstateNumber)
        {

            object res = null;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.GetEstateStatus", connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID.HasValue ? (object)EstateID.Value : DBNull.Value;
                    cmd.Parameters.Add("@EstateNumber", SqlDbType.NVarChar).Value = EstateNumber != null ? (object)EstateNumber : DBNull.Value;

                    connection.Open();

                    res = cmd.ExecuteScalar();

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return (res == null || res == DBNull.Value) ? (byte)0 : Convert.ToByte(res);

        }



        public static DataTable getEstatesHasBuildPeriod(DateTime? Before = null, DateTime? After = null, DateTime? With = null)
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.GetEstatesWithBuildDate", connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Before", SqlDbType.Date).Value = Before != null ? (object)Before : DBNull.Value;
                    cmd.Parameters.Add("@After", SqlDbType.Date).Value = After != null ? (object)After : DBNull.Value;
                    cmd.Parameters.Add("@With", SqlDbType.Date).Value = With != null ? (object)With : DBNull.Value;


                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }

                }

            }
            catch (Exception ex) { dt = null; Console.WriteLine(ex.Message); }


            return dt;

        }


        public static DataTable getAllEstatesMarkedForSell(byte SetFor)
        {
            DataTable results = new DataTable();

            string query = @"select * from Estates where SetFor = @SetFor";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@SetFor", SqlDbType.TinyInt).Value = SetFor;


                    connection.Open();


                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.HasRows)
                    {
                        results.Load(reader);
                    }


                }



            }
            catch (Exception ex) { results = null; Console.WriteLine(ex.Message); }


            return results;

        }

        public static DataTable getEstatesWithGarages()
        {
            DataTable results = new DataTable();

            string query = @"
                            select EstateID, EstateName,EstateNumber,
                            SellingEstate,SetFor,EstateSpace,
                            EstateType,GarageLocation,GarageSize,GarageNumber
                            from vw_AllEstatesInformations
                            where GarageID is not NULL";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            results.Load(reader);
                        }
                    }

                }



            }
            catch (Exception ex) { results = null; Console.WriteLine(ex.Message); }


            return results;

        }

        public static DataTable getEstatesWithGaragesNumberEqualsTo(byte GarageNumber)
        {


            DataTable result = new DataTable();


            string query = @"select EstateID, EstateName,EstateNumber,SellingEstate,SetFor,EstateSpace,
                            EstateType,GarageLocation,GarageSize,GarageNumber from vw_AllEstatesInformations 
                            where GarageNumber = @GarageNumber";



            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@GarageNumber", SqlDbType.TinyInt).Value = GarageNumber;

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            result.Load(reader);
                        }
                    }

                }





            }
            catch (Exception ex) { result = null; Console.WriteLine(ex.Message); }


            return result;
        }


        public static DataTable getEstatesWithGaragesSizeEqualsTo(decimal GarageSize)
        {
            DataTable result = new DataTable();


            string query = @"select EstateID, EstateName,EstateNumber,SellingEstate,SetFor,EstateSpace,
                            EstateType,GarageLocation,GarageSize,GarageNumber from vw_AllEstatesInformations 
                            where GarageSize = @GarageSize";



            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@GarageSize", SqlDbType.Decimal).Value = GarageSize;

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            result.Load(reader);
                        }
                    }

                }





            }
            catch (Exception ex) { result = null; Console.WriteLine(ex.Message); }


            return result;
        }


        public static DataTable getEstatesWithPools()
        {
               DataTable table = new DataTable();

                 string query = @"select EstateID, EstateName,EstateNumber,SellingEstate,SetFor,EstateSpace,
                                EstateType,PoolNumber,PoolVolume
                                from vw_AllEstatesInformations
                                where PoolID is not NULL";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {


                        if (reader.HasRows)
                        {
                            table.Load(reader);
                        }

                    }
                }


            }catch(Exception ex) { table = null;    Console.WriteLine(ex.Message); }


            return table;
        }
    
        public static DataTable getEstatesWithPoolsNumberEqualsTo(byte PoolNumber)
        {

                DataTable table = new DataTable();


            string query = @"select EstateID, EstateName,EstateNumber,SellingEstate,SetFor,EstateSpace,
                                EstateType,PoolNumber,PoolVolume
                                from vw_AllEstatesInformations
                                where PoolNumber = @PoolNumber";

            try
            {


                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection)) {


                    cmd.Parameters.Add("@PoolNumber", SqlDbType.TinyInt).Value = PoolNumber;
                
                   connection.Open();


                    using (SqlDataReader reader = cmd.ExecuteReader()) {

                        if (reader.HasRows) { 
                        
                            table.Load(reader);
                        }
                    
                    
                    }
                } 


            }catch(Exception ex)
            {
                table = null;
                Console.WriteLine(ex.Message);  
            }


            return table;   

        }


        public static DataTable getEstatesWithPoolVolumeEqualsTo(decimal PoolVolume)
        {
            DataTable table = new DataTable();


            string query = @"select EstateID, EstateName,EstateNumber,SellingEstate,SetFor,EstateSpace,
                                EstateType,PoolNumber,PoolVolume
                                from vw_AllEstatesInformations
                                where PoolVolume = @PoolVolume";

            try
            {


                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {


                    cmd.Parameters.Add("@PoolVolume", SqlDbType.Decimal).Value = PoolVolume;

                    connection.Open();


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {

                            table.Load(reader);
                        }


                    }
                }


            }
            catch (Exception ex)
            {
                table = null;
                Console.WriteLine(ex.Message);
            }


            return table;
        }




    } 
}
