using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ContosoCrafts.WebSite.Models
{
    public class ProductModel
    {
        public string Id { get; set; }
        public string Maker { get; set; }

        [JsonPropertyName("img")]
        
        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Fighter-page link is required")]
        public string Url { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(maximumLength: 33, MinimumLength = 1, ErrorMessage = "The Title should have a length of more than {2} and less than {1}")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Record is required")]
        public string Record { get; set;  }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public int[] Ratings { get; set; }
        
        [Required(ErrorMessage = "Nickname is required")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Heights is required")]
        public string Height { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        public string Weight { get; set; }

        [Required(ErrorMessage = "Reach is required")]
        public string Reach { get; set; }

        [Required(ErrorMessage = "Stance is required")]
        public string Stance { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [JsonPropertyName("DOB")]
        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; }
        
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Hometown is required")]
        public string Hometown { get; set; }

        [JsonPropertyName("earnings")]
        public long Earnings { get; set; }

        // Store the Comments entered by the users on this product
        public List<CommentModel> CommentList { get; set; } = new List<CommentModel>();

        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);


    }
}
