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
using System.Web.Http;
using System.Web.Http.Results;

namespace NUnitTestHotelBooking
{
    class HotelsControllerTests
    { 

        [Test]
        public void ValidGetByIdShouldReturnSelectedHotel()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(ValidGetByIdShouldReturnSelectedHotel))
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
        public void ValidPostShouldCreateANewHotel()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidPostShouldCreateANewHotel))
              .Options;

            using (var context = new BookingsDbContext(options))
            {

                var hotelService = new ReviewService(context);
                var controller = new HotelsController(hotelService);

                Hotel hotel = new Hotel
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

                controller.PostHotel(hotel);

                Assert.IsNotEmpty(context.Hotels);
            }
        }




        [Test]
        public void ValidDeleteShouldDeleteHotel()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidDeleteShouldDeleteHotel))
              .Options;

            using (var context = new BookingsDbContext(options))
            {
                var hotelService = new ReviewService(context);
                var controller = new HotelsController(hotelService);

                Hotel hotel = new Hotel
                {
                    Id = 100,
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


                hotelService.Create(hotel);
                context.Entry(hotel).State = EntityState.Detached;

                IActionResult actionResult = controller.DeleteHotel(100);
                var createdResult = actionResult as OkObjectResult;

                Assert.IsNotNull(createdResult);
                Assert.IsEmpty(context.Hotels);
            }
        }





        [Test]
        public void ValidDeleteByIdNotFoundId()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidDeleteShouldDeleteHotel))
              .Options;

            using (var context = new BookingsDbContext(options))
            {
                var hotelService = new ReviewService(context);
                var controller = new HotelsController(hotelService);

                Hotel hotel = new Hotel
                {
                    Id = 100,
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


                hotelService.Create(hotel);
                context.Entry(hotel).State = EntityState.Detached;

                IActionResult actionResult = controller.DeleteHotel(200);
                
              //  var createdResult = actionResult as OkObjectResult;

              //  Assert.IsInstanceOf(createdResult.GetType(), typeof(System.Web.Http.Results.NotFoundResult));
                Assert.IsNotEmpty(context.Hotels);
            }
        }





        [Test]
        public void ValidGetHotelsShouldListAll()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidGetHotelsShouldListAll))
              .Options;

            using (var context = new BookingsDbContext(options))
            {

                var hotelService = new ReviewService(context);
                var controller = new HotelsController(hotelService);

                Hotel hotel2 = new Hotel
                {
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

                controller.PostHotel(hotel1);
                controller.PostHotel(hotel2);

                context.Entry(hotel1).State = EntityState.Detached;
                context.Entry(hotel2).State = EntityState.Detached;

                Assert.AreEqual(hotelService.GetHotels().Count(), 2);

                IActionResult actionResult = controller.GetHotels();
                var createdResult = actionResult as OkObjectResult;

                Assert.IsNotNull(createdResult);
                Assert.AreEqual(200, createdResult.StatusCode);

            }
        }
    }
}
