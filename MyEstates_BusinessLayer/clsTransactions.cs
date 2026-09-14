using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
    public class clsTransactions : SaveData
    {
        private int _TransactionsID;
        private decimal _Amount;
        private byte _TransactionStatus;
        private byte _TranscationType;
        private DateTime _PaymentDate;
        private int _ContractID;


        public int TransactionsID { get { return _TransactionsID; } set { _TransactionsID = value; } }
        public int ContractID { get { return _ContractID; } set { _ContractID = value; } }
        public decimal Amount { get { return _Amount; } set { _Amount = value; } }
        public byte TransactionStatus { get { return _TransactionStatus; } set { _TransactionStatus = value; } }
        public byte TranscationType { get { return _TranscationType; } set { _TranscationType = value; } }
        public DateTime PaymentDate { get { return _PaymentDate; } set { _PaymentDate = value; } }

        private enMode Mode;
        private enDeletetype deletetype;

        public clsTransactions() {
            TransactionsID = -1;
            Amount=0;
            TransactionStatus = 0;
            TranscationType = 0;
            PaymentDate =DateTime.MinValue;
            ContractID= -1;
            Mode = enMode.add;
        
        }

        private clsTransactions( int TransactionsID, decimal Amount, byte TransactionStatus,
            byte TranscationType,DateTime PaymentDate,int ContractID)
        {
            this.TransactionsID = TransactionsID;
            this.Amount = Amount;
            this.TransactionStatus = TransactionStatus;
            this.TranscationType = TranscationType;   
            this.PaymentDate =PaymentDate;
            this.ContractID = ContractID;

            Mode = enMode.update;

        }

        public static clsTransactions Find(int TransactionsID)
        {
            int id = -1;

            decimal Amount = 0;
            byte TransactionStatus = 0;
            byte TranscationType = 0;
            DateTime PaymentDate =DateTime.MinValue ; 
            int ContractID = -1;

            id = clsTransactionsDataAccesslayer.FindTranscation(ref TransactionsID, ref Amount, ref TransactionStatus
                ,ref TranscationType, ref PaymentDate , ref ContractID);
            
            if(id != -1)
            {
                return new clsTransactions(TransactionsID, Amount, TransactionStatus
                , TranscationType, PaymentDate, ContractID);
            }
            return null;
        }

        // get transactions with payemntDate = 
        // get transactions with TransactionStatus = 
        // get transactions with TranscationType = 
        // get transactions with amount = 
        // get transactions with ContractID = 
        // get transactions with payemntDate and amount  = 
        // get transactions with TranscationType and TransactionStatus and amount =   ??? using named parameters so we dont duplicate functions



        public DataTable getTransactionsWithPaymentDate(DateTime PaymentDate) {

            return clsTransactionsDataAccesslayer.getTransactionWithPaymentDateFromDB(PaymentDate);
        }

        public DataTable getTransactionsWithTransactionStatus(byte TransactionStatus) {

            return clsTransactionsDataAccesslayer.getTransactionsWithTransactionStatusFromDB(TransactionStatus);
        }

        public DataTable getTransactionsWithTranscationType(byte TranscationType) {
            return clsTransactionsDataAccesslayer.getTransactionsWithTranscationTypeFromDB(TranscationType);
        }

        public DataTable getTransactionsWithAmount(decimal Amount) {
            return clsTransactionsDataAccesslayer.getTransactionsWithAmountFromDB(Amount);
        }

        public DataTable getTransactionsWithContractID(int ContractID) {
            return clsTransactionsDataAccesslayer.getTransactionsWithContractIDFromDB(ContractID);
        }

        public DataTable getTransactionsWithPaymentDateAndAmount(DateTime PaymentDate,decimal Amount) {
            return clsTransactionsDataAccesslayer.getTransactionsWithPaymentDateAndAmountFromDB(PaymentDate, Amount);
        }

        public DataTable getTransactionsWithStatusAndType(byte TransactionStatus, byte TranscationType ) { 
            return clsTransactionsDataAccesslayer.getTransactionsStatusAndTypeFromDB(TransactionStatus, TranscationType);
        }


    









        // add delete update 

        private bool _AddnewTransaction()
        {
            int ID = -1;
            ID = clsTransactionsDataAccesslayer.AddNewTransactionToDB(this.Amount, this.TransactionStatus, this.TranscationType,
                this.PaymentDate, this.ContractID);
                if (ID != -1){
                     return true;
                }
            return false;
        }


        private bool _UpdateTransaction()
        {
            return clsTransactionsDataAccesslayer.UpdateTransactionInDB(this.TransactionsID, this.Amount, this.TransactionStatus,
                this.TranscationType, this.PaymentDate, this.ContractID);
        }


        public bool DeleteTransactions(List<int> ids)
        {
            if (ids == null || ids.Count == 0) return false;

            deletetype = ids.Count > 1 ? enDeletetype.deleteSingle : enDeletetype.deleteGroup;

            return _DeleteTranscationFromDB(ids, deletetype);
        }

        private bool _DeleteTranscationFromDB(List<int> ids, enDeletetype deletetype)
        {
            switch (deletetype) {
                case enDeletetype.deleteGroup:
                    return clsTransactionsDataAccesslayer.DeleteMultiTransactions(ids);
                case enDeletetype.deleteSingle:
                    return clsTransactionsDataAccesslayer.DeleteSingleTransactions(ids[0]);
                    
            }
            return false;
        }

        public bool IfExsist()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }
    }
}
