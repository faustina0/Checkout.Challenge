using System;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.Challenge.Api.Dto;
using Checkout.Challenge.Services;
using Checkout.Challenge.Services.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Challenge.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController: ControllerBase
    {
        private readonly IPaymentService _service;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }


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

            var payment = _mapper.Map<PaymentRequest, PaymentRequestModel>(payload);
            var result = await _service.ProcessPayment(payment);
            var response = _mapper.Map<PaymentResponseModel, PaymentResponse>(result);
            
            return Ok(response);
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaymentResponse>> Get(string id)
        {
            var result = await _service.GetPayment(id);
            if(result == null)
            {
                return NotFound($"Payment with id {id} cannot be found");
            }
            var response = _mapper.Map<PaymentResponseModel, PaymentResponse>(result);
            return Ok(response);
        }

    }
}