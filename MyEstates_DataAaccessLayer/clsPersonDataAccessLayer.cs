using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    static public class clsPersonDataAccessLayer
    {
        static public int AddnewAddress(string City, string Region, string Street)
        {

            int AddressID = -1;
            string query = @"
                          INSERT INTO Addresses
                         (City ,Region ,Street )
                          VALUES(@City,@Region,@Street);
                          SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
                    command.Parameters.Add("@Region", SqlDbType.NVarChar).Value = Region;
                    command.Parameters.Add("@Street", SqlDbType.NVarChar).Value = Street;

                    conn.Open();

                    object res = command.ExecuteScalar();

                    if (res != null && int.TryParse(res.ToString(), out int resID))
                    {
                        AddressID = resID;
                    }
                }



            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }


            return AddressID;

        }
        static public int AddnewPerson
                 (string FirstName, string LastName,
                string Phone, string MotherName, string FatherName,
                long NationalNumber, DateTime BirthDate,
                string BirthPlace, string CivilRegistry, string Kaidinfo
               , int AddressID)
        {

            int PersonID = -1;

            string query = @"
                          INSERT INTO Person
                         (FirstName ,LastName ,Phone ,FatherName ,MotherName
                         ,NationalNumber ,BirthDate ,BirthPlace ,CivilRegistry ,Kaidinfo ,AddressID)
                          VALUES(@FirstName,@LastName,@Phone ,@FatherName,@MotherName,
                          @NationalNumber, @BirthDate ,@BirthPlace, @CivilRegistry, @Kaidinfo, @AddressID);
                          SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = FirstName;
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = LastName;
                    command.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = Phone;
                    command.Parameters.Add("@FatherName", SqlDbType.NVarChar).Value = FatherName;
                    command.Parameters.Add("@MotherName", SqlDbType.NVarChar).Value = MotherName;
                    command.Parameters.Add("@NationalNumber", SqlDbType.BigInt).Value = NationalNumber;
                    command.Parameters.Add("@BirthDate", SqlDbType.Date).Value = BirthDate;
                    command.Parameters.Add("@BirthPlace", SqlDbType.NVarChar).Value = BirthPlace;
                    command.Parameters.Add("@CivilRegistry", SqlDbType.NVarChar).Value = CivilRegistry;
                    command.Parameters.Add("@Kaidinfo", SqlDbType.NVarChar).Value = Kaidinfo;
                    command.Parameters.Add("@AddressID", SqlDbType.Int).Value = AddressID;



                    connection.Open();


                    object res = command.ExecuteScalar();

                    if (res != null && int.TryParse(res.ToString(), out int resultID))
                    {
                        PersonID = resultID;
                    }


                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }


            return PersonID;
        }


        static public bool UpdateAddressData(int AddressID, string City, string Region, string Street)
        {
            int rowAffected = 0;

            string query = @"Update Addresses Set City = @City , Region = @Region ,Street = @Street Where AddressID = @AddressID";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@AddressID", SqlDbType.Int).Value = AddressID;
                    command.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
                    command.Parameters.Add("@Region", SqlDbType.NVarChar).Value = Region;
                    command.Parameters.Add("@Street", SqlDbType.NVarChar).Value = Street;

                    connection.Open();

                    rowAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { rowAffected = 0; Console.WriteLine(ex.Message); }

            return (rowAffected > 0);
        }


        public static bool UpdatePersonData(int PersonID, string FirstName, string LastName,
                string Phone, string FatherName, string MotherName,
                long NationalNumber, DateTime BirthDate, string BirthPlace,
                string CivilRegistry, string KaidInfo,int AddressID)
        {
            int rowsAffected = 0;
            string query = @"Update Person Set FirstName = @FirstName , LastName = @LastName ,Phone =@Phone,
                            FatherName = @FatherName ,MotherName = @MotherName, NationalNumber = @NationalNumber,
                            BirthDate = @BirthDate  ,BirthPlace = @BirthPlace ,CivilRegistry = @CivilRegistry 
                            , KaidInfo = @KaidInfo , AddressID = @AddressID Where PersonID = @PersonID ";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = FirstName;
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = LastName;
                    command.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = Phone;
                    command.Parameters.Add("@FatherName", SqlDbType.NVarChar).Value = FatherName;
                    command.Parameters.Add("@MotherName", SqlDbType.NVarChar).Value = MotherName;
                    command.Parameters.Add("@NationalNumber", SqlDbType.BigInt).Value = NationalNumber;
                    command.Parameters.Add("@BirthDate", SqlDbType.Date).Value = BirthDate;
                    command.Parameters.Add("@BirthPlace", SqlDbType.NVarChar).Value = BirthPlace;
                    command.Parameters.Add("@CivilRegistry", SqlDbType.NVarChar).Value = CivilRegistry;
                    command.Parameters.Add("@KaidInfo", SqlDbType.NVarChar).Value = KaidInfo;
                    command.Parameters.Add("@AddressID", SqlDbType.Int).Value = AddressID;

                    connection.Open();


                    rowsAffected = command.ExecuteNonQuery();


                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                rowsAffected = -1;

            }
            return rowsAffected > 0;
        }


        static public bool DeletePersonsFromDataBase(List<int>PersonsIDS)
        {

            bool res = false;

            if (PersonsIDS == null || PersonsIDS.Count == 0) return false;

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.DeletePersonsAndRelatedUsers", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable tvp = new DataTable();

                    tvp.Columns.Add("ID",typeof(int));

                    foreach(int id in PersonsIDS)
                    {
                        tvp.Rows.Add(id);
                    }


                    SqlParameter p = cmd.Parameters.Add("@PersonsIds", SqlDbType.Structured);
                    p.TypeName = "dbo.IntIdList";
                    p.Value = tvp;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    res = true;
                }
            }
            catch (Exception ex) { 
            
                Console.WriteLine (ex.Message);
                return false;
            }


            return res;
            
        }



        public static bool DoesPersonExsist(int PersonID)
        {
            if (PersonID <= 0) return false;

            string query = "select COUNT(1) from Person where PersonID = @PersonID";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

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
    }
}
