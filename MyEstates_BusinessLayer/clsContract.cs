using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
	public class clsContract  : SaveData
	{
		private int _ContractID;
		private enContractType _ContractType;
		private string _ContractImg;
		private short _ContractDuration;
		private DateTime _StartDate;
		private DateTime _EndDate;
		private decimal _SettledPrice;
		private decimal _Commission;
		private bool _IsActive;
		private DateTime _Created_At;

		public int ContractID { get { return _ContractID; } set { _ContractID = value; } }
		public enContractType ContractType { get { return _ContractType; } set { _ContractType = value; } }

		public string ContractImg { get { return _ContractImg; } set { _ContractImg = value; } }
		public short ContractDuration
		{
			get { return _ContractDuration; }
			set { _ContractDuration = value; }
		}

		public DateTime StartDate { get { return _StartDate; } set {	_StartDate = value; } }
		public DateTime EndDate { get { return _EndDate;	} set { _EndDate = value; } }
		public decimal SettledPrice { get { return _SettledPrice; } set {  _SettledPrice = value; } }	
		public decimal Commission { get { return _Commission;  } set { _Commission = value; } }	
		
		public bool IsActive { get { return _IsActive;	} set { _IsActive = value; } }
		public DateTime Created_At { get { return _Created_At; } set { _Created_At = value; } }


		public enMode Mode;
		public enDeletetype deleteType;

		public clsContract()
		{
			ContractID = -1;
			ContractType  =enContractType.sell;
			ContractImg = "";
			ContractDuration = 0;
			StartDate = DateTime.MinValue;
			EndDate = DateTime.MinValue;	
			SettledPrice = 0;
			Commission = 0;
			IsActive = false;
			Created_At = DateTime.MinValue;
			Mode = enMode.add;

        }


		private clsContract(int ContractID, enContractType ContractType, string ContractImg,
			short ContractDuration, DateTime StartDate,DateTime EndDate, decimal SettledPrice,
			decimal Commission, bool IsActive, DateTime Created_At)
		{
			this.ContractID = ContractID;	
			this.ContractType = ContractType;
			this.ContractImg = ContractImg;
			this.ContractDuration = ContractDuration;
			this.StartDate = StartDate;
			this.EndDate = EndDate;
			this.Commission = Commission;
			this.SettledPrice = SettledPrice;
			this.IsActive = IsActive;
			this.Created_At = Created_At;
			Mode = enMode.update;
		}

		public static clsContract Find(int ContractID) {

			bool res = false;

			bool ContractType = false; // false then sell , true then rent
            enContractType ENContractType = enContractType.sell;
            string ContractImg = "";
			short ContractDuration = 0;
			DateTime StartDate = DateTime.MinValue;
			DateTime EndDate= DateTime.MinValue;
			decimal SettledPrice = 0;
			decimal Commission = 0;
			bool IsActive = false;
			DateTime Created_At = DateTime.MinValue;

			res = clsContractDataAccessLayer.FindContractWithId(ref ContractID, ref ContractType, ref ContractImg,
                    ref ContractDuration, ref StartDate, ref EndDate, ref SettledPrice
                    , ref Commission, ref IsActive, ref Created_At);

			if (res)
			{
                ENContractType  = ContractType  ? enContractType.rent : enContractType.sell;


                return new clsContract( ContractID, ENContractType,  ContractImg,
					 ContractDuration,  StartDate,  EndDate,  SettledPrice
					,  Commission,  IsActive,  Created_At);
			}

			return null;
		}


        // contract methods 

        // get all contracts with type :
			
		public static DataTable getAllContractsWithContracatType(byte contractType)
		{
	
			return clsContractDataAccessLayer.getContractsTypeFromDB(contractType);
		}

        // get contracts with duration equal / bigger / less than :  Duration

		public static DataTable getAllContractsWithDurationEqualTo(short ContractDuration)
		{
			return clsContractDataAccessLayer.getContractsWithDurationEqualTo(ContractDuration);

        }
        public static DataTable getAllContractsWithDurationMoreThan(short ContractDuration)
        {
            return clsContractDataAccessLayer.getContractsWithDurationMoreThan(ContractDuration);
        }
        public static DataTable getAllContractsWithDurationLessThan(short ContractDuration)
        {
            return clsContractDataAccessLayer.getContractsWithDurationLessThan(ContractDuration);
        }


		// get contracts with settled price equal / bigger / less than :  price

		public static DataTable getAllContractsWithSettledPriceEqualTo(decimal SettledPrice)
		{
			return clsContractDataAccessLayer.getContractsWithSettledPriceEqual(SettledPrice);
		}
        public static DataTable getAllContractsWithSettledPriceMoreThan(decimal SettledPrice)
        {
            return clsContractDataAccessLayer.getContractsWithSettledPriceMore(SettledPrice);
        }

        public static DataTable getAllContractsWithSettledPriceLessThan(decimal SettledPrice)
        {
            return clsContractDataAccessLayer.getContractsWithSettledPriceLess(SettledPrice);
        }



        // get contracts with Commission  equal / bigger / less than :  Commission
		public static DataTable getAllContractsWithCommissionEqualTo(decimal Commission)
		{
			return clsContractDataAccessLayer.getContractsWithCommisionEqual(Commission);
		}
        public static DataTable getAllContractsWithCommissionMoreThan(decimal Commission)
        {
			return clsContractDataAccessLayer.getContractsWithCommisionMore(Commission);

        }
        public static DataTable getAllContractsWithCommissionLessThan(decimal Commission)
        {
			return clsContractDataAccessLayer.getContractsWithCommisionLess(Commission);

        }


		public static DataTable getActiveContracts()
		{
			return clsContractDataAccessLayer.getAllActiveContracts();
		}

		public static DataTable getInActiveContracts()
		{
			return clsContractDataAccessLayer.getAllInActiveContracts();
		}


		// get contracts created before / after / at  :  Created_At Date

		public static DataTable getContractsWithCreatedDateEqualTo(DateTime Created_At)
        {
            return clsContractDataAccessLayer.getAllContractsWithCreatedDateEqualTo(Created_At);
        }
        public static DataTable getContractsWithCreatedDateMoreThan(DateTime Created_At)
        {
            return clsContractDataAccessLayer.getAllContractsWithCreatedDateMoreThan(Created_At);
        }
        public static DataTable getContractsWithCreatedDateLessThan(DateTime Created_At)
        {
            return clsContractDataAccessLayer.getAllContractsWithCreatedDateLessThan(Created_At);
        }

        // get contracts with StartDate = date
        public static DataTable getContractsWithStartDate(DateTime Created_At)
        {
            return clsContractDataAccessLayer.getAllContractsWithStartDate(Created_At);
        }


        // get contracts with EndDate = date

        public static DataTable getContractsWithEndDate(DateTime Created_At)
        {
            return clsContractDataAccessLayer.getAllContractsWithEndDate(Created_At);
        }







        // add delete update 


        private bool _AddNewContract()
		{
			int ID = -1;
			bool contractType = this.ContractType == enContractType.sell ? false : true;

            ID = clsContractDataAccessLayer.AddNewContractToDB(contractType, this.ContractImg,
                     this.ContractDuration, this.StartDate, this.EndDate, this.SettledPrice
                    , this.Commission, this.IsActive, this.Created_At);

			if(ID == - 1) return false;

			return true;

        }

		private bool _UpdateContract()
        {
            bool contractType = this.ContractType == enContractType.sell ? false : true;

            return clsContractDataAccessLayer.UpdateContractInDB(this.ContractID, contractType, this.ContractImg,
                     this.ContractDuration, this.StartDate, this.EndDate, this.SettledPrice
                    , this.Commission, this.IsActive, this.Created_At);
		}


        public bool DeleteContract(List<int> ids)
        {

            if (ids == null || ids.Count == 0) return false;

            deleteType = ids.Count > 1 ? enDeletetype.deleteGroup : enDeletetype.deleteSingle;


            return _DeleteContractFormDB(ids, deleteType);
        }

        private bool _DeleteContractFormDB(List<int> ids, enDeletetype deletetype)
        {


            switch (deletetype)
            {
                case enDeletetype.deleteGroup:
                    return clsContractDataAccessLayer.DeleteMultiContractsFromDB(ids);

                case enDeletetype.deleteSingle:
                    return clsContractDataAccessLayer.DeleteSingleContractFromDB(ids[0]);
            }

            return false;
        }

        public bool Save()
		{
			switch (Mode) {
				case enMode.add:
					
					if (_AddNewContract())
					{
						Mode = enMode.update;
						return true;
					}return false;

				case enMode.update:
					return _UpdateContract();
			
			}
			return false;
		}

		public bool IfExsist()
        {
            throw new NotImplementedException();
        }






    }
}
