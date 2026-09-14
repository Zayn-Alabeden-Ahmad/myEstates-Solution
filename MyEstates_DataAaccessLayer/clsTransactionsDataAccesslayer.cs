using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    public static class clsTransactionsDataAccesslayer
    {
        public static int FindTranscation(ref int TransactionsID, ref decimal Amount, ref byte TransactionStatus
                , ref byte TranscationType, ref DateTime PaymentDate, ref int ContractID)
        {
            int res = -1;
            string query = @"select * from Transactions where TransactionsID = @TransactionsID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@TransactionsID", SqlDbType.Int).Value = TransactionsID;


                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            res = 1;
                            TransactionsID = reader["TransactionsID"] != DBNull.Value ? (int)reader["TransactionsID"] : -1;
                            ContractID = reader["ContractID"] != DBNull.Value ? (int)reader["ContractID"] : -1;
                            Amount = reader["Amount"] != DBNull.Value ? (decimal)reader["Amount"] : 0;
                            TransactionStatus = reader["TransactionStatus"] != DBNull.Value ? (byte)reader["TransactionStatus"] : (byte)0;
                            TranscationType = reader["TranscationType"] != DBNull.Value ? (byte)reader["TranscationType"] : (byte)0;
                            PaymentDate = reader["PaymentDate"] != DBNull.Value ? (DateTime)reader["PaymentDate"] : DateTime.MinValue;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                {
                    res = -1;
                }

            }
            return res;
        }

        public static int AddNewTransactionToDB(decimal Amount, byte TransactionStatus
                , byte TranscationType, DateTime PaymentDate, int ContractID)
        {
            int res = -1;
            string query = @"insert into Transactions(Amount,TransactionStatus,TranscationType,PaymentDate,ContractID) 
                values (@Amount,@TransactionStatus,@TranscationType,@PaymentDate,@ContractID); 
                SELECT SCOPE_IDENTITY();";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
                    command.Parameters.Add("@TransactionStatus", SqlDbType.TinyInt).Value = TransactionStatus;
                    command.Parameters.Add("@TranscationType", SqlDbType.TinyInt).Value = TranscationType;
                    command.Parameters.Add("@PaymentDate", SqlDbType.DateTime2).Value = PaymentDate;
                    command.Parameters.Add("@ContractID", SqlDbType.Int).Value = ContractID;

                    connection.Open();

                    object obj = command.ExecuteScalar();

                    if (obj != DBNull.Value && int.TryParse(obj.ToString(), out int resID))
                    {
                        res = resID;
                    }
                }

            }
            catch (Exception ex)
            {
                res = -1;
            }

            return res;

        }

        public static bool UpdateTransactionInDB(int TransactionsID, decimal Amount, byte TransactionStatus
                , byte TranscationType, DateTime PaymentDate, int ContractID)
        {
            int rowsAffected = -1;

            string query = @"update Transactions set Amount = @Amount, TransactionStatus = @TransactionStatus 
                , TranscationType = @TranscationType, PaymentDate = @PaymentDate , ContractID = @ContractID
                    where TransactionsID = @TransactionsID ";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@TransactionsID", SqlDbType.Int).Value = TransactionsID;
                    command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
                    command.Parameters.Add("@TransactionStatus", SqlDbType.TinyInt).Value = TransactionStatus;
                    command.Parameters.Add("@TranscationType", SqlDbType.TinyInt).Value = TranscationType;
                    command.Parameters.Add("@PaymentDate", SqlDbType.DateTime2).Value = PaymentDate;
                    command.Parameters.Add("@ContractID", SqlDbType.Int).Value = ContractID;

                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();


                }
            }
            catch (Exception ex)
            {
                rowsAffected = -1;
            }
            return (rowsAffected > 0);
        }


        public static bool DeleteMultiTransactions(List<int> ids)
        {
            int rowsAffected = -1;

            if (ids == null || ids.Count == 0) return false;

            var parmeters = ids.Select((id, index) => "@id" + index).ToArray();

            string query = $@"delete from Transactions where TransactionsID in ({string.Join(",", parmeters)})";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    for (int i = 0; i < cmd.Parameters.Count; i++)
                    {
                        cmd.Parameters.Add(parmeters[i], SqlDbType.Int).Value = ids[i];
                    }

                    conn.Open();

                    rowsAffected = cmd.ExecuteNonQuery();


                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return (rowsAffected > 0);


        }
        public static bool DeleteSingleTransactions(int TransactionsID)
        {

            int rowsAffected = -1;

            string query = @"delete from Transactions where TransactionsID = @TransactionsID";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {


                    cmd.Parameters.Add("@TransactionsID", SqlDbType.Int).Value = TransactionsID;


                    conn.Open();

                    rowsAffected = cmd.ExecuteNonQuery();


                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return (rowsAffected > 0);


        }

        public static DataTable getTransactionWithPaymentDateFromDB(DateTime PaymentDate)
        {
            DataTable dt = new DataTable();

            string query = @"select * from Transactions where PaymentDate = @PaymentDate";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@PaymentDate", SqlDbType.DateTime2).Value = PaymentDate;

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
            catch (Exception ex) { dt = null; }


            return dt;
        }

        public static DataTable getTransactionsWithTransactionStatusFromDB(byte TransactionStatus)
        {
            DataTable dt = new DataTable();

            string query = @"select * from Transactions where TransactionStatus = @TransactionStatus";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@TransactionStatus", SqlDbType.TinyInt).Value = TransactionStatus;

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
            catch (Exception ex) { dt = null; }


            return dt;
        }

        public static DataTable getTransactionsWithTranscationTypeFromDB(byte TranscationType) {

            DataTable dt = new DataTable();

            string query = @"select * from Transactions where TranscationType = @TranscationType";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@TranscationType", SqlDbType.TinyInt).Value = TranscationType;

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
            catch (Exception ex) { dt = null; }


            return dt;


        }

        public static DataTable getTransactionsWithAmountFromDB(decimal Amount) {

            DataTable dt = new DataTable();

            string query = @"select * from Transactions where Amount = @Amount";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;

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
            catch (Exception ex) { dt = null; }


            return dt;



        }

        public static DataTable getTransactionsWithContractIDFromDB(int ContractID)
        {
            DataTable dt = new DataTable();

            string query = @"select * from Transactions where ContractID = @ContractID";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@ContractID", SqlDbType.Int).Value = ContractID;

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
            catch (Exception ex) { dt = null; }


            return dt;

        }


        public static DataTable getTransactionsWithPaymentDateAndAmountFromDB(DateTime PaymentDate, decimal Amount)
        {

            DataTable dt = new DataTable();

            string query = @"select * from Transactions where PaymentDate = @PaymentDate and Amount = @Amount";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@PaymentDate", SqlDbType.DateTime2).Value = PaymentDate;
                    cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;

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
            catch (Exception ex) { dt = null; }


            return dt;

        }

        public static DataTable getTransactionsStatusAndTypeFromDB(byte TransactionStatus, byte TranscationType)
        {

            DataTable dt = new DataTable();

            string query = @"select * from Transactions where TransactionStatus = @TransactionStatus and TranscationType = @TranscationType";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@TransactionStatus", SqlDbType.TinyInt).Value = TransactionStatus;
                    cmd.Parameters.Add("@TranscationType", SqlDbType.TinyInt).Value = TranscationType;

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
            catch (Exception ex) { dt = null; }


            return dt;

        }
    }
}
