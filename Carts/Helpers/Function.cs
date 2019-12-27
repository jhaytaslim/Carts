using Newtonsoft.Json;
using System;

namespace Carts.Helpers
{
    public static partial  class Function
    {

        public static T ConvertJsonToObject<T>(string RequestJSON)
        {
            var Request = !string.IsNullOrEmpty(RequestJSON) ? JsonConvert.DeserializeObject<T>(RequestJSON) : default(T);
            return Request;
        }

        public static Exception FormatMessage(this Exception ex)
        {
            Exception newEx = null;
            if (ex is NullReferenceException)
            {
                newEx = new Exception("A problem occured while processing this request.", ex.InnerException);
            }
            return newEx??ex;
        }

        public static string GetOrderRef(this string transactionRef)
        {
            string newString = Guid.NewGuid().ToString().Replace("-", "").Substring(0,16).ToUpper();//"0000000000000000";
            
            if (!string.IsNullOrEmpty(transactionRef))
            {
                return newString.Substring(0, (newString.Length - transactionRef.ToString().Length) - 1) + transactionRef;
            }
            return newString;
        }

        public static string GetInvoiceRef(this string transactionRef)
        {
            string newString = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16).ToUpper();//"0000000000000000";
            //newString.Re
            if (!string.IsNullOrEmpty(transactionRef))
            {
                return (newString.Substring(0, (newString.Length - transactionRef.ToString().Length) - 1) + transactionRef).ToUpper();
            }
            return string.Empty;
        }


    }
}
