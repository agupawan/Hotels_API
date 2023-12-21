using HotelsAPI.Data;
using HotelsAPI.Models;
using HotelsAPI.Models.dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace HotelsAPI.Controllers
{
    //[Route('api/[controller]")]
    [Route("api/HotelsAPI")]
    [ApiController]
    public class HotelsAPIController : ControllerBase
    {
        private readonly ILogger<HotelsAPIController> _logger;
        public HotelsAPIController(ILogger<HotelsAPIController> logger) {
            _logger = logger;   
        }



        [HttpGet]
        public ActionResult<IEnumerable<HotelDTO>> GetHotels()
        {
            _logger.LogInformation("Getting info");
            return Ok(HotelStore.hotelList);
        }

        [HttpGet("{id:int}", Name ="GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type = typeof(HotelDTO))]
        public ActionResult<HotelDTO> GetHotel(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Get Hotel error with id " + id);
                return BadRequest();
            }
            var hotel = HotelStore.hotelList.FirstOrDefault(u => u.Id == id);
            if(hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        [HttpGet("test")]
        public string GetTest()
        {
            return "working";
        }

        [HttpPost]
        public ActionResult<HotelDTO> CreateHotel([FromBody]HotelDTO hotel)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if(HotelStore.hotelList.Any(u => u.Name.ToLower() == hotel.Name.ToLower())){
                ModelState.AddModelError("CustomError", "Hotel already exists");
                return BadRequest(ModelState);
            }
            if(hotel == null)
            {
                return BadRequest(hotel);
            }
            hotel.Id = HotelStore.hotelList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            HotelStore.hotelList.Add(hotel);

            return CreatedAtRoute("GetHotel",new {id = hotel.Id},hotel);

        }

        [HttpDelete("{id:int}", Name = "DeleteHotel")]
        public ActionResult DeleteHotel(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var hotel = HotelStore.hotelList.FirstOrDefault(u => u.Id == id);
            if(hotel == null)
            {
                return NotFound();
            }
            HotelStore.hotelList.Remove(hotel);
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateHotel")]
        public ActionResult UpdateHotel(int id, [FromBody]HotelDTO hotel)
        {
           if(hotel == null || hotel.Id != id)
            {
                return BadRequest();
            }
           var item = HotelStore.hotelList.FirstOrDefault(hotel => hotel.Id == id);
            item.Name = hotel.Name; ;
            item.Occupancy = hotel.Occupancy;
            item.Sqft = hotel.Sqft;

            return NoContent();
        }

        [HttpPatch("{id:int}",Name ="UpdatePartialHotel")]
        public ActionResult UpdatePartialHotel(int id, JsonPatchDocument<HotelDTO> patchDTO)
        {
            if(patchDTO == null || id == 0)
            {
                 return BadRequest();
            }
            var hotel = HotelStore.hotelList.FirstOrDefault(u => u.Id == id);
            if(hotel == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(hotel, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
