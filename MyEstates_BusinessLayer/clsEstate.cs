using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
    public class clsEstate : SaveData
    {
      


        private int _EstateID;
        private string _EstateName;
        private string _EstateNumber;
        private decimal _Price;
        private byte _EstateType; // byte refers tiny int
        private byte _SellingEstate;
        private byte _EstateStatus;
        private byte _SetFor;
        private bool _HasKeys;
        private DateTime _BuiltDate;
        private clsEstatesFeatures _EstatesFeatures;
        private clsBuilding _EstateBuilding;


        public int EstateID { set { _EstateID = value; } get { return _EstateID; } }
        public string EstateName { set { _EstateName = value; } get { return _EstateName; } }
        public string EstateNumber { set { _EstateNumber = value; } get { return _EstateNumber; } }
        public decimal Price { set { _Price = value; } get { return _Price; } }
        public byte EstateType { set { _EstateType = value; } get { return _EstateType; } } // byte refers tiny int  {apartment,villa ,house,shalle}
        public byte SellingEstate { set { _SellingEstate = value; } get { return _SellingEstate; } } //  { sold, rented, available }
        public byte EstateStatus { set { _EstateStatus = value; } get { return _EstateStatus; } }// {skulled,furnished}
        public byte SetFor { set { _SetFor = value; } get { return _SetFor; } }
        public bool HasKeys { set { _HasKeys = value; } get { return _HasKeys; } }
        public DateTime BuiltDate { set { _BuiltDate = value; } get { return _BuiltDate; } }

        // estateFeatures + building composition 
        public clsEstatesFeatures EstatesFeatures { set { _EstatesFeatures = value; } get { return _EstatesFeatures; } }
        public clsBuilding EstateBuilding { set { _EstateBuilding = value; } get { return _EstateBuilding; } }

        public enMode Mode = enMode.add;
        public enDeletetype deletetype = enDeletetype.deleteSingle;
        public enFilter filter = enFilter.estateStatus;
        public enPeriod_filter enPeriod = enPeriod_filter.before;

        // use the vw_AllEstatesInformations => this view will have the estsate + its features + building + pool + garage

        public clsEstate()
        {

            EstateID = -1;
            EstateName = string.Empty;
            EstateNumber = string.Empty;
            Price = 0;
            EstateType = 0;
            SellingEstate = 0;
            EstateStatus = 0;
            SetFor = 0;
            HasKeys = false;
            BuiltDate = DateTime.Now;

            this.EstatesFeatures = new clsEstatesFeatures(); // Essential! ,has add_new mode
            this.EstateBuilding = new clsBuilding(); // has add_new mode
            Mode = enMode.add;
        }


        // first constructor  inforce user to add estate-feature 
        private clsEstate(int estateID, string estateName, string estateNumber, decimal price, byte estateType,
            byte sellingEstate, byte estateStatus, byte setFor, bool hasKeys, DateTime builtDate, clsEstatesFeatures estatesFeatures
            , clsBuilding building)
        {
            EstateID = estateID;
            EstateName = estateName;
            EstateNumber = estateNumber;
            Price = price;
            EstateType = estateType;
            SellingEstate = sellingEstate;
            EstateStatus = estateStatus;
            SetFor = setFor;
            HasKeys = hasKeys;
            BuiltDate = builtDate;

            this.EstatesFeatures = estatesFeatures;
            this.EstateBuilding = building;
            Mode = enMode.update;
        }

        // secound constructor  optional  add estate then add  estate-feature later => using addEstateFeatures method 'did not write it yet' 
        private clsEstate(int EstateID, string EstateName, string EstateNumber, decimal Price, byte EstateType,
            byte SellingEstate, byte EstateStatus, byte SetFor, bool HasKeys, DateTime BuiltDate, clsBuilding Building)
        {
            this.EstateID = EstateID;
            this.EstateName = EstateName;
            this.EstateNumber = EstateNumber;
            this.Price = Price;
            this.EstateType = EstateType;
            this.SellingEstate = SellingEstate;
            this.EstateStatus = EstateStatus;
            this.SetFor = SetFor;
            this.HasKeys = HasKeys;
            this.BuiltDate = BuiltDate;
            this.EstateBuilding = Building;
            Mode = enMode.update;
        }

        static public clsEstate Find(int EstateID)
        {

            string EstateName = string.Empty;
            string EstateNumber = string.Empty;
            decimal Price = 0;
            byte EstateType = 0;
            byte SellingEstate = 0;
            byte EstateStatus = 0;
            byte SetFor = 0;
            bool HasKeys = false;
            DateTime BuiltDate = DateTime.Now;



            // get estate-feature if there is => use constructor 1 , else use constructor 2

            int EstateFeatureID = -1;
            int EstateBuildingID = -1;
            clsEstatesFeatures EstatesFeatures = null;
            clsBuilding EstateBuilding = null;


            // this is wrong because we will get the ids from the database when searching for estate
            // if ids returend as -1 => they are null in the DB so i will deal with it

            //EstateFeatureID = clsEstateDataAccessLayer.getEstateFeaturIDFormEstateTable(EstateID);
            //EstateBuildingID = clsEstateDataAccessLayer.getBuildingIDofEstate(EstateID);


            if (clsEstateDataAccessLayer.FindEstateWithEstateID(ref EstateID,
                ref EstateName, ref EstateNumber, ref Price, ref EstateType, ref SellingEstate
                    , ref EstateStatus, ref SetFor, ref HasKeys, ref BuiltDate, ref EstateFeatureID, ref EstateBuildingID))
            {
                EstateBuilding = clsBuilding.Find(EstateBuildingID);



                if (EstateFeatureID == -1)
                {

                    return new clsEstate(EstateID, EstateName, EstateNumber, Price, EstateType, SellingEstate
                        , EstateStatus, SetFor, HasKeys, BuiltDate, EstateBuilding);
                }

                else
                {
                    EstatesFeatures = clsEstatesFeatures.Find(EstateFeatureID);

                    return new clsEstate(EstateID, EstateName, EstateNumber, Price, EstateType, SellingEstate
                            , EstateStatus, SetFor, HasKeys, BuiltDate, EstatesFeatures, EstateBuilding);
                }

            }


            return null;
        }



        // estate methods 
        /*
            1_ get estate/s set for sell , and get if estate is for sell or not => true / false 
            2_ get estate/s status * done
            3_ get estate/s price * done
            4_ get estate/s selling status * done
            5_ get all estates information * done
            6_ get single estate information * done
            7_ get estate type * done
            8_ get all estates with estate type = , selling estate , estate status * done

         */


    





        public static DataTable getEstatesForSell(byte SetFor)
        {
            return clsEstateDataAccessLayer.getAllEstatesMarkedForSell(SetFor);
        }

        public static DataTable getEstatesWithBuildDate(DateTime value,enPeriod_filter enPeriod)
        {

            switch (enPeriod)
            {
                case enPeriod_filter.before :
                    return clsEstateDataAccessLayer.getEstatesHasBuildPeriod(Before : value);
                case enPeriod_filter.after :
                    return clsEstateDataAccessLayer.getEstatesHasBuildPeriod(After  : value);
                case enPeriod_filter.with :
                    return clsEstateDataAccessLayer.getEstatesHasBuildPeriod(With : value);

            }
            return null;
        }

        public static decimal getEstatePrice(int EstateID)
        {
            return clsEstateDataAccessLayer.GetEstatePriceFor(EstateID,null);
        }
        public static decimal getEstatePrice(string EstateNumber)
        {
            return clsEstateDataAccessLayer.GetEstatePriceFor(null,EstateNumber);
        }

        public static byte getEstateStatus(int EstateID)
        {
            return clsEstateDataAccessLayer.GetEstateStatusFor(EstateID,null);
        } 
        public static byte getEstateStatus(string EstateNumber)
        {
            return clsEstateDataAccessLayer.GetEstateStatusFor(null,EstateNumber);
        }
        public static DataTable getAllEStatesInformation()
        {
            return clsEstateDataAccessLayer.getAllEstatesInfosFromDB();

        }
        public clsEstate getSingleEstateInfos(int EstateID)
        {
            if (IfExsist(EstateID))
            {
                return Find(EstateID);
            }
            return null;
        }

        public static byte GetSellingEstateForEstateWith(int EstateID)
        {
            return clsEstateDataAccessLayer.ExecuteGetSellingEstateStatus(EstateID, null);
        }

        public static byte GetSellingEstateForEstateWith(string EstateNumber)
        {
            return clsEstateDataAccessLayer.ExecuteGetSellingEstateStatus(null, EstateNumber);
        }

        public byte getEstateTypeFor(int EstateID)
        {
            return clsEstateDataAccessLayer.getEstateTypeFor(EstateID,null);
        }
        public byte getEstateTypeFor(string EstateNumber)
        {
            return clsEstateDataAccessLayer.getEstateTypeFor(null,EstateNumber);

        }
  
        public DataTable getAllEstatesWith(byte value,enFilter filterType) {

            // first time using named arrgument
            // means we pass the value to the arrgument we want
            // in the DAL function we must send null to all parameters ,because we will not send values to all of them

            switch (filterType)
            {
                case enFilter.estateType: 
                    // named arrgument 
                return clsEstateDataAccessLayer.getAllEstatesWithFilter(EstateType: value);

                case enFilter.sellingEstate:
                return clsEstateDataAccessLayer.getAllEstatesWithFilter(SellingEstate :value);

                case enFilter.estateStatus:
                return clsEstateDataAccessLayer.getAllEstatesWithFilter(EstateStatus: value);

            }
            return null;

        }

        public DataTable getAllEstatesWithGarages()
        {
            return clsEstateDataAccessLayer.getEstatesWithGarages();
        }

        public DataTable getEstatesWithNumberOfGaragesEquals(byte numOfGaragesWanted)
        {
            return clsEstateDataAccessLayer.getEstatesWithGaragesNumberEqualsTo(numOfGaragesWanted);
        }

        public DataTable getEstatesWithGaragesSizeEquals(decimal size)
        {
            return clsEstateDataAccessLayer.getEstatesWithGaragesSizeEqualsTo(size);
        }

        public DataTable getAllEstatesWithPools()
        {
            return clsEstateDataAccessLayer.getEstatesWithPools();
        }

        public DataTable getEstatesWithNumberOfPoolsEquals(byte numOfPoolsWanted)
        {
            return clsEstateDataAccessLayer.getEstatesWithPoolsNumberEqualsTo(numOfPoolsWanted);
        }

        public DataTable getEstatesWithPoolVolumeEquals(decimal Volume)
        {
            return clsEstateDataAccessLayer.getEstatesWithPoolVolumeEqualsTo(Volume);
        }








        private bool _AddNewEstate()
        { 
            int EstateID = -1;
            int EstateFeaturesID = -1;
            int BuildingID = -1;

            // in Persentation layer we define an EstateFeature Object and when we finish adding its values we assign it
            // to the Estate.EstatesFeatures (:
            // we dont save the EstateFeatures Form Persentation Layer , We Save it From Here

            // Same for Building

            if (this.EstatesFeatures != null && this.EstatesFeatures.Mode == enMode.add)
            {
                if (!this.EstatesFeatures.Save())
                    return false;
            }
            if (this.EstateBuilding != null && this.EstateBuilding.Mode == enMode.add)
            {
                if (!this.EstateBuilding.Save())
                    return false;
            }


            EstateFeaturesID = (this.EstatesFeatures != null) ? this.EstatesFeatures.EstateFeatureID : -1;
            BuildingID = (this.EstateBuilding != null) ? this.EstateBuilding.BuildingID : -1;



            EstateID = clsEstateDataAccessLayer.AddNewEstateToDB(this.EstateName, this.EstateNumber, this.Price,
                   this.EstateType, this.SellingEstate, this.EstateStatus, this.SetFor, this.HasKeys, this.BuiltDate,
                   EstateFeaturesID, BuildingID);


            if ( EstateID == -1)
            {
                return false;
            }
            return true;
        }

        private bool _UpdateEstateData()
        {

            if (this.EstatesFeatures != null && !this.EstatesFeatures.Save())
                return false;

            if (this.EstateBuilding != null && !this.EstateBuilding.Save())
                return false;


            int featureId = (this.EstatesFeatures != null) ? this.EstatesFeatures.EstateFeatureID : -1;
            int buildingId = (this.EstateBuilding != null) ? this.EstateBuilding.BuildingID : -1;


            return clsEstateDataAccessLayer.UpdateEstateInDB(this.EstateID,this.EstateName, this.EstateNumber, this.Price,
                   this.EstateType, this.SellingEstate, this.EstateStatus, this.SetFor, this.HasKeys, this.BuiltDate,
                   featureId, buildingId);
        }


        // Delete EstateCode ,  Find Estates by {what you want} , + and cluster index on SetFor column
        // Deleting an Estate Will Delete ALL the related records to it 
        // Estate delete => delete its record from EstateFeatures ;


        // First Transaction + stored procedure  in Written for This double deletion (:

        // this delete method uses a Transction and a stored prosedure

        public bool DeleteEstates(List<int> EstatesIDs)
        {
            if (EstatesIDs == null || EstatesIDs.Count == 0) return false;

            return _DeleteEstatesFormDataBase(EstatesIDs);
        }

        private bool _DeleteEstatesFormDataBase(List<int> EstatesIDs) {


            return clsEstateDataAccessLayer.DeleteEstatesFromDB(EstatesIDs);
        
        }

        // if u have the object filled , for updating record
         public bool IfExsist()
        {
            return clsEstateDataAccessLayer.DoesEstateExsist(this.EstateID);
        }

        // if u dont have an object , just id for serching by id 
        public  bool IfExsist(int EstateID)
        {
            return clsEstateDataAccessLayer.DoesEstateExsist(EstateID);
        }








        public bool Save()
        {


            switch (Mode)
            {
                case enMode.add:
                    if (_AddNewEstate())
                    {
                        Mode = enMode.update;
                        return true;
                    }
                    return false;
                case enMode.update: return _UpdateEstateData();
            }


            return false;
        }

    }
}
