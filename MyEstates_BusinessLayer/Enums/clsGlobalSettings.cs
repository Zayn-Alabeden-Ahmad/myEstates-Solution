using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyEstates_BusinessLayer.Enums
{
    public enum enMode { add, update };
    public enum enDeletetype { deleteSingle, deleteGroup };
    public enum enNum_Range { equal = 0, greater = 1, smaller = 2, greaterOrEqual = 3, smallerOrEqual = 4 };

    public enum enFilter  {estateType=1,estateStatus=2,sellingEstate=3}
    public enum enEstate_Type { apartment=1, villa=2, house=3, shalle=4 };
    public enum enSelling_Estate { sold=1, rented=2, available=3 };
    public enum enEstate_Status { skulled = 1, furnished=2 };

    public enum enPeriod_filter {before = 1, after = 2, with = 3}; // with means identical to
    public enum enCustomerType { buyer, seller, renter };
    public enum enCustomerPreferences { None = 0, Elevator = 1, LowFloor = 2, Garage = 4, Garden = 8, Furnished = 16, SeaView = 32, NearUniversity = 64 };

    public enum enContractType { sell = 0, rent =1};


    public class clsGlobalSettings
    {
    }
}
