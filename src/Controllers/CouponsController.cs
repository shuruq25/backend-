using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Services;
using src.Utils;
using static src.DTO.CouponDTO;

namespace src.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class CouponsController : ControllerBase
    {
        protected readonly ICouponService _couponService;

        public CouponsController(ICouponService service)
        {
            _couponService = service;
        }

        //create

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CouponReadDto>> CreateOne(
            [FromBody] CouponCreateDto createDto
        )
        {
            var CouponCreated = await _couponService.CreateOneAsync(createDto);
            return Created($"/api/v1/Coupons/{CouponCreated.CouponId}", CouponCreated);
        }

        // Get all
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAllCoupons()
        {
            return Ok(await _couponService.GetAllAsync());
        }


        // Delete

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteCoupon([FromRoute] Guid id)
        {
            var deleted = await _couponService.DeleteOneAsync(id);
            if (!deleted)
            {
                throw CustomException.NotFound();
            }
            return NoContent();
        }

        // get by id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CouponReadDto>> GetById([FromRoute] Guid id)
        {
            CouponReadDto? coupon = await _couponService.GetByIdAsync(id);
            if (coupon is null)
            {
                throw CustomException.NotFound();
            }
            return Ok(coupon);
        }
    }
}
