using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using ContosoCrafts.WebSite.Models;

using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
   public class JsonFileProductService
    {
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
        }

        public IEnumerable<ProductModel> GetAllData()
        {
            using(var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<ProductModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        /// <summary>
        /// Add Rating
        /// 
        /// Take in the product ID and the rating
        /// If the rating does not exist, add it
        /// Save the update
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="rating"></param>
        public bool AddRating(string productId, int rating)
        {
            if (!IsValidRating(rating))
            {
                return false;
            }

            if (!TryGetProductById(productId, out var products, out var product))
            {
                return false;
            }

            // Check to see if the rating exist, if there are none, then create the array
            if(product.Ratings == null)
            {
                product.Ratings = Array.Empty<int>();
            }

            // Add the Rating to the Array
            var ratings = product.Ratings.ToList();
            ratings.Add(rating);
            product.Ratings = ratings.ToArray();

            // Save the data back to the data store
            SaveData(products);

            return true;
        }

        /// <summary>
        /// Find the data record
        /// Update the fields
        /// Save to the data store
        /// </summary>
        /// <param name="data"></param>
        public ProductModel UpdateData(ProductModel data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.Id))
            {
                return null;
            }

            if (!TryGetProductById(data.Id, out var products, out var productData))
            {
                return null;
            }

            // Update the data to the new passed in values
            productData.Title = data.Title;
            productData.Description = data.Description.Trim();
            productData.Url = data.Url;
            productData.Image = data.Image;

            productData.Nickname = data.Nickname;
            productData.Record = data.Record;
            productData.Height = data.Height;
            productData.Weight = data.Weight;
            productData.Reach = data.Reach;
            productData.Stance = data.Stance;
            productData.DateOfBirth = data.DateOfBirth;

            productData.Quantity = data.Quantity;
            productData.Price = data.Price;

            productData.CommentList = data.CommentList;

            SaveData(products);

            return productData;
        }

        /// <summary>
        /// Retrieve a single product by identifier.
        /// </summary>
        /// <param name="productId">The unique product identifier.</param>
        public ProductModel GetProductById(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                return null;
            }

            return GetAllData().FirstOrDefault(x => string.Equals(x.Id, productId, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Helper to lookup a product and the full product list in a single pass.
        /// </summary>
        private bool TryGetProductById(string productId, out List<ProductModel> products, out ProductModel product)
        {
            products = null;
            product = null;

            if (string.IsNullOrWhiteSpace(productId))
            {
                return false;
            }

            products = GetAllData().ToList();
            product = products.FirstOrDefault(x => string.Equals(x.Id, productId, StringComparison.OrdinalIgnoreCase));

            return product != null;
        }

        /// <summary>
        /// Validate rating boundaries (inclusive 0-5)
        /// </summary>
        private static bool IsValidRating(int rating) => rating >= 0 && rating <= 5;

        /// <summary>
        /// Save All products data to storage
        /// </summary>
        private void SaveData(IEnumerable<ProductModel> products)
        {

            using (var outputStream = File.Create(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    products
                );
            }
        }

        /// <summary>
        /// Create a new product using default values
        /// After create the user can update to set values
        /// </summary>
        /// <returns></returns>
        public ProductModel CreateData()
        {
            var data = new ProductModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Title = "Enter Title",
                Description = "Enter Description",
                Url = "Enter URL",
                Image = "",
            };

            // Get the current set, and append the new record to it because IEnumerable does not have Add
            var dataSet = GetAllData();
            dataSet = dataSet.Append(data);

            SaveData(dataSet);

            return data;
        }

        /// <summary>
        /// Remove the item from the system
        /// </summary>
        /// <returns></returns>
        public ProductModel DeleteData(string id)
        {
            // Get the current set, and append the new record to it
            var dataSet = GetAllData();
            var data = dataSet.FirstOrDefault(m => m.Id.Equals(id));

            var newDataSet = GetAllData().Where(m => m.Id.Equals(id) == false);
            
            SaveData(newDataSet);

            return data;
        }
        
    }
}
