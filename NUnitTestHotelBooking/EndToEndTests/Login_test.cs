using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EndToEndTestsHotelBooking
{
    [TestFixture]
    public class Login_test
    {
        private IWebDriver driver;
        public string homeURL;


        [Test(Description = "Login with Admin credentials")]
        public void Login_toAdminPage()
        { 
            homeURL = "http://localhost:4200";
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(homeURL);
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(100000));

            wait.Until(driver => driver.FindElement(By.Name("Username")));
            IWebElement username = driver.FindElement(By.Name("Username"));
            username.SendKeys("admin");

            wait.Until(driver => driver.FindElement(By.Name("Password")));
            IWebElement password = driver.FindElement(By.Name("Password"));
            password.SendKeys("admin");

            wait.Until(driver => driver.FindElement(By.Id("submit")));
            IWebElement submit = driver.FindElement(By.Id("submit"));
            submit.Click();

            Thread.Sleep(5000);

            wait.Until(driver => driver.FindElement(By.TagName("h4")));
            IWebElement welcomeAdmin = driver.FindElement(By.TagName("h4"));

            Assert.AreEqual("Welcome, admin", welcomeAdmin.GetAttribute("textContent"));

        }

        [TearDown]
        public void TearDownTest()
        {
            driver.Close();
        }

        [SetUp]
        public void SetupTest()
        {
            homeURL = "http://localhost:4200";
            driver = new ChromeDriver();
        }
    }
}