using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics.CodeAnalysis;

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
            options.AddArgument("--headless=new");
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
            // Arrange done in SetUp

            // Act
            var table = _driver.FindElement(By.TagName("tbody"));
            var recordRows = table.FindElements(By.TagName("tr"));

            // Assert
            Assert.IsNotEmpty(recordRows);
            
        }

        [TestMethod]
        public void HeaderTextDisplayedOnPageLoad()
        {
            // Arrange done in SetUp
            string expectedHeaderText = "RESTful records";

            // Act
            var headerText = _driver.FindElement(By.ClassName("card-title")).Text;

            // Assert
            Assert.AreEqual(expectedHeaderText, headerText);

        }
    }
}
