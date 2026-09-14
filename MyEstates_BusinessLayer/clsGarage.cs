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
    public class clsGarage : SaveData
    {

        private int _GarageID;
        private decimal _Size;
        private string _GarageLocation;
        private byte _Number;
        private int _EstateFeatureID;

        public  int GarageID { get { return _GarageID; } set { _GarageID = value; }}
        public decimal Size { get { return _Size;  } set { _Size = value; }}
        public string GarageLocation { get { return _GarageLocation; } set {_GarageLocation = value; }}
        public byte Number { get{ return _Number;} set {_Number = value;}}
        public int EstateFeatureID { get{ return _EstateFeatureID; } set { _EstateFeatureID = value;}}

        public enMode Mode = enMode.add;

        public clsGarage()
        {
            GarageID = -1;
            Size = 0;
            Number = 0;
            GarageLocation = string.Empty;
            Mode = enMode.add;
        }

        public clsGarage(int GarageID,decimal Size ,string GarageLocation,byte Number,int EstateFeatureID)
        {
            this.GarageID = GarageID;
            this.Size = Size;
            this.GarageLocation = GarageLocation;
            this.Number = Number;
            this.EstateFeatureID = EstateFeatureID;
            Mode = enMode.update;
        }

        static public clsGarage Find(int GarageID)
        {
            decimal Size = 0;
            string GarageLocation = string.Empty;
            byte Number = 0;
            int EstateFeatureID = -1;

            if (clsGarageDataAccessLayer.FindGarageByID( ref GarageID, ref GarageLocation,ref Size,ref Number,ref EstateFeatureID)) { 
                return new clsGarage(GarageID, Size, GarageLocation, Number, EstateFeatureID);
            }

            return null;
        }



        /*
       * 1_ get all Garages
       * 2_ get Garages with Size
       * 3_ get Garages with EstateFeatureID = ID
        */


        public DataTable getAllGarages()
        {
            return clsGarageDataAccessLayer.getAllGaragesFormDB();
        }
        public DataTable getAllGaragesWithSize(decimal Size)
        {
            return clsGarageDataAccessLayer.getAllGaragesWithSizeFromDB(Size);
        }
        public DataTable getAllGaragesWhomBelongsToEstateFeature(int EstateFeatureID)
        {
            return clsGarageDataAccessLayer.getAllGaragesWithEstateFeatureID(EstateFeatureID);
        }






        // add , update only used after adding EstateFeature
        // by creating Garage obj in EstateFeature and using its Save Method which will
        // declare any method to use by detecting the obj mode , depends on the constructor that initialize it
        private bool _AddNewGarage()
        {

            int id = -1;

            id = clsGarageDataAccessLayer.AddNewGarageToDB(this.Size,this.GarageLocation,this.Number,this.EstateFeatureID);

            if (id != -1) { 
                return true;
            }
            return false;

        }

        private bool _UpdateGarageData()
        {

            return clsGarageDataAccessLayer.UpdateGarageDataInDB(this._GarageID,this.Size,this.GarageLocation,
                this.Number,this.EstateFeatureID);
        }

        public bool DeleteGarage(List<int> ids)
        {
            if(ids == null || ids.Count == 0)  return false;

            return _DeleteGarageFromDB(ids);
        }

        private bool _DeleteGarageFromDB(List<int>ids)
        {
   
             return clsGarageDataAccessLayer.DeleteMultiGaragesFromDB(ids);

        }

        public bool IfExsist()
        {
            return clsGarageDataAccessLayer.DoesGarageExsist(this.GarageID);
        }









        public bool Save()
          {
            switch (Mode)
            {
                case enMode.add:
                    if (_AddNewGarage())
                    {
                        Mode = enMode.update;
                        return true;
                    }
                    return false;

                case enMode.update: return _UpdateGarageData();

            }
            

            return false;
          }




    }
}
