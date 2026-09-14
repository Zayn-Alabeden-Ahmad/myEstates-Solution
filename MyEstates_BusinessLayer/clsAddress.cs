using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
    public class clsAddress : SaveData
    {

        private int _AddressID;
        private string _City;
        private string _Region;
        private string _Street;
      

        public int AddressID { get {return _AddressID; } set {_AddressID = value; } }
        public string City { get {return _City; } set {_City = value; } }
        public string Region { get {return _Region; } set {_Region = value; } }
        public string Street { get {return _Street; } set {_Street = value; } }

        public enMode Mode = enMode.add;
        public enDeletetype deletetype = enDeletetype.deleteSingle;

        public clsAddress() {
            this.AddressID = -1;
            this.City= string.Empty;
            this.Region= string.Empty;
            this.Street= string.Empty;
            Mode  = enMode.add;
        }
        private clsAddress(int addressID,string city ,string region ,string street)
        {
            this.AddressID = addressID;
            this.City = city;
            this.Region = region;
            this.Street = street;
            Mode = enMode.update;
          
        }


        static public clsAddress Find(int AddressID)
        {

            string City=string.Empty;
            string Region=string.Empty;
            string Street= string.Empty;

    
            if (clsAddressesDataAccessLayer.FindAddress(ref AddressID, ref City, ref Region, ref Street))
            {
                return new clsAddress(AddressID,City,Region,Street);
            }

              return null;
        }


        private bool _AddNewAddress()
        {

            int AddressID = -1;

            AddressID = clsAddressesDataAccessLayer.AddNewAddressToDB(this.City,this.Region, this.Street);

            if(AddressID == -1)return false;

            this.AddressID = AddressID;

            return true;
        } 

        private bool _UpdateAddress()
        {

            return clsAddressesDataAccessLayer.UpdateAddressInDB(this.AddressID, this.City, this.Region, this.Street);
        }



        // Delete Address 
        // AddressID can be null its no problem
        // each address is linked to a building and each bulidng has lots of estates "Of Coures" shares the same address
        // deleteing the address wont delete any thing else

        // Person and Building Has AddressID as FK so it requires deleting the address form their also


        public bool DeleteAddresses(List<int> addressIds)
        {
            if (addressIds == null || addressIds.Count == 0) return false;

            return _DeleteAddressFromDB(addressIds);
        }

        private bool _DeleteAddressFromDB(List<int> addressIds)
        {
           return clsAddressesDataAccessLayer.DeleteAddressesAndUnlinkRefsFromDB(addressIds);
        }


        public bool IfExsist()
        {
            return clsAddressesDataAccessLayer.DoesAddressExsist(this.AddressID);
        }



        public bool Save() {

            switch (Mode)
            {
                case enMode.add:
                    if (_AddNewAddress())
                    {
                        Mode = enMode.update;
                        return true;
                    }
                    return false;

                case enMode.update:
                    if (_UpdateAddress())
                    {
                        return true;
                    }
                    return false;
            }

            return false;
        }

        

    }
}
