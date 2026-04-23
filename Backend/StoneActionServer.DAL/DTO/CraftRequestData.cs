using System;

namespace StoneActionServer.WebApi.DTO
{
    public class CraftRequestData
    {
        public int ItemIdToCraft { get; set; }
        public int Quantity { get; set; }
    }
}