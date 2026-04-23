using System;

namespace StoneActionServer.WebApi.DTO
{
    public class CraftResponseData
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int? CraftedItemId { get; set; }
        public int? CraftedQuantity { get; set; }
    }
}