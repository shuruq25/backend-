using Microsoft.AspNetCore.Mvc;
using src.DTO;
using src.Entity;
using src.Services;
using src.Utils;
using static src.DTO.AddressDTO;

namespace src.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class AddressesController : ControllerBase
    {
        protected readonly IAddressService _addressService;

        //DI logic of  the Services.Address in AdressesController
        public AddressesController(IAddressService service)
        {
            _addressService = service;
        }

        //create

        [HttpPost]
        public async Task<ActionResult<AddressReadDto>> CreateOne(
            [FromBody] AddressCreateDto createDto
        )
        {
            var addressCreated = await _addressService.CreatOneAsync(createDto);
            return Created($"/api/v1/Addresses/{addressCreated.AddressId}", addressCreated);
        }

    

        // get by id
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressReadDto>> GetById([FromRoute] Guid id)
        {
            var Address = await _addressService.GetByIdAsync(id);

            return Ok(Address);
        }


        // Get all 
        [HttpGet]
        public async Task<ActionResult> GetAllAddresses()
        {
            return Ok(await _addressService.GetAllAsync());
        }



// Update 
           [HttpPut("{id}")]
        public async Task<ActionResult<AddressReadDto>> UpdateAddress(Guid id,[FromBody] AddressUpdateDto updateAddress)
        {
            var isUpdated = await _addressService.UpdateOneAsync(id, updateAddress);
            if (!isUpdated)
            {
                return NotFound();
            }
            var result = await _addressService.GetByIdAsync(id);
            return Ok(result);
        }

// Delete

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress([FromRoute] Guid id)
        {
            var deleted = await _addressService.DeleteOneAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
