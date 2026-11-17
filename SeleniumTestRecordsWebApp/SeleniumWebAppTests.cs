using System.Data;
using System.Diagnostics.CodeAnalysis;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

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
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
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

        [TestMethod]
        public void GetAllMethodCallForExistingRecord()
        {
            //arrange
            var table = _driver.FindElement(By.TagName("tbody"));
            var recordRowsCount = table.FindElements(By.TagName("tr")).Count;

            //act
            var input = _driver.FindElement(By.Id("id"));
            input.SendKeys("1");

            var button = _driver.FindElement(By.Id("filterButton"));
            Thread.Sleep(100);
            button.Click();
            Thread.Sleep(150);
            var filteredRecordRowsCount = _driver.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr")).Count;
            //assert

            Assert.AreNotEqual(recordRowsCount, filteredRecordRowsCount);
        }

        [TestMethod]
        public void LoginDropDownIsNotDisplayedInitially()
        {
            // Arrange done in setup
            // Act & Assert
            Assert.IsFalse(_driver.FindElement(By.ClassName("dropdown-menu")).Displayed);
            
        }

        public void LoginButtonOpensLoginDropdownMenu(string username, string password, string role)
        {
            // Arrange
            var loginButton = _driver.FindElement(By.Id("loginDropdown"));

            // Act
            loginButton.Click();

            // Assert
            Assert.IsTrue(_driver.FindElement(By.ClassName("dropdown-menu")).Displayed);
        }

        [TestMethod]
        public void LoginIndicatorShowsNotLoggedInInitially()
        {
            // Arrange
            string expectedText = "Not logged in";

            // Act & Assert
            Assert.AreEqual(expectedText, _driver.FindElement(By.Id("loginStatusSpan")).Text);
        }

        [TestMethod]
        [DataRow("user", "1234", "user")]
        [DataRow("admin", "1234", "admin")]
        public void LoginSuccessfullyLogsIn(string username, string password, string role)
        {
            // Arrange
            var loginDropdown = _driver.FindElement(By.Id("loginDropdown"));
            var userNameField = _driver.FindElement(By.Id("loginUsername"));
            var passwordField = _driver.FindElement(By.Id("loginPassword"));
            var loginButton = _driver.FindElement(By.Id("loginButton"));
            var expectedText = "Logged in as " + role;

            // Act
            loginDropdown.Click();
            userNameField.SendKeys(username);
            passwordField.SendKeys(password);
            loginButton.Click();

            Thread.Sleep(200);
            string loginStatusText = _driver.FindElement(By.Id("loginStatusSpan")).Text;

            // Assert
            Assert.AreEqual(expectedText, loginStatusText);
        }

        [TestMethod]
        [DataRow("user", "wrongPassword")]
        [DataRow("admin", "wrongPassword")]
        [DataRow("nonExistentUsername1234512uh23i", "1234")]
        [DataRow("doesntexist", "wrongPassword")]
        public void IncorrectLoginInfoShowsErrorMessage(string username, string password)
        {
            // Arrange
            var loginDropdown = _driver.FindElement(By.Id("loginDropdown"));
            var userNameField = _driver.FindElement(By.Id("loginUsername"));
            var passwordField = _driver.FindElement(By.Id("loginPassword"));
            var loginButton = _driver.FindElement(By.Id("loginButton"));

            // Act
            loginDropdown.Click();
            userNameField.SendKeys(username);
            passwordField.SendKeys(password);
            loginButton.Click();

            Thread.Sleep(100);
            var authMessage = _driver.FindElement(By.Id("dropdownAuthMessage"));

            Assert.IsTrue(authMessage.Displayed);
        }
        [TestMethod]
        public void AddRecordSuccessfullyAdds()
        {
            // arrange
            LoginSetup();
            var table = _driver.FindElement(By.TagName("tbody"));
            var recordRows = table.FindElements(By.TagName("tr"));

            var titleField = _driver.FindElement(By.Id("newTitle"));
            var artistField = _driver.FindElement(By.Id("newArtist"));
            var durationField = _driver.FindElement(By.Id("newDuration"));
            var pubYearField = _driver.FindElement(By.Id("newPubYear"));
            var addButton = _driver.FindElement(By.Id("addButton"));
            // act
            titleField.SendKeys("title1");
            artistField.SendKeys("DuranDuran");
            durationField.SendKeys("270");
            pubYearField.SendKeys("2001");
            Thread.Sleep(100);

            addButton.Click();
            Thread.Sleep(100);

            var recordRowsAfter = _driver.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));
            // assert
            Assert.HasCount(recordRows.Count + 1, recordRowsAfter);
        }

        [TestMethod]
        public void DeleteSuccessfullyRemovesRecord()
        {
            // arrange
            LoginSetup();
            Thread.Sleep(100);
            var table = _driver.FindElement(By.TagName("tbody"));
            var recordRows = table.FindElements(By.TagName("tr"));
            

            var deleteButton = recordRows.Last().FindElement(By.TagName("button"));
            var modalConfirmButton = _driver.FindElement(By.Id("modalDeleteButton"));
            // act
            new Actions(_driver)
                .MoveToElement(deleteButton)
                .Perform();

            deleteButton.Click();
            Thread.Sleep(100);

            modalConfirmButton.Click();
            Thread.Sleep(100);

            var recordRowsAfter = _driver.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));
            // assert
            Assert.HasCount(recordRows.Count - 1, recordRowsAfter);
        }

        private void LoginSetup()
        {
            // Arrange
            var loginDropdown = _driver.FindElement(By.Id("loginDropdown"));
            var userNameField = _driver.FindElement(By.Id("loginUsername"));
            var passwordField = _driver.FindElement(By.Id("loginPassword"));
            var loginButton = _driver.FindElement(By.Id("loginButton"));

            // Act
            loginDropdown.Click();
            userNameField.SendKeys("user");
            passwordField.SendKeys("1234");
            loginButton.Click();

        }
    }
}
