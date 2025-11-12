using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

using Moq;

using NUnit.Framework;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;

namespace UnitTests.Pages.Product.AddFighter
{
    public class AddFighterModelTests
    {
        [Test]
        public void OnGet_ShouldInitializeProduct_And_ReturnPage()
        {
            var service = new FakeProductService();
            var model = CreateModel(service);

            var result = model.OnGet();

            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsNotNull(model.Product);
        }

        [Test]
        public void OnPost_InvalidModelState_ShouldReturnPage()
        {
            var service = new FakeProductService();
            var model = CreateModel(service);
            model.ModelState.AddModelError("Title", "required");

            var result = model.OnPost();

            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsFalse(service.CreateDataCalled);
            Assert.IsFalse(service.UpdateDataCalled);
        }

        [Test]
        public void OnPost_CreateDataReturnsNull_ShouldAddModelErrorAndStayOnPage()
        {
            var service = new FakeProductService
            {
                CreateDataResult = null
            };
            var model = CreateModel(service);
            model.Product = CreateValidProduct();

            var result = model.OnPost();

            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsFalse(model.ModelState.IsValid);
        }

        [Test]
        public void OnPost_UpdateDataReturnsNull_ShouldAddModelErrorAndStayOnPage()
        {
            var service = new FakeProductService
            {
                CreateDataResult = new ProductModel { Id = "generated-id" },
                UpdateDataResult = null
            };
            var model = CreateModel(service);
            model.Product = CreateValidProduct();

            var result = model.OnPost();

            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsFalse(model.ModelState.IsValid);
            Assert.AreEqual("generated-id", model.Product.Id);
        }

        [Test]
        public void OnPost_Success_ShouldRedirectToIndexWithNewId()
        {
            var service = new FakeProductService
            {
                CreateDataResult = new ProductModel { Id = "generated-id" },
                UpdateDataResult = new ProductModel { Id = "generated-id" }
            };
            var model = CreateModel(service);
            model.Product = CreateValidProduct();

            var result = model.OnPost();

            var redirect = result as RedirectToPageResult;
            Assert.IsNotNull(redirect);
            Assert.AreEqual("./Index", redirect.PageName);
            Assert.AreEqual("generated-id", redirect.RouteValues["id"]);
            Assert.AreSame(model.Product, service.LastUpdatedProduct);
        }

        private static ProductModel CreateValidProduct()
        {
            return new ProductModel
            {
                Title = "Title",
                Description = "Description",
                Url = "http://contoso",
                Image = "image",
                Nickname = "Nickname",
                Record = "10-0-0",
                Height = "6ft",
                Weight = "170 lbs",
                Reach = "72 in",
                Stance = "Orthodox",
                DateOfBirth = "1990-01-01",
                Status = "Active",
                Hometown = "Contoso City"
            };
        }

        private static AddFighterModel CreateModel(FakeProductService service)
        {
            var model = new AddFighterModel(service);
            var modelState = new ModelStateDictionary();
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var metadataProvider = new EmptyModelMetadataProvider();
            model.PageContext = new PageContext(actionContext)
            {
                ViewData = new ViewDataDictionary(metadataProvider, modelState)
            };
            model.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            return model;
        }

        private sealed class FakeProductService : JsonFileProductService
        {
            public FakeProductService()
                : base(Mock.Of<IWebHostEnvironment>())
            {
            }

            public ProductModel CreateDataResult { get; set; } = new ProductModel { Id = Guid.NewGuid().ToString() };
            public ProductModel UpdateDataResult { get; set; } = new ProductModel { Id = Guid.NewGuid().ToString() };
            public ProductModel LastUpdatedProduct { get; private set; }
            public bool CreateDataCalled { get; private set; }
            public bool UpdateDataCalled { get; private set; }

            public override ProductModel CreateData()
            {
                CreateDataCalled = true;
                return CreateDataResult;
            }

            public override ProductModel UpdateData(ProductModel data)
            {
                UpdateDataCalled = true;
                LastUpdatedProduct = data;
                return UpdateDataResult;
            }
        }
    }
}
