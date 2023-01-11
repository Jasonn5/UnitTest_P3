using libraryApp.Controllers;
using libraryApp.Data.MockData;
using libraryApp.Data.Models;
using libraryApp.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LibraryApp.Test
{
    public class BooksControllerTest
    {
        [Fact]
        public void IndexUnitTest()
        {
            //arrange
            var mockRepo = new Mock<IBookService>();
            mockRepo.Setup(n => n.GetAll()).Returns(MockData.GetTestBookItems());
            var controller = new BooksController(mockRepo.Object);

            //act
            var result = controller.Index();

            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewResultBooks = Assert.IsAssignableFrom<List<Book>>(viewResult.ViewData.Model);
            Assert.Equal(5, viewResultBooks.Count);
        }

        [Theory]
        [InlineData("117366b8-3541-4ac5-8732-860d698e26a2", "117366b8-3541-4ac5-8732-860d698e2123")]
        public void DetailsUnitTest(string validGuid, string invalidGuid) 
        {
            //arrange
            var mockRepo = new Mock<IBookService>();
            var validBookGuid = new Guid(validGuid);
            mockRepo.Setup(n => n.GetById(validBookGuid)).Returns(MockData.GetTestBookItems().FirstOrDefault(b => b.Id == validBookGuid));
            var controller = new BooksController(mockRepo.Object);

            //act
            var result = controller.Details(validBookGuid);

            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewResultValue = Assert.IsAssignableFrom<Book>(viewResult.ViewData.Model);
            Assert.Equal("Evolutionary Psychology", viewResultValue.Title);
            Assert.Equal("David Buss", viewResultValue.Author);
            Assert.Equal(validBookGuid, viewResultValue.Id);

            //arrange
            var invalidBookGuid = new Guid(invalidGuid);
            mockRepo.Setup(n => n.GetById(invalidBookGuid)).Returns(MockData.GetTestBookItems().FirstOrDefault(b => b.Id == invalidBookGuid));

            //act
            var notFoundResult = controller.Details(invalidBookGuid);

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void CreateTest() 
        {
            //arrange
            var mockRepo = new Mock<IBookService>();
            var controller = new BooksController(mockRepo.Object);
            var newValidItem = new Book() 
            {
                Author = "Author",
                Title= "Title",
                Description= "Description"
            };

            //act
            var result = controller.Create(newValidItem)
                ;
            //assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Null(redirectToActionResult.ControllerName);

            //arrange
            var newInvalidItem = new Book()
            {
                Author = "Author",
                Description = "Description",
            };
            controller.ModelState.AddModelError("Author", "The Author value is required");

            //act
            var resultInvalid = controller.Create(newInvalidItem);

            //assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultInvalid);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Theory]
        [InlineData("117366b8-3541-4ac5-8732-860d698e26a2")]
        public void DeleteTest(string validGuid) 
        {
            //arrange
            var mockRepo = new Mock<IBookService>();
            mockRepo.Setup(n => n.GetAll()).Returns(MockData.GetTestBookItems());
            var controller = new BooksController(mockRepo.Object);
            var bookGuid = new Guid(validGuid);

            //act
            var result = controller.Delete(bookGuid, null);

            //assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result );
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Null(actionResult.ControllerName);
        }
    }
}