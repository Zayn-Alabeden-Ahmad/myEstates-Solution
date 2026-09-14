using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    public static class clsCustomersInContractsDataAccessLayer
    {

        public static int Find_CinC_Relation( ref int CustomersInContract, ref int CustomerID , ref int ContractID)
        {
            short res = -1;
            string query = @"select CustomersInContract , CustomerID ,ContractID from CustomersInContract 
                                  where CustomersInContract = @CustomersInContract";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn)) { 
                        
                        cmd.Parameters.Add("@CustomersInContract",SqlDbType.Int).Value = CustomersInContract; 

                        conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {

                        if (reader.Read()) {

                            res = 1;

                            CustomersInContract = reader["CustomersInContract"] != DBNull.Value ? (int)reader["CustomersInContract"] : -1;
                            CustomerID = reader["CustomerID"] != DBNull.Value ? (int)reader["CustomerID"] : -1;
                            ContractID = reader["ContractID"] != DBNull.Value ? (int)reader["ContractID"] : -1;

                        }
                    
                    }
                    

                }


            }
            catch (Exception ex) { 
                res = -1;
               
            }

            return res;

        }

        public static int AddCustomersToContract(int CustomerID ,int ContractID)
        {
            int ID = -1;

            string query = @"insert into CustomersInContract(CustomerID,ContractID) values (@CustomerID,@ContractID);
                            SELECT SCOPE_IDENTITY();";


            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@CustomerID",SqlDbType.Int).Value = CustomerID;  
                    cmd.Parameters.Add("@ContractID", SqlDbType.Int).Value = ContractID;  

                    conn.Open();

                    object obj = cmd.ExecuteScalar();

                    if (obj != DBNull.Value && int.TryParse(obj.ToString(), out int resID))
                    {
                        ID = resID;
                    }
            


                }
            }
            catch (Exception ex) {
                ID = -1;
            }


            return ID;
        }


        public static bool UpdateCustomersToContractData(int CustomersInContract, int CustomerID,int ContractID)
        {

            int rowsAffected = -1;

            string query = @" update CustomersInContract set 
                            CustomerID = @CustomerID , ContractID = @ContractID
                            where  CustomersInContract = @CustomersInContract";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using(SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@CustomersInContract",SqlDbType.Int).Value= CustomersInContract;
                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value=CustomerID;
                    cmd.Parameters.Add("@ContractID", SqlDbType.Int).Value= ContractID;

                    connection.Open();


                    rowsAffected = cmd.ExecuteNonQuery();

                }


            }
            catch (Exception ex) {
                rowsAffected = -1;
            }

            return (rowsAffected > 0);

        }


        public static bool DeleteMultiCustomerContracts(List<int> ids) {

            int rowsAffected = -1;

            if(ids == null || ids.Count == 0 ) return false;

            var parameters = ids.Select((id,index)=>"@id"+index).ToArray();

            string query = $@"delete from CustomersInContract where CustomersInContract in({string.Join(",", parameters)})";


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


            }catch  (Exception ex)
            {
                rowsAffected = -1;

            }

            return (rowsAffected > 0);
        }
        public static bool DeleteSingleCustomerContract(int CustomersInContract)
        {

            int rowsAffected = -1;

   

            string query = @"delete from CustomersInContract where CustomersInContract = @CustomersInContract";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                  
                    cmd.Parameters.Add("@CustomersInContract", SqlDbType.Int).Value = CustomersInContract;
                    

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
