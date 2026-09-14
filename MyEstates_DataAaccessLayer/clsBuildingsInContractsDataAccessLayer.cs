using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    public class clsBuildingsInContractsDataAccessLayer
    {

        public static int Find_BinC_Relation(ref int BuildingsInContract, ref int BuildingID, ref int ContractID)
        {
            short res = -1;
            string query = @"select BuildingsInContract , BuildingID ,ContractID from BuildingsInContract 
                                  where BuildingsInContract = @BuildingsInContract";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@BuildingsInContract", SqlDbType.Int).Value = BuildingsInContract;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            res = 1;

                            BuildingsInContract = reader["BuildingsInContract"] != DBNull.Value ? (int)reader["BuildingsInContract"] : -1;
                            BuildingID = reader["BuildingID"] != DBNull.Value ? (int)reader["BuildingID"] : -1;
                            ContractID = reader["ContractID"] != DBNull.Value ? (int)reader["ContractID"] : -1;

                        }

                    }


                }


            }
            catch (Exception ex)
            {
                res = -1;

            }

            return res;

        }
    
        public static int AddBuildingToContract(int BuildingID,  int ContractID)
        {
            int ID = -1;

            string query = @"insert into BuildingsInContract(BuildingID,ContractID) values (@BuildingID,@ContractID);
                            SELECT SCOPE_IDENTITY();";


            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;
                    cmd.Parameters.Add("@ContractID", SqlDbType.Int).Value = ContractID;

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
            }


            return ID;
        }

        public static bool UpdateBuildingToContractData(int BuildingsInContract, int BuildingID, int ContractID)
        {
            int rowsAffected = -1;

            string query = @" update BuildingsInContract set 
                            BuildingID = @BuildingID , ContractID = @ContractID
                            where  BuildingsInContract = @BuildingsInContract";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@BuildingsInContract", SqlDbType.Int).Value = BuildingsInContract;
                    cmd.Parameters.Add("@BuildingID", SqlDbType.Int).Value = BuildingID;
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

        public static bool DeleteSingleBuildingContract(int BuildingsInContract)
        {

            int rowsAffected = -1;



            string query = @"delete from BuildingsInContract where BuildingsInContract = @BuildingsInContract";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {


                    cmd.Parameters.Add("@BuildingsInContract", SqlDbType.Int).Value = BuildingsInContract;


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

        public static bool DeleteMultiBuildingContracts(List<int> ids)
        {
            int rowsAffected = -1;

            if (ids == null || ids.Count == 0) return false;

            var parameters = ids.Select((id, index) => "@id" + index).ToArray();

            string query = $@"delete from BuildingsInContract where BuildingsInContract in({string.Join(",", parameters)})";


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
