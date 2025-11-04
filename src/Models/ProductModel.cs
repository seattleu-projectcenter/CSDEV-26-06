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
        public string Image { get; set; }

        public string Url { get; set; }
        
        [StringLength(maximumLength: 33, MinimumLength = 1, ErrorMessage = "The Title should have a length of more than {2} and less than {1}")]
        public string Title { get; set; }
        
        public string Record { get; set;  }

        public string Description { get; set; }

        public int[] Ratings { get; set; }

        public ProductTypeEnum ProductType { get; set; } = ProductTypeEnum.Undefined;

        public string Nickname { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public string Reach { get; set; }

        public string Stance { get; set; }

        [JsonPropertyName("DOB")]
        public string DateOfBirth { get; set; }
        
        public string Status { get; set; }

        public string Hometown { get; set; }

        [JsonPropertyName("earnings")]
        public long Earnings { get; set; }

        // Store the Comments entered by the users on this product
        public List<CommentModel> CommentList { get; set; } = new List<CommentModel>();

        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);


    }
}
