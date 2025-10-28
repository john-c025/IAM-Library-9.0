using IAM_Library.models.auth;
using IAM_Library.models.general;
using IAM_Library.models.dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections;

namespace IAM_Library.models.reports
{
    //implement in-memory caching using akavance or monkey-cache, benchmark firswt 
    internal class ReportsModel
    {
        public ApiResponseModel<object> apiResponse { get; set; }
        public ReportsDataFull reportsDataFull { get; set; }
    }
    // to remove this and implement individual model with response models
    public class ReportsDataFull
    {
        public ReportsDataCommissionSummary commissionSummary { get; set; }
        //public ReportsDataCommissionHistory commissionHistory { get; set; } //has date params
        public ReportsDataReferralCommission referralCommission { get; set; }
        public ReportsDataDSCCommission dscCommission { get; set; }
        public ReportsDataISCCommission sCCommission { get; set; }
        public ReportsDataJ4UCommission j4UCommission { get; set; } 
        public ReportsDataGM5Commission gm5Commission { get; set; }
        public ReportsDataUnilevelCommission unilevelCommission { get; set; }
        public ReportsDataPairsHistory pairsHistory { get; set; } //has date params



        List<ReportsDataCommissionSummary> summaryList;

        public IEnumerator<ReportsDataCommissionSummary> GetEnumerator()
        {
            foreach (var summary in summaryList)
                yield return summary;
        }

    }
    public class infinityHistory
    {
        public string refno { get; set; }
        public int rollUpNo { get; set; }
        public DateTime tranDate { get; set; }
        public string downLineID { get; set; }
        public string downLineName { get; set; }
        public string package { get; set; }
        public double commission { get; set; }

        List<infinityHistory> summaryList;

        public IEnumerator<infinityHistory> GetEnumerator()
        {
            foreach (var summary in summaryList)
                yield return summary;
        }
    }
    public class rqvSummary
    {
        public string refno { get; set; }
        public int levelNo { get; set; }
        public int rollUpNo { get; set; }
        public DateTime tranDate { get; set; }
        public string downLineID { get; set; }
        public string downLineName { get; set; }
        public string package { get; set; }
        public int rqv_Left { get; set; }
        public int rqv_Right { get; set; }

        List<rqvSummary> summaryList;

        public IEnumerator<rqvSummary> GetEnumerator()
        {
            foreach (var summary in summaryList)
                yield return summary;
        }
    }

    public class moverSummary
    {
        public string refno { get; set; }
        public int levelNo { get; set; }
        public int prankNo { get; set; }
        public DateTime moverDate { get; set; }
        public string downLineID { get; set; }
        public string downLineName { get; set; }
        public string package { get; set; }
        public string side { get; set; }

        List<moverSummary> summaryList;

        public IEnumerator<moverSummary> GetEnumerator()
        {
            foreach (var summary in summaryList)
                yield return summary;
        }

    }

    public class SystemAccessModel
    {
        public bool hasAccess { get; set; }
        
    }
    public class CommissionCtrModel
    {
        public int? dscCtr { get; set; } = 0;
        public string just4uCtr { get; set; }
        public string giveMe5Ctr { get; set; }
    }
    
    public class ReportsDataCommissionCoverage
    {
        public string dfrom { get; set; } //get as string then convert to date
        public string dto { get; set; } //get as string then convert to date

        List<ReportsDataCommissionCoverage> commissionCoverageList;

        public IEnumerator<ReportsDataCommissionCoverage> GetEnumerator()
        {
            foreach (var coverage in commissionCoverageList)
                yield return coverage;
        }
    }




    public class ResidualSalesMatch
    {
        
            public string refno { get; set; }
            public DateTime tranDate { get; set; }
            public int levelNo { get; set; }
            public int rollUpNo { get; set; }
            public string downLineID { get; set; }
            public string downLineName { get; set; }
            public string downlinePckageCode { get; set; }
            public string downlinePckageDesc { get; set; }
            public string prodName { get; set; }
            public double ptValue { get; set; }
            public int qty { get; set; }
            public double netPTValue { get; set; }
            public double begBalLeft { get; set; }
            public double begBalRight { get; set; }
            public double curBalLeft { get; set; }
            public double curBalRight { get; set; }
            public double endBalLeft { get; set; }
            public double endBalRight { get; set; }
            public double pairs { get; set; }
            public double commission { get; set; }

