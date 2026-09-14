using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
    public class clsPropertyOwnerShips : SaveData
    {
        private int _PropertyOwnerShipID;
        private decimal _OwnerShipPercentage;
        private int _EstateID;
        private int _OwnerID;

        private enMode Mode;
        private enDeletetype deletetype;



        public int  PropertyOwnerShipID {get {return _PropertyOwnerShipID;} set { _PropertyOwnerShipID = value; } }
        public decimal OwnerShipPercentage { get {return _OwnerShipPercentage; } set { _OwnerShipPercentage = value; } }
        public int EstateID { get {return _EstateID; } set { _EstateID = value; } }
        public int OwnerID { get {return _OwnerID; } set { _OwnerID = value; } }



        public clsPropertyOwnerShips() {
            PropertyOwnerShipID = - 1;
            OwnerShipPercentage = 0;
            EstateID = -1;
            OwnerID = -1;
            Mode = enMode.add;
        } 

        private clsPropertyOwnerShips(int propertyOwnerShipID, decimal ownerShipPercentage, int estateID, int ownerID)
        {
            PropertyOwnerShipID = propertyOwnerShipID;
            OwnerShipPercentage = ownerShipPercentage;
            EstateID = estateID;
            OwnerID = ownerID;
            Mode = enMode.update;
        }



        // find 
        static public clsPropertyOwnerShips Find(int PropertyOwnerShipID)
        {

         decimal OwnerShipPercentage = 0;
         int EstateID = -1;
         int OwnerID = -1;

            if (clsPropertyOwnerShipsDataAccesslayer.FindOwnerShipWithID(ref PropertyOwnerShipID,ref OwnerShipPercentage, ref EstateID, ref OwnerID))
            {
                return new clsPropertyOwnerShips(PropertyOwnerShipID, OwnerShipPercentage,EstateID,OwnerID);
            }

            return null;    


        }
        


        // get Estate Owners With Persentage of = amount

        public static DataTable getOwnersWithOwiningPercentage(decimal amount)
        {
                return clsPropertyOwnerShipsDataAccesslayer.getOwnersWithPercentage(amount);

        }
        
        public static DataTable getAllOwnersWitOwningPersentageBiggerThan(decimal amount)
        {
            return clsPropertyOwnerShipsDataAccesslayer.getOwnersWithPercentageBiggerThan(amount);
        }

        public static DataTable getAllOwnersWitOwningPersentageLessThan(decimal amount)
        {
            return clsPropertyOwnerShipsDataAccesslayer.getOwnersWithPercentageLessThan(amount);
        }



        public static DataTable getOwnersOfThisEstate(int EstateID)
        {
            return clsPropertyOwnerShipsDataAccesslayer.getAllOwnersOfEstateWith(EstateID);
        }

        public static DataTable getEstatesWithThisOwner(int OwnerID)
        {
            return clsPropertyOwnerShipsDataAccesslayer.getAllEstatesOfOwnerWith(OwnerID);
        }





        // add update delete 

        private bool _AddNewPropertyOwnerShip()
        {
            int PropID = -1;

            PropID = clsPropertyOwnerShipsDataAccesslayer.AddNewOwnerShip(this.OwnerShipPercentage,this.EstateID,this.OwnerID);

            if (PropID == -1)
            {
                return false;
            }

            return true;
        }

        private bool _UpdatePropertyOwnerShip()
        {

            return clsPropertyOwnerShipsDataAccesslayer.UpdateOwnerShip(this.PropertyOwnerShipID, this.OwnerShipPercentage,this.EstateID,this.OwnerID);

        }

        public bool DeleteOwnerShip(List<int> ids)
        {

            if (ids == null || ids.Count == 0) return false;

            deletetype = ids.Count > 1 ? enDeletetype.deleteGroup : enDeletetype.deleteSingle;


            return _DeleteEstateOwnerShipFormDB(ids, deletetype);
        }

        private bool _DeleteEstateOwnerShipFormDB(List<int> ids, enDeletetype deletetype)
        {


            switch (deletetype)
            {
                case enDeletetype.deleteGroup:
                    return clsPropertyOwnerShipsDataAccesslayer.DeleteMultiOwnerShipsFromDB(ids);

                case enDeletetype.deleteSingle:
                    return clsPropertyOwnerShipsDataAccesslayer.DeleteSingleOwnerShipFromDB(ids[0]);
            }

            return false;
        }



        // save 

        public bool Save() {

            switch (Mode)
            {
                case enMode.add:
                    if (_AddNewPropertyOwnerShip())
                    {
                        Mode = enMode.update;
                        return true;
                    }
                    return false;
                case enMode.update: return _UpdatePropertyOwnerShip();
            }

            return false;

        }

   

       public bool IfExsist()
        {
            throw new NotImplementedException();
        }
    }
}
