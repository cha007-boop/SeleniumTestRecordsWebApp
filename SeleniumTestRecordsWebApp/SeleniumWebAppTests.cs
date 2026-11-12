using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTestRecordsWebApp
{
    [TestClass]
    public sealed class SeleniumWebAppTests
    {
        private IWebDriver _driver;

        [TestInitialize]
        public void SetUp()
        {
            var options = new ChromeOptions();
            //options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1280,1000");

            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Navigate().GoToUrl("http://127.0.0.1:5500/");
        }

        [TestCleanup]
        public void TearDown()
        {
            _driver.Quit();
        }

        [TestMethod]
        public void TableOfRecordsDisplayedOnPageLoad()
        {
            // Arrage

        }
    }
}
