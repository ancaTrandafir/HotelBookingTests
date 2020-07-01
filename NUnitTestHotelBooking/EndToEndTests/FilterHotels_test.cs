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
    public class FilterHotels_test
    {
        private IWebDriver driver;
        public string homeURL;


        [Test(Description = "Check FetchHotels page for Filter button")]
        public void FilterHotels_FetchHotelsPage()
        {

            homeURL = "http://localhost:4200";
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(homeURL);
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(100000));

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/app-root/body/app-nav-menu/header/nav/div/div/ul[1]/li[2]/a")));
            IWebElement fetchHotelsFromNavBar = driver.FindElement(By.XPath("/html/body/app-root/body/app-nav-menu/header/nav/div/div/ul[1]/li[2]/a"));
            fetchHotelsFromNavBar.Click();
            Thread.Sleep(5000);

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/app-root/body/div/app-hotel/app-hotel-list/form/input")));
            IWebElement cityTextBox = driver.FindElement(By.XPath("/html/body/app-root/body/div/app-hotel/app-hotel-list/form/input"));
            cityTextBox.SendKeys("Cluj Napoca");

            IWebElement filterButton = driver.FindElement(By.XPath("/html/body/app-root/body/div/app-hotel/app-hotel-list/form/button"));
            filterButton.Click();
            Thread.Sleep(5000);

            wait.Until(driver => driver.FindElement(By.Id("test_filterHotels")));
            IWebElement cityColumn = driver.FindElement(By.Id("test_filterHotels"));
            Assert.AreEqual("Cluj Napoca", cityColumn.GetAttribute("value"));

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