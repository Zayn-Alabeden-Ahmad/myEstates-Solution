using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    static public class clsOwnerDataAccessLayer
    {

        static public bool FindOwnerByID(ref int OwnerID, ref int PersonID,ref int AddressID, ref string FirstName, ref string LastName,
                ref string Phone, ref string MotherName, ref string FatherName, ref long NationalNumber, 
                ref short EstatesOwned, ref short BuidlingsOwned,
                ref DateTime BirthDate, ref string BirthPlace, ref string CivilRegistry, ref string KaidInfo, 
                ref string City, ref string Region, ref string Street)
        {

            bool res = false;


            string query = "Select * from vw_AllAboutOwners where OwnerID = @OwnerID";


            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    command.Parameters.Add("@OwnerID",SqlDbType.Int).Value = OwnerID;
                    conn.Open();


                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read()) {

                        res = true;

                        OwnerID = reader["OwnerID"] != DBNull.Value ? (int)reader["OwnerID"] : -1;
                        PersonID = reader["PersonID"] != DBNull.Value ? (int)reader["PersonID"] : -1;
                        AddressID = reader["AddressID"] != DBNull.Value ? (int)reader["AddressID"] : -1;
                        FirstName = reader["FirstName"]?.ToString() ?? ""; 
                        LastName = reader["LastName"]?.ToString() ?? "";
                        Phone = reader["Phone"]?.ToString() ?? "";
                        MotherName = reader["MotherName"]?.ToString() ?? "";
                        FatherName = reader["FatherName"]?.ToString() ?? "";
                        NationalNumber = reader["NationalNumber"] != DBNull.Value ? (long)reader["NationalNumber"] : 0;
                        EstatesOwned = reader["EstatesOwned"] != DBNull.Value ? Convert.ToInt16(reader["EstatesOwned"]) : Convert.ToInt16(0);
                        BuidlingsOwned = reader["BuidlingsOwned"] != DBNull.Value ? Convert.ToInt16(reader["BuidlingsOwned"]) : Convert.ToInt16(0);
                        BirthDate = reader["BirthDate"] != DBNull.Value ? Convert.ToDateTime(reader["BirthDate"]) : DateTime.Now;
                        BirthPlace = reader["BirthPlace"]?.ToString() ?? "";
                        CivilRegistry = reader["CivilRegistry"]?.ToString() ?? "";
                        KaidInfo = reader["KaidInfo"]?.ToString() ?? "";
                        City = reader["City"]?.ToString() ?? "";
                        Region = reader["Region"]?.ToString() ?? "";
                        Street = reader["Street"]?.ToString() ?? "";


                    }

                }

            }
            catch (Exception ex) { 
                res =false;
            Console.WriteLine(ex.Message);
            
            }

            return res;

        }
        static public bool FindByNationalNumber(ref int OwnerID, ref int PersonID, ref string FirstName, ref string LastName,
                     ref string Phone, ref string MotherName, ref string FatherName, ref long NationalNumber,
                     ref short EstatesOwned, ref short BuidlingsOwned,
                     ref DateTime BirthDate, ref string BirthPlace, ref string CivilRegistry, ref string KaidInfo,
                     ref string City, ref string Region, ref string Street)
        {

            bool res = false;


            string query = "Select * from vw_AllAboutOwners where NationalNumber = @NationalNumber";


            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    command.Parameters.Add("@NationalNumber", SqlDbType.Int).Value = NationalNumber;
                    conn.Open();


                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        res = true;

                        NationalNumber = reader["NationalNumber"] != DBNull.Value ? (long)reader["NationalNumber"] : 0;

                        OwnerID = reader["OwnerID"] != DBNull.Value ? (int)reader["OwnerID"] : 0;
                        PersonID = reader["PersonID"] != DBNull.Value ? (int)reader["PersonID"] : 0;
                        FirstName = reader["FirstName"]?.ToString() ?? "";
                        LastName = reader["LastName"]?.ToString() ?? "";
                        Phone = reader["Phone"]?.ToString() ?? "";
                        MotherName = reader["MotherName"]?.ToString() ?? "";
                        FatherName = reader["FatherName"]?.ToString() ?? "";
                        EstatesOwned = reader["EstatesOwned"] != DBNull.Value ? Convert.ToInt16(reader["EstatesOwned"]) : Convert.ToInt16(0);
                        BuidlingsOwned = reader["BuidlingsOwned"] != DBNull.Value ? Convert.ToInt16(reader["BuidlingsOwned"]) : Convert.ToInt16(0);
                        BirthDate = reader["BirthDate"] != DBNull.Value ? Convert.ToDateTime(reader["BirthDate"]) : DateTime.Now;
                        BirthPlace = reader["BirthPlace"]?.ToString() ?? "";
                        CivilRegistry = reader["CivilRegistry"]?.ToString() ?? "";
                        KaidInfo = reader["KaidInfo"]?.ToString() ?? "";
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




        static public int AddNewOwnerToTable(short EstatesOwned, short BuidlingsOwned, int PersonID)
        {
            int OwnerID = -1;

            string query = @"Insert into Owner (EstatesOwned,BuidlingsOwned,PersonID) 
                            values (@EstatesOwned,@BuidlingsOwned,@PersonID); 
                            SELECT SCOPE_IDENTITY();";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PersonID",SqlDbType.Int).Value = PersonID;
                    command.Parameters.Add("@EstatesOwned", SqlDbType.SmallInt).Value = EstatesOwned;
                    command.Parameters.Add("@BuidlingsOwned", SqlDbType.SmallInt).Value = BuidlingsOwned;
                    connection.Open();


                    object res = command.ExecuteScalar();

                    if (res != null && int.TryParse(res.ToString(), out int resID)) 
                    {
                        OwnerID = resID;
                    }

                }
            }
            catch (Exception ex) { 
                OwnerID = -1;
                Console.WriteLine(ex.Message);
            }

            return OwnerID;
        }

        static public bool UpdateOwnerInfo(int OwnerID,short EstatesOwned, short BuidlingsOwned)
        {
            int rowsAffected  = -1;

            string query = @"Update Owner Set EstatesOwned = @EstatesOwned , BuidlingsOwned = @BuidlingsOwned Where OwnerID = @OwnerID";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@OwnerID",SqlDbType.Int).Value = OwnerID;
                    command.Parameters.Add("@EstatesOwned", SqlDbType.SmallInt).Value = EstatesOwned;
                    command.Parameters.Add("@BuidlingsOwned", SqlDbType.SmallInt).Value = BuidlingsOwned;

                    connection.Open();


                    rowsAffected = command.ExecuteNonQuery();




                }


            }
            catch (Exception ex) {
                rowsAffected = -1;
                Console.WriteLine(ex.Message);
            }
            return (rowsAffected > 0);    
        }




        static public bool DeleteGroupOfOwners(List<int> OwnerIDs) {
            bool res = false;

            if (OwnerIDs == null || OwnerIDs.Count == 0) return false;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.DeleteOwners", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable tvp = new DataTable();

                    tvp.Columns.Add("ID", typeof(int));

                    foreach (int id in OwnerIDs)
                    {
                        tvp.Rows.Add(id);
                    }

                    SqlParameter p = cmd.Parameters.Add("@OwnersIds", SqlDbType.Structured);
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


        static public DataTable getAllOwnersFromDB()
        {
            DataTable AllOwners = new DataTable();

            string query = "Select * from vw_AllAboutOwners";

            try
            {

                using(SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) {

                        AllOwners.Load(reader);
                    
                    }

                
                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }   

            return AllOwners;

        }

        static public DataTable getAllOwnersAddresss()
        {
            DataTable AllOwnersAddresses = new DataTable();

            string query = "Select * from vw_OwnersAddresses";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {

                        AllOwnersAddresses.Load(reader);

                    }


                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return AllOwnersAddresses;
        }

        static public DataTable getAllOwnersWithEstatesOwned(int numOfEstates,short range)
        {
            DataTable AllOwnersAddresses = new DataTable();

            string query = "";

            // think about making this switch control global 

            switch (range)
            {
                case 0:
                    query = "Select * from vw_OwnersStatistics where EstatesOwned = @numOfEstates";
                    break;
                case 1:
                    query = "Select * from vw_OwnersStatistics where EstatesOwned > @numOfEstates";
                    break;
                case 2:
                    query = "Select * from vw_OwnersStatistics where EstatesOwned < @numOfEstates";
                    break;
                case 3:
                    query = "Select * from vw_OwnersStatistics where EstatesOwned >= @numOfEstates";
                    break;
                case 4:
                    query = "Select * from vw_OwnersStatistics where EstatesOwned =< @numOfEstates";
                    break;
            }

     

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@numOfEstates",SqlDbType.SmallInt).Value = numOfEstates;

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {

                        AllOwnersAddresses.Load(reader);

                    }


                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return AllOwnersAddresses;
        }

        static public DataTable getAllOwnersWithBuildingsOwned(int numOfBuildings ,short range)
        {
            DataTable AllOwnersAddresses = new DataTable();

            string query = "";

            // think about making this switch control global 

            switch (range)
            {
                case 0:
                    query = "Select * from vw_OwnersStatistics where BuidlingsOwned = @numOfBuildings";
                    break;
                case 1:
                    query = "Select * from vw_OwnersStatistics where BuidlingsOwned > @numOfBuildings";
                    break;
                case 2:
                    query = "Select * from vw_OwnersStatistics where BuidlingsOwned < @numOfBuildings";
                    break;
                case 3:
                    query = "Select * from vw_OwnersStatistics where BuidlingsOwned >= @numOfBuildings";
                    break;
                case 4:
                    query = "Select * from vw_OwnersStatistics where BuidlingsOwned =< @numOfBuildings";
                    break;
            }
            

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@numOfBuildings", SqlDbType.SmallInt).Value = numOfBuildings;

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {

                        AllOwnersAddresses.Load(reader);

                    }


                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return AllOwnersAddresses;
        }

        static public DataTable getOwnersWithKaidInfo(string kaidinfo)
        {
            DataTable dataTable = new DataTable();


            string query = "select * from vw_AllAboutOwners where kaidinfo = @kaidinfo";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@kaidinfo", SqlDbType.NVarChar).Value = kaidinfo;

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

        static public DataTable getOwnersWithCivilRegistry(string CivilRegistry)
        {
            DataTable dataTable = new DataTable();


            string query = "select * from vw_AllAboutOwners where CivilRegistry = @CivilRegistry";

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



        public static bool DoesOwnerExsist(int OwnerID)
        {

            if (OwnerID <= 0) return false;

            string query = "select COUNT(1) from Owner where OwnerID = @OwnerID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = OwnerID;

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

        public static DataTable SearchOwnersinDB(string searchText)
        {
            DataTable dt = new DataTable();

            string query = @"  SELECT *
                    FROM vw_AllAboutOwners
                    WHERE FirstName LIKE @searchText
                        OR LastName LIKE @searchText
                        OR FatherName LIKE @searchText
                        OR MotherName LIKE @searchText
                        OR CAST(NationalNumber AS NVARCHAR) LIKE @searchText
                        OR CAST(Phone AS NVARCHAR) LIKE @searchText";


            using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt;

        }
    }
}
