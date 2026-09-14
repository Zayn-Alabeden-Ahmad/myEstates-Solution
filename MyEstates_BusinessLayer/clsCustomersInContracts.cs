using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
    public class clsCustomersInContracts : SaveData
    {


        private int _CustomersInContractID;
        private int _CustomerID;
        private int _ContractID;

        public int CustomersInContractID { get { return _CustomersInContractID; } set { _CustomersInContractID = value; } }
        public int CustomerID { get { return _CustomerID; } set { _CustomerID = value; } }
        public int ContractID { get { return _ContractID; } set { _ContractID = value; } }

        private enMode Mode;
        private enDeletetype deletetype;

        public clsCustomersInContracts() {

            CustomersInContractID = -1;
            CustomerID = -1;
            ContractID = -1;
            Mode = enMode.add;
        
        }

        public clsCustomersInContracts(int CustomersInContractID,int CustomerID,int ContractID)
        {

            this.CustomersInContractID = CustomersInContractID;
            this.CustomerID = CustomerID;
            this.ContractID = ContractID;
            Mode = enMode.update;

        }

        public static clsCustomersInContracts Find(int CustomersInContractID)
        {
            int CustomerID = -1;
            int ContractID = -1;

            int res = clsCustomersInContractsDataAccessLayer.Find_CinC_Relation(ref CustomersInContractID,ref CustomerID ,ref ContractID);

            if(res != -1)
            {
                return new clsCustomersInContracts(CustomersInContractID,CustomerID, ContractID); 
            }

            return null;


        }


        // add delete update 

        private bool _AddNewCustomersInContract()
        {
            int ID = -1;

            ID = clsCustomersInContractsDataAccessLayer.AddCustomersToContract(this.CustomerID,this.ContractID);

            if(ID != - 1)
            {
                return true;
            }
            return false;

        }


        private bool _UpdateCustomersInContract()
        {
            return clsCustomersInContractsDataAccessLayer.UpdateCustomersToContractData(this.CustomersInContractID, this.CustomerID,this.ContractID);
        }



        public bool DeleteCustomersInContract(List<int> ids)
        {
            if (ids == null || ids.Count == 0) return false;

            deletetype = ids.Count > 1 ? enDeletetype.deleteSingle : enDeletetype.deleteGroup;

            return _DeleteCustomersInContract(ids,deletetype);

        }
        private bool _DeleteCustomersInContract(List<int>ids,enDeletetype deleteGroup)
        {
            switch (deletetype) { 
                    
                case enDeletetype.deleteSingle:
                    return clsCustomersInContractsDataAccessLayer.DeleteSingleCustomerContract(ids[0]);
                case enDeletetype.deleteGroup:
                    return clsCustomersInContractsDataAccessLayer.DeleteMultiCustomerContracts(ids);
            }

            return false;
        }



        public bool IfExsist()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.add:
                    if (_AddNewCustomersInContract())
                    {
                        Mode = enMode.update;
                        return true;

                    }
                    return false;

                case enMode.update:
                    return _UpdateCustomersInContract();
            }
            return false;
        }
    }
}
