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
    public class clsEstatesFeatures : SaveData
    {


        private int _EstateFeatureID;
        private decimal _EstateSpace;
        private short _FloorNumber;
        private byte _NumberOfRooms;
        private string _Description;
        private short _BitwiseFeatures;
        private bool _HasGarden;
        private List<clsGarage> _Garages;
        private List<clsPool> _Pools;
        public int EstateFeatureID { get { return _EstateFeatureID; } set {_EstateFeatureID = value; } }
        public decimal EstateSpace { get {return _EstateSpace; } set { _EstateSpace = value; } }
        public short FloorNumber { get {return _FloorNumber; } set {_FloorNumber = value; } }
        public byte NumberOfRooms { get { return _NumberOfRooms; } set {_NumberOfRooms = value; } }
        public string Description { get {return _Description; } set {_Description = value; } }
        public short BitwiseFeatures { get {return _BitwiseFeatures; } set {_BitwiseFeatures =value; } } // to calc preferences depending on customer  choices 
        public bool HasGarden { get {return _HasGarden; } set {_HasGarden = value; } }    
        // garage and pool aggrigation // no need due to database edit
        //public List <clsGarage> Garages { get {return _Garages; } set { _Garages = value; } } // estate can have more than one Garage
        //public List <clsPool> Pools { get {return _Pools; } set {_Pools =value; } } // estate can have more than one Pool

        public enMode Mode = enMode.add;
        public enDeletetype deletetype = enDeletetype.deleteSingle;

        public clsEstatesFeatures()
        {
            EstateFeatureID = -1;
            EstateSpace = 0;
            FloorNumber = 0;
            NumberOfRooms = 0;
            Description = string.Empty;
            BitwiseFeatures = 0;
            HasGarden = false;

            Mode = enMode.add;

            //Garages = new List <clsGarage>();
            //Pools = new List <clsPool>();


        
        }


        // to initialize Feature object inside estate class

        // featuresID is Unique in DataBase in Building and Estates

        private clsEstatesFeatures(int EstateFeatureID, decimal EstateSpace  ,short FloorNumber ,byte NumberOfRooms 
            , string Description ,short BitwiseFeatures , bool HasGarden)
        {
            this.EstateFeatureID = EstateFeatureID;
            this.EstateSpace = EstateSpace;
            this.FloorNumber = FloorNumber;
            this.NumberOfRooms = NumberOfRooms;
            this.Description = Description;
            this.BitwiseFeatures = BitwiseFeatures;
            this.HasGarden = HasGarden;

            //this.Garages = garages ?? null;
            //this.Pools = pools ?? null;

            Mode= enMode.update;

    
        }

       //static private DataTable _GaragesInfo(int EstateFeatureID)
       // {
       //     return clsEstatesFeaturesDataAccessLayer.GetGaragesRelatedToEstateWith(EstateFeatureID);
       // }

       //static private DataTable _PoolsInfo(int EstateFeatureID)
       // {
       //     return clsEstatesFeaturesDataAccessLayer.GetPoolsRelatedToEstateWith(EstateFeatureID);
       // }

       static public clsEstatesFeatures Find(int EstateFeatureID)
        {
            decimal EstateSpace = 0;
            short FloorNumber = 0;
            byte NumberOfRooms = 0;
            string Description = "";
            short BitwiseFeatures = 0;
            bool HasGarden = false;

            // garage and pool initialize and send as lists

            //List<clsGarage> Garages = new List<clsGarage>();
            //List<clsPool> Pools = new List<clsPool>();






            // return DataTable of grages each data member alone 
            // the query = select * form EstateFeatures where EstateFeatureID = @EstateFeatureID
            // return DataTable of pools each data member alone
            // each one alone => then use FindFeatureByID then ,create the obejcts and send them to the constructor




            if (clsEstatesFeaturesDataAccessLayer.FindFeatureByID(ref EstateFeatureID, ref EstateSpace,
            ref FloorNumber, ref NumberOfRooms, ref Description, ref BitwiseFeatures, ref HasGarden))
            {



                return new clsEstatesFeatures(EstateFeatureID, EstateSpace, FloorNumber, NumberOfRooms
                , Description, BitwiseFeatures, HasGarden);

                //DataTable garagesDT = new DataTable();
                //DataTable poolDT = new DataTable();

                //garagesDT = _GaragesInfo(EstateFeatureID);
                //poolDT = _PoolsInfo(EstateFeatureID);



                // get-rid off this and use the DataTable Directly , search about it
                //if (garagesDT.Rows.Count > 0)
                //{

                //    foreach (DataRow row in garagesDT.Rows)
                //    {
                //        // creating alot of objects which is bad
                //        clsGarage garage = new clsGarage();

                //        garage.GarageID = row["GarageID"] != DBNull.Value ? (int)row["GarageID"] : -1;
                //        garage.GarageLocation = row["GarageLocation"]?.ToString() ?? "";
                //        garage.Size = row["Size"] != DBNull.Value ? (decimal)row["Size"] : 0;
                //        garage.Number = row["Number"] != DBNull.Value ? (byte)row["Number"] : (byte)0;

                //        // add to list
                //        Garages.Add(garage);

                //    }
                //}

                //if (poolDT.Rows.Count > 0)
                //{
                //    foreach (DataRow row in poolDT.Rows)
                //    {
                //        clsPool pool = new clsPool();

                //        pool.PoolID = row["PoolID"] != DBNull.Value ? (int)row["PoolID"] : -1;
                //        pool.Volume = row["Volume"] != DBNull.Value ? (decimal)row["Volume"] : 0;
                //        pool.Number = row["Number"] != DBNull.Value ? (byte)row["Number"] : (byte)0;

                //        Pools.Add(pool);


                //    }
                //}


            }

            return null;
        }



        private bool _AddNewEstateFeature()
        {

            int featureID = -1;

            featureID = clsEstatesFeaturesDataAccessLayer.AddNewEstateFeatureToDB(
                this.EstateSpace, this.FloorNumber,this.NumberOfRooms, this.Description, this.BitwiseFeatures, this.HasGarden);

            if (featureID == -1) { return false; }

            // to use it 
            this.EstateFeatureID = featureID;

            return true;

        }


        private bool _UpdateEstateFeatures()
        {
            return clsEstatesFeaturesDataAccessLayer.UpdateEstateFeatureInDB(this.EstateFeatureID,
                this.EstateSpace, this.FloorNumber, this.NumberOfRooms, this.Description, this.BitwiseFeatures, this.HasGarden);
        }



        // Delete EstateFeatures => be aware of data integrity , cause FeatureID is FK in Estate ,Pool ,Garage // ON DELETE CASCADE 



        public bool DeleteEstateFeature(List<int> estatesFeaturesIds)
        {

            if(estatesFeaturesIds == null ||estatesFeaturesIds.Count == 0) return false;
            

            return _DeleteEstateFeature(estatesFeaturesIds);

        }


        private bool _DeleteEstateFeature(List<int> estatesFeaturesIds)
        {
            return clsEstatesFeaturesDataAccessLayer.DeleteEstateFeaturesFromDB(estatesFeaturesIds);
        }


        public bool IfExsist()
        {
            return clsEstatesFeaturesDataAccessLayer.DoesEstateFeaturExsist(this.EstateFeatureID);
        }


        // to be used in Estate Class only
        public bool Save()
        {


            switch (Mode)
            {
                case enMode.add:
                    if (_AddNewEstateFeature())
                    {
                        Mode = enMode.update;
                        return true;
                    }
                    return false;

                case enMode.update: return _UpdateEstateFeatures();
            }
            

            return false;
        }



    }
}
