using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;

namespace MyEstates_BusinessLayer
{
    public class clsCustomer : clsPerson 
    {


        

        // in presentation layer => choose your preferences => add them , in Sql bitwise them while searching

        public enCustomerType customerType;

        public enCustomerPreferences customerPerferences;

        private int _CustomerID;
        private byte _CustomerType;
        private short _Preferences;
        private bool _Loyality;
        private decimal _OfferedMouny;
        private List<int> _CustomerIDs;


        public int CustomerID { get { return _CustomerID; } set {_CustomerID = value; } }
        public byte CustomerType { get {return _CustomerType; } set {_CustomerType = value; } } // send from presentation layer due to enCustomerType
        public short Preferences { get {return _Preferences; } set {_Preferences = value; } } // bitwise operator with enCustomerPreferences 
        public bool Loyality { get {return _Loyality; } set {_Loyality = value; } }
        public decimal OfferedMouny { get {return _OfferedMouny; } set {_OfferedMouny = value; } }
        
        // list used to determain which customer to delete
        public List<int> CustomerIDs { get {return _CustomerIDs; } set {_CustomerIDs = value; } }



        public clsCustomer() : base() {

            CustomerID = -1;
            CustomerType = 0;
            Preferences = -1;
            Loyality = false;
            OfferedMouny = 0;
            Mode = enMode.add;
        }

        private clsCustomer(int CustomerID,byte CustomerType ,short Preferences,bool Loyality , decimal OfferedMouny,
            int PersonID, string FirstName, string LastName,
                string Phone, string FatherName, string MotherName,
                long NationalNumber, DateTime BirthDate, string BirthPlace,
                string CivilRegistry, string KaidInfo, clsAddress AddressInfo) : 
                base (PersonID, FirstName, LastName,  Phone , FatherName, MotherName,
                     NationalNumber, BirthDate, BirthPlace, CivilRegistry, KaidInfo, AddressInfo)
        {
            this.CustomerID = CustomerID;
            this.CustomerType = CustomerType;
            this.Preferences = Preferences;
            this.Loyality = Loyality;
            this.OfferedMouny = OfferedMouny;
            Mode = enMode.update;
        }
            


        // methods For single customer

        static public clsCustomer Find(int CustomerID)
        {
            string FirstName = "", LastName = "", Phone = "", FatherName="", MotherName = "", CivilRegistry = "",
                KaidInfo = "", City = "", Region = "", Street = "" , BirthPlace="";
            int PersonID = -1;
            long NationalNumber = 0;
            byte CustomerType = 0;
            short Preferences = -1;
            bool Loyality = false; 
            decimal OfferedMouny = 0;
            DateTime BirthDate = DateTime.Now;
            int AddressID = -1;
            
            clsAddress address = new clsAddress();

            short res = clsCustomerDataAccessLayer.FindCustomerByID(ref CustomerID,ref PersonID, ref AddressID, ref FirstName, ref  LastName,
                ref  Phone, ref  MotherName, ref  FatherName, ref  NationalNumber,
                ref  CustomerType, ref  Preferences, ref  Loyality, ref  OfferedMouny,
                ref  BirthDate, ref  BirthPlace, ref  CivilRegistry, ref KaidInfo,
                ref  City, ref  Region, ref  Street);

            address.AddressID = AddressID;
            address.City = City;
            address.Region = Region;
            address.Street = Street;

            if (res!= -1)
            {
                return new clsCustomer(CustomerID, CustomerType, Preferences, Loyality, OfferedMouny
                   , PersonID, FirstName, LastName, Phone, FatherName, MotherName, NationalNumber
                   , BirthDate, BirthPlace, CivilRegistry, KaidInfo, address);
            }

            return null;
            

        }

        static public clsCustomer FindByName(string FirstName,string LastName,string MotherName)
        {
            string Phone = "", FatherName = "" , CivilRegistry = "",
               KaidInfo = "", City = "", Region = "", Street = "", BirthPlace = "";
            int PersonID = -1;
            int CustomerID = -1;
            long NationalNumber = 0;
            byte CustomerType = 0;
            short Preferences = -1;
            bool Loyality = false;
            decimal OfferedMouny = 0;
            DateTime BirthDate = DateTime.Now;

            clsAddress address = new clsAddress();

            short res = clsCustomerDataAccessLayer.FindByName(ref CustomerID, ref PersonID, ref FirstName, ref LastName,
                ref Phone, ref MotherName, ref FatherName, ref NationalNumber,
                ref CustomerType, ref Preferences, ref Loyality, ref OfferedMouny,
                ref BirthDate, ref BirthPlace, ref CivilRegistry, ref KaidInfo,
                ref City, ref Region, ref Street);

            address.City = City;
            address.Region = Region;
            address.Street = Street;

            if (res != -1)
            {
                return new clsCustomer(CustomerID, CustomerType, Preferences, Loyality, OfferedMouny
                   , PersonID, FirstName, LastName, Phone, FatherName, MotherName, NationalNumber
                   , BirthDate, BirthPlace, CivilRegistry, KaidInfo, address);
            }

            return null;
        }

