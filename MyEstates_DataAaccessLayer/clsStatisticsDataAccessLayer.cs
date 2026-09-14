using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_DataAaccessLayer
{

    /*
         Unlike SqlDataReader, which normally keeps the database connection open while reading,
        SqlDataAdapter is commonly used in a disconnected architecture.
     */

    public static class clsStatisticsDataAccessLayer
    {

        public static long GetEstatesCount()
        {
            long count = 0;

            string query = @"select count(EstateID) as EstatesCount from Estates";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {


                    connection.Open();  
                        
                    object res = command.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        count = Convert.ToInt64(res);
                    }

                }

            }
            catch (Exception ex) { 
                count = 0;
                Console.WriteLine(ex.Message);  
            }

            return count;

        }

        public static long GetRentedEstatesCount()
        {
            long count = 0;

            string query = @"select count(EstateID) as RentedEstatesCount from Estates where SellingEstate = 2";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    connection.Open();

                    object res = cmd.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        count = Convert.ToInt64(res);
                    }


                }

            }
            catch (Exception ex) { count = 0; Console.WriteLine(ex.Message); }


            return count;

        }

        public static long GetSoldEstatesCount()
        {
            long count = 0;

            string query = @"select count(EstateID) as SoldEstatesCount from Estates where SellingEstate = 1";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    connection.Open();

                    object res = cmd.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        count = Convert.ToInt64(res);
                    }


                }

            }
            catch (Exception ex) { count = 0; Console.WriteLine(ex.Message); }


            return count;

        }

        public static long GetAvailableEstatesCount()
        {


            long count = 0;

            string query = @"select count(EstateID) as AvailableEstatesCount from Estates where SellingEstate = 3";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    connection.Open();

                    object res = cmd.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        count = Convert.ToInt64(res);
                    }


                }

            }
            catch (Exception ex) { count = 0; Console.WriteLine(ex.Message); }


            return count;
        }

        public static decimal GetAllEstatesPriceSum()
        {

            decimal sum = 0;

            string query = "select sum(Price) AllEstatesPrices from Estates";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    connection.Open();

                    object res = cmd.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        sum = Convert.ToDecimal(res);
                    }


                }

            }
            catch (Exception ex) { sum = 0; Console.WriteLine(ex.Message); }


            return sum;

        }


        public static long GetNumOfEstatesInBuildingById(int BuildingID)
        {

            long total = 0;

            string query = @"select count(*) as NumOfEstatesInBuilding from Estates e inner join Building b
                            on e.BuildingID = b.BuildingID 
                            where b.BuildingID = @BuildingID";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.Add("@BuildingID",SqlDbType.Int).Value = BuildingID;

                    connection.Open();

                    object res = cmd.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        total = Convert.ToInt64(res);
                    }


                }

            }
            catch (Exception ex) { total = 0; Console.WriteLine(ex.Message); }


            return total;


        }
        public static long GetNumOfEstatesInBuildingByNumber(string BuildingNumber)
        {

            long total = 0;

            string query = @"select count(*) as NumOfEstatesInBuilding from Estates e inner join Building b
                            on e.BuildingID = b.BuildingID 
                            where b.BuildingNumber = @BuildingNumber";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@BuildingNumber", SqlDbType.NVarChar).Value = BuildingNumber;

                    connection.Open();

                    object res = cmd.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        total = Convert.ToInt64(res);
                    }


                }

            }
            catch (Exception ex) { total = 0; Console.WriteLine(ex.Message); }


            return total;


        }

        public static long GetNumberOfEstatesWithFeatureById(int EstateFeatureID)
        {

            long total = 0;

            string query = @"
                            select count(*) NumOfEstatesWithFeature from Estates e inner join EstatesFeatures ef
                            on e.FeaturesID = ef.EstateFeatureID
                            where ef.EstateFeatureID = @EstateFeatureID";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@EstateFeatureID", SqlDbType.Int).Value = EstateFeatureID;

                    connection.Open();

                    object res = cmd.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        total = Convert.ToInt64(res);
                    }


                }

            }
            catch (Exception ex) { total = 0; Console.WriteLine(ex.Message); }


            return total;


        }


        public static long GetNumberOfEstatesBySpace(decimal EstateSpace)
        {

            long total = 0;

            string query = @"
                            select count(EstateFeatureID) NumOfEstatesWithSpace from Estates e inner join EstatesFeatures ef
                            on e.FeaturesID = ef.EstateFeatureID
                            where ef.EstateFeatureID = 1
                            group by ef.EstateSpace
                            having EstateSpace = @EstateSpace";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@EstateSpace", SqlDbType.Decimal).Value = EstateSpace;

                    connection.Open();

                    object res = cmd.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        total = Convert.ToInt64(res);
                    }


                }

            }
            catch (Exception ex) { total = 0; Console.WriteLine(ex.Message); }


            return total;

        }



        public static long countOfBuildingsWithFloorAmount(int floorAmount)
        {

            long total = 0;

            string query = @"select count(*) as NumOfBuildings from BuildingFeatures where NumberOfFloors = @floorAmount";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@floorAmount", SqlDbType.Int).Value = floorAmount;

                    connection.Open();

                    object res = cmd.ExecuteScalar();
                    if(res != null && res != DBNull.Value)
                    {
                           total = Convert.ToInt64(res);   
                    }


                }

            }
            catch(Exception ex) { 
                total = -1;
                Console.WriteLine(ex.Message);
            }

            return total;
        }

        public static long countOFBuildingsWithBlocksAmount(int BlocksAmount)
        {

            long total = 0;

            string query = @"select count(*) as NumOfBuildings from BuildingFeatures where NumberOfBlocks = @BlocksAmount";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@BlocksAmount", SqlDbType.Int).Value = BlocksAmount;

                    connection.Open();

                    object res = cmd.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        total = Convert.ToInt64(res);
                    }


                }

            }
            catch (Exception ex)
            {
                total = -1;
                Console.WriteLine(ex.Message);
            }

            return total;
        }


        public static long countOfBuidlingsWithAlivator()
        {

            long total = 0;

            string query = @"select count(*) as NumOfBuildings from BuildingFeatures where HasAlivator = @hasAlivator";

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@hasAlivator", SqlDbType.Bit).Value = true;

                    connection.Open();

                    object res = cmd.ExecuteScalar();   

                    if(res != null && res != DBNull.Value)
                    {
                        total = Convert.ToInt64(res);
                    }


                }

            }
            catch (Exception ex) { 
                total = -1;
                Console.WriteLine(ex.Message);
            }

            return total ;   

        }


        public static DataTable getAllEstateRelatedToContractFromDB(int ContractID)
        {
            DataTable db = new DataTable();
            string query = @"select * from vw_ContractInfoAboutRelatedEstates where ContractID = @ContractID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessLayerSetting.ConnectionString))
                using(SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@ContractID",SqlDbType.Int).Value = ContractID;  

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {

                        if (reader.HasRows) { 
                            db.Load(reader);
                        }
                    
                    }
                    

                }

            }
            catch (Exception ex) {
                db = null;
            }

            return db;
        }

    }
}
