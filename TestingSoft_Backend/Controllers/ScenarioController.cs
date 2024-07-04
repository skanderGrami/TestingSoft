using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using Microsoft.AspNetCore.Mvc;

using OpenQA.Selenium;

using Newtonsoft.Json;

using Bogus;
using System.Text;
using TechTalk.SpecFlow.Plugins;

namespace TestingSoft_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScenarioController : Controller
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly IBuildRepository _buildRepository;

        public static IWebDriver driver;
        private readonly IWebDriver _driver;

        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

        public ScenarioController(IScenarioRepository scenarioRepository, ITestCaseRepository testCaseRepository, IBuildRepository buildRepository, IWebDriver driver)
        {
            _scenarioRepository = scenarioRepository;
            _testCaseRepository = testCaseRepository;
            _buildRepository = buildRepository;
            _driver = driver;
        }

        /*public static void ConfigNavigator(string browser)
        {
            if (string.IsNullOrEmpty(browser))
            {
                throw new ArgumentNullException(nameof(browser), "Browser name cannot be null or empty");
            }
            Console.WriteLine("Configuring browser: " + browser);

            // Initialisation en fonction du navigateur
            switch (browser.ToLower())
            {
                case "chrome":
                    driver = new ChromeDriver();
                    break;
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                default:
                    throw new ArgumentException("Unsupported browser: " + browser);
            }
            if (driver == null)
            {
                throw new InvalidOperationException("WebDriver initialization failed.");
            }
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(10); // Page load timeout
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30); // Async script timeout
            if (browser.Equals("Firefox", StringComparison.OrdinalIgnoreCase))
            {
                driver = new FirefoxDriver(new FirefoxOptions { BrowserExecutableLocation = ".\\geckodriver.exe" });
            }
            else if (browser.Equals("Google Chrome", StringComparison.OrdinalIgnoreCase))
            {
                driver = new ChromeDriver(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Drivers"));
            }
            else if (browser.Equals("Internet Explorer", StringComparison.OrdinalIgnoreCase))
            {
                driver = new EdgeDriver(".\\MicrosoftWebDriver.exe");
            }
            else if (browser.Equals("Safari", StringComparison.OrdinalIgnoreCase))
            {
                driver = new SafariDriver(".\\.exe");
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }*/

        [HttpGet]
        public async Task<ActionResult<List<Scenario>>> GetScenarios()
        {
            return await _scenarioRepository.GetScenarios();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Scenario>> GetScenarioById(int id)
        {
            var scenario = await _scenarioRepository.GetScenarioById(id);
            if (scenario == null)
            {
                return NotFound();
            }
            return scenario;
        }

        [HttpPost]
        public async Task<ActionResult<Scenario>> AddScenario(Scenario scenario)
        {
            var addedScenario = await _scenarioRepository.AddScenario(scenario);
            return CreatedAtAction(nameof(GetScenarioById), new { id = addedScenario.ScenarioId }, addedScenario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScenario(int id, Scenario scenario)
        {
            try
            {
                await _scenarioRepository.UpdateScenario(id, scenario);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScenario(int id)
        {
            var result = await _scenarioRepository.DeleteScenario(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }



        /////////////////////////////////////////
        public class RecordedAction
        {
            public DateTime Timestamp { get; set; }
            public string ActionDescription { get; set; }
            public string XPath { get; set; }
        }
        public class JsonToSpecFlow
        {
            public static List<RecordedAction> ReadJson(string filePath)
            {
                string json = System.IO.File.ReadAllText(filePath);
                List<RecordedAction> recordedActions = JsonConvert.DeserializeObject<List<RecordedAction>>(json);
                return recordedActions;
            }
        }


        ////////////////////////////////////////////////
        //Integration RunTest
       /* internal class ActionData
        {
            public DateTime Timestamp { get; set; }
            public string XPath { get; set; }
            public string idelement { get; set; }


            public string Value { get; set; }
        }
        private async Task<List<ActionData>> ReadActionsFromFileAsync(string filePath)
        {
            try
            {
                var jsonString = await System.IO.File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<List<ActionData>>(jsonString);
            }
            catch (Exception ex)
            {
                return new List<ActionData>();
            }
        }
        [HttpGet("runsenario")]
        public async Task<IActionResult> RunTest(string url)
        {
            var options = new ChromeOptions();
            driver = new ChromeDriver(options);

            try
            {
                driver.Navigate().GoToUrl(url);

                var actions = await ReadActionsFromFileAsync(outputPath);

                foreach (var action in actions)
                {
                    if (action.XPath != null)
                    {

                        var element = driver.FindElement(By.XPath(action.XPath));
                        if (!string.IsNullOrEmpty(action.Value))
                        {
                            element.SendKeys(action.Value);
                        }
                        else
                        {
                            element.Click();
                        }
                        await Task.Delay(1000);
                    }
                    else
                    {

                        var element = driver.FindElement(By.Id(action.idelement));
                        if (!string.IsNullOrEmpty(action.Value))
                        {
                            element.SendKeys(action.Value);
                        }
                        else
                        {
                            element.Click();
                        }
                        await Task.Delay(1000);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the Selenium test execution.");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                driver?.Quit();
            }

            return Ok("Selenium test completed successfully.");
        }*/

        //////////////////////////////////////////////
        
        [HttpPost("Recorder")]
        public async Task<IActionResult> Recorder(string url)
        {
            var faker = new Faker();
            string fakeId = faker.Random.Guid().ToString();
            List<RecordedAction> recordedActions = new List<RecordedAction>();
            string outputPath = $@"E:\StagePFE_TriWeb\TriExport\recordedActions{fakeId}.json";
            string specFlowCodePath = $@"E:\StagePFE_TriWeb\TriExport\specFlowSteps{fakeId}.cs";
            bool browserClosed = false;

            try
            {
                ChromeOptions options = new ChromeOptions();
                driver = new ChromeDriver(options);
                driver.Navigate().GoToUrl(url);
                driver.Manage().Window.Maximize();
                string script = @"

                function getElementXPath(element) {
                if (element && element.id) {
                    return 'id(\""' + element.id + '\"")';
                } else {
                    return getElementTreeXPath(element);
                }
            }

                    function getElementTreeXPath(element) {
                        var paths = [];
                        for (; element && element.nodeType == Node.ELEMENT_NODE; element = element.parentNode)  {
                            var index = 0;
                            var siblings = element.parentNode.childNodes;
                            for (var i = 0; i < siblings.length; i++) {
                                var sibling = siblings[i];
                                if (sibling == element) {
                                    index += 1;
                                    break;
                                } else if (sibling.nodeType == Node.ELEMENT_NODE && sibling.tagName == element.tagName) {
                                    index += 1;
                                }
                            }
                            var tagName = element.tagName.toLowerCase();
                            var pathIndex = '[' + index + ']';
                            paths.splice(0, 0, tagName + pathIndex);
                        }
                        return paths.length ? '/' + paths.join('/') : null;
                    }
                                document.addEventListener('click', function(event) {
                                let element = event.target;
                                let xpath = getElementXPath(element);
                                console.log(xpath);
                        localStorage.setItem('clickedElementXPath', xpath);
                    });
                    window.elemvalue = '';
                    document.addEventListener('input', function(event) {
                    let element = event.target;
                    if (element.type === 'text' || element.type === 'password') {
                        localStorage.setItem('clickedElementValeur', element.value);
                    }
                });
                
                ";

                ((IJavaScriptExecutor)driver).ExecuteScript(script);

                string scriptToRetrieveXPath = @"
                    return localStorage.getItem('clickedElementXPath');
                ";
                string scriptToRetrieveValue = @"
                    return localStorage.getItem('clickedElementValeur');
                ";

                string scriptToRetrieveResetValue = @"
                    return localStorage.setItem('clickedElementValeur' ,  null);
                ";
                string currPath = url;

                string previousXPath = null;
                string elemvalue = null;
                await Task.Run(async () =>
                {
                    while (!browserClosed)
                    {
                        try { 
                            string currentXPath = (string)((IJavaScriptExecutor)driver).ExecuteScript(scriptToRetrieveXPath);
                            string elemvalue = (string)((IJavaScriptExecutor)driver).ExecuteScript(scriptToRetrieveValue);
                            string newUrl = driver.Url;

                            if (currPath != newUrl)
                            {
                                ((IJavaScriptExecutor)driver).ExecuteScript(script);
                                currPath = newUrl;
                                Console.WriteLine("the Url is Changed to : ", newUrl);
                            }
                            if (!string.IsNullOrEmpty(currentXPath) && currentXPath != previousXPath)
                            {
                                if (previousXPath != null)
                                {
                                    recordedActions.Add(new RecordedAction
                                    {
                                        Timestamp = DateTime.Now,
                                        ActionDescription = "Entered text '" + elemvalue + "'",
                                        XPath = previousXPath
                                    });

                                    // Save recorded actions to JSON file
                                    string json = JsonConvert.SerializeObject(recordedActions, Newtonsoft.Json.Formatting.Indented);
                                    System.IO.File.WriteAllText(outputPath, json);

                                    // Read recorded actions from JSON file
                                    List<RecordedAction> actions = JsonToSpecFlow.ReadJson(outputPath);

                                    // Generate SpecFlow code
                                    //GenerateSpecFlowCode(actions, url, fakeId);

                                    StringBuilder specFlowCode = new StringBuilder();

                                    specFlowCode.AppendLine("using TechTalk.SpecFlow;");
                                    specFlowCode.AppendLine("using OpenQA.Selenium;");
                                    specFlowCode.AppendLine("using OpenQA.Selenium.Chrome;");
                                    specFlowCode.AppendLine();
                                    specFlowCode.AppendLine("[Binding]");
                                    specFlowCode.AppendLine("public class SpecFlowSteps");
                                    specFlowCode.AppendLine("{");
                                    specFlowCode.AppendLine("    private IWebDriver driver;");
                                    specFlowCode.AppendLine();
                                    specFlowCode.AppendLine("    [BeforeScenario]");
                                    specFlowCode.AppendLine("    public void Setup()");
                                    specFlowCode.AppendLine("    {");
                                    specFlowCode.AppendLine("        driver = new ChromeDriver();");
                                    specFlowCode.AppendLine("        driver.Manage().Window.Maximize();");
                                    specFlowCode.AppendLine("    }");
                                    specFlowCode.AppendLine();
                                    specFlowCode.AppendLine("    [AfterScenario]");
                                    specFlowCode.AppendLine("    public void TearDown()");
                                    specFlowCode.AppendLine("    {");
                                    specFlowCode.AppendLine("        driver.Quit();");
                                    specFlowCode.AppendLine("    }");
                                    specFlowCode.AppendLine();

                                    foreach (var action in recordedActions)
                                    {
                                        if (action.ActionDescription.StartsWith("Clicked element"))
                                        {
                                            specFlowCode.AppendLine($"    [When(@\"I click the element with XPath '{action.XPath}'\")]");
                                            specFlowCode.AppendLine("    public void WhenIClickTheElement()");
                                            specFlowCode.AppendLine("    {");
                                            specFlowCode.AppendLine($"        var element = driver.FindElement(By.XPath(\"{action.XPath}\"));");
                                            specFlowCode.AppendLine("        element.Click();");
                                            specFlowCode.AppendLine("    }");
                                            specFlowCode.AppendLine();
                                        }
                                        else if (action.ActionDescription.StartsWith("Entered text"))
                                        {
                                            var text = action.ActionDescription.Split('\'')[1];
                                            specFlowCode.AppendLine($"    [When(@\"I enter text '{text}' into the element with XPath '{action.XPath}'\")]");
                                            specFlowCode.AppendLine("    public void WhenIEnterTextIntoElement()");
                                            specFlowCode.AppendLine("    {");
                                            specFlowCode.AppendLine($"        var element = driver.FindElement(By.XPath(\"{action.XPath}\"));");
                                            specFlowCode.AppendLine($"        element.SendKeys(\"{text}\");");
                                            specFlowCode.AppendLine("    }");
                                            specFlowCode.AppendLine();
                                        }
                                    }

                                    specFlowCode.AppendLine("}");

                                    System.IO.File.WriteAllText(specFlowCodePath, specFlowCode.ToString());


                                    ((IJavaScriptExecutor)driver).ExecuteScript(scriptToRetrieveResetValue);

                                }
                            }

                            previousXPath = currentXPath;
                            await Task.Delay(500);
                        }
                        catch (WebDriverException)
                        {
                            browserClosed = true;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                driver?.Quit();
            }
            string jsonContent = System.IO.File.ReadAllText(outputPath);
            string specFlowContent = System.IO.File.ReadAllText(specFlowCodePath);

            var responseContent = new
            {
                JsonFileContent = jsonContent,
                SpecFlowFileContent = specFlowContent
            };

            return Ok(responseContent);
        }
/*            private void GenerateSpecFlowCode(List<RecordedAction> recordedActions, string url, string fakeId)
        {
            StringBuilder specFlowCode = new StringBuilder();

            specFlowCode.AppendLine("using TechTalk.SpecFlow;");
            specFlowCode.AppendLine("using OpenQA.Selenium;");
            specFlowCode.AppendLine("using OpenQA.Selenium.Chrome;");
            specFlowCode.AppendLine();
            specFlowCode.AppendLine("[Binding]");
            specFlowCode.AppendLine("public class SpecFlowSteps");
            specFlowCode.AppendLine("{");
            specFlowCode.AppendLine("    private IWebDriver driver;");
            specFlowCode.AppendLine();
            specFlowCode.AppendLine("    [BeforeScenario]");
            specFlowCode.AppendLine("    public void Setup()");
            specFlowCode.AppendLine("    {");
            specFlowCode.AppendLine("        driver = new ChromeDriver();");
            specFlowCode.AppendLine("        driver.Manage().Window.Maximize();");
            specFlowCode.AppendLine("    }");
            specFlowCode.AppendLine();
            specFlowCode.AppendLine("    [AfterScenario]");
            specFlowCode.AppendLine("    public void TearDown()");
            specFlowCode.AppendLine("    {");
            specFlowCode.AppendLine("        driver.Quit();");
            specFlowCode.AppendLine("    }");
            specFlowCode.AppendLine();

            foreach (var action in recordedActions)
            {
                if (action.ActionDescription.StartsWith("Clicked element"))
                {
                    specFlowCode.AppendLine($"    [When(@\"I click the element with XPath '{action.XPath}'\")]");
                    specFlowCode.AppendLine("    public void WhenIClickTheElement()");
                    specFlowCode.AppendLine("    {");
                    specFlowCode.AppendLine($"        var element = driver.FindElement(By.XPath(\"{action.XPath}\"));");
                    specFlowCode.AppendLine("        element.Click();");
                    specFlowCode.AppendLine("    }");
                    specFlowCode.AppendLine();
                }
                else if (action.ActionDescription.StartsWith("Entered text"))
                {
                    var text = action.ActionDescription.Split('\'')[1];
                    specFlowCode.AppendLine($"    [When(@\"I enter text '{text}' into the element with XPath '{action.XPath}'\")]");
                    specFlowCode.AppendLine("    public void WhenIEnterTextIntoElement()");
                    specFlowCode.AppendLine("    {");
                    specFlowCode.AppendLine($"        var element = driver.FindElement(By.XPath(\"{action.XPath}\"));");
                    specFlowCode.AppendLine($"        element.SendKeys(\"{text}\");");
                    specFlowCode.AppendLine("    }");
                    specFlowCode.AppendLine();
                }
            }

            specFlowCode.AppendLine("}");

            string specFlowCodePath = $@"E:\StagePFE_TriWeb\TriExport\specFlowSteps{fakeId}.cs";
            System.IO.File.WriteAllText(specFlowCodePath, specFlowCode.ToString());
        }
*/


    }

}