        static public clsCustomer FindByNationalNumber(long NationalNumber)
        {
            string Phone = "", FatherName = "", CivilRegistry = "", FirstName = "", LastName="", MotherName ="",
               KaidInfo = "", City = "", Region = "", Street = "", BirthPlace = "";
            int PersonID = -1;
            int CustomerID = -1;
            byte CustomerType = 0;
            short Preferences = -1;
            bool Loyality = false;
            decimal OfferedMouny = 0;
            DateTime BirthDate = DateTime.Now;

            clsAddress address = new clsAddress();

            short res = clsCustomerDataAccessLayer.FindByNationalNumber(ref CustomerID, ref PersonID, ref FirstName, ref LastName,
                ref Phone, ref MotherName, ref FatherName, ref NationalNumber,
                ref CustomerType, ref Preferences, ref Loyality, ref OfferedMouny,
                ref BirthDate, ref BirthPlace, ref CivilRegistry, ref KaidInfo,
                ref City, ref Region, ref Street);

            address.City = City;
            address.Region = Region;
            address.Street = Street;

            if (res != -1)
            {
                return new clsCustomer(CustomerID, CustomerType, Preferences, Loyality, OfferedMouny
                   , PersonID, FirstName, LastName, Phone, FatherName, MotherName, NationalNumber
                   , BirthDate, BirthPlace, CivilRegistry, KaidInfo, address);
            }

            return null;

        }

        public clsAddress getAddressForCustomerWith(int CustomerID)
        {
            clsCustomer customer = Find(CustomerID);

            if (customer != null)
            {
                return customer.AddressInfo;
            }

            return null;
        }

        public byte getTypeForCustomerWith(int CustomerID)
        {
            clsCustomer customer = Find(CustomerID);

            if (customer != null)
            {
                return customer.CustomerType;
            }

            return 0;
        }

        public short getPerferencesForCustomerWith(int CustomerID)
        {
            clsCustomer customer = Find(CustomerID);

            if (customer != null)
            {
                return customer.Preferences;
            }

            return 0;
        }

        static public string getCustomerCivilRegistry(int CustomerID)
        {
            clsCustomer customer = Find(CustomerID);

            if (customer != null)
            {
                return customer.CivilRegistry;
            }
            return string.Empty;
        }

        static public string getCustomerKaidInfo(int CustomerID)
        {
            clsCustomer customer = Find(CustomerID);

            if (customer != null)
            {
                return customer.KaidInfo;
            }
            return string.Empty;
        }



        public  static DataTable searchCustomers(string searchText)
        {
            return clsCustomerDataAccessLayer.SearchCustomers(searchText);
        }



        // methods For all customers

        static public DataTable getCustomersWithOfferedMouny(decimal OfferedMouny,enNum_Range range)
        {

            return clsCustomerDataAccessLayer.getCutomersWithOfferedMouny(OfferedMouny,(short)range);
        }


        private static string GetCustomerPreferences(short preferences)
        {
            if (preferences == 0)
                return enCustomerPreferences.None.ToString();

            List<string> selected = new List<string>();

            foreach (enCustomerPreferences preference
                     in Enum.GetValues(typeof(enCustomerPreferences))) // typeof(enCustomerPreferences) tells C# which enum you want to get the values from.
            {
                if (preference == enCustomerPreferences.None)
                    continue;

                if ((preferences & (short)preference) != 0)
                {
                    selected.Add(preference.ToString());
                }
            }

            return string.Join(", ", selected);
        }



        static public DataTable getAllCustomers()
        {
            DataTable dt =  clsCustomerDataAccessLayer.getAllAboutCustomers();

            if (dt.Columns.Contains("CustomerType"))
            {
                DataColumn newColumn = new DataColumn("CustomerTypeName", typeof(string));
                DataColumn newColumn2 = new DataColumn("PreferencesName", typeof(string));


                dt.Columns.Add(newColumn);
                dt.Columns.Add(newColumn2);
                newColumn.SetOrdinal(3);
                newColumn2.SetOrdinal(4);


                foreach (DataRow row in dt.Rows)
                {
                    byte value = Convert.ToByte(row["CustomerType"]);

                    if (Enum.IsDefined(typeof(enCustomerType), (int) value)) // is value in  enCustomerType
                    {
                        row["CustomerTypeName"] = ((enCustomerType)value).ToString();
                    }
                    else
                    {
                        row["CustomerTypeName"] = "Unknown"; 
                    }

                    short preferences = Convert.ToInt16(row["Preferences"]);

                    row["PreferencesName"] = GetCustomerPreferences(preferences);
                }
            }

            return dt;
        }

