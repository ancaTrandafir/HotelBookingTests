using HotelBooking;
using HotelBooking.Models;
using HotelBooking.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUnitTestHotelBooking
{
    class HotelServiceTests
    {

  

        [Test]
        public void UpdateShouldUpdateExistingHotel()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(UpdateShouldUpdateExistingHotel))
              .Options;

            using (var context = new BookingsDbContext(options))
            {
                var hotelService = new ReviewService(context);
                var addedHotel = hotelService.Create(new HotelBooking.Hotel
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
                            Rating = 10
                        }
                    }
                });
              

                context.Entry(addedHotel).State = EntityState.Detached;
                Assert.AreEqual("Transilvania Hotel", addedHotel.HotelName);
                addedHotel.HotelName = "updated hotel";
                var updatedHotel= hotelService.Update(addedHotel.Id, addedHotel);
                Assert.AreEqual("updated hotel", updatedHotel.HotelName);
            }
        }






        [Test]
        public void ValidCreateShouldCreateANewHotel()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidCreateShouldCreateANewHotel))
              .Options;

            using (var context = new BookingsDbContext(options))
            {

                var hotelService = new ReviewService(context);
                var addedHotel = new Hotel
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

                var result = hotelService.Create(addedHotel);

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
                var addedHotel = hotelService.Create(new HotelBooking.Hotel
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
                });

                hotelService.Delete(100);

                Assert.IsNull(hotelService.GetHotelById(100));
            }
        }





        [Test]
        public void ValidGetHotelByIdShouldGetSpecifiedHotel()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidGetHotelByIdShouldGetSpecifiedHotel))
              .Options;

            using (var context = new BookingsDbContext(options))
            {

                var hotelService = new ReviewService(context);
                Hotel hotel = new Hotel
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

                hotel.Id = 1;
                hotelService.Create(hotel);
                context.Entry(hotel).State = EntityState.Detached;

                Assert.AreEqual(hotelService.GetHotelById(1).HotelName, "Transilvania Hotel");

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

                hotelService.Create(hotel1);
                hotelService.Create(hotel2);

                context.Entry(hotel1).State = EntityState.Detached;
                context.Entry(hotel2).State = EntityState.Detached;

                Assert.AreEqual(hotelService.GetHotels().Count(), 2);

            }
        }
    }
}
