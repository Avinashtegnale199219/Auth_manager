using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF_FD_SA_AUTH_MANAGER
{
    public static class ErrorMsg
    {
     

        public static readonly ReadOnlyDictionary<string, KeyValuePair<string, string>> ErrorMsgs = new ReadOnlyDictionary<string, KeyValuePair<string, string>>(
         new Dictionary<string, KeyValuePair<string, string>>(){

             //security
             {"Unauthorized",new KeyValuePair<string, string>("MFFDSUP-API-AUTH-SYS-0001","You are unauthorized to access the requested resource") },
             {"InvalidAccess",new KeyValuePair<string, string>("MFFDSUP-API-AUTH-SYS-0002","Your account is not authorized to access the requested resource")},
             {"TokenEncryptionKey",new KeyValuePair<string, string>("MFFDSUP-API-AUTH-SYS-0003","Token encryption key is not available")},
             {"TokenEncryptionIV",new KeyValuePair<string, string>("MFFDSUP-API-AUTH-SYS-0004","Token encryption IV is not available")},
             {"InvalidToken",new KeyValuePair<string, string>("MFFDSUP-API-AUTH-SYS-0005","Invalid token")},

             //system
             //{"InvalidResource", new KeyValuePair<string, string>("MFFDCB-API-AUTH-SYS-0001","We could not find the resource you requested")},
             //{"InvalidRequest", new KeyValuePair<string, string>("MFFDCB-API-AUTH-SYS-0002","Invalid syntax for this request was provided")},
             //{"MethodType", new KeyValuePair<string, string>("MFFDCB-API-AUTH-SYS-0003","This method type is not currently supported")},
             //{"ReqTimeOut", new KeyValuePair<string, string>("MFFDCB-API-AUTH-SYS-0004","Request Time Out")},
             //{"InvalidName", new KeyValuePair<string, string>("MFFDCB-API-AUTH-SYS-0005","The requested resource does not support the media type provided")},
             //{"ServerUnavail", new KeyValuePair<string, string>("MFFDCB-API-AUTH-SYS-0006","The server is currently unavailable")},

             ////transaction
             //{"Wrong", new KeyValuePair<string, string>("MFFDCB-API-E-TRN-0001","Something went Wrong")},
             //{"TrnTimeOut", new KeyValuePair<string, string>("MFFDCB-API-E-TRN-0002","Transaction Time out")},
         
             //{"NoContent", new KeyValuePair<string, string>("MFFDCB-API-E-TRN-0004","Data not found")},

             //AuthManagerBO
             {"UsernameRequired", new KeyValuePair<string, string>("MFFDSUP-API-AUTH-VAL-0001","Username cannot be null/empty")},
             {"PasswordRequired", new KeyValuePair<string, string>("MFFDSUP-API-AUTH-VAL-0002","Password cannot be null/empty")},
             {"DuplicateInput", new KeyValuePair<string, string>("MFFDSUP-API-AUTH-VAL-0003","Duplicate key not allowed")},
             {"InvalidInput", new KeyValuePair<string, string>("MFFDSUP-API-AUTH-VAL-0004","Input is not valid")},
             {"InvalidRequest", new KeyValuePair<string, string>("MFFDSUP-API-AUTH-VAL-0005","Invalid request")},
             ////
             //{ "MobileNoRequired", new KeyValuePair<string, string>("MFFDSUP-API-E-OVD-VAL-0001","MobileNo cannot be null/empty")},
             //{ "MobileNoInvalid", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0002","MobileNo is not in valid format")},
             //{ "PANRequired", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0003","PAN cannot be null/empty")},
             //{ "PANInvalid", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0004","PAN is not in valid format")},
             //{ "DOBRequired", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0005","DOB cannot be null/empty")},
             //{ "DOBInvaid", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0006","DOB is not in valid format(dd-MMM-yyyy)")},
             //{ "OTPRequired", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0007","OTP cannot be null/empty")},
             //{ "OTPInvalid", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0008","OTP is invalid")},
             //{ "FolioNoRequired", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0009","FolioNo cannot be null/empty")},
             //{ "FolioNoInvalid", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0010","FolioNo is invalid")},
             //{ "ApplicationNoRequired", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0011","ApplicationNo cannot be null/empty")},
             //{ "ApplicationNoInvalid", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0012","ApplicationNo is invalid")},
             //{ "FdrRequired", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0013","FDR_NO is invalid")},
             //{ "FinYearRequired", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0014","FinYear cannot be null/empty")},
             //{ "QuarterRequired", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0015","Quarter cannot be null/empty")},
             //{ "DocTypeRequired", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0016","DocType cannot be null/empty")},
             //{ "DocTypeInvalid", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0017","DocType is invalid")},
             //{ "DocSubTypeRequired", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0018","DocSubType cannot be null/empty")},
             //{ "DocSubTypeInvalid", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0019","DocSubType is invalid")},
             //{ "NotEligible", new KeyValuePair<string, string>("MFFDCB-API-CB-VAL-0020","Not eligible for Form 15GH")},
         });


    }
}
