using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Common
{
    public class ConstantHelper
    {
        public enum APIResponseCode
        {
            CODE_SUCCESS = 0,
            CODE_FAIL,
            CODE_RESOURCE_NOT_FOUND,
            CODE_INTERNAL_SERVER,
            CODE_RESOURCE_CONFLICT
        }

        public struct APIResponseMessage
        {
            public static readonly string MESSAGE_OK = "";
            public static readonly string MESSAGE_USER_WRONG_NAME = "Wrong user name, cannot get user data";
            public static readonly string MESSAGE_USER_WRONG_PASS = "Wrong password, try again";
            public static readonly string MESSAGE_USER_DUPLICATE = "Cannot register the same user";
            public static readonly string MESSAGE_STATION_EMPTY_FOUND = "No stations found";
            public static readonly string MESSAGE_STATION_NOT_FOUND = "No station matches the search";
            public static readonly string MESSAGE_STATION_DUPLICATE = "Cannot add the same station";
            public static readonly string NOT_ENOUGH_MONNEY = "User have not enough monney please recharge and try again";
            public static readonly string ATTACHED_RG = "Some cloud accounts are attached to this resource group.";
            public static readonly string RESOURCE_UPDATE_FAILED = "Update resource unsuccessfully";
            public static readonly string SUPSCRIPTION_ALREADY_ACTIVE = "Subscription already activated";
            public static readonly string LICENSE_NOT_EXISTS = "The license key does not exist";
            public static readonly string SUBSCRIPTION_NOT_EXISTS = "The subscription does not exist";
            public static readonly string USERNAME_ALREADY_EXISTS = "The username already exist";
            public static readonly string LICENSE_ALREADY_ACTIVE = "The license key has already been used";
            public static readonly string LICENSE_ACTIVE_SUCCESSED = "Activate license key successfully";
            public static readonly string LICENSE_INVALID = "Invalid license key";
            public static readonly string UPDATE_USER_ERROR = "User update error";
            public static readonly string SUBSCRIPTION_INVALID = "Invalid subscription";
            public static readonly string LICENSE_EXPIRED = "The license key has expired";
            public static readonly string SUBSCRIPTION_EXPIRED = "The subscription has expired";
            public static readonly string LICENSE_NOT_START = "The license key start time is in the future";
            public static readonly string SUBSCRIPTION_NOT_START = "The subscription start time is in the future";
            public static readonly string LICENSE_GENERATE_SUCCESSED = "Generate license key successfully";
            public static readonly string TOKEN_INVALID = "Invalid or expired token"; // + unauthorized
            public static readonly string ACCESS_DENIED = "Access denied";
            public static readonly string INVALID_PARAMETER = "Invalid parameters";
        }
    }
}
