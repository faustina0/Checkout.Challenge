using System;

namespace Checkout.Challenge.Services.Model
{
    public class Error
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string InternalErrorMessage { get; set; }
    }
}
