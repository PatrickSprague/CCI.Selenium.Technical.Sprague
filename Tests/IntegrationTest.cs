using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CCI.Selenium.Technical.IntegrationTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class IntegrationTest
    {
        private ChromeDriver? driver;
        private readonly By _usernameInputXpath = By.XPath("//input[contains(@id, 'user-name')]");
        private readonly By _passwordInputXpath = By.XPath("//input[contains(@id, 'password')]");
        private readonly By _loginButtonXpath = By.XPath("//input[contains(@class, 'submit-button')]");
        private readonly By _addBackpackCartXpath = By.XPath("//button[contains(@data-test, 'add-to-cart-sauce-labs-backpack')]");
        private readonly By _shoppingCartXpath = By.XPath("//div[contains(@id, 'shopping_cart_container')]");
        private readonly By _checkoutXpath = By.XPath("//button[contains(@data-test, 'checkout')]");
        private readonly By _firstNameXpath = By.XPath("//input[contains(@id, 'first-name')]");
        private readonly By _lastNameXpath = By.XPath("//input[contains(@id, 'last-name')]");
        private readonly By _zipCodeXpath = By.XPath("//input[contains(@id, 'postal-code')]");
        private readonly By _cartContinueXpath = By.XPath("//input[contains(@id, 'continue')]");
        private readonly By _finishButtonXpath = By.XPath("//button[contains(@data-test, 'finish')]");
        private readonly By _successMessageXpath = By.XPath("//h2[contains(@data-test, 'complete-header')]");
        private readonly By _removeBackpackCartXpath = By.XPath("//button[contains(@data-test, 'remove-sauce-labs-backpack')]");
        private readonly By _continueShoppingXpath = By.XPath("//button[contains(@data-test, 'continue-shopping')]");
        private readonly By _productSortContiner = By.CssSelector("[class='product_sort_container']");
        private readonly By _priceHiloSort = By.CssSelector("[value='hilo']");
        

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        [Test]
        public void OrderBackpack()
        {
            string username = "standard_user";
            string password = "secret_sauce";
            string firstName = "Patrick";
            string lastName = "Sprague";
            string zipCode = "97206";
            IWebElement usernameInput = driver.FindElement(_usernameInputXpath);
            IWebElement passwordInput = driver.FindElement(_passwordInputXpath);
            IWebElement loginButton = driver.FindElement(_loginButtonXpath);

            usernameInput.SendKeys(username);
            passwordInput.SendKeys(password);
            loginButton.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(10);

            //add backpack to cart
            IWebElement addBackpackCart = driver.FindElement(_addBackpackCartXpath);
            addBackpackCart.Click();
            //go to shopping cart
            IWebElement shoppingCart = driver.FindElement(_shoppingCartXpath);
            shoppingCart.Click();
            //go to checkout
            IWebElement checkoutButton = driver.FindElement(_checkoutXpath);
            checkoutButton.Click();
            //enter name and zip
            driver.FindElement(_firstNameXpath).SendKeys(firstName);
            driver.FindElement(_lastNameXpath).SendKeys(lastName);
            driver.FindElement(_zipCodeXpath).SendKeys(zipCode);
            //continue
            IWebElement continueButton = driver.FindElement(_cartContinueXpath);
            continueButton.Click();
            //finish
            IWebElement finishButton = driver.FindElement(_finishButtonXpath);
            finishButton.Click();
            //assert
            IWebElement successMessage = driver.FindElement(_successMessageXpath);
            string message = successMessage.Text;
            Assert.That(message, Does.Contain("Thank you for your order!"));
        }

        [Test]
        public void RemoveBackpackFromCart()
        {
            string username = "standard_user";
            string password = "secret_sauce";
            IWebElement usernameInput = driver.FindElement(_usernameInputXpath);
            IWebElement passwordInput = driver.FindElement(_passwordInputXpath);
            IWebElement loginButton = driver.FindElement(_loginButtonXpath);

            usernameInput.SendKeys(username);
            passwordInput.SendKeys(password);
            loginButton.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(10);

            //add backpack to cart
            IWebElement addBackpackCart = driver.FindElement(_addBackpackCartXpath);
            addBackpackCart.Click();
            //go to shopping cart
            IWebElement shoppingCart = driver.FindElement(_shoppingCartXpath);
            shoppingCart.Click();
            //remove backpack from cart
            IWebElement removeBackpackFromCart = driver.FindElement(_removeBackpackCartXpath);
            removeBackpackFromCart.Click();
            //continue shopping
            IWebElement continueShopping = driver.FindElement(_continueShoppingXpath);
            continueShopping.Click();
            //assert
            Assert.That(driver.FindElement(_addBackpackCartXpath).Displayed, Is.True);
            
        }

        [Test]
        public void FilterProducts()
        {
            //navigate to products
            string username = "standard_user";
            string password = "secret_sauce";
            IWebElement usernameInput = driver.FindElement(_usernameInputXpath);
            IWebElement passwordInput = driver.FindElement(_passwordInputXpath);
            IWebElement loginButton = driver.FindElement(_loginButtonXpath);

            usernameInput.SendKeys(username);
            passwordInput.SendKeys(password);
            loginButton.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(10);
            //filter products
            IWebElement productSort = driver.FindElement(_productSortContiner);
            productSort.Click();

            IWebElement hiLoSort = driver.FindElement(_priceHiloSort);
            hiLoSort.Click();

            Thread.Sleep(5000);

            //find all inventory_item in inventory_list
            var inventoryList = driver.FindElements(By.CssSelector("[class='inventory_item']"));
            //get name of first item on page
            string jacketItem = inventoryList[0].Text;
            string jacketName = jacketItem.Substring(0, 24);
            //assert
            Assert.That(jacketName, Is.EqualTo("Sauce Labs Fleece Jacket"));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
