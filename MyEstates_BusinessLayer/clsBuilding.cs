using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
    public  class clsBuilding : SaveData
    {


        private int _BuildingID;
        private string _Name;
        private string _BuildingNumber;
        private clsAddress _BuidlingAddress;
        

        public int BuildingID { get { return _BuildingID; } set {_BuildingID = value; } }
        public string Name { get { return _Name; } set {_Name = value; } }
        public string BuildingNumber { get {return _BuildingNumber; } set {_BuildingNumber = value; } }

        public clsAddress BuidlingAddress { get { return _BuidlingAddress; } set { _BuidlingAddress = value; } }

        // ***************************************

        /* update occured i've edited the database tables => now BuildingFeatures Has BuidlingID as FK */
       
        // ***************************************

        // public clsBuildingFeatures BuildingFeatures { get { return _BuildingFeatures; } set {_BuildingFeatures = value; } }

        public enMode Mode = enMode.add;
        public enDeletetype deletetype = enDeletetype.deleteSingle;

        public clsBuilding()
        {
            BuildingID = -1;
            Name = string.Empty;
            BuildingNumber = string.Empty;
            BuidlingAddress = new clsAddress();
            Mode = enMode.add;
        }

   
        private clsBuilding(int buildingId, string name, string buildingNumber,
           clsAddress buildingAddress)
        {
            BuildingID = buildingId;
            Name = name;
            BuildingNumber = buildingNumber;
            BuidlingAddress = buildingAddress;
            Mode = enMode.update;
        }

        public static clsBuilding Find(int BuildingID)
        {

        // int BuildingFeatureId = -1;

         string Name = string.Empty;

         string BuildingNumber = string.Empty;

         clsAddress BAddress = new clsAddress();
         int AddressID = -1;
         string City = "", Region = "", Street = "";

        // clsBuildingFeatures BuildingFeatures = null;


         // no need i will ill get it because we are searching the db and returning the valuse and its in the table we are searching
         // BuildingFeatureId = clsBuildingDataAccessLayer.FindFeatureForBuildingWithID(BuildingID);

       

                short res = clsBuildingDataAccessLayer.FindBuildingWithID(ref BuildingID ,ref Name ,
                    ref BuildingNumber, ref City , ref Region ,ref Street, ref AddressID);

                BAddress.AddressID = AddressID;
                BAddress.City = City;
                BAddress.Street = Street;
                BAddress.Region = Region;
                BAddress.Mode = enMode.update;


            if (res == -1) return null;

            //if (BuildingFeatureId != -1) {

            //    // i create the object only when there is a value in Database take it as a note for life

            //        BuildingFeatures = clsBuildingFeatures.Find(BuildingFeatureId);

            //        return new clsBuilding(BuildingID, Name, BuildingNumber, BAddress, BuildingFeatures);
            //}

             return new clsBuilding(BuildingID, Name, BuildingNumber, BAddress);
     
                
 
        }

        //featuresID is Unique in DataBase in Building and Estates

        // DataTable GetEstatesInThisBuilding(buildingID) method => that returns all estates in the building we passed


        public DataTable GetEstatesInThisBuilding(int BuildingID)
        {
            return clsBuildingDataAccessLayer.GetEstateInBuilding(BuildingID);
        }


        // DONE

        // methods to write 
        /*
            1_ get building number for BuildingID
            2_ find number by name
            3_ get Building with Features
            4_ get Buildings With Estates For Sell (int number of estates for sell in building)
           

            now i will do these : 
            
            5_ get bulidngs in specific Address
            7_ get bulidngs with number of floors equals (int num of floors)
            8_ get bulidngs with number of blocks equals (int num of blocks)
            9_ get buildings with AVG Estates price  = amount

         */


        public DataTable getBuildingsWithAverageEstatesPriceEquals(decimal amount)
        {
            return clsBuildingDataAccessLayer.getBuildingsWithAverageEstatesPrice(amount);
        }

        public string getBuildingNumberFor(int BuildingID)
        {

            return clsBuildingDataAccessLayer.getBuildingNumberFromDB(BuildingID);

        }
        
        // as a whole Feature record  numOFBlocks / floors / has alivator
        public DataTable getBuildingsWithFeature(int BuildingFeatureID)
        {
            return clsBuildingDataAccessLayer.getBuildingsWithFeatureFromDB(BuildingFeatureID);
        }

        public DataTable getBuildingsWithEstatesForSaleEquals(long EstatesForSellNumber)
        {
           return clsBuildingDataAccessLayer.GetBuildingsWithNumOfEstatesForSell(EstatesForSellNumber);
        }

        public DataTable getBuildingsWithEstatesForSale()
        {
            return clsBuildingDataAccessLayer.GetBuildingsWithAnyEstatesForSell();
        }

        public DataTable getBuildingsWithAddress(int AddressID)
        {
            return clsBuildingDataAccessLayer.getBuildingsInAddress(AddressID);  
        }
        public DataTable getBuildingWithNumberOfFloorsEqual(int NumberOfFloors)
        {
           return clsBuildingDataAccessLayer.getAllBuildingsWithFloorNumber(NumberOfFloors);
        }
        public DataTable getBuildingWithNumberOfBlocksEqual(int NumberOfBlocks)
        {
            return clsBuildingDataAccessLayer.getAllBuildingsWithBlocksrNumber(NumberOfBlocks);
        }



        public static DataTable getAllBuildings()
        {
            return clsBuildingDataAccessLayer.getAllBuildingsFromDB();
        }


        // add delete update buildings


        private bool _AddNewBuilding()
        {

            int BuildingID = -1;
    
            int AddressID = -1;



            if(this.BuidlingAddress != null && !this.BuidlingAddress.Save())
            {
                return false;
            }




            AddressID = this.BuidlingAddress != null ? this.BuidlingAddress.AddressID : -1;

            BuildingID = clsBuildingDataAccessLayer.AddNewBuilding(this.Name ,this.BuildingNumber
                , AddressID);


            if(BuildingID == -1) return false;

            this.BuildingID = BuildingID;

            return true;

        }


        private bool _UpdateBuilding()
        {
            return clsBuildingDataAccessLayer.UpdateBuildingData(this.BuildingID,this.Name, this.BuildingNumber
               , this.BuidlingAddress.AddressID);
        }




        // DONE

        // Edit on the functions below so : 

        /* ========= ALL DONE FROM DATABASE ========= */

        // Deleting a building deletes all the estates contaiend , 

        // + its BuildingFeatures record : solved by : ON delete Cascade and mission done + editing tables

        //note : BuildingID can be null  

       
        public bool DeleteBuildings(List<int> ids)
        {

            if(ids == null || ids.Count == 0) return false;

            deletetype = ids.Count > 1 ? enDeletetype.deleteGroup : enDeletetype.deleteSingle;


            return _DeleteBuildingsFormDB(ids, deletetype);
        }

        private bool _DeleteBuildingsFormDB(List<int> ids, enDeletetype deletetype)
        {

            

            switch (deletetype)
            {
                case enDeletetype.deleteGroup:
                    return clsBuildingDataAccessLayer.DeleteMultiBuildingsFromDB(ids);

                case enDeletetype.deleteSingle:
                    return clsBuildingDataAccessLayer.DeleteSingleBuildingsFromDB(ids[0]);      
            }

            return false;
        }


        // to be used in Estate Class and Persentation layer

        public bool IfExsist()
        {
            return clsBuildingDataAccessLayer.DoesBuildingExsist(this.BuildingID);
        }


        public bool Save()
        {


            switch (Mode)
            {
                case enMode.add:
                    if (_AddNewBuilding())
                    {
                        Mode = enMode.update;
                        return true;
                    }
                    return false;
                case enMode.update: return _UpdateBuilding();
            }

            return false;
        }



    }
}
