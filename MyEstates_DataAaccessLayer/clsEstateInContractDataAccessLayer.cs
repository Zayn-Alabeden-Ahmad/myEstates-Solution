using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    public static class clsEstateInContractDataAccessLayer
    {

        public static int Find_EinC_Relation( ref int EstatesInContract, ref int EstateID, ref int ContractID)
        {
            int id = -1;
            string query = @"select * from EstatesInContract where EstatesInContract = @EstatesInContract";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) { 
                
                    command.Parameters.Add("@EstatesInContract",SqlDbType.Int).Value = EstatesInContract;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader()) {

                        if (reader.Read()) {
                            
                            id = 1;

                            EstatesInContract = reader["EstatesInContract"] != DBNull.Value ? (int)reader["EstatesInContract"] : -1;
                            EstateID = reader["EstateID"] != DBNull.Value ? (int)reader["EstateID"] : -1;
                            ContractID = reader["ContractID"] != DBNull.Value ? (int)reader["ContractID"] : -1;
                        
                        }
                    
                    }
                
                }

            }
            catch (Exception ex) {
                id = -1;
            }

            return id;

        }


        public static int AddEstateToContract(int EstateID, int ContractID)
        {
            int id = -1;
            string query = @"insert into EstatesInContract(EstateID,ContractID) values (@EstateID,@ContractID);
                            SELECT SCOPE_IDENTITY();";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@EstateID",SqlDbType.Int).Value=EstateID;
                    command.Parameters.Add("@ContractID", SqlDbType.Int).Value= ContractID;

                    connection.Open();  

                    object obj = command.ExecuteScalar();

                    if (obj != DBNull.Value && int.TryParse(obj.ToString(), out int redID))
                    {
                        id = redID;
                    }


                } 

            }catch (Exception ex) 
            { 
                id = -1; 
            }
            return id;

        }

        public static bool UpdateEstateToContractData( int EstatesInContract,  int EstateID,  int ContractID)
        {
            int rowsAffected = -1;

            string query = @"update EstatesInContract set 
                            EstateID = @EstateID , ContractID = @ContractID
                            where  EstatesInContract = @EstatesInContract";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@EstatesInContract", SqlDbType.Int).Value = EstatesInContract;
                    cmd.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID;
                    cmd.Parameters.Add("@ContractID", SqlDbType.Int).Value = ContractID;

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

        public  static bool  DeleteSingleEstateContract(int EstatesInContract)
        {

            int rowsAffected = -1;

            string query = @"delete from EstatesInContract where EstatesInContract = @EstatesInContract";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {


                    cmd.Parameters.Add("@EstatesInContract", SqlDbType.Int).Value = EstatesInContract;


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

        public static bool DeleteMultiEstateContracts(List<int> ids)
        {
            int rowsAffected = -1;

            if (ids == null || ids.Count == 0) return false;

            var parameters = ids.Select((id, index) => "@id" + index).ToArray();

            string query = $@"delete from EstatesInContract where EstatesInContract in({string.Join(",", parameters)})";


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

    }
}