            List<ResidualSalesMatch> salesMatched;


            public IEnumerator<ResidualSalesMatch> GetEnumerator()
            {
                foreach (var summary in salesMatched)
                    yield return summary;
            }


    }

    public class ReportsDataCommissionSummary
    {
        public double dsc { get; set; }
        public double infsc { get; set; }
        public double giveMe5 { get; set; }
        public double just4u { get; set; }
        public double checkMatch { get; set; }
        public double gsc { get; set; }
        public double rpsc { get; set; }
        public double lsc { get; set; }
        public double proj001 { get; set; }
        public  double grossComm { get; set; }
        public double wtax { get; set; }
        public double netComm { get; set; }
        public double ecom { get; set; }

        List<ReportsDataCommissionSummary> commissionSummaryList;


        public IEnumerator<ReportsDataCommissionSummary> GetEnumerator()
        {
            foreach (var summary in commissionSummaryList)
                yield return summary;
        }

    }

    public class accountRank
    {
        public string package { get; set; }
        public string position { get; set; }
        public string rank { get; set; }
        public string title { get; set; }
        public string uniLDRSRank { get; set; }
    }

    public class TotalUnicomModel
    {
        public double totUniLDRSComm { get; set; } = 0.00;
        public double totUniComm { get; set; } = 0.00;


        List < TotalUnicomModel > totalUni;

        public IEnumerator<TotalUnicomModel> GetEnumerator()
        {
            foreach (var summary in totalUni)
                yield return summary;
        }


    }

    public class MainProductPurchaseResponse
    {
        public DateTime tranDate { get; set; }
        public string refno { get; set; }
        public int tranType { get; set; }
        public string tranDesc { get; set; }
        public int salesID { get; set; }
        public string salesDesc { get; set; }
        public List<InclusionPurchaseModel> inclusion { get; set; }
        public double grossAmt { get; set; }
        public double wtax { get; set; }
        public double fee { get; set; }
        public double netAmt { get; set; }


        List<MainProductPurchaseResponse> purchases;


        public IEnumerator<MainProductPurchaseResponse> GetEnumerator()
        {
            foreach (var pr in purchases)
                yield return pr;
        }

    }

    public class InclusionPurchaseModel
    {
        public string prodCode { get; set; }
        public string prodName { get; set; } 
        public int qty { get; set; }
        public double ptValue { get; set; }
        public double rptValue { get; set; }
        public double selPri { get; set; }
        public double totPri { get; set; }

        List<InclusionPurchaseModel> inclusions;


        public IEnumerator<InclusionPurchaseModel> GetEnumerator()
        {
            foreach (var product in inclusions)
                yield return product;
        }
    }





    public class ReportsCommHistory
    {
        public string refno { get; set; }
        public DateTime tranDate { get; set; }
        public string coverage { get; set; }
        public string idNumber { get; set; }
        public double payout { get; set; }
        public string payoutDesc { get; set; }
        public double dsc { get; set; }
        public double infsc { get; set; }
        public double giveMe5 { get; set; }
        public double just4u { get; set; }
        public double checkMatch { get; set; }
        public double gsc { get; set; }
        public double rpsc { get; set; }
        public double lsc { get; set; }
        public double proj001 { get; set; }
        public double ecom { get; set; }
        public double grossComm { get; set; }
        public double wtax { get; set; }
        public double netComm { get; set; }

        List<ReportsCommHistory> reportsCommHistory;


        public IEnumerator<ReportsCommHistory> GetEnumerator()
        {
            foreach (var summary in reportsCommHistory)
                yield return summary;
        }
    }

    public class ReportsCheckMatched
    {
        public DateTime tranDate { get; set; }
        public string refno { get; set; }
        public string downLineID { get; set; }
        public string downLineName { get; set; }
        public double amt { get; set; }
        public double rate { get; set; }
        public double commission { get; set; }

        List<ReportsCheckMatched> reportsCheckedMatched;


        public IEnumerator<ReportsCheckMatched> GetEnumerator()
        {
            foreach (var summary in reportsCheckedMatched)
                yield return summary;
        }
    }



