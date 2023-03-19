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
            CODE_RESOURCE_DUPLICATE,
        }

        public struct APIResponseMessage
        {
            public static readonly string MESSAGE_OK = "";

            public static readonly string MESSAGE_USER_WRONG_NAME = "Wrong user name, cannot get user data";
            public static readonly string MESSAGE_USER_WRONG_PASS = "Wrong password, try again";
            public static readonly string MESSAGE_USER_DUPLICATE = "Cannot register the same user";

            public static readonly string MESSAGE_STATION_EMPTY_FOUND = "No stations found";
            public static readonly string MESSAGE_STATION_NOT_FOUND = "No station matches the station name";
            public static readonly string MESSAGE_STATION_DUPLICATE = "Cannot add the same station";

            public static readonly string MESSAGE_PUMP_LIST_EMPTY = "No pumps found";
            public static readonly string MESSAGE_PUMP_EMPTY_FOUND = "No pump matches the station name";
            public static readonly string MESSAGE_PUMP_NOT_FOUND = "No pump matches the station name and pump name";
            public static readonly string MESSAGE_PUMP_DUPLICATE = "Cannot add the same pump";
        }
    }
}
