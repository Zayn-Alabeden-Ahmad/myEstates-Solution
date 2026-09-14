using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
    public class clsEstatesInContract : SaveData
    {
        private int _EstatesInContractID;
        private int _EstateID;
        private int _ContractID;

        public int EstatesInContractID { get { return _EstatesInContractID; } set { _EstatesInContractID = value; } }
        public int EstateID { get { return _EstateID; } set { _EstateID = value; } }
        public int ContractID { get { return _ContractID; } set { _ContractID = value; } }

        private enMode Mode;
        private enDeletetype deletetype;

        public clsEstatesInContract()
        {

            EstatesInContractID = -1;
            EstateID = -1;
            ContractID = -1;
            Mode = enMode.add;

        }

        public clsEstatesInContract(int EstatesInContractID, int EstateID, int ContractID)
        {

            this.EstatesInContractID = EstatesInContractID;
            this.EstateID = EstateID;
            this.ContractID = ContractID;
            Mode = enMode.update;

        }

        public static clsEstatesInContract Find(int EstatesInContractID)
        {
            int EstateID = -1;
            int ContractID = -1;

            int res = clsEstateInContractDataAccessLayer.Find_EinC_Relation(ref EstatesInContractID, ref EstateID, ref ContractID);

            if (res != -1)
            {
                return new clsEstatesInContract(EstatesInContractID, EstateID, ContractID);
            }

            return null;


        }
        private bool _AddNewEstateInContract()
        {
            int ID = -1;

            ID = clsEstateInContractDataAccessLayer.AddEstateToContract(this.EstateID, this.ContractID);

            if (ID != -1)
            {
                return true;
            }
            return false;

        }


        private bool _UpdateEstatesInContract()
        {
            return clsEstateInContractDataAccessLayer.UpdateEstateToContractData(this.EstatesInContractID, this.EstateID, this.ContractID);
        }



        public bool DeleteEstateInContract(List<int> ids)
        {
            if (ids == null || ids.Count == 0) return false;

            deletetype = ids.Count > 1 ? enDeletetype.deleteSingle : enDeletetype.deleteGroup;

            return _DeleteEstatesInContract(ids, deletetype);

        }
        private bool _DeleteEstatesInContract(List<int> ids, enDeletetype deleteGroup)
        {
            switch (deletetype)
            {

                case enDeletetype.deleteSingle:
                    return clsEstateInContractDataAccessLayer.DeleteSingleEstateContract(ids[0]);
                case enDeletetype.deleteGroup:
                    return clsEstateInContractDataAccessLayer.DeleteMultiEstateContracts(ids);
            }

            return false;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.add:

                    if (_AddNewEstateInContract())
                    {
                        Mode = enMode.update;
                        return true;
                    }
                    return false;

                case enMode.update:
                    return _UpdateEstatesInContract();

            }
            return false;
        }

        public bool IfExsist()
        {
            throw new NotImplementedException();
        }
    }
}