    public class ReportsDataCommissionHistory
    {
        public string refno { get; set; }
        public string tranDate { get; set; } //datetime pero string
        public string coverage { get; set; }
        public string idNumber { get; set; }
        public double payout { get; set; }
        public string payoutDesc { get; set; }
        public double dsc { get; set; }
        public double infsc { get; set; }
        public double giveMe5 { get; set; }
        public double just4u { get; set; }
        public double checkMatch { get; set; }
        public double gsc { get; set; }
        public double rpsc { get; set; }
        public double lsc { get; set; }
        public double proj001 { get; set; }
        public double grossComm { get; set; }
        public double wtax { get; set; }
        public double netComm { get; set; }

        List<ReportsDataCommissionHistory> commissionHistoryList;


        public IEnumerator<ReportsDataCommissionHistory> GetEnumerator()
        {
            foreach (var history in commissionHistoryList)
                yield return history;
        }


        //overriding test
        public override string ToString()
        {
            return tranDate;
        }
        public Dictionary<string, object> GetValuePairs()
        {
            Dictionary<string, object> temp = new Dictionary<string, object>();
            temp.Add("refno", refno);
            temp.Add("tranDate", tranDate);
            temp.Add("coverage", coverage);
            temp.Add("idNumber", idNumber);
            temp.Add("payout", payout);
            temp.Add("payoutDesc", payoutDesc);
            temp.Add("dsc", dsc);
            temp.Add("infsc", infsc);
            temp.Add("giveMe5", giveMe5);
            temp.Add("just4u", just4u);
            temp.Add("checkMatch", checkMatch);
            temp.Add("gsc", gsc);
            temp.Add("rpsc", rpsc);
            temp.Add("lsc", lsc);
            temp.Add("proj001", proj001);
            temp.Add("grossComm", grossComm);
            temp.Add("wtax", wtax);
            temp.Add("netComm", netComm);

            return temp;
        }


        // read on this 
        //https://passos.com.au/converting-json-object-into-c-list/

        /*
         * 
         * public override string ToString()
            {
                string booksInLibrary = "";

                foreach (Book b in Books)
                {
                    booksInLibrary += b + "\n";
                }

                return $"Books: \n {booksInLibrary}" +
                    $"Library Name: {LibraryName}\n" +
                    $"Address: {StreetAddress}, {City}, {State} {Zip}";
            }
         * 
         * 
         * 
         */
    }

    public class ReportsDataReferralCommission
    {

    }

    public class ReportsDataDSCCommission
    {
        
            public string refno { get; set; } // String type for alphanumeric reference numbers.
            public DateTime tranDate { get; set; } // DateTime type is correct for dates.
            public string coverage { get; set; } // String type is appropriate.
            public string idNumber { get; set; } // String type is appropriate.
            public string downLineID { get; set; } // String type is appropriate.
            public string downLineName { get; set; } // String type is appropriate.
            public int grp { get; set; } // Integer type for group numbers.
            public int levelNo { get; set; } // Integer type for level numbers.
            public int rankID { get; set; } // Integer type for rank identification.
            public string pkageCode { get; set; } // String type is appropriate.
            public string pkageDesc { get; set; } // String type is appropriate.
            public int pts { get; set; } // Integer type for points.
            public double commission { get; set; } // Double type for currency amounts.

            List<ReportsDataDSCCommission> commissionDSCList;

            public IEnumerator<ReportsDataDSCCommission> GetEnumerator()
            {
                foreach (var dsc in commissionDSCList)
                    yield return dsc;
            }



    }




    public class ReportsDataISCCommission
    {
        public string refno { get; set; } // String type for alphanumeric reference numbers.
        public DateTime tranDate { get; set; } // DateTime type is correct for dates.
        public string coverage { get; set; } // String type is appropriate.
        public string idNumber { get; set; } // String type is appropriate.
        public string downLineID { get; set; } // String type is appropriate.
        public string downLineName { get; set; } // String type is appropriate.
        public int grp { get; set; } // Integer type for group numbers.
        public int levelNo { get; set; } // Integer type for level numbers.
        public int rankID { get; set; } // Integer type for rank identification.
        public string pkageCode { get; set; } // String type is appropriate.
        public string pkageDesc { get; set; } // String type is appropriate.
        public int pts { get; set; } // Integer type for points.
        public double commission { get; set; } // Double type for currency amounts.

        List<ReportsDataISCCommission> commissionISCList;

