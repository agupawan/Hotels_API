using HotelsAPI.Data;
using HotelsAPI.Loggers;
using HotelsAPI.Models;
using HotelsAPI.Models.dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

namespace HotelsAPI.Controllers
{
    //[Route('api/[controller]")]
    [Route("api/HotelsAPI")]
    [ApiController]
    public class HotelsAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _db;
        //private readonly ILogger<HotelsAPIController> _logger;
        private readonly ILogging _logger;
        public HotelsAPIController(ILogging logger, ApplicationDbContext db) {
            _logger = logger;
            _db = db;
        }



        [HttpGet]
        public ActionResult<IEnumerable<HotelDTO>> GetHotels()
        {
            _logger.Log("Getting info","");
            return Ok(_db.Hotels.ToList());
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
                _logger.Log("Get Hotel error with id " + id,"error");
                return BadRequest();
            }
            var hotel = _db.Hotels.FirstOrDefault(u => u.Id == id);
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

            if(_db.Hotels.Any(u => u.Name.ToLower() == hotel.Name.ToLower())){
                ModelState.AddModelError("CustomError", "Hotel already exists");
                return BadRequest(ModelState);
            }
            if(hotel == null)
            {
                return BadRequest(hotel);
            }
            Hotel model = new()
            {
                Amenity = hotel.Amenity,
                Details = hotel.Details,
                Id = hotel.Id,
                ImageUrl = hotel.ImageUrl,
                Name = hotel.Name,
                Occupancy = hotel.Occupancy,
                Rate = hotel.Rate,
                Sqft = hotel.Sqft
            };
            _db.Hotels.Add(model);
            _db.SaveChanges();

            return CreatedAtRoute("GetHotel",new {id = hotel.Id},hotel);

        }

        [HttpDelete("{id:int}", Name = "DeleteHotel")]
        public ActionResult DeleteHotel(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var hotel = _db.Hotels.FirstOrDefault(u => u.Id == id);
            if(hotel == null)
            {
                return NotFound();
            }
            _db.Hotels.Remove(hotel); _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateHotel")]
        public ActionResult UpdateHotel(int id, [FromBody]HotelDTO hotel)
        {
           if(hotel == null || hotel.Id != id)
            {
                return BadRequest();
            }
            //var item = HotelStore.hotelList.FirstOrDefault(hotel => hotel.Id == id);
            // item.Name = hotel.Name; ;
            // item.Occupancy = hotel.Occupancy;
            // item.Sqft = hotel.Sqft;

            Hotel model = new()
            {
                Amenity = hotel.Amenity,
                Details = hotel.Details,
                Id = hotel.Id,
                ImageUrl = hotel.ImageUrl,
                Name = hotel.Name,
                Occupancy = hotel.Occupancy,
                Rate = hotel.Rate,
                Sqft = hotel.Sqft
            };
            _db.Hotels.Update(model);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}",Name ="UpdatePartialHotel")]
        public ActionResult UpdatePartialHotel(int id, JsonPatchDocument<HotelDTO> patchDTO)
        {
            if(patchDTO == null || id == 0)
            {
                 return BadRequest();
            }
            var hotel = _db.Hotels.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if(hotel == null)
            {
                return BadRequest();
            }
            HotelDTO hotelDTO = new()
            {
                Amenity = hotel.Amenity,
                Details = hotel.Details,
                Id = hotel.Id,
                ImageUrl = hotel.ImageUrl,
                Name = hotel.Name,
                Occupancy = hotel.Occupancy,
                Rate = hotel.Rate,
                Sqft = hotel.Sqft
            };
            patchDTO.ApplyTo(hotelDTO, ModelState);
            Hotel model = new()
            {
                Amenity = hotelDTO.Amenity,
                Details = hotelDTO.Details,
                Id = hotelDTO.Id,
                ImageUrl = hotelDTO.ImageUrl,
                Name = hotelDTO.Name,
                Occupancy = hotelDTO.Occupancy,
                Rate = hotelDTO.Rate,
                Sqft = hotelDTO.Sqft
            };
            _db.Update(model);
            _db.SaveChanges();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
