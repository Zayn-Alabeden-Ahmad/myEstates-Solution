using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
    public class clsBuildingInContract
    {

        private int _BuildingsInContractID;
        private int _BuildingID;
        private int _ContractID;

        public int BuildingsInContractID { get { return _BuildingsInContractID; } set { _BuildingsInContractID = value; } }
        public int BuildingID { get { return _BuildingID; } set { _BuildingID = value; } }
        public int ContractID { get { return _ContractID; } set { _ContractID = value; } }

        private enMode Mode;
        private enDeletetype deletetype;

        public clsBuildingInContract()
        {

            BuildingsInContractID = -1;
            BuildingID = -1;
            ContractID = -1;
            Mode = enMode.add;

        }

        public clsBuildingInContract(int BuildingsInContractID, int BuildingID, int ContractID)
        {

            this.BuildingsInContractID = BuildingsInContractID;
            this.BuildingID = BuildingID;
            this.ContractID = ContractID;
            Mode = enMode.update;

        }

        public static clsBuildingInContract Find(int BuildingsInContractID)
        {
            int BuildingID = -1;
            int ContractID = -1;

            int res = clsBuildingsInContractsDataAccessLayer.Find_BinC_Relation(ref BuildingsInContractID, ref BuildingID, ref ContractID);

            if (res != -1)
            {
                return new clsBuildingInContract(BuildingsInContractID, BuildingID, ContractID);
            }

            return null;


        }
        

        // add delete update 

        private bool _AddNewBuildlingInContract()
        {
            int ID = -1;

            ID = clsBuildingsInContractsDataAccessLayer.AddBuildingToContract(this.BuildingID, this.ContractID);

            if (ID != -1)
            {
                return true;
            }
            return false;

        }
        

        private bool _UpdateBuildingsInContract()
        {
            return clsBuildingsInContractsDataAccessLayer.UpdateBuildingToContractData(this.BuildingsInContractID, this.BuildingID, this.ContractID);
        }



        public bool DeleteBuildingInContract(List<int> ids)
        {
            if (ids == null || ids.Count == 0) return false;

            deletetype = ids.Count > 1 ? enDeletetype.deleteSingle : enDeletetype.deleteGroup;

            return _DeleteBuidlingsInContract(ids, deletetype);

        }
        private bool _DeleteBuidlingsInContract(List<int> ids, enDeletetype deleteGroup)
        {
            switch (deletetype)
            {

                case enDeletetype.deleteSingle:
                    return clsBuildingsInContractsDataAccessLayer.DeleteSingleBuildingContract(ids[0]);
                case enDeletetype.deleteGroup:
                    return clsBuildingsInContractsDataAccessLayer.DeleteMultiBuildingContracts(ids);
            }

            return false;
        }



        public bool IfExsist()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.add:
                    if (_AddNewBuildlingInContract())
                    {
                        Mode = enMode.update;
                        return true;

                    }
                    return false;

                case enMode.update:
                    return _UpdateBuildingsInContract();
            }
            return false;
        }

    }
}
