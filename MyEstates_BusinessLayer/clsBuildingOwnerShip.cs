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
    public class clsBuildingOwnerShip : SaveData
    {

        private int _BuildingOwnerShipID;
        private decimal _OwnerShipPercentage;
        private int _BuildingID;
        private int _OwnerID;

        private enMode Mode;
        private enDeletetype deletetype;



        public int BuildingOwnerShipID { get { return _BuildingOwnerShipID; } set { _BuildingOwnerShipID = value; } }
        public decimal OwnerShipPercentage { get { return _OwnerShipPercentage; } set { _OwnerShipPercentage = value; } }
        public int BuildingID { get { return _BuildingID; } set { _BuildingID = value; } }
        public int OwnerID { get { return _OwnerID; } set { _OwnerID = value; } }



        public clsBuildingOwnerShip()
        {
            BuildingOwnerShipID = -1;
            OwnerShipPercentage = 0;
            BuildingID = -1; 
            OwnerID = -1;
            Mode = enMode.add;
        }


        public clsBuildingOwnerShip(int buildingOwnerShipID,decimal ownerShipPercentage,int buildingID,int ownerID)
        {
            this.BuildingOwnerShipID=buildingOwnerShipID;
            this.OwnerShipPercentage = ownerShipPercentage;
            this.BuildingID = buildingID;
            this.OwnerID = ownerID;
            Mode = enMode.update;
        }


        static public clsBuildingOwnerShip Find(int BuildingOwnerShipID)
        {

            decimal OwnerShipPercentage = 0;
            int BuildingID = -1;
            int OwnerID = -1;

            if (clsBuildingOwnerShipDataAccessLayer.FindBuildingOwnerShipWithID(ref BuildingOwnerShipID, ref OwnerShipPercentage, ref BuildingID, ref OwnerID))
            {
                return new clsBuildingOwnerShip(BuildingOwnerShipID, OwnerShipPercentage, BuildingID, OwnerID);
            }

            return null;

        }


        public static DataTable getOwnersWithOwiningPercentage(decimal amount)
        {
            return clsBuildingOwnerShipDataAccessLayer.getOwnersWithPercentage(amount);

        }

        public static DataTable getAllOwnersWitOwningPersentageBiggerThan(decimal amount)
        {
            return clsBuildingOwnerShipDataAccessLayer.getOwnersWithPercentageBiggerThan(amount);
        }

        public static DataTable getAllOwnersWitOwningPersentageLessThan(decimal amount)
        {
            return clsBuildingOwnerShipDataAccessLayer.getOwnersWithPercentageLessThan(amount);
        }


        public static DataTable getOwnersOfThisBuilding(int BuildingID)
        {
            return clsBuildingOwnerShipDataAccessLayer.getAllOwnersOfThisBuildingWith(BuildingID);
        }

        public static DataTable getBuildingsWithThisOwner(int OwnerID)
        {
            return clsBuildingOwnerShipDataAccessLayer.getAllBuildingsOfOwnerWith(OwnerID);
        }
















        // add update delete 


        private bool _AddNewBuildingOwnerShip()
        {

            int PropID = -1;

            PropID = clsBuildingOwnerShipDataAccessLayer.AddNewBuildingOwnerShip(this.OwnerShipPercentage,this.BuildingID,this.OwnerID);

            return PropID != -1 ? true : false;
        }

        private bool _UpdateBuildingOwnerShip()
        {
            return clsBuildingOwnerShipDataAccessLayer.UpdateBuildingOwnerShip(this.BuildingOwnerShipID, this.OwnerShipPercentage, this.BuildingID, this.OwnerID);

        }




        public bool DeletBuildingOwnerShip(List<int>ids) {

            if (ids == null || ids.Count == 0) return false;

            deletetype = ids.Count > 1 ? enDeletetype.deleteGroup : enDeletetype.deleteSingle;

            return _DeleteBuildingOwnerShipFromDB(ids, deletetype);

        }

        private bool _DeleteBuildingOwnerShipFromDB(List<int>ids ,enDeletetype deletetype)
        {
            switch (deletetype) { 
                    
                case enDeletetype.deleteGroup:
                    return clsBuildingOwnerShipDataAccessLayer.DeleteGroupOfBuildingOwnerShip(ids);
                case enDeletetype.deleteSingle:
                    return clsBuildingOwnerShipDataAccessLayer.DeleteSingleOfBuildingOwnerShip(ids[0]);
 
            }

            return false;
        }




        public bool IfExsist()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            switch (Mode) { 
                
                case enMode.add:
                    if (_AddNewBuildingOwnerShip()) { 
                        Mode = enMode.update; 
                        return true;
                    }
                    return false;

                case enMode.update:
                    return _UpdateBuildingOwnerShip();
            }

            return false;
        }
    }
}