        static public DataTable getLoyalCustomers()
        {
            return clsCustomerDataAccessLayer.getLoyalCustomers();
        }

        static public DataTable getUnLoyalCustomers()
        {
            return clsCustomerDataAccessLayer.getUnLoyalCustomers();
        }

        static public DataTable getCutomersWithCustomerType(byte CustomerType)
        {
            return clsCustomerDataAccessLayer.getCustomersWithType(CustomerType);
        }

        static public DataTable getCutomersWithPreferences(short Preferences)
        {
            return clsCustomerDataAccessLayer.getCutomersWithThisPreferences(Preferences);
        }

        static public DataTable getCustomersAddresses() { 
            return clsCustomerDataAccessLayer.getCustomersAddresses();  
        }

        //return customers with CivilRegistry ,  kaidInfo

        // these are not the best way to search but i offered them maybe some how we used them 
        static public DataTable getCustomersWithSameKaidInfo(string kaidInfo) {
            return clsCustomerDataAccessLayer.getCustomersWithKaidInfo(kaidInfo);
        }
        static public DataTable getCustomersWithSameCivilRegistry(string CivilRegistry) {
            return clsCustomerDataAccessLayer.getCustomersWithCivilRegistry(CivilRegistry);
        }



        // add delete update operations

        private bool _AddNewCustomer()
        {
            int PersonID = -1;
            int AddressID = -1;
            int CustomerID = -1;

            if (this.AddressInfo != null)
            {
                this.AddressInfo.Save();
            }

            AddressID = this.AddressInfo != null ? this.AddressInfo.AddressID : -1;


            // to add an exsistent person as a customer ,means i dont want to save it in db
            //  pass the value of its id to this.PersonID


            if (AddressID != -1)
            {
                // i will be sending its value from presentation layer ,if i want to add an exsistent person as a customer
                // add exsisted person as customer  , same for owner
                // returns the personId of the user u clicked on and assign it to this.PersonID

                if (this.PersonID == -1)
                {
                   

                    PersonID = clsPersonDataAccessLayer.AddnewPerson(this.FirstName, this.LastName,
                         this.Phone, this.MotherName, this.FatherName,
                          this.NationalNumber, this.BirthDate,
                          this.BirthPlace, this.CivilRegistry, this.KaidInfo
                        , AddressID);
                }else
                {
                    PersonID = this.PersonID;
                }

            }
            else { 
                return false; 
            }


            if (PersonID != -1)
            {
                CustomerID =  clsCustomerDataAccessLayer.AddNewCustomerToTable(this.CustomerType, this.Preferences,
                    this.Loyality, this.OfferedMouny, PersonID);
            }

            if (CustomerID != -1) { 
                
                return true; 
            }


            return false;

        }

        private bool _UpdateCustomer()
        {
            //  check if exsist before calling update and no need to use Find  , if exsist is better , ligth-weight and fast
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


            bool CustomerUpdate =
               clsCustomerDataAccessLayer.UpdateCustomerInfo(this.CustomerID, this.CustomerType,
                  this.Preferences, this.Loyality, this.OfferedMouny, this.PersonID);

            if (!CustomerUpdate)
                return false;


            return true;
     
        }



        public static  bool DeleteMarkedCustomers(List<int> customersIDs)
        {
            if (customersIDs == null || customersIDs.Count == 0) return false;

            return _DeleteCustomers(customersIDs);

        }

        private static bool _DeleteCustomers(List<int> ids)
        {
   
              return  clsCustomerDataAccessLayer.DeleteMarkedCustomersFromDataBase(ids);
  
        }

        /*
         We changed your inheritance from method hiding to real polymorphic overriding.

            Before: clsPerson had normal methods (Save, IfExsist), and child classes wrote methods with same names.
            Result: child methods were hidden, not truly overridden.

            After: in clsPerson, we made methods virtual.
            In clsCustomer and clsOwner, we used override.
         
         */


        // IF EXSIST Method

        public override bool IfExsist()
        {
            return clsCustomerDataAccessLayer.DoesCustomerExsist(this.CustomerID);
        }




        // this method will be called in the presentation layer
        public override bool Save()
        {
       
            switch (Mode)
            {
                case enMode.add:
                    if (_AddNewCustomer())
                    {
                        Mode = enMode.update;
                        return true;
                    }
                    return false;

                case enMode.update:
                    if (_UpdateCustomer())
                    {
                        return true;
                    }
                    return false; 
            }

            return false;

        }

    }
}
