using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;

namespace MyEstates_BusinessLayer
{
    public class clsPerson : SaveData
    {
        private int _PersonID;
        private string _FirstName ;
        private string _LastName ;
        private string _Phone ;
        private string _FatherName ;
        private string _MotherName ;
        private long _NationalNumber ;
        private DateTime _BirthDate ;
        private string _BirthPlace ;
        private string _CivilRegistry ;
        private string _KaidInfo ;

        public int PersonID { get {return _PersonID; } set { _PersonID =value; } }
        public string FirstName { get {return _FirstName; } set { _FirstName = value; } }
        public string LastName { get {return _LastName; } set { _LastName = value; } }
        public string Phone { get { return _Phone; } set { _Phone =value; } }
        public string FatherName { get { return _FatherName; } set {_FatherName = value; } }
        public string MotherName { get {return _MotherName ; } set {_MotherName = value; } }
        public long NationalNumber { get {return _NationalNumber; } set {_NationalNumber = value; } }
        public DateTime BirthDate { get {return _BirthDate; } set {_BirthDate =value; } }
        public string BirthPlace { get {return _BirthPlace; } set {_BirthPlace = value; } }
        public string CivilRegistry{ get {return _CivilRegistry; } set {_CivilRegistry = value; } }
        public string KaidInfo{ get {return _KaidInfo; } set {_KaidInfo = value; } }
        public clsAddress addressInfo { get {return AddressInfo; } set { AddressInfo = value; } }

        public enMode Mode = enMode.add;

        public enDeletetype DeleteType = enDeletetype.deleteSingle;

        public  enNum_Range Range = enNum_Range.equal;

        // apply composition
        protected clsAddress AddressInfo {  get; set; }


        public clsPerson()
        {

            PersonID = -1;
            FirstName = string.Empty;
            LastName = string.Empty;
            Phone = string.Empty;
            FatherName = string.Empty;
            MotherName = string.Empty;
            NationalNumber = 0;
            BirthDate = DateTime.Now;
            BirthPlace = string.Empty;
            CivilRegistry = string.Empty;
            KaidInfo = string.Empty;

            this.addressInfo = new clsAddress();
             
            Mode = enMode.add;

        }

        protected clsPerson (int PersonID, string FirstName,string LastName,
                string Phone ,string FatherName,string MotherName ,
                long NationalNumber,DateTime BirthDate, string BirthPlace ,
               string CivilRegistry , string KaidInfo , clsAddress AddressInfo
            )
        {

            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Phone = Phone;
            this.FatherName = FatherName;
            this.MotherName = MotherName;
            this.NationalNumber = NationalNumber;
            this.BirthDate = BirthDate;
            this.BirthPlace = BirthPlace;
            this.CivilRegistry = CivilRegistry;
            this.KaidInfo = KaidInfo;
            this.addressInfo = AddressInfo;
          
            Mode = enMode.update;

        }


        /*
              NO Single Responsbility princible any more
          ❌  adding and updating person is handled in Customer and Owner Classes  ❌
         
           ✔️✔️ Each class has his own properties and we handle them

            i have created this to esaily handle adding Persons as Customers and Owners  ,by managing Ids
         */


        private bool _AddNewPerson()
        {
            int PersonID = -1;
            int AddressID = -1;

            if (this.addressInfo != null)
            {
                this.addressInfo.Save();
            }

            AddressID = this.addressInfo != null ? this.addressInfo.AddressID : -1;


            PersonID = clsPersonDataAccessLayer.AddnewPerson(this.FirstName, this.LastName,
                     this.Phone, this.MotherName, this.FatherName,
                      this.NationalNumber, this.BirthDate,
                      this.BirthPlace, this.CivilRegistry, this.KaidInfo , AddressID);


            if(PersonID == -1) return false;

            this.PersonID = PersonID;

            return true;

        }

        private bool _UpdatePerson()
        {
            return clsPersonDataAccessLayer.UpdatePersonData(this.PersonID, this.FirstName, this.LastName, this.Phone,
                this.MotherName, this.FatherName, this.NationalNumber,this.BirthDate,
                this.BirthPlace, this.CivilRegistry, this.KaidInfo,this.addressInfo.AddressID);
        }


        // delete Person 

        // bear in mind that Person can be customer and owner at the same time 

        /*
          Delete Customer -> delete from Customer only.
          Delete Owner -> delete from Owner only.
          Delete Completely -> call Person delete flow (with confirmation). , unlink customers and owners realted then delete person record
       */



        public bool DeletePersons(List<int> PersonsIDS)
        {
            if(PersonsIDS == null  || PersonsIDS.Count == 0) return false;


            return _DeletePersonsData(PersonsIDS);
        }


        private bool _DeletePersonsData(List<int> PersonsIDS)
        {

            return clsPersonDataAccessLayer.DeletePersonsFromDataBase(PersonsIDS);

        }


        /*
       We changed your inheritance from method hiding to real polymorphic overriding.

          Before: clsPerson had normal methods (Save, IfExsist), and child classes wrote methods with same names.
          Result: child methods were hidden, not truly overridden.

          After: in clsPerson, we made methods virtual.
          In clsCustomer and clsOwner, we used override.

       */

        public virtual bool IfExsist()
        {
            return clsPersonDataAccessLayer.DoesPersonExsist(this.PersonID);
        }



        public virtual bool Save()
        {

            switch (Mode)
            {
                case enMode.add:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.update;
                        return true;
                    }
                    return false;

                case enMode.update:
                    return _UpdatePerson();
            }

            return false;

        }



    }
}
