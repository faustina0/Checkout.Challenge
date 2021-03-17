using System;
using System.Threading.Tasks;
using Checkout.Challenge.Bank.Api.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Challenge.Bank.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController: ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaymentResponse>> Create(PaymentRequest payload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // just a mock logic to send different codes
            var rnd = new Random();
            var response = new PaymentResponse
                           {
                               Identifier = rnd.Next(1000000,9999999),
                               AuthorizationCode = rnd.Next(100000, 999999),
                               StatusCode = "10000",
                               StatusMessage = "Authorized",
                               CreatedDate = DateTime.Now
            };

            if(!payload.CardNumber.StartsWith("5"))
            {
                return Ok(response);
            }

            response.StatusCode = "50000";
            response.StatusMessage = "Declined";


            return await Task.FromResult(Ok(response));
        }
    }
}
