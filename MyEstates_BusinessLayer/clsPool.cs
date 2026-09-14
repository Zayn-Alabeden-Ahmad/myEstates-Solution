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
    public class clsPool : SaveData
    {

        private int _PoolID;
        private decimal _Volume;
        private byte _Number;
        private int _EstateFeatureID;
        public int PoolID { get {return _PoolID; } set {_PoolID = value; } }
        public int EstateFeatureID { get {return _EstateFeatureID; } set { _EstateFeatureID = value; } }
        public decimal Volume { get { return _Volume; } set { _Volume = value; } }
        public byte Number { get {return _Number; } set { _Number = value; } }

        public enMode Mode = enMode.add;
      
        
        public clsPool() {
            PoolID = -1;
            Volume = 0;
            Number = 0;
            Mode = enMode.add;
            
        }

        private clsPool(int PoolID,decimal Volume, byte Number,int EstateFeatureID) {
        
            this.PoolID = PoolID;
            this.Volume = Volume;
            this.Number = Number;
            this.EstateFeatureID = EstateFeatureID;
            Mode = enMode.update;
        }

        // find pool 

        public static clsPool Find(int PoolID)
        {
            decimal Volume = 0;
            byte Number = 0;
            int EstateFeatureID = -1;
            bool res = clsPoolDataAccessLayer.FindPool(ref PoolID, ref Volume, ref Number,ref EstateFeatureID);

            if (res) { 
                 return new clsPool(PoolID, Volume, Number, EstateFeatureID);    
            }
            return null;    
        }



        /*
         * 1_ get all Pools
         * 2_ get Pools with Volume
         * 3_ get Pools with EstateFeatureID = ID
        */


        public DataTable getAllPools()
        {
            return clsPoolDataAccessLayer.getAllPoolsFormDB();
        }
        public DataTable getAllPoolsWithVolume(decimal Volume)
        {
            return clsPoolDataAccessLayer.getAllPoolsWithVolumeFromDB(Volume);
        }
        public DataTable getAllPoolsWhomBelongsToEstateFeature(int EstateFeatureID)
        {
            return clsPoolDataAccessLayer.getAllPoolWithEstateFeatureID(EstateFeatureID);
        }









        // when adding a Pool or Garage in the UI a list apper to let the user decide which estateFeature is realted to this garage/pool

        // add , update only used after adding EstateFeature
        // by creating Pool obj in EstateFeature and using its Save Method which will
        // declare any method to use by detecting the obj mode , depends on the constructor that initialize it

        private int _AddNewPool() 
        {
            int PoolID = -1;

            PoolID = clsPoolDataAccessLayer.AddNewPoolToDB(this.Volume,this.Number,this.EstateFeatureID);
            return PoolID;
        }

        private bool _UpdatePoolInfo()
        {
            return clsPoolDataAccessLayer.UpdatePoolInfo(this.PoolID,this.Volume, this.Number, this.EstateFeatureID);
        }

        // Delete code

        public bool DeletePool(List<int>ids)
        {
            if (ids == null || ids.Count == 0) return false;

            return _DeletePoolFromDB(ids);
        }

        private bool _DeletePoolFromDB(List<int>ids)
        {

            return clsPoolDataAccessLayer.DeleteMultiPoolsFromDB(ids);
        }


        // get all Pools related to EstateFeature

        public bool IfExsist()
        {
            return clsPoolDataAccessLayer.DoesPoolExsist(this.PoolID);
        }


        public bool Save()
        {

            switch (Mode)
            {
                case enMode.add:
                    if (_AddNewPool() != -1)
                    {
                        Mode = enMode.update;
                        return true;
                    }
                    return false;

                case enMode.update: return _UpdatePoolInfo();
            }

            return false;
        }

    }






}
