using IAM_Library.models.reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.models.wallet
{
    public class WalletBalanceGETResponseModel
    {
        public string? result { get; set; }
        public ValueData value { get; set; }


    }

    public class ValueData
    {
        public double walletBalance { get; set; }
    }




    public class J4UPreview
    {
        public int selectedCtr { get; set; }
        public double amtToEncash { get; set; }
    }

    public class TransactionListModel
    {
        public string tranno { get; set; }
        public string refno { get; set; }
        public DateTime tranDate { get; set; }
        public string idNumber { get; set; }
        public string comType { get; set; }
        public int tranType { get; set; }
        public string tranDesc { get; set; }
        public double nIn { get; set; }
        public double nOut { get; set; }
        public double pFee { get; set; }
        public double wtax { get; set; }
        public double netComm { get; set; }

        List<TransactionListModel> walletTransactionList;

        public IEnumerator<TransactionListModel> GetEnumerator()
        {
            foreach (var transaction in walletTransactionList)
                yield return transaction;
        }
    }

    public class AutoGenNumberTransaction
    {
        public string autoNum { get; set; }
    }



    public class WithdrawWalletPOSTModel
    {
        public string refno { get; set; }
        public string idNumber { get; set; }
        public int tranType { get; set;}
        public double encashedAmt { get; set; }
        public string areaCode { get; set; }
    }

    public class SuccessfulWithdrawalResponse
    {
        public string message { get; set; }
        public WithdrawWalletPOSTModel data { get; set; }
    }

    public class EnashmentGetDetailsModel
    {
        public string refno { get; set; }
        public DateTime tranDate { get; set; }
        public string idNumber { get; set; }
        public int tranType { get; set; }
        public string tranDesc { get; set; }
        public decimal amt { get; set; }
        public decimal wtax { get; set; }
        public decimal pFee { get; set; }
        public decimal netAmt { get; set; }
        public int bankID { get; set; }
        public string bank { get; set; }
        public string bankAccount { get; set; }
        public string bankAccountName { get; set; }
        public string areaCode { get; set; }
    }



    public class EncashmentTypeModel
    {
        public int encashmentID { get; set; }
        public string encashmentDesc { get; set; }
        public double minAmt { get;set; }
        public double pFee { get; set; }
        public double wTax { get; set; }

        List<EncashmentTypeModel> encashTypes;

        public IEnumerator<EncashmentTypeModel> GetEnumerator()
        {
            foreach (var encashment in encashTypes)
                yield return encashment;
        }
    }

    public class GetEncashmentDetailModel {
        public string refno { get; set; }
        public DateTime tranDate { get; set; }
        public string idNumber { get; set; }
        public int tranType { get; set; }
        public string tranDesc { get; set; }
        public decimal amt { get; set; }
        public decimal wtax { get; set; }
        public decimal pFee { get; set; }
        public decimal netAmt { get; set; }
        public int bankID { get; set; }
        public string bank { get; set; }
        public string bankAccount { get; set; }
        public string bankAccountName { get; set; }
        public string areaCode { get; set; }

    }

    public class BankAccountDetail
    {
        public string idNumber { get; set; }
        public int bankID { get; set; }
        public string bankName { get; set; }
        public string bankAccountNo { get; set; }
        public string bankAccountName { get; set; }
    }

    public class WalletJ4UModel
    {
        public string refno { get; set; }
        public DateTime tranDate { get; set; }
        public string idNumber { get; set; }
        public string package { get; set; }
        public decimal amt { get; set; }
        public bool isSelected { get; set; }

        List<WalletJ4UModel> walletJ4UList;

        public IEnumerator<WalletJ4UModel> GetEnumerator()
        {
            foreach (var entry in walletJ4UList)
                yield return entry;
        }
    }

    public class WalletJ4uUpdaterepsonseModel
    {
        public string message { get; set; }
        public WalletJ4UModel data { get; set; }
    }

    public class J4UEncashModel
    {
        public string refno { get; set; }
        public string idNumber { get; set; }
    }

    public class J4UWithdrawalResponse
    {
        public string message { get; set; }
        public J4UEncashModel data { get; set; }
    }


    /*
     * 
     * 
     * {
          "refno": "TST10010",
          "idNumber": "00000001",
          "tranType": 202,
          "encashedAmt": 3000,
          "areaCode": "000"
        }
     * 
     * 
     * 
     * 
     * 
     * 
     */


    /*
     * GetWalletBalance 
             {
                "result": null,
                "value": {
                    "walletBalance": 1953606.89
                }
            }
     * 
     */

    /*
     * Load Wallet Commission History
     * {
        "tranno": "        ",
        "refno": "NA      ",
        "tranDate": "2024-05-10T02:10:00",
        "idNumber": "88888888",
        "comType": "ULC",
        "tranType": 100,
        "tranDesc": "COMMISSION",
        "nIn": 977695.48,
        "nOut": 0,
        "pFee": 0.00,
        "wtax": 0.00,
        "netComm": 0
         },
     * 
     *  GenerateAutoNumber
     *  {
            "autoNum": "E0423194"
        }
     * 
     */
}
