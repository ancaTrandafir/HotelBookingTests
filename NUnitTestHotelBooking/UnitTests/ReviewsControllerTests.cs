using FluentAssertions;
using HotelBooking;
using HotelBooking.Controllers;
using HotelBooking.Models;
using HotelBooking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;


// https://www.c-sharpcorner.com/article/unit-testing-controllers-in-web-api/
// https://www.matheus.ro/2018/05/28/unit-tests-in-asp-net-core-controllers/
// https://asp.net-hacker.rocks/2017/09/27/testing-aspnetcore.html


namespace NUnitTestHotelBooking
{
    class ReviewsControllerTests
    { 

        [Test]
        public void ValidGetByIdShouldReturnSelectedReview()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(ValidGetByIdShouldReturnSelectedReview))
            .Options;

            using (var context = new BookingsDbContext(options))
            {

                var hotelService = new ReviewService(context);
                var controller = new HotelsController(hotelService);

                Hotel hotel2 = new Hotel
                {
                    Id = 1,
                    HotelName = "Transilvania Hotel",
                    City = "Sibiu",
                    Capacity = 120,
                    Rating = 8,
                    Reviews = new List<Review>()
                        {
                            new Review
                            {
                                Text = "fain",
                                Rating = 10,

                            }
                        }
                };

                Hotel hotel1 = new Hotel
                {
                    Id = 2,
                    HotelName = "Belvedere Hotel",
                    City = "Sibiu",
                    Capacity = 120,
                    Rating = 8,
                    Reviews = new List<Review>()
                        {
                            new Review
                            {
                                Text = "fain",
                                Rating = 10,

                            }
                        }
                };

                hotelService.Create(hotel1);
                hotelService.Create(hotel2);

                context.Entry(hotel1).State = EntityState.Detached;
                context.Entry(hotel2).State = EntityState.Detached;

                var response = controller.GetHotelById(1);

                var contentResult = response as Hotel;
                
                Assert.IsNotNull(contentResult);
                Assert.AreEqual("Transilvania Hotel", contentResult.HotelName);
            }
            }






        [Test]
        public void ValidPostShouldCreateANewReview()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidPostShouldCreateANewReview))
              .Options;

            using (var context = new BookingsDbContext(options))
            {
                // Arrange
                var controller = new ReviewsController(context);

                Review review = new Review
                {
                    HotelId = 1,
                    UserId = 1,
                    Text = "Cool",
                    Rating = 8
                };

                // Act
                var result = controller.PostReview(review);
               
                // Assert
                var okResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
                var addedReview = okResult.Value.Should().BeAssignableTo<Review>().Subject;
                addedReview.Text.Should().Be("Cool");
            }
        }




        [Test]
        public void ValidDeleteShouldDeleteReview()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidDeleteShouldDeleteReview))
              .Options;

            using (var context = new BookingsDbContext(options))
            {
                // Arrange
                var controller = new ReviewsController(context);

                Review review = new Review
                {
                    HotelId = 1,
                    UserId = 1,
                    Text = "Cool",
                    Rating = 8
                };

                controller.PostReview(review);

                // Act
                var result1 = controller.DeleteReview(1);
                var result2 = controller.DeleteReview(100);

                // Assert
                Assert.IsEmpty(context.Reviews);
            //    var okResult2 = result2.Should().BeOfType<>().Subject;
                Assert.Throws<InvalidOperationException>(() => controller.GetReview(100));
            }
        }





        [Test]
        public void DeleteById_IdNotFound()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteById_IdNotFound))
              .Options;

            using (var context = new BookingsDbContext(options))
            {
                var controller = new ReviewsController(context);

                Review review = new Review
                {
                    HotelId = 1,
                    UserId = 1,
                    Text = "Cool",
                    Rating = 8
                };

                controller.PostReview(review);
                context.Entry(review).State = EntityState.Detached;

                var actionResult = controller.DeleteReview(3).Result;

              //  Assert.IsInstanceOf(actionResult.GetType(), typeof(System.Web.Http.Results.NotFoundResult));
                Assert.IsNotEmpty(context.Reviews);
            }
        }





        [Test]
        public void ValidGetReviewsShouldListAll()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidGetReviewsShouldListAll))
              .Options;

            using (var context = new BookingsDbContext(options))
            {
                var controller = new ReviewsController(context);

                Review review1 = new Review
                {
                    HotelId = 1,
                    UserId = 1,
                    Text = "Cool",
                    Rating = 8  
                };

                Review review2 = new Review
                {
                    HotelId = 2,
                    UserId = 2,
                    Text = "Cool",
                    Rating = 8
                };
            
               // Arrange
                controller.PostReview(review1);
                controller.PostReview(review2);

                // Act
                Task<ActionResult<IEnumerable<Review>>> result = controller.GetReviews();

                // Assert
                Assert.IsNotNull(result);
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                var reviews = okResult.Value.Should().BeAssignableTo<IEnumerable<Review>>().Subject;
                reviews.Count().Should().Be(2);
            }
        }






       
    
}
}
