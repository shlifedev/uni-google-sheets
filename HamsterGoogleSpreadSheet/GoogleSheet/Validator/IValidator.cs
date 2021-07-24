using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.Validator
{
    public class UGSValidateException : System.Exception
    {
        public UGSValidateException(string message) : base(message)
        {
        }
    }
    interface IValidator
    {
        void isValid(System.Action<bool> callback, params object[] objs); 
    }

    public abstract class UGSValidator : IValidator
    {
        protected static IHttpProtcol requester;
        public static void InitializeValidatorRequester(IHttpProtcol requester)
        {
            UGSValidator.requester = requester;
        }
        public abstract void isValid(System.Action<bool>callback, params object[] objs); 
    } 
}
