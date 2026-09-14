using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    public static class clsContractDataAccessLayer
    {

        public static bool FindContractWithId( ref int ContractID, ref bool ContractType, ref string ContractImg,
                    ref short ContractDuration, ref DateTime StartDate, ref DateTime EndDate, ref decimal SettledPrice
                    , ref decimal Commission, ref bool IsActive, ref DateTime Created_At)
        {

            bool res = false;


            string query = @"select * from Contract  where ContractID = @ContractID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {

                    command.Parameters.Add("@ContractID", SqlDbType.Int).Value = ContractID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader()) {

                        if (reader.Read()) { 
                            
                            res = true;

                            ContractID = reader["ContractID"] != DBNull.Value ? (int)reader["ContractID"] : -1;

                            // True => rent  False => sell
                            ContractType = reader["ContractType"] != DBNull.Value ? (bool)reader["ContractType"] : false;

                            ContractImg = reader["ContractImg"]?.ToString() ?? "NO IMAGE FOUND";
                            ContractDuration = reader["ContractDuration"] != DBNull.Value ? (short)reader["ContractDuration"] : (short) 0 ;
                            StartDate = reader["StartDate"] != DBNull.Value ? (DateTime)reader["StartDate"] : DateTime.Now;
                            EndDate = reader["EndDate"] != DBNull.Value ? (DateTime)reader["EndDate"] : DateTime.Now;
                            SettledPrice = reader["SettledPrice"] != DBNull.Value ? (decimal)reader["SettledPrice"] : -1;
                            Commission = reader["Commission"] != DBNull.Value ? (decimal)reader["Commission"] : -1;
                            IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : false;
                            Created_At = reader["Created_At"] != DBNull.Value ? (DateTime)reader["Created_At"] : DateTime.Now;

                        }
                    
                    }

                }



            }
            catch (Exception ex) { 
            
                res = false;

            }

            return res;


        }

        public static int AddNewContractToDB(bool ContractType,  string ContractImg,
                     short ContractDuration,  DateTime StartDate,  DateTime EndDate,  decimal SettledPrice
                    ,  decimal Commission,  bool IsActive,  DateTime Created_At)
        {

            int ID = -1;

            string query = @"
            insert into Contract(ContractType,ContractImg,ContractDuration,StartDate,
                                EndDate,SettledPrice,Commission,IsActive,Created_At)
                values(@ContractType,@ContractImg,@ContractDuration,@StartDate,
                @EndDate,@SettledPrice,@Commission,@IsActive,@Created_At);
                SELECT SCOPE_IDENTITY();                                    
             ";


            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@ContractType", SqlDbType.TinyInt).Value = ContractType;
                    cmd.Parameters.Add("@ContractDuration", SqlDbType.SmallInt).Value = ContractDuration;
                    cmd.Parameters.Add("@ContractImg", SqlDbType.NVarChar).Value = ContractImg;
                    cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value = EndDate;
                    cmd.Parameters.Add("@SettledPrice", SqlDbType.Decimal).Value = SettledPrice;
                    cmd.Parameters.Add("@Commission", SqlDbType.Decimal).Value = Commission;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
                    cmd.Parameters.Add("@Created_At", SqlDbType.DateTime2).Value = Created_At;

                    conn.Open();    


                    object obj = cmd.ExecuteScalar();

                    if(obj != DBNull.Value && int.TryParse(obj.ToString(),out int resID))
                    {
                        ID = resID;
                    }

                }

                
            }catch(Exception ex)
            {
                ID = -1;
                Console.WriteLine(ex.Message);  
            }

            return ID;


        }

        public static bool UpdateContractInDB(int ContractID, bool ContractType, string ContractImg,
                     short ContractDuration, DateTime StartDate, DateTime EndDate, decimal SettledPrice
                    , decimal Commission, bool IsActive, DateTime Created_At)
        {
            int rowsAffected = 0;

            string query = @"
                          Update Contract set  ContractType = @ContractType , ContractImg = @ContractImg,  
                          ContractDuration = @ContractDuration, StartDate = @StartDate, EndDate = @EndDate,
                          SettledPrice = @SettledPrice, Commission = @Commission,IsActive = @IsActive,
                          Created_At = @Created_At where ContractID = @ContractID";


            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@ContractID", SqlDbType.Int).Value = ContractID;
                    cmd.Parameters.Add("@ContractType", SqlDbType.TinyInt).Value = ContractType;
                    cmd.Parameters.Add("@ContractDuration", SqlDbType.SmallInt).Value = ContractDuration;
                    cmd.Parameters.Add("@ContractImg", SqlDbType.NVarChar).Value = ContractImg;
                    cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value = EndDate;
                    cmd.Parameters.Add("@SettledPrice", SqlDbType.Decimal).Value = SettledPrice;
                    cmd.Parameters.Add("@Commission", SqlDbType.Decimal).Value = Commission;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
                    cmd.Parameters.Add("@Created_At", SqlDbType.DateTime2).Value = Created_At;

                    conn.Open();

                        
                   rowsAffected = cmd.ExecuteNonQuery();

                }


            }
            catch (Exception ex)
            {
                rowsAffected = -1;
                Console.WriteLine(ex.Message);
            }

            return (rowsAffected > 0);


        }

        public static bool DeleteMultiContractsFromDB(List<int>ids)
        {
            if(ids == null || ids.Count == 0) return false;
            
            int rowsAffected = -1;

            var parameters = ids.Select((id,index)=> "@id"+index).ToArray();

            string query = $@"delete from Contract where ContractID in ({string.Join(",", parameters)})";


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

            }catch(Exception ex) { rowsAffected = -1; Console.WriteLine(ex.Message); }


            return (rowsAffected > 0);
        }
        public static bool DeleteSingleContractFromDB(int ContractID)
        {
     

            int rowsAffected = -1;


            string query = $@"delete from Contract where ContractID = @ContractID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                
                    command.Parameters.Add("@ContractID", SqlDbType.Int).Value = ContractID;
                    

                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();

                }

            }
            catch (Exception ex) { rowsAffected = -1; Console.WriteLine(ex.Message); }


            return (rowsAffected > 0);
        }
        public static DataTable getContractsTypeFromDB(byte ContractType) 
        {
            
            DataTable contracts = new DataTable();

            string query = @"select * from Contract where ContractType = @ContractType";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ContractType", SqlDbType.TinyInt).Value = ContractType;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            contracts.Load(reader);
                        }

                    }
                }

            }catch(Exception ex)
            {
                contracts = null;
                Console.WriteLine(ex.Message);
            }

            return contracts;
        }
        public static DataTable getContractsWithDurationEqualTo(short ContractDuration) { 
        
            DataTable dataTable = new DataTable();

            string query = @"select * from Contract where ContractDuration = @ContractDuration";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@ContractDuration",SqlDbType.SmallInt).Value = ContractDuration;    

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                              dataTable.Load(reader);
                        }
                    }
                }   

            }catch (Exception ex)
            {
                dataTable  = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;

        }
        public static DataTable getContractsWithDurationMoreThan(short ContractDuration) {

            DataTable dataTable = new DataTable();

            string query = @"select * from Contract where ContractDuration > @ContractDuration";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@ContractDuration", SqlDbType.SmallInt).Value = ContractDuration;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;
        }
        public static DataTable getContractsWithDurationLessThan(short ContractDuration) {

            DataTable dataTable = new DataTable();

            string query = @"select * from Contract where ContractDuration < @ContractDuration";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@ContractDuration", SqlDbType.SmallInt).Value = ContractDuration;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;



        }
        public static DataTable getContractsWithSettledPriceEqual(decimal SettledPrice)
        {

            DataTable dataTable = new DataTable();
            string query = @"select * from Contract where SettledPrice = @SettledPrice";
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@SettledPrice", SqlDbType.Decimal).Value = SettledPrice;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;

        }
        public static DataTable getContractsWithSettledPriceMore(decimal SettledPrice)
        {

            DataTable dataTable = new DataTable();
            string query = @"select * from Contract where SettledPrice > @SettledPrice";
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@SettledPrice", SqlDbType.Decimal).Value = SettledPrice;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;

        }
        public static DataTable getContractsWithSettledPriceLess(decimal SettledPrice)
        {

            DataTable dataTable = new DataTable();
            string query = @"select * from Contract where SettledPrice < @SettledPrice";
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@SettledPrice", SqlDbType.Decimal).Value = SettledPrice;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;

        }

        public static DataTable getContractsWithCommisionEqual(decimal Commission) { 
            
            DataTable dataTable = new DataTable();
            string query = @"select * from Contract where Commission = @Commission";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Commission", SqlDbType.Decimal).Value = Commission;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) {
                            dataTable.Load(reader); 
                        }

                    }
                }
            }
            catch (Exception ex) {

                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable ;
    
        }
        public static DataTable getContractsWithCommisionMore(decimal Commission)
        {

            DataTable dataTable = new DataTable();
            string query = @"select * from Contract where Commission > @Commission";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Commission", SqlDbType.Decimal).Value = Commission;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;

        }

        public static DataTable getContractsWithCommisionLess(decimal Commission)
        {

            DataTable dataTable = new DataTable();
            string query = @"select * from Contract where Commission < @Commission";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Commission", SqlDbType.Decimal).Value = Commission;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;

        }


        public static DataTable getAllActiveContracts()
        {
            DataTable dataTable = new DataTable();
            string query = @"select * from vw_ActiveContract";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;
        }
        public static DataTable getAllInActiveContracts()
        {
            DataTable dataTable = new DataTable();
            string query = @"select * from vw_InActiveContract";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;
        }

        public static DataTable getAllContractsWithCreatedDateEqualTo(DateTime Created_At)
        {
            DataTable dataTable = new DataTable();
            string query = @"select * from Contract where Created_At = @Created_At";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Created_At",SqlDbType.DateTime2).Value = Created_At;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;

        }
        public static DataTable getAllContractsWithCreatedDateMoreThan(DateTime Created_At)
        {
            DataTable dataTable = new DataTable();
            string query = @"select * from Contract where Created_At > @Created_At";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Created_At", SqlDbType.DateTime2).Value = Created_At;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;

        }
        public static DataTable getAllContractsWithCreatedDateLessThan(DateTime Created_At)
        {
            DataTable dataTable = new DataTable();
            string query = @"select * from Contract where Created_At < @Created_At";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Created_At", SqlDbType.DateTime2).Value = Created_At;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;

        }

        public static DataTable getAllContractsWithStartDate(DateTime StartDate) { 
            
            DataTable dataTable = new DataTable();

            string query = @"select * from Contract where StartDate = @StartDate";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@StartDate", SqlDbType.Date).Value = StartDate;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;

        }

        public static DataTable getAllContractsWithEndDate(DateTime EndDate)
        {

            DataTable dataTable = new DataTable();

            string query = @"select * from Contract where EndDate = @EndDate";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@EndDate", SqlDbType.Date).Value = EndDate;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                dataTable = null;
                Console.WriteLine(ex.Message);
            }

            return dataTable;


        }

    }

}
