using HotelsAPI.Models.dto;

namespace HotelsAPI.Data
{
    public static class HotelStore
    {
        public static List<HotelDTO> hotelList = new List<HotelDTO>
            {
                new HotelDTO{Id=1,Name="Pool View",Sqft=100, Occupancy=4},
                new HotelDTO{Id=2,Name="Beach View",Sqft=300, Occupancy=3}
            };
    }
}
