using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Support.UI;

namespace TestingSoft_Backend.Standard
{
    public class StepStandard
    {
        public IWebDriver driver;

        public StepStandard()
        {
        }

        [Given(@"User Launch (.*)")]
        public void GivenUserLaunch(string url)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("start-maximized"); // Launch Chrome in maximized mode
            options.AddArguments("disable-infobars"); // Disable info bars on Chrome
            options.AddArguments("--disable-extensions"); // Disable extensions on Chrome
            options.AddArguments("--disable-gpu"); // Disable GPU acceleration on Chrome
            options.AddArguments("--disable-dev-shm-usage"); // Disable shared memory usage on Chrome
            options.AddArguments("--no-sandbox"); // Disable sandbox for Chrome
            options.AddArguments("--disable-popup-blocking"); // Disable popup blocking on Chrome
            options.AddArguments("--incognito"); // Launch Chrome in incognito mode

            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(url);
        }

        [When(@"User opens URL (.*)")]
        public void WhenUserOpensURL(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        [When(@"User clic on element has xpath (.*)")]
        public void WhenUserClicOnElementHasXpath(string xpath)
        {
            driver.FindElement(By.XPath(xpath)).Click();
        }

        [When(@"User clic in input has xpath (.*) and Value as (.*)")]
        public void WhenUserClicInInputHasXpathAndValueAs(string xpath, string value)
        {
            driver.FindElement(By.XPath(xpath)).Clear();
            driver.FindElement(By.XPath(xpath)).SendKeys(value);
        }

        [When(@"User clic in input submit has xpath (.*)")]
        public void WhenUserClicInInputSubmitHasXpath(string xpath)
        {
            driver.FindElement(By.XPath(xpath)).Click();
            Thread.Sleep(2000);
        }

        [When(@"User clic in select has xpath (.*) and Value as (.*)")]
        public void WhenUserClicInSelectHasXpathAndValueAs(string xpath, string value)
        {
            SelectElement selectElement = new SelectElement(driver.FindElement(By.XPath(xpath)));
            selectElement.SelectByText(value);
        }

        [When(@"User clic in lien has xpath (.*)")]
        public void WhenUserClicInLienHasXpath(string xpath)
        {
            driver.FindElement(By.XPath(xpath)).Click();
            Thread.Sleep(2000);
        }

        [When(@"User clic in dp has xpath (.*)")]
        public void WhenUserClicInDpHasXpath(string xpath)
        {
            driver.FindElement(By.XPath(xpath)).SendKeys("");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [When(@"User clic in textarea has xpath (.*) and Value as (.*)")]
        public void WhenUserClicInTextareaHasXpathAndValueAs(string xpath, string value)
        {
            driver.FindElement(By.XPath(xpath)).Clear();
            driver.FindElement(By.XPath(xpath)).SendKeys(value);
        }

        [When(@"User clic in selectize has xpath (.*)")]
        public void WhenUserClicInSelectizeHasXpath(string xpath)
        {
            driver.FindElement(By.XPath(xpath)).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [When(@"User clic in input radio has xpath (.*)")]
        public void WhenUserClicInInputRadioHasXpath(string xpath)
        {
            string s = xpath.Replace("input", "label");
            IWebElement radioElement = driver.FindElement(By.XPath(s));
            if (radioElement != null)
            {
                radioElement.Click();
            }
            else
            {
                IWebElement element = driver.FindElement(By.XPath(xpath));
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].checked = true;", element);
                element.Click();
            }
        }


        [Then(@"close browser")]
        public void ThenCloseBrowser()
        {
            driver.Quit();
        }
    }

}
