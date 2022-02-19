using System.Collections.Generic;

namespace BattleshipLiteLibrary.Models
{
    public class PlayerInfoModel
    {
        public string UsersName { get; set; }

        public List<GridSpotModel> ShipLocations { get; set; } = new List<GridSpotModel>();

        public List<GridSpotModel> ShotGrid { get; set; } = new List<GridSpotModel>();
    }
}
