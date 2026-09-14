using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
    public class clsOwner : clsPerson
    {


        private int _OwnerID;
        private short _EstatesOwned;
        private short _BuidlingsOwned;

        public int OwnerID { get {return _OwnerID; } set {_OwnerID = value; } }
        public short EstatesOwned { get {return _EstatesOwned; } set {_EstatesOwned = value; } }
        public short BuidlingsOwned { get {return _BuidlingsOwned; } set {_BuidlingsOwned = value; } }


        public clsOwner() : base(){ 
            OwnerID =-1;
            EstatesOwned = 0; 
            BuidlingsOwned = 0;
            Mode = enMode.add;
        }
        


        private clsOwner(int OwnerID, short EstatesOwned, short BuidlingsOwned,
                   int PersonID, string FirstName, string LastName,
                   string Phone, string FatherName, string MotherName,
                   long NationalNumber, DateTime BirthDate, string BirthPlace,
                   string CivilRegistry, string KaidInfo, clsAddress AddressInfo) :
                   base(PersonID, FirstName, LastName, Phone, FatherName, MotherName,
                        NationalNumber, BirthDate, BirthPlace, CivilRegistry, KaidInfo, AddressInfo)
        {
            this.OwnerID = OwnerID;
            this.EstatesOwned = EstatesOwned;
            this.BuidlingsOwned = BuidlingsOwned;
            Mode = enMode.update;
        }


        // for specific Owner

        public static clsOwner Find(int OwnerID)
        {

           short EstatesOwned = 0;
            int AddressID = -1;
           short BuidlingsOwned = 0;
            string FirstName = "", LastName = "", Phone = "", FatherName = "", MotherName = "", CivilRegistry = "",
                KaidInfo = "", City = "", Region = "", Street = "", BirthPlace = "";
            int PersonID = -1;
            long NationalNumber = 0;
            DateTime BirthDate = DateTime.Now;

            clsAddress address = new clsAddress();

            bool res = clsOwnerDataAccessLayer.FindOwnerByID(ref OwnerID, ref PersonID,  ref AddressID, ref FirstName, ref LastName,
                 ref Phone, ref MotherName, ref FatherName, ref NationalNumber, ref EstatesOwned, ref BuidlingsOwned,
                 ref BirthDate, ref BirthPlace, ref CivilRegistry, ref KaidInfo, ref City, ref Region, ref Street);

            address.AddressID = AddressID;
            address.City = City;
            address.Region = Region;
            address.Street = Street;

            if (res)
            {
                return new clsOwner(OwnerID, EstatesOwned, BuidlingsOwned, PersonID, FirstName,
                                    LastName, Phone, MotherName, FatherName, NationalNumber,
                                     BirthDate, BirthPlace, CivilRegistry, KaidInfo, address);
            }

            return null;

        }
        static clsOwner FindByNationalNumber(long NationalNumber)
        {

            short EstatesOwned = 0;
            short BuidlingsOwned = 0;
            string FirstName = "", LastName = "", Phone = "", FatherName = "", MotherName = "", CivilRegistry = "",
                KaidInfo = "", City = "", Region = "", Street = "", BirthPlace = "";
            int PersonID = -1;
            int OwnerID = -1;
            DateTime BirthDate = DateTime.Now;

            clsAddress address = new clsAddress();

            bool res = clsOwnerDataAccessLayer.FindByNationalNumber(ref OwnerID, ref PersonID, ref FirstName, ref LastName,
                 ref Phone, ref MotherName, ref FatherName, ref NationalNumber, ref EstatesOwned, ref BuidlingsOwned,
                 ref BirthDate, ref BirthPlace, ref CivilRegistry, ref KaidInfo, ref City, ref Region, ref Street);

            address.City = City;
            address.Region = Region;
            address.Street = Street;

            if (res)
            {
                return new clsOwner(OwnerID, EstatesOwned, BuidlingsOwned, PersonID, FirstName,
                                    LastName, Phone, MotherName, FatherName, NationalNumber,
                                     BirthDate, BirthPlace, CivilRegistry, KaidInfo, address);
            }

            return null;

        }

        static public string getOwnerCivilRegistry(int OnwerID)
        {
            clsOwner owner = Find(OnwerID);

            if (owner != null)
            {
                return owner.CivilRegistry;
            }
            return string.Empty;
        }

        static public string getOwnerKaidInfo(int OnwerID)
        {
            clsOwner owner = Find(OnwerID);

            if (owner != null)
            {
                return owner.KaidInfo;
            }
            return string.Empty;
        }


        // get specific owner number of estates and buildings
        static public short getNumOfEstatesOwnedBy(int OnwerID)
        {
            clsOwner owner = Find(OnwerID);

            if (owner != null)
            {
                return owner.EstatesOwned;
            }
            return 0;
        }
        static public short getNumOftBuildingsOwnedBy(int OnwerID)
        {
            clsOwner owner = Find(OnwerID);

            if (owner != null)
            {
                return owner.BuidlingsOwned;
            }
            return 0;
        }




        static public DataTable SearchOwners(string searchText)
        {
            return clsOwnerDataAccessLayer.SearchOwnersinDB(searchText);
        }



        // TO DO : get information about buildings + estates first last father mother phone address 




        // for all owners 

        /*
         * in OwnerShip classes do this , write the classes first
         * you have two classes to write 
         *  first : PropertyOwnerShip
         *  second : BuildingOwnerShip
                   select * from BuildingOwnerShip
                   select * from BuildingOwnerShip
                  // 1_ find owners of building
                 // 2_ find owner of Estates 

         */



        static public DataTable getAllOwners()
        {
            return clsOwnerDataAccessLayer.getAllOwnersFromDB();
        }
        static public DataTable getOwnersAddresses()
        {
            return clsOwnerDataAccessLayer.getAllOwnersAddresss();
        }


        //static public DataTable getOwnersWithEstatesOwnerShip()
        //{
        //  do it when you create the Estate Class
        //}

        //static public DataTable getOwnersWithBuildingsOwnerShip()
        //{
        //  do it when you create the Building Class
        //}


        static public DataTable getOwnersWitnEstateOwnedNumber(int numOfEstatesOwned, enNum_Range range)
        {
            return clsOwnerDataAccessLayer.getAllOwnersWithEstatesOwned(numOfEstatesOwned, (short)range);
        }
        static public DataTable getOwnersWithBuildingsOwnedNumber(int numOfBuildings, enNum_Range range)
        {
            return clsOwnerDataAccessLayer.getAllOwnersWithBuildingsOwned(numOfBuildings, (short)range);
        }
        static public DataTable getOwnersWithSameKaidInfo(string kaidinfo)
        {
            return clsOwnerDataAccessLayer.getOwnersWithKaidInfo(kaidinfo);
        }
        static public DataTable getOwnersWithSameCivilRegistry(string CivilRegistry)
        {
            return clsOwnerDataAccessLayer.getOwnersWithCivilRegistry(CivilRegistry);
        }






        // add delete update owners from data base
        private bool _AddNewOwner()
        {
            int AddressID = -1;
            int PersonID = -1;
            int OwnerID = -1;


            if(this.AddressInfo != null)
            {
                this.AddressInfo.Save();
            }


            AddressID = this.AddressInfo != null ? this.AddressInfo.AddressID : -1;



            if (AddressID != -1)
            {

                // i will be sending its value from presentation layer ,if i want to add an exsistent person as an owner
                // add exsisted person as owner  , same for customer
                // returns the personId of the user u clicked on and assign it to this.PersonID

                if (this.PersonID == -1)
                {

                    PersonID = clsPersonDataAccessLayer.AddnewPerson(this.FirstName, this.LastName,
                             this.Phone, this.MotherName, this.FatherName,
                              this.NationalNumber, this.BirthDate,
                              this.BirthPlace, this.CivilRegistry, this.KaidInfo
                            , AddressID);
                }
                else
                {
                    PersonID = this.PersonID;
                }
            }
            else {
                return false; 
            }


            if (PersonID != -1) {

                OwnerID = clsOwnerDataAccessLayer.AddNewOwnerToTable(this.EstatesOwned,this.BuidlingsOwned,PersonID);
            
            }
            else {
                return false; 
            } 

            if(OwnerID != -1) {return true; }

            return false;
        }


        //update

        private bool _UpdateOwner()
        {

            bool addressUpdated =
              clsPersonDataAccessLayer.UpdateAddressData(
                  this.addressInfo.AddressID,
                  this.addressInfo.City,
                  this.addressInfo.Region,
                  this.addressInfo.Street);

            if (!addressUpdated)
                return false;


            bool personUpdated =
                clsPersonDataAccessLayer.UpdatePersonData(
                    this.PersonID,
                    this.FirstName,
                    this.LastName,
                    this.Phone,
                    this.FatherName,
                    this.MotherName,
                    this.NationalNumber,
                    this.BirthDate,
                    this.BirthPlace,
                    this.CivilRegistry,
                    this.KaidInfo,
                    this.addressInfo.AddressID);

            if (!personUpdated)
                return false;


            bool ownerUpdated =
                clsOwnerDataAccessLayer.UpdateOwnerInfo(
                    this.OwnerID,
                    this.EstatesOwned,
                    this.BuidlingsOwned);

            if (!ownerUpdated)
                return false;


            return true;
        }



        //delete , has two modes => single and group and depending on them we will call the rigth method




        public static bool DeleteMarkedOwners(List<int> ids)
        {
            if(ids == null || ids.Count == 0) return false;

                return _DeleteOwners(ids);

        }


        private static bool _DeleteOwners(List<int>ids)
        {
           
          return clsOwnerDataAccessLayer.DeleteGroupOfOwners(ids);

        }


        /*
         We changed your inheritance from method hiding to real polymorphic overriding.

          Before: clsPerson had normal methods (Save, IfExsist), and child classes wrote methods with same names.
          Result: child methods were hidden, not truly overridden.

          After: in clsPerson, we made methods virtual.
          In clsCustomer and clsOwner, we used override.

       */

        public override bool IfExsist()
        {
            return clsOwnerDataAccessLayer.DoesOwnerExsist(this.OwnerID);
        }

        public override bool Save()
        {
            switch (Mode)
            {
                case enMode.add:
                    if (_AddNewOwner())
                    {
                        Mode = enMode.update;
                        return true;

                    }
                    return false;

                case enMode.update:
                    if (_UpdateOwner())
                    {
                        return true;
                    }
                    return false;
            }

            return false;
        }






    }
}
