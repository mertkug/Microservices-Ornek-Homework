using Inveon.Services.ProductAPI.Models;
using Label = Microsoft.Data.SqlClient.DataClassification.Label;

namespace Inveon.Services.ProductAPI.Dto
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; } 
        public string ImageUrl { get; set; }
        public string HoverImageUrl { get; set; }
        public ICollection<LabelDto> Labels { get; set; }
        public RatingDto Rating { get; set; }
        public ICollection<ColourDto> Colours { get; set; }
    }
}