        public IEnumerator<ReportsDataISCCommission> GetEnumerator()
        {
            foreach (var isc in commissionISCList)
                yield return isc;
        }
    }

    public class ReportsDataJ4UCommission
    {
        public string refno { get; set; } // String type for alphanumeric reference numbers.
        public DateTime tranDate { get; set; } // DateTime type is correct for dates.
        public string coverage { get; set; } // String type is appropriate.
        public string idNumber { get; set; } // String type is appropriate.
        public string downLineID { get; set; } // String type is appropriate.
        public string downLineName { get; set; } // String type is appropriate.
        public int grp { get; set; } // Integer type for group numbers.
        public int levelNo { get; set; } // Integer type for level numbers.
        public int rankID { get; set; } // Integer type for rank identification.
        public string pkageCode { get; set; } // String type is appropriate.
        public string pkageDesc { get; set; } // String type is appropriate.
        public int pts { get; set; } // Integer type for points.
        public double commission { get; set; } // Double type for currency amounts.

        List<ReportsDataJ4UCommission> commissionJ4UList;

        public IEnumerator<ReportsDataJ4UCommission> GetEnumerator()
        {
            foreach (var j4u in commissionJ4UList)
                yield return j4u;
        }
    }

    public class ReportsDataGM5Commission
    {
        public string refno { get; set; } // String type for alphanumeric reference numbers.
        public DateTime tranDate { get; set; } // DateTime type is correct for dates.
        public string coverage { get; set; } // String type is appropriate.
        public string idNumber { get; set; } // String type is appropriate.
        public string downLineID { get; set; } // String type is appropriate.
        public string downLineName { get; set; } // String type is appropriate.
        public int grp { get; set; } // Integer type for group numbers.
        public int levelNo { get; set; } // Integer type for level numbers.
        public int rankID { get; set; } // Integer type for rank identification.
        public string pkageCode { get; set; } // String type is appropriate.
        public string pkageDesc { get; set; } // String type is appropriate.
        public int pts { get; set; } // Integer type for points.
        public double commission { get; set; } // Double type for currency amounts.

        List<ReportsDataGM5Commission> commissionGM5List;

        public IEnumerator<ReportsDataGM5Commission> GetEnumerator()
        {
            foreach (var gm5 in commissionGM5List)
                yield return gm5;
        }
    }

    public class ReportsDataUnilevelCommission
    {
        public int levelNo { get; set; }
        public int rollUpNo { get; set; }
        public string tranDate { get; set; }
        public string downLineID { get; set; }
        public string downLineName { get; set; }
        public double baseValue { get; set; }
        public double rate { get; set; }
        public double commission { get; set; }

        List<ReportsDataUnilevelCommission> unilevelCommissionList;

        public IEnumerator<ReportsDataUnilevelCommission> GetEnumerator()
        {
            foreach (var uni in unilevelCommissionList)
                yield return uni;
        }

    }


    public class ReportsDataUnilevelLeadershipCommission
    {
        public int levelNo { get; set; }
        public string tranDate { get; set; }
        public string downLineID { get; set; }
        public string downLineName { get; set; }
        public double baseValue { get; set; }
        public double rate { get; set; }
        public double commission { get; set; }

        List<ReportsDataUnilevelLeadershipCommission> unilevelLeadershipCommissionList;

        public IEnumerator<ReportsDataUnilevelLeadershipCommission> GetEnumerator()
        {
            foreach (var uni in unilevelLeadershipCommissionList)
                yield return uni;
        }

    }







    public class ReportsDataPairsHistory
    {


        public string refno { get; set; }
        public DateTime tranDate { get; set; }
        public int levelNo { get; set; }
        public int rollUpNo { get; set; }
        public string downLineID { get; set; }
        public string downLineName { get; set; }
        public string downlinePkageCode { get; set; } 
        public string downlinePkageDesc { get; set; } 
        public double begBalLeft { get; set; }
        public double begBalRight { get; set; }
        public double curBalLeft { get; set; }
        public double curBalRight { get; set; }
        public double endBalLeft { get; set; }
        public double endBalRight { get; set; }
        public double commission { get; set; }

        List<ReportsDataPairsHistory> pairHistoryList;

        public IEnumerator<ReportsDataPairsHistory> GetEnumerator()
        {
            foreach (var pair in pairHistoryList)
                yield return pair;
        }



    }





}
