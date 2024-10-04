using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTO;
using src.Services;
using src.Utils;

namespace src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaymentsController : ControllerBase
    {
        protected readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> GetAllPayments()
        {
            return Ok(await _paymentService.GetAllPaymets());
        }

        [HttpGet("{id}")]
        [Authorize]
        //filite user in service
        public async Task<ActionResult> GetPaymentById(Guid id)
        {
            var payment = await _paymentService.GetPaymentById(id);
            if (payment == null)
            {
                throw CustomException.NotFound();
            }
            return Ok(payment);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreatePayment(
            [FromBody] PaymentDTO.PaymentCreateDto newPayment
        )
        {
            if (newPayment == null || newPayment.FinalPrice <= 0)
            {
                throw CustomException.BadRequest("Invalid payment data.");
            }
            var createdPaymentDto = await _paymentService.CreatePayment(newPayment);

            return CreatedAtAction(
                nameof(GetPaymentById),
                new { id = createdPaymentDto.Id },
                createdPaymentDto
            );
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdatePayment(
            Guid id,
            [FromBody] PaymentDTO.PaymentUpdateDto updatedPayment
        )
        {
            if (await _paymentService.UpdatePaymentById(id, updatedPayment))
            {
                return NoContent();
            }
            throw CustomException.NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize]
        //in the service check the py statues
        public async Task<ActionResult> DeletePayment(Guid id)
        {
            if (await _paymentService.DeletePaymentById(id))
            {
                return NoContent();
            }

            throw CustomException.NotFound();
        }
    }
}
