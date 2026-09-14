using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
    public class clsBuildingFeatures : SaveData
    {
        private int  _BuildingFeatureID;
        private byte _NumberOfBlocks;
        private byte _NumberOfFloors;
        private bool _HasAlivator;
        private int _BuildingID;

        public enMode mode = enMode.add;
        public enDeletetype deleteType = enDeletetype.deleteSingle;


        public int BuildingFeaturesID { get { return _BuildingFeatureID; } set { _BuildingFeatureID = value; } }
        public byte NumberOfBlocks { get { return _NumberOfBlocks; } set { _NumberOfBlocks = value; } }
        public byte NumberOfFloors { get { return _NumberOfFloors; } set { _NumberOfFloors = value; } }
        public bool HasAlivator { get { return _HasAlivator; } set { _HasAlivator = value; } }
        public int BuildingID { get { return _BuildingID; } set { _BuildingID = value; } }


        public clsBuildingFeatures()
        {
            BuildingFeaturesID = -1;
            BuildingID = -1;
            NumberOfBlocks = 0;
            NumberOfFloors = 0;
            HasAlivator = false;
            mode= enMode.add;
        }

        private clsBuildingFeatures(int buildingFeaturesID, byte numberOfBlocks,byte numberOfFloors ,bool hasAlivator,int buildingID)
        {
            BuildingFeaturesID = buildingFeaturesID;
            NumberOfBlocks = numberOfBlocks;
            NumberOfFloors = numberOfFloors;
            HasAlivator = hasAlivator;
            BuildingID = buildingID;
            mode = enMode.update;
        }

        static public clsBuildingFeatures Find(int BuildingFeaturesID)
        {
       
         byte NumberOfBlocks = 0;
         byte NumberOfFloors = 0;
         bool HasAlivator = false;
         int BuildingID = -1; 
         bool res = false;

            res = clsBuildingFeaturesDataAccessLayer.FindBuildingFeatureBy(ref BuildingFeaturesID, ref NumberOfBlocks, ref NumberOfFloors, ref HasAlivator,ref BuildingID);

            if (res)
            {
                return new clsBuildingFeatures(BuildingFeaturesID, NumberOfBlocks, NumberOfFloors, HasAlivator, BuildingID);
            }


            return null;
        }



        /*
         
        it will work like pool and garages 
            in the presentation layer a List will show up after adding the the feature will tell
             you which Bulidng do you want to link this feature with 

            ** adding building is the first step then adding its feature ,
         
         */


        /*
            methods to write :
               getNumberOfBlocksForBuildingWith(ID);
               getNumberOfFloorsForBuildingWith(ID);
               DoesBuildingHasAlivator(ID)
         */


        public byte getNumberOfBlocksForBuildingWith(int BuildingID)
        {
            return clsBuildingFeaturesDataAccessLayer.getNumberOfBlocksForBuilding(BuildingID);

        }
        public byte getNumberOfFloorsForBuildingWith(int BuildingID)
        {
            return clsBuildingFeaturesDataAccessLayer.getNumberOfFloorsForBuilding(BuildingID);

        }
        public bool DoesBuildingHasAlivator(int BuildingID)
        {
            return clsBuildingFeaturesDataAccessLayer.checkAlivatorInBuilding(BuildingID);
        }





        // add delete update code 

        //featuresID is Unique 

        private bool _AddNewBuildingFeature()
        {
 
            int FeatureID =  clsBuildingFeaturesDataAccessLayer.AddNewFeatureOfBuidling(this.NumberOfBlocks,this.NumberOfFloors,this.HasAlivator,this.BuildingID);

            if (FeatureID == -1) return false;

            this.BuildingFeaturesID = FeatureID;

            return true;
        }
        private bool _UpdateBuildingFeature() {

            bool res = false;

            res = clsBuildingFeaturesDataAccessLayer.UpdateTheBuildingFeature(this.BuildingFeaturesID,this.NumberOfBlocks,this.NumberOfFloors,this.HasAlivator,this.BuildingID);

            return res;
        
        }
        public bool DeleteBuildingFeatures(List<int>Ids)
        {
            if(Ids == null) return false;

            if(Ids.Count == 0)return false;

            deleteType = Ids.Count > 1 ? enDeletetype.deleteGroup : enDeletetype.deleteSingle;


            return _DeleteFeature(Ids,deleteType);
        }
        private bool _DeleteFeature(List<int>ids,enDeletetype deleteType)
        {
            switch (deleteType)
            {
                case enDeletetype.deleteGroup:
                    return clsBuildingFeaturesDataAccessLayer.DeleteMultiBuidligFeatureFromDB(ids);
                case enDeletetype.deleteSingle:
                    return clsBuildingFeaturesDataAccessLayer.DeleteSingleBuidligFeatureFromDB(ids[0]);

            }
            return false;
            
        }
        public bool IfExsist()
        {
            return clsBuildingFeaturesDataAccessLayer.DoesBuildingFeatureExsist(this.BuildingFeaturesID);
        }


        public bool Save()
        {

            switch (mode) {
                case enMode.add:
                    if (_AddNewBuildingFeature())
                    {
                     mode =enMode.update;
                        return true;

                    }return false;

                case enMode.update: return _UpdateBuildingFeature();
            
            }
            return false;
        }


    }
}
