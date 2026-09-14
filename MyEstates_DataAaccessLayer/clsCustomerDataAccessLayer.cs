using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{

    static public class clsCustomerDataAccessLayer
    {
        
        static public DataTable getAllAboutCustomers()
        {
            string query = "select * from vw_AllAboutCustomers";

            DataTable AllCustomersTable = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) { AllCustomersTable.Load(reader); }
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return AllCustomersTable;
        }

        static public short FindCustomerByID(
                ref int CustomerID, ref int PersonID, ref int AddressID, ref string FirstName, ref string LastName,
                ref string Phone, ref string MotherName, ref string FatherName, ref long NationalNumber,
                ref byte CustomerType, ref short Preferences, ref bool Loyality, ref decimal OfferedMouny,
                ref DateTime BirthDate, ref string BirthPlace, ref string CivilRegistry, ref string Kaidinfo,
                ref string City, ref string Region, ref string Street
            )
        {
            short res = -1;

            string query = "select * from vw_AllDataAboutCustomersView where CustomerID = @CustomerID";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;

                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();



                    if (reader.Read())
                    {

                        res = 1;

                        CustomerID = reader["CustomerID"] != DBNull.Value ? (int)reader["CustomerID"] : 0;
                        PersonID = reader["PersonID"] != DBNull.Value ? (int)reader["PersonID"] : 0;
                        AddressID = reader["AddressID"] != DBNull.Value ? (int)reader["AddressID"] : 0;

                        FirstName = reader["FirstName"]?.ToString() ?? "";
                        LastName = reader["LastName"]?.ToString() ?? "";
                        Phone = reader["Phone"]?.ToString() ?? "";
                        FatherName = reader["FatherName"]?.ToString() ?? "";
                        MotherName = reader["MotherName"]?.ToString() ?? "";
                        BirthPlace = reader["BirthPlace"]?.ToString() ?? "";
                        CivilRegistry = reader["CivilRegistry"]?.ToString() ?? "";
                        Kaidinfo = reader["Kaidinfo"]?.ToString() ?? "";
                        City = reader["City"]?.ToString() ?? "";
                        Region = reader["Region"]?.ToString() ?? "";
                        Street = reader["Street"]?.ToString() ?? "";

                        NationalNumber = reader["NationalNumber"] != DBNull.Value ? (long)reader["NationalNumber"] : 0;
                        CustomerType = reader["CustomerType"] != DBNull.Value ? (byte)reader["CustomerType"] : (byte)0;
                        Preferences = reader["Preferences"] != DBNull.Value ? Convert.ToInt16(reader["Preferences"]) : Convert.ToInt16(0);
                        Loyality = reader["Loyality"] != DBNull.Value ? (bool)reader["Loyality"] : false;

                        OfferedMouny = reader["OfferedMouny"] != DBNull.Value ? (decimal)reader["OfferedMouny"] : 0m;
                        BirthDate = reader["BirthDate"] != DBNull.Value ? (DateTime)reader["BirthDate"] : DateTime.Now;


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


        static public short FindByName(ref int CustomerID, ref int PersonID, ref string FirstName, ref string LastName,
                ref string Phone, ref string MotherName, ref string FatherName, ref long NationalNumber,
                ref byte CustomerType, ref short Preferences, ref bool Loyality, ref decimal OfferedMouny,
                ref DateTime BirthDate, ref string BirthPlace, ref string CivilRegistry, ref string Kaidinfo,
                ref string City, ref string Region, ref string Street)
        {
            short res = -1;
            string query = @"select * from vw_AllAboutCustomers where 
                                FirstName = @FirstName and 
                                LastName = @LastName
                                and MotherName = @MotherName";
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = FirstName;
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = LastName;
                    command.Parameters.Add("@MotherName", SqlDbType.NVarChar).Value = MotherName;


                    if (reader.Read())
                    {

                        res = 1;

                        CustomerID = reader["CustomerID"] != DBNull.Value ? (int)reader["CustomerID"] : 0;
                        PersonID = reader["PersonID"] != DBNull.Value ? (int)reader["PersonID"] : 0;

                        FirstName = reader["FirstName"]?.ToString() ?? "";
                        LastName = reader["LastName"]?.ToString() ?? "";
                        Phone = reader["Phone"]?.ToString() ?? "";
                        FatherName = reader["FatherName"]?.ToString() ?? "";
                        MotherName = reader["MotherName"]?.ToString() ?? "";
                        BirthPlace = reader["BirthPlace"]?.ToString() ?? "";
                        CivilRegistry = reader["CivilRegistry"]?.ToString() ?? "";
                        Kaidinfo = reader["Kaidinfo"]?.ToString() ?? "";
                        City = reader["City"]?.ToString() ?? "";
                        Region = reader["Region"]?.ToString() ?? "";
                        Street = reader["Street"]?.ToString() ?? "";

                        NationalNumber = reader["NationalNumber"] != DBNull.Value ? (long)reader["NationalNumber"] : 0;
                        CustomerType = reader["CustomerType"] != DBNull.Value ? (byte)reader["CustomerType"] : (byte)0;
                        Preferences = reader["Preferences"] != DBNull.Value ? Convert.ToInt16(reader["Preferences"]) : Convert.ToInt16(0);
                        Loyality = reader["Loyality"] != DBNull.Value ? (bool)reader["Loyality"] : false;

                        OfferedMouny = reader["OfferedMouny"] != DBNull.Value ? (decimal)reader["OfferedMouny"] : 0m;
                        BirthDate = reader["BirthDate"] != DBNull.Value ? (DateTime)reader["BirthDate"] : DateTime.Now;


                    }

                }
            }
            catch
            {
                res = -1;
            }

            return res;

        }


        static public short FindByNationalNumber(
            ref int CustomerID, ref int PersonID, ref string FirstName, ref string LastName,
            ref string Phone, ref string MotherName, ref string FatherName, ref long NationalNumber,
            ref byte CustomerType, ref short Preferences, ref bool Loyality, ref decimal OfferedMouny,
            ref DateTime BirthDate, ref string BirthPlace, ref string CivilRegistry, ref string Kaidinfo,
            ref string City, ref string Region, ref string Street
        )
        {
            short res = -1;

            string query = "select * from vw_AllAboutCustomers where NationalNumber = @NationalNumber";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    command.Parameters.Add("@NationalNumber", SqlDbType.Int).Value = NationalNumber;


                    if (reader.Read())
                    {

                        res = 1;

                        NationalNumber = reader["NationalNumber"] != DBNull.Value ? (long)reader["NationalNumber"] : 0;
                        CustomerID = reader["CustomerID"] != DBNull.Value ? (int)reader["CustomerID"] : 0;
                        PersonID = reader["PersonID"] != DBNull.Value ? (int)reader["PersonID"] : 0;

                        FirstName = reader["FirstName"]?.ToString() ?? "";
                        LastName = reader["LastName"]?.ToString() ?? "";
                        Phone = reader["Phone"]?.ToString() ?? "";
                        FatherName = reader["FatherName"]?.ToString() ?? "";
                        MotherName = reader["MotherName"]?.ToString() ?? "";
                        BirthPlace = reader["BirthPlace"]?.ToString() ?? "";
                        CivilRegistry = reader["CivilRegistry"]?.ToString() ?? "";
                        Kaidinfo = reader["Kaidinfo"]?.ToString() ?? "";
                        City = reader["City"]?.ToString() ?? "";
                        Region = reader["Region"]?.ToString() ?? "";
                        Street = reader["Street"]?.ToString() ?? "";

                        CustomerType = reader["CustomerType"] != DBNull.Value ? (byte)reader["CustomerType"] : (byte)0;
                        Preferences = reader["Preferences"] != DBNull.Value ? Convert.ToInt16(reader["Preferences"]) : Convert.ToInt16(0);
                        Loyality = reader["Loyality"] != DBNull.Value ? (bool)reader["Loyality"] : false;

                        OfferedMouny = reader["OfferedMouny"] != DBNull.Value ? (decimal)reader["OfferedMouny"] : 0m;
                        BirthDate = reader["BirthDate"] != DBNull.Value ? (DateTime)reader["BirthDate"] : DateTime.Now;


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



        static public DataTable getLoyalCustomers()
        {

            string query = "select * from vw_loyalCustomers";

            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) { dataTable.Load(reader); }
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return dataTable;

        }


        static public DataTable getUnLoyalCustomers()
        {

            string query = "select * from vw_UnloyalCustomers";

            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) { dataTable.Load(reader); }
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return dataTable;

        }


        static public DataTable getCustomersWithType(byte CustomerType)
        {
            string query = "select * form vw_AllAboutCustomers where CustomerType = @CustomerType";

            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    conn.Open();

                    command.Parameters.Add("@CustomerType", SqlDbType.TinyInt).Value = CustomerType;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }


                    }

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return dataTable;
        }


        static public DataTable getCutomersWithThisPreferences(short Preferences)
        {

            string query = "select * form vw_AllAboutCustomers where Preferences = @Preferences";

            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    conn.Open();

                    command.Parameters.Add("@Preferences", SqlDbType.SmallInt).Value = Preferences;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }


                    }

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return dataTable;


        }


        static public DataTable getCustomersAddresses()
        {
            DataTable dataTable = new DataTable();


            string query = "select * from vw_CutomersAddresses";

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
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return dataTable;

        }


        static public DataTable getCutomersWithOfferedMouny(decimal OfferedMouny, short range)
        {
            string query = "";

            switch (range)
            {
                case 0:
                     query = "select * form vw_AllAboutCustomers where OfferedMouny = @OfferedMouny";
                    break;
                case 1:
                    query = "select * form vw_AllAboutCustomers where OfferedMouny > @OfferedMouny";
                    break;
                case 2:
                    query = "select * form vw_AllAboutCustomers where OfferedMouny < @OfferedMouny";
                    break;
                case 3:
                    query = "select * form vw_AllAboutCustomers where OfferedMouny >= @OfferedMouny";
                    break ;
                case 4:
                    query = "select * form vw_AllAboutCustomers where OfferedMouny =< @OfferedMouny";
                    break;
            }
           

            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    conn.Open();

                    command.Parameters.Add("@OfferedMouny", SqlDbType.Decimal).Value = OfferedMouny;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }


                    }

                }
            }

            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return dataTable;
        }

        
        static public DataTable getCustomersWithKaidInfo(string Kaidinfo)
        {
            DataTable dataTable = new DataTable();


            string query = "select * from vw_AllAboutCustomers where Kaidinfo = @Kaidinfo";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@Kaidinfo", SqlDbType.NVarChar).Value = Kaidinfo;

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
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return dataTable;
        }
        static public DataTable getCustomersWithCivilRegistry(string CivilRegistry)
        {
            DataTable dataTable = new DataTable();


            string query = "select * from vw_AllAboutCustomers where CivilRegistry = @CivilRegistry";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@CivilRegistry", SqlDbType.NVarChar).Value = CivilRegistry;

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
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return dataTable;
        }


        static public int AddNewCustomerToTable( byte CustomerType, 
            short Preferences,  bool Loyality,  decimal OfferedMouny,int PersonID)        
        {


            int CustomerID = -1;
            string query = @"
                            INSERT INTO Customer(CustomerType , Preferences  , Loyality , OfferedMouny ,PersonID)
                            VALUES(@CustomerType,@Preferences,@Loyality,@OfferedMouny,@PersonID);
                            SELECT SCOPE_IDENTITY();";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) { 
                
                    command.Parameters.Add("@CustomerType",SqlDbType.TinyInt).Value = CustomerType;
                    command.Parameters.Add("@Preferences", SqlDbType.SmallInt).Value = Preferences;
                    command.Parameters.Add("@Loyality", SqlDbType.Bit).Value = Loyality;
                    command.Parameters.Add("@OfferedMouny", SqlDbType.Decimal).Value = OfferedMouny;
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

                    connection.Open();
                    

                    object res = command.ExecuteScalar();

                    if(res!=null && int.TryParse(res.ToString(),out int resID))
                    {
                        CustomerID = resID;
                    }


                
                }   


            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }


            return CustomerID ;
        }



        public static bool UpdateCustomerInfo(int CustomerID, byte CustomerType ,short Preferences,bool Loyality,decimal OfferedMouny
            ,int PersonID)
        {
            int rowsAffected = 0;

            string query = @"Update Customer Set CustomerType = @CustomerType , Preferences = @Preferences ,
                            Loyality  = @Loyality , OfferedMouny = @OfferedMouny , PersonID = @PersonID Where CustomerID = @CustomerID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@CustomerID",SqlDbType.Int).Value = CustomerID;
                    command.Parameters.Add("@CustomerType", SqlDbType.TinyInt).Value = CustomerType;
                    command.Parameters.Add("@Preferences", SqlDbType.SmallInt).Value = Preferences;
                    command.Parameters.Add("@Loyality", SqlDbType.Bit).Value = Loyality;
                    command.Parameters.Add("@OfferedMouny", SqlDbType.Decimal).Value = OfferedMouny;
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();


                }


            }
            catch (Exception ex) {
                rowsAffected = -1;
                Console.WriteLine (ex.Message);
            }

           return rowsAffected > 0;
        }
        




       public static bool DeleteMarkedCustomersFromDataBase(List<int> CustomerIDs)
        {

            bool res = false;

            if (CustomerIDs == null || CustomerIDs.Count == 0) return false;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.DeleteCustomers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable tvp = new DataTable();

                    tvp.Columns.Add("ID", typeof(int));

                    foreach (int id in CustomerIDs)
                    {
                        tvp.Rows.Add(id);
                    }

                    SqlParameter p = cmd.Parameters.Add("@CustomersIds", SqlDbType.Structured);
                    p.TypeName = "dbo.IntIdList";
                    p.Value = tvp;

                    // the explenation of the three lines above 
                    // , hi sql server  take the data from the c# object tvp and add it into your sql var named @CustomersIds


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


        public static bool DoesCustomerExsist(int CustomerID) {

            if (CustomerID <= 0) return false;

            string query = "select COUNT(1) from Customer where CustomerID = @CustomerID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;

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

        public static DataTable SearchCustomers(string searchText)
        {
            DataTable dataTable = new DataTable();

            string query = @"select * from vw_AllDataAboutCustomersView 
                                 WHERE FirstName LIKE @searchText
                        OR LastName LIKE @searchText
                        OR FatherName LIKE @searchText
                        OR MotherName LIKE @searchText
                        OR CAST(NationalNumber AS NVARCHAR) LIKE @searchText
                        OR CAST(Phone AS NVARCHAR) LIKE @searchText";
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");


                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) { 
                        
                            dataTable.Load(reader);
                        }
                            
                    }
                }
            }
            catch (Exception ex) { dataTable = null; }

            return dataTable;

        }
            




    }
}
