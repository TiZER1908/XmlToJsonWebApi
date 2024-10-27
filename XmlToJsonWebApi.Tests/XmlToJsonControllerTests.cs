using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using XmlToJsonWebApi.Controllers;
using XmlToJsonWebApi.Repositories;
using XmlToJsonWebApi.Data.Model;
using XmlToJsonWebApi.Share.DTOs;
using Xunit;
using Moq;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection;
using Microsoft.AspNetCore.Hosting.Server;


namespace XmlToJsonWebApi.Tests
{
    public class XmlToJsonControllerTests
    {
        IWebHostEnvironment _appEnvironment;

        [Fact]
        public void TestGetAll()
        {
            // Arrange
            var mock = new Mock<IDictionaryRepository>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTestUsers().AsQueryable());
            XmlToJsonController controller = new XmlToJsonController(_appEnvironment, mock.Object);

            // Act
            var testGetAll = controller.GetAll();

            // Assert
            var viewResultGetAll = Assert.IsType<ViewResult>(testGetAll);
            var modelGetAll = Assert.IsAssignableFrom<EnumerableQuery<DictionaryDTO>>(viewResultGetAll.Model);
            Assert.NotNull(testGetAll);
            Assert.Equal(GetTestUsers().Count, modelGetAll.Count());

        }

        [Fact]
        public void TestGetbyId()
        {
            // Arrange
            int testId = 2;
            var mock = new Mock<IDictionaryRepository>();
            mock.Setup(repo => repo.GetByKey(testId)).Returns(GetTestUsers().FirstOrDefault(p => p.Id == testId));
            XmlToJsonController controller = new XmlToJsonController(_appEnvironment, mock.Object);

            // Act
            var testGetbyId = controller.GetbyId(testId);

            // Assert

            var viewResultGetbyId = Assert.IsType<ViewResult>(testGetbyId);
            var modelGetbyId = Assert.IsType<Dictionary>(viewResultGetbyId.ViewData.Model);
            Assert.NotNull(testGetbyId);
            Assert.Equal("test2", modelGetbyId.Name);
        }

        [Fact]
        public void TestGetbyIdNoFound()
        {
            // Arrange
            int testId = 2;
            var mock = new Mock<IDictionaryRepository>();
            mock.Setup(repo => repo.GetByKey(testId)).Returns(GetTestUsers().FirstOrDefault(p => p.Id == testId));
            XmlToJsonController controller = new XmlToJsonController(_appEnvironment, mock.Object);

            // Act
            var testGetbyId = controller.GetbyId(4);

            // Assert
            Assert.IsType<NotFoundResult>(testGetbyId);
        }

        [Fact]
        public void TestPost()
        {
            // Arrange
            var mock = new Mock<IDictionaryRepository>();
            XmlToJsonController controller = new XmlToJsonController(_appEnvironment, mock.Object);
            var dict = new DictionaryDTO
            {
                BeginDate = DateTime.Now,
                EndDate = DateTime.MaxValue,
                Code = "1",
                Name = "test1",
                Comments = "comment1"
            };

            // Act
            var testPost = controller.Post(dict);

            // Assert
            Assert.IsType<OkResult>(testPost);
        }
        [Fact]
        public void TestPostBadRequest()
        {
            // Arrange
            var mock = new Mock<IDictionaryRepository>();
            XmlToJsonController controller = new XmlToJsonController(_appEnvironment, mock.Object);

            // Act
            var testPost = controller.Post(null);

            // Assert
            Assert.IsType<BadRequestResult>(testPost);
        }

        [Fact]
        public void TestPut()
        {
            // Arrange
            int testId = 1;
            var mock = new Mock<IDictionaryRepository>();
            mock.Setup(repo => repo.GetByKey(testId)).Returns(GetTestUsers().FirstOrDefault(p => p.Id == testId));
            XmlToJsonController controller = new XmlToJsonController(_appEnvironment, mock.Object);
            var dict = new DictionaryDTO
            {
                Code = "1",
                Name = "edit test1",
                Comments = "edit comment1"
            };

            // Act
            var testPut = controller.Put(testId, dict);
            var testAfterPut = controller.GetbyId(testId);

            // Assert
            Assert.IsType<OkResult>(testPut);
            var viewResultPut = Assert.IsType<ViewResult>(testAfterPut);
            var modelPut = Assert.IsType<Dictionary>(viewResultPut.ViewData.Model);
            Assert.Equal("edit test1", modelPut.Name);

        }

        [Fact]
        public void TestPutNoFound()
        {
            // Arrange
            int testId = 1;
            var mock = new Mock<IDictionaryRepository>();
            mock.Setup(repo => repo.GetByKey(testId)).Returns(GetTestUsers().FirstOrDefault(p => p.Id == testId));
            XmlToJsonController controller = new XmlToJsonController(_appEnvironment, mock.Object);
            var dict = new DictionaryDTO
            {
                Name = "edit test1",
            };

            // Act
            var testPut = controller.Put(4, dict);

            // Assert
            Assert.IsType<NotFoundResult>(testPut);
        }

        [Fact]
        public void TestPutBadRequest()
        {
            // Arrange
            int testId = 1;
            var mock = new Mock<IDictionaryRepository>();
            mock.Setup(repo => repo.GetByKey(testId)).Returns(GetTestUsers().FirstOrDefault(p => p.Id == testId));
            XmlToJsonController controller = new XmlToJsonController(_appEnvironment, mock.Object);

            // Act
            var testPut = controller.Put(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(testPut);
        }

        [Fact]
        public void TestDelete()
        {
            // Arrange
            int testId = 2;
            var mock = new Mock<IDictionaryRepository>();
            mock.Setup(repo => repo.GetByKey(testId)).Returns(GetTestUsers().FirstOrDefault(p => p.Id == testId));
            XmlToJsonController controller = new XmlToJsonController(_appEnvironment, mock.Object);

            // Act
            var testDelete = controller.Delete(testId, 1);
            var testAfterDelete = controller.GetbyId(testId);

            // Assert
            Assert.IsType<OkResult>(testDelete);
        }

        private List<Dictionary> GetTestUsers()
        {
            var dictionaries = new List<Dictionary>
            {
                new Dictionary 
                {
                    Id=1,
                    CreateDate=DateTime.Now,
                    EditDate=DateTime.Now,
                    IsDeleted=false,
                    DeleteDate=DateTime.MinValue,
                    DeletedDictId=0,
                    BeginDate=DateTime.Now,
                    EndDate=DateTime.MaxValue,
                    Code="1",
                    Name="test1",
                    Comments="comment1"
                },
                new Dictionary
                {
                    Id=2,
                    CreateDate=DateTime.Now,
                    EditDate=DateTime.Now,
                    IsDeleted=false,
                    DeleteDate=DateTime.MinValue,
                    DeletedDictId=0,
                    BeginDate=DateTime.Now,
                    EndDate=DateTime.MaxValue,
                    Code="2",
                    Name="test2",
                    Comments="comment2"
                },
                new Dictionary
                {
                    Id=3,
                    CreateDate=DateTime.Now,
                    EditDate=DateTime.Now,
                    IsDeleted=false,
                    DeleteDate=DateTime.MinValue,
                    DeletedDictId=0,
                    BeginDate=DateTime.Now,
                    EndDate=DateTime.MaxValue,
                    Code="3",
                    Name="test3",
                    Comments="comment3"
                }
            };
            return dictionaries;
        }
        public IFormFile GetFormFile(FileStream fsr)
        {
            return new FormFile(fsr, 0, fsr.Length, "test", fsr.Name);
        }
    }
}
