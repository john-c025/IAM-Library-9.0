using IAM_Library.models.auth;
using IAM_Library.models.general;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.models.dashboard
{
    internal class AccountDetailsModel
    {
        public ApiResponseModel<object> apiResponse { get; set; }
        public AccountDetailData accountData { get; set; }
    }
    // MONDAY - WORK ON ACCOUNT FETCHING AND CONTACT AND ADDRESS! -- oct 27 iam wallet
    public class AccountDetailData
    {
        public PrimaryInfoModel primaryInfo { get; set; }
        public RankInfoModel rankInfo { get; set; }
        public ContactInfoModel contactInfo { get; set; }
        public AddressInfoModel addressInfo { get; set; }
        public BinaryPtsBal binaryPtsBal { get; set; }
        public RqvPtsBal rqvPtsBal { get; set; }
    }
    public class BinaryPtsBal
    {
        public double leftPts { get; set; }
        public double rightPts { get; set; }
    }

    public class RqvPtsBal
    {
        public double leftPts { get; set; }
        public double rightPts { get; set; }
    }

    public class PrimaryInfoModel
    {
        public string refno { get; set; }
        public string idNumber { get; set; }
        public string primaryID { get; set; }
        public string sponsorID { get; set; }
        public string uplineID { get; set; }
        public int grp { get; set; }
        public DateTime? dateRegistered { get; set; }
        public DateTime? mdate { get; set; }
        public DateTime? paydate { get; set; }
        public DateTime? bdate { get; set; }
        public string? fname { get; set; }
        public string? mname { get; set; }
        public string? sname { get; set; }
        public bool? isMover { get; set; }
        public DateTime? moverDate { get; set; }
    }

    public class RankInfoModel
    {
        public string? package { get; set; }
        public string? position { get; set; }
        public string? rank { get; set; }
        public string? title { get; set; }
    }

    public class ContactInfoModel
    {
        public string email { get; set; }
        public string contactNo { get; set; }
    }

    public class AddressInfoModel
    {
        public string? country { get; set; }
        public string? province { get; set; }
        public string? city { get; set; }
        public string? brgy { get; set; }
        public string? homeAddress { get; set; }
    }









}
