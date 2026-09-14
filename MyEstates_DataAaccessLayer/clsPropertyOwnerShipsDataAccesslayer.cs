using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{
    public static class clsPropertyOwnerShipsDataAccesslayer
    {

        public static bool FindOwnerShipWithID(ref int PropertyOwnerShipID, ref decimal OwnerShipPercentage, ref int EstateID, ref int OwnerID)
        {

            bool res = false;

            string query = @"select PropertyOwnerShipID,OwnerShipPercentage
                                ,EstateID,OwnerID from PropertyOwnerShip where PropertyOwnerShipID = @PropertyOwnerShipID";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@PropertyOwnerShipID", SqlDbType.Int).Value = PropertyOwnerShipID;
                    command.Parameters.Add("@OwnerShipPercentage", SqlDbType.Decimal).Value = OwnerShipPercentage;
                    command.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID;
                    command.Parameters.Add("@OwnerID", SqlDbType.Int).Value = OwnerID;

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        res = true;

                        PropertyOwnerShipID = reader["PropertyOwnerShipID"] != DBNull.Value ? (int)reader["PropertyOwnerShipID"] : -1;
                        OwnerShipPercentage = reader["OwnerShipPercentage"] != DBNull.Value ? (decimal)reader["OwnerShipPercentage"] : -1;
                        EstateID = reader["EstateID"] != DBNull.Value ? (int)reader["EstateID"] : -1;
                        OwnerID = reader["OwnerID"] != DBNull.Value ? (int)reader["OwnerID"] : -1;
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


        public static int AddNewOwnerShip(decimal OwnerShipPercentage, int EstateID, int OwnerID)
        {
            int id = -1;

            string query = @"insert into PropertyOwnerShip (OwnerShipPercentage,EstateID,OwnerID) values
                                    (@OwnerShipPercentage,@EstateID,@OwnerID);
                                    SELECT SCOPE_IDENTITY();";


            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@OwnerShipPercentage", SqlDbType.Decimal).Value = OwnerShipPercentage;
                    cmd.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID;
                    cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = OwnerID;

                    connection.Open();

                    object res = cmd.ExecuteScalar();

                    if (res != DBNull.Value && int.TryParse(res.ToString(), out int resID))
                    {
                        id = resID;
                    }

                }

            }
            catch (Exception ex)
            {

                id = -1;
                Console.WriteLine(ex.Message);

            }


            return id;

        }

        public static bool UpdateOwnerShip(int PropertyOwnerShipID, decimal OwnerShipPercentage, int EstateID, int OwnerID)
        {

            int rowsAffected = 0;

            string query = @"
                               Update PropertyOwnerShip Set 
                                 OwnerShipPercentage = @OwnerShipPercentage,
                                 EstateID= @EstateID,
                                 OwnerID= @OwnerID 
                                 where PropertyOwnerShipID = @PropertyOwnerShipID";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@PropertyOwnerShipID", SqlDbType.Int).Value = PropertyOwnerShipID;
                    cmd.Parameters.Add("@OwnerShipPercentage", SqlDbType.Decimal).Value = OwnerShipPercentage;
                    cmd.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID;
                    cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = OwnerID;


                    connection.Open();

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


        public static bool DeleteMultiOwnerShipsFromDB(List<int> ids)
        {
            if (ids == null || ids.Count == 0) return false;

            int rowsAffected = 0;

            var parameters = ids.Select((id, index) => "@id" + index).ToArray();

            string query = $@"delete from PropertyOwnerShip where PropertyOwnerShipID in ({string.Join(",", parameters)})";


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
                Console.WriteLine(ex.Message);

            }

            return (rowsAffected > 0);
        }


        public static bool DeleteSingleOwnerShipFromDB(int PropertyOwnerShipID)
        {
            int rowsAffected = 0;


            string query = @"delete from PropertyOwnerShip where PropertyOwnerShipID = @PropertyOwnerShipID";


            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@PropertyOwnerShipID", SqlDbType.Int).Value = PropertyOwnerShipID;

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


        public static DataTable getOwnersWithPercentage(decimal OwnerShipPercentage)
        {

            DataTable dt = new DataTable();

            string query = @"SELECT O.* ,p.FirstName,p.LastName,p.FatherName,p.MotherName,p.CivilRegistry,p.Kaidinfo,POS.EstateID 
                                FROM owner O
                                inner join PropertyOwnerShip POS
                                    ON O.OwnerID = POS.OwnerID
                                inner join Person p on O.PersonID = p.PersonID
                                WHERE POS.OwnerShipPercentage = @OwnerShipPercentage";


            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@OwnerShipPercentage",SqlDbType.Decimal).Value = OwnerShipPercentage;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    }

                }

            }catch(Exception ex)
            {
                dt = null;
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        
        public static DataTable getOwnersWithPercentageBiggerThan(decimal OwnerShipPercentage)
        {
            DataTable dt = new DataTable();

            string query = @"select O.*  ,p.FirstName,p.LastName,p.FatherName,p.MotherName,p.CivilRegistry,p.Kaidinfo,
                            POS.EstateID from owner O inner join PropertyOwnerShip POS on O.OwnerID = POS.OwnerID 
                            inner join Person p on O.PersonID = p.PersonID
                            where OwnerShipPercentage > @OwnerShipPercentage";

            try
            {

                using(SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@OwnerShipPercentage",SqlDbType.Int).Value = OwnerShipPercentage;


                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            dt.Load(reader);

                        }
                    }
                }


            }catch(Exception ex)
            {
                dt = null;
                Console.WriteLine(ex.Message);
            }


            return dt;
        }

        public static DataTable getOwnersWithPercentageLessThan(decimal OwnerShipPercentage) {

            DataTable dt = new DataTable();

            string query = @"select O.*  ,p.FirstName,p.LastName,p.FatherName,p.MotherName,p.CivilRegistry,p.Kaidinfo,
                            POS.EstateID from owner O inner join PropertyOwnerShip POS on O.OwnerID = POS.OwnerID 
                            inner join Person p on O.PersonID = p.PersonID
                            where OwnerShipPercentage < @OwnerShipPercentage";

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@OwnerShipPercentage", SqlDbType.Int).Value = OwnerShipPercentage;


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


        public static DataTable getAllOwnersOfEstateWith(int EstateID)
        {

            DataTable dt = new DataTable();

            string query = @"  select O.*  ,p.FirstName,p.LastName,p.FatherName,p.MotherName,p.CivilRegistry,p.Kaidinfo,
                            POS.EstateID from owner O inner join PropertyOwnerShip POS on O.OwnerID = POS.OwnerID 
                            inner join Person p on O.PersonID = p.PersonID
                            where EstateID = @EstateID";
            

            try
            {

                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@EstateID", SqlDbType.Int).Value = EstateID;

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
            catch (Exception ex) {
                dt = null;
                Console.WriteLine(ex.Message);  
            }

            return dt;

        }

        public static DataTable getAllEstatesOfOwnerWith(int OwnerID) { 

            DataTable dt = new DataTable();

            string query = @"
                            select E.*,POS.OwnerShipPercentage from Estates E inner join PropertyOwnerShip POS 
                            on E.EstateID = POS.EstateID
                            where OwnerID = @OwnerID";

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = OwnerID;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) { 
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

    }
}
