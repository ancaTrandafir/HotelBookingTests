using HotelBooking.Helpers;
using HotelBooking.Models;
using HotelBooking.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Linq;


// referinta la proiectul pe care il testez in NuNitTestHotelBooking

// Fixing the error "Program has more than one entry point defined": added <GenerateProgramFile>false</GenerateProgramFile> inside my main project, not test project

namespace NUnitTestHotelBooking
{
    public class UserServiceTests
    {
        private IOptions<AppSettings> appSettings;


        [SetUp]
        public void Setup()
        {
            appSettings = Options.Create(new AppSettings
            {
                Secret = "asdfghjkl"
            });
        }



        [Test]
        public void ValidCreateShouldCreateANewUser()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidCreateShouldCreateANewUser))
              .Options;

            using (var context = new BookingsDbContext(options))
            {

                var userService = new UserService(context, appSettings, null);
                var user = new HotelBooking.Models.User
                {
                    Email = "anca@ymail.com",
                    FirstName = "Anca",
                    LastName = "Tudor",
                    Username = "anka"
                };

                var password = "12345";

                userService.Create(user, password);
                context.Entry(user).State = EntityState.Detached;

                Assert.IsNotEmpty(context.Users);
            }
        }





        [Test]
        public void ValidDeleteShouldDeleteUser()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidDeleteShouldDeleteUser))
              .Options;

            using (var context = new BookingsDbContext(options))
            {

                var userService = new UserService(context, appSettings, null);

                var user = new User
                {
                    Id = 1,
                    FirstName = "Anca",
                    LastName = "Tudor",
                    Email = "anca@ymail.com",
                    Username = "anka"
                };

                string pass = "123";

                userService.Create(user, pass);
                context.Entry(user).State = EntityState.Detached;

                userService.Delete(1);

                Assert.IsEmpty(context.Users);
            }
        }





        [Test]
        public void ValidGetUserByIdShouldGetSpecifiedUser()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidGetUserByIdShouldGetSpecifiedUser))
              .Options;

            using (var context = new BookingsDbContext(options))
            {

                var userService = new UserService(context, appSettings, null);
                User user= new User
                {
                    Id = 1,
                    FirstName = "Anca",
                    LastName = "Tudor",
                    Email = "anca@ymail.com",
                    Username = "anka"
                };

                string password = "12345";

                userService.Create(user, password);
                context.Entry(user).State = EntityState.Detached;

                Assert.AreEqual(userService.GetById(1).FirstName, "Anca");
            }
        }





        [Test]
        public void ValidGetUsersShouldListAll()
        {
            var options = new DbContextOptionsBuilder<BookingsDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidGetUsersShouldListAll))
              .Options;

            using (var context = new BookingsDbContext(options))
            {

                var hotelService = new ReviewService(context);

                var userService = new UserService(context, appSettings, null);
                User user1 = new User
                {
                    Id = 1,
                    FirstName = "Anca",
                    LastName = "Tudor",
                    Email = "anca@ymail.com",
                    Username = "anka"
                };

                string password1 = "12345";

                User user2 = new User
                {
                    Id = 2,
                    FirstName = "Anca",
                    LastName = "Tudor",
                    Email = "anca@ymail.com",
                    Username = "anka"
                };

                string password2 = "12345";

                userService.Create(user1, password1);
                userService.Create(user2, password2);
                context.Entry(user1).State = EntityState.Detached;
                context.Entry(user2).State = EntityState.Detached;

                Assert.AreEqual(hotelService.GetHotels().Count(), 2);

            }
        }
    }
}