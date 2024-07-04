using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

[TestFixture]
public class WebTests
{
    private IWebDriver driver;

    [SetUp]
    public void SetUp()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
    }

    [Test]
    public void PerformTestActions()
    {
        driver.Navigate().GoToUrl("");
        driver.FindElement(By.Id("email")).SendKeys("skander@gmail.com");
        driver.FindElement(By.Id("pass")).SendKeys("testpass");
        driver.FindElement(By.XPath("login")).Click();
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}
