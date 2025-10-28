using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.models.registration
{
    public class RegistrationModel
    {
        public string refno { get; set; }
        public string activeNo { get; set; }
        public string pinNo { get; set; }
        public string idNumber { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string? email { get; set; }
        public string? contactNo { get; set; }
        public string sponsorID { get; set; }
        public string uplineID { get; set; }
        public int grp { get; set; }
        public string country { get; set; }
        public string? province { get; set; }
        public string? city { get; set; }
        public string? homeAddress { get; set; }
        public string? fname { get; set; }
        public string? mname { get; set; }
        public string? sname { get; set; }

    }
 
    public class VerificationInputModel
    {
        public string activeno { get; set; }
        public string pinno { get; set; }
    }
    public class VerificationMainModel
    {
        public ActivationModel activation { get; set; }
        public ConditionModel condition { get; set; }
    }

    public class ActivationModel
    {
        public string refno { get; set; }
        public string activeNo { get; set; }
        public string pinNo { get; set; }
        public string idNumber { get; set; }
        public string? fname { get; set; }
        public string? mname { get; set; }
        public string? sname { get; set; }


    }

    public class DupeUserResponse
    {
       public bool isDuplicated { get; set; }
    }

    public class ConditionModel
    {
       public bool isValidPackage { get; set; }
        public bool ext_Left { get; set; }
        public bool ext_Right { get; set; }

    }


    public class UplineId
    {
        public string sponsorID { get; set; }
        public string uplineID { get; set; }
        public int grp { get; set; }
    }

    // TODO
    // Crate models and functionality for ExtremeCondition,ExtremeUpline,CheckCrossLine, <- used for registration

    // Create model and functionality for verification of activation sponsoreid is the logged in user

    //VERIFY
    /*
    {
    "activation": {
        "refno": "F0199298",
        "activeNo": "7544292638",
        "pinNo": "1386679",
        "idNumber": "05762673"
    },
    "condition": {
        "isValidPackage": "true",
        "ext_Left": "false",
        "ext_Right": "true"
    }
    }
    */


    /* UPLINE
     * 
     * 
        {
        "sponsorID": "00000001",
        "uplineID": "01779412",
        "grp": 1
     * 
     * 
     * 
     * 
     * 
     */
    public class CrossLineValidationResponse
    {
        public bool isCrossline { get; set;}
    }
    public class RegistrationResponse
    {
        public string message { get; set; }
        public RegistrationModel data { get; set; }
        public int grp { get; set; }
    }

    public class UserRegistrationInputModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string? emailParam { get; set; }
        public string? contactNoParam { get; set; }
        public string? countryParam { get; set; }
        public string? provinceParam { get; set; }
        public string? cityParam { get; set; }

        public string? homeAddrParam { get; set; }
    }

    public class UserUpdatePrimaryDetailsModel
    {
        public string? primaryID { get; set; }
        public string? idNumber { get; set; }
        public DateTime? bdate { get; set; } //"1988-11-11",

        public int gender { get; set; }
        public string? email { get; set; }
        public string? contactNo { get; set; }
        public string? country { get; set; }
        public string? province { get; set; }
        public string? city { get; set; }
        public string? brgy { get; set; }

        public string? homeAddress { get; set; }
        public string? zipCode { get; set; }

        public string? benefName { get; set; }
        public string? benefMobileNo { get; set; }
    }

    public class BankDetails
    {
        public string idNumber { get; set; }
        public int? bankID { get; set; }
        public string? bankName { get; set; }
        public string? bankAccountNo { get; set; }
        public string? bankAccountName { get; set; }
    }

    public class UpdateBankDetailsResponse
    {
        public string message { get; set; }
        public BankDetails data { get; set; }
    }

    public class UserUpdateBankDetailsModel
    {
        
        public string? idNumber { get; set; }
        public int bankID { get; set; }
        public string? bankName { get; set; }
        public string? bankAccountNo { get; set; }
        public string? bankAccountName { get; set; }
       
    }
    public class MemberDetails
    {
        public string primaryID { get; set; }
        public string idNumber { get; set; }
        public DateTime? bdate { get; set; }
        public int? gender { get; set; }
        public string? email { get; set; }
        public string? contactNo { get; set; }
        public string? country { get; set; }
        public string? province { get; set; }
        public string? city { get; set; }
        public string? brgy { get; set; }
        public string? homeAddress { get; set; }
        public string? zipCode { get; set; }
        public string? benefName { get; set; }
        public string? benefMobileNo { get; set; }
    }

    public class UpdatePrimaryDetailsResponse
    {
        public string message { get; set; }
        public MemberDetails data { get; set; }
    }


    ///Upload Image
    public class FilePathDetails
    {
        public string contentType { get; set; }
        public string contentDisposition { get; set; }
        public Dictionary<string, List<string>> headers { get; set; }
        public long length { get; set; }
        public string name { get; set; }
        public string fileName { get; set; }
    }

    public class UploadData
    {
        public string idNumber { get; set; }
        public int idCode { get; set; }
        public int status { get; set; }
        public string accIDNo { get; set; }
        public string accIDName { get; set; }
        public FilePathDetails filePath { get; set; }
        public bool isVerified { get; set; }
        public bool isActive { get; set; }
    }

    public class MainUploadResponse
    {
        public string message { get; set; }
        public UploadData data { get; set; }
    }



    //
    public class BankDetail
    {
        public int bankID { get; set; }
        public string bankName { get; set; }
    }

    public class BankDetailsList
    {
        public List<BankDetail> BankDetails { get; set; }
    }

    public class IdDetail
    {
        public int idCode { get; set; }
        public string idType { get; set; }
        public string idDetails { get; set; }
    }

    public class IdDetailsList
    {
        public List<IdDetail> IdDetails { get; set; }
    }
    /*
     * 
     * REGISTRATION SUCCESSFUL RESPONSE
     * 
     * {
            "message": "Registration successfully!",
            "data": {
                "refno": "F0199298",
                "activeNo": "7544292638",
                "pinNo": "1386679",
                "idNumber": "88888888",
                "email": "john@gmail.com",
                "contactNo": "09567937967",
                "sponsorID": "00000001",
                "uplineID": "01779412",
                "grp": 1,
                "country": "Philippines",
                "province": "Manila",
                "city": "Manila",
                "homeAddress": "Manila"
            }
        }
     * 
     * 
     * VerifyActivation -> GetExtreme -> GetExtremeUpline -> Get Groups -> Register
     * 
     */
}

