using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
    public  class clsStatistics
    {




        //   getting the values from the DataBase directly is faster in this case ,
        //   Less RAM +
        //   Less comm between layers +
        //   DB engine is better for aggregation 


        //  public enum enSelling_Estate { sold=1, rented=2, available=3 };

        /*
            create a class statistics that has the methods for 

            it will be a seperated front-end Windows Form Called "Statistics"

                Aggregation fucntions : 
                    1_ show the count of Estates you have
                    2_ show the count of rented / sold / idle Estates                    
                    3_ show the sum of all estate prices : rented , sold , idle , all
                    4_ show count of estates in specific building 
                    5_ count of Estate with specific feature
                    6_ getCountOfBuilddingsWithNumOFfloors = @amount 
                    7_ getCountOfBuilddingsWithNumOFBlocks = @amount
                    8_ getCountOfBuildingsWithAlivator
                

        // not done yet  
                9_ view to collect all the information about whom involved in contract from Estates+Buildings+customers
                    // select * from vw_ContractInfoAboutRelatedEstates "view name in Sql Server"

            do this for all Entities in your DataBase that will make the user Experience Better

            
      
         */

        public static long GetCountOfEstates()
            => clsStatisticsDataAccessLayer.GetEstatesCount();

        public static long GetCountOfRentedEstates()
        {
            return clsStatisticsDataAccessLayer.GetRentedEstatesCount(); 
        }

        public static long GetCountOfSoldEstates()
            => clsStatisticsDataAccessLayer.GetSoldEstatesCount();

        public static long GetCountOfAvailableEstates()
            => clsStatisticsDataAccessLayer.GetAvailableEstatesCount();

        public static decimal GetPricesSumForAllEstates()
            => clsStatisticsDataAccessLayer.GetAllEstatesPriceSum();


        // From Building Table

        public static long GetCountOfEstatesInBuildingById(int BuildingID)
            => clsStatisticsDataAccessLayer.GetNumOfEstatesInBuildingById(BuildingID);

        public static long GetCountOfEstatesInBuildingByNumber(string BuildingNumber)
            => clsStatisticsDataAccessLayer.GetNumOfEstatesInBuildingByNumber(BuildingNumber);


        // from EstateFeature Table

        public static long GetCountOfEstatesWithFeatureById(int FeatureID)
            => clsStatisticsDataAccessLayer.GetNumberOfEstatesWithFeatureById(FeatureID);

        public static long GetCountOfEstatesBySpace(decimal EstateSpace)
            => clsStatisticsDataAccessLayer.GetNumberOfEstatesBySpace(EstateSpace);


        public static long getCountOfBuilddingsWithNumOFfloors(int amount)
        {
            return clsStatisticsDataAccessLayer.countOfBuildingsWithFloorAmount(amount);
        }

        public static long getCountOfBuilddingsWithNumOFBlocks(int amount)
        {
            return clsStatisticsDataAccessLayer.countOFBuildingsWithBlocksAmount(amount);
        }

        public static long getCountOfBuildingsWithAlivator()
        {
            return clsStatisticsDataAccessLayer.countOfBuidlingsWithAlivator();
        }


        public static DataTable getAllEstateRelatedToContract(int ContractID)
        {
            return clsStatisticsDataAccessLayer.getAllEstateRelatedToContractFromDB(ContractID);
        }

    }
}
