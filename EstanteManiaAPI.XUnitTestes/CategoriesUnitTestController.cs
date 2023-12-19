using AutoMapper;
using EstanteMania.API.Context;
using EstanteMania.API.Controllers;
using EstanteMania.API.DTO_s;
using EstanteMania.API.Mappings;
using EstanteMania.API.UnitOfWork;
using EstanteMania.API.UnitOfWork.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstanteManiaAPI.XUnitTestes
{
    public class CategoriesUnitTestController
    {
        private IMapper mapper;
        private IUnitOfWork unitOfWork;

        public static DbContextOptions<AppDbContext> Options { get; }
        public static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EstanteManiaAapiDB;Integrated Security=True;";

        static CategoriesUnitTestController()
        {
            Options = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;
        }

        public CategoriesUnitTestController()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            mapper = config.CreateMapper();

            var context = new AppDbContext(Options);
            unitOfWork = new UnitOfWork(context);
        }

        [Fact]
        public async void GetCategories_Return_OKResult()
        {
            var controller = new CategoryController(unitOfWork, mapper);
            var data = await controller.GetAll();

            Assert.IsType<List<CategoryDTO>>(data.Value);
        }

        [Fact]
        public async void GetCategoryById_Return_OkResult()
        {
            var controller = new CategoryController(unitOfWork, mapper);
            var data = await controller.GetById(7);

            Assert.IsType<OkObjectResult>(data.Result);
        }

        [Fact]
        public async void GetCategoryById_Return_NotFound()
        {
            var controller = new CategoryController(unitOfWork, mapper);
            var data = await controller.GetById(999);

            Assert.IsType<NotFoundObjectResult>(data.Result);
        }

        [Fact]
        public async void Post_Category_Return_CreateResult()
        {
            var controller = new CategoryController(unitOfWork, mapper);

            var category = new CategoryDTO { Name = "Teste inclusao", Description = "Test", IconCSS = "test.png" };

            var data = await controller.Post(category);

            Assert.IsType<CreatedAtRouteResult>(data);
        }


        [Fact]
        public async void Put_Category_Return_OkResult()
        {
            var controller = new CategoryController(unitOfWork, mapper);

            var newCat = new CategoryDTO { Id = 25, Name = "Teste put", Description = "testing", IconCSS = "test2.png" };
            var data = await controller.Put(25, newCat);

            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Delete_Category_Return_OkResult()
        {
            var controller = new CategoryController(unitOfWork, mapper);
            var data = await controller.Delete(25);

            Assert.IsType<OkObjectResult>(data);
        }
    }
}
