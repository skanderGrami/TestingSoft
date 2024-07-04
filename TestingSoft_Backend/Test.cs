using System;
using System.Diagnostics;
using System.Xml.Linq;
using TestingSoft_Backend;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using Microsoft.IdentityModel.Tokens;
using System.Security.Policy;
using Bogus;

namespace TestingSoft_Backend
{
    [TestFixture]
    public class Test
    {
        private IWebDriver? driver;
        private string? Url;
        private string inputElements;
        private string folderPath;
        private string videoFilePath;
        private Process ffmpegProcess;
        private ScenarioExporter scenarioExporter;
        public string bdVideoFilePath;
        public string bdJsonFilePath;
        public string bdCsharpFilePath;
        private string fakeId;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
        }

        public Test(string folderPath)
        {
            this.folderPath = folderPath;

            var faker = new Faker();
            fakeId = faker.Random.Guid().ToString();

            videoFilePath = System.IO.Path.Combine(folderPath, $"TestVideo{fakeId}.mp4");
            scenarioExporter = new ScenarioExporter();
            bdVideoFilePath= System.IO.Path.Combine("ReportsVideo", $"TestVideo{fakeId}.mp4");
            bdJsonFilePath = System.IO.Path.Combine("ReportsVideo", $"testCase{fakeId}.json");
            bdCsharpFilePath = System.IO.Path.Combine("ReportsVideo", $"testCase{fakeId}.cs");

        }

        private IWebElement FindElementByScenario(Scenario scenario , IWebDriver driver)
        {
            Console.WriteLine($"Type de scénario : {scenario.IdTypeValue}");
            
            switch (scenario.IdTypeValue)
            {
                case 1:
                   return driver.FindElement(By.Id(scenario.TagValue));
                case 2:
                    return driver.FindElement(By.Name(scenario.TagValue));
                case 3:
                    return driver.FindElement(By.ClassName(scenario.TagValue));
                case 4:
                    return driver.FindElement(By.CssSelector(scenario.TagValue));
                case 5:
                    return driver.FindElement(By.XPath(scenario.TagValue));
                case 6:
                    return driver.FindElement(By.TagName(scenario.TagValue));
                case 7:
                    return driver.FindElement(By.PartialLinkText(scenario.TagValue));
                case 8:
                    return driver.FindElement(By.LinkText(scenario.TagValue));
             
                default:
                    return null;
            } 
        }

        [Test]
        public void ExecuteTest(TestCase testCase, List<Scenario> scenarios)
        {
            StartVideoRecording();
            try
            {
               
                driver?.Navigate().GoToUrl(testCase.Url);
                Console.WriteLine($"Exécution du test '{testCase.TestCaseName}' sur '{testCase.Url}'...");

                // Capturer l'action de navigation
                scenarioExporter.CaptureTestAction($"Navigated to '{Url}'");



                Thread.Sleep(2000); // Attendre pour la démonstration

                StartPerformanceMonitoring();

                foreach (Scenario scenario in scenarios)
                {
                  
                    var a = scenario.IdTypeValue.ToString();
                    IWebElement element = FindElementByScenario(scenario, driver);
                     
                    if (scenario.IdTypeValue == null || string.IsNullOrEmpty(a) || string.IsNullOrEmpty(scenario.TagValue))
                    {
                        Console.WriteLine("Le scénario est null ou manque des informations.");
                        continue;
                    }

                    if (element != null)
                    {
                        Thread.Sleep(2000);
                        switch (scenario.Commande)
                        {
                            case "input":
                                element.SendKeys(scenario.Value);
                                scenarioExporter.CaptureTestAction($"Entered text '{scenario.Value}' in element identified by '{scenario.TagValue}'");
                                break;
                            case "button":
                                element.Click();
                                scenarioExporter.CaptureTestAction($"Clicked button with tag value '{scenario.TagValue}'");
                                break;
                            case "select":
                                var selectElement = new SelectElement(element);
                                selectElement.SelectByValue(scenario.Value);
                                scenarioExporter.CaptureTestAction($"Selected option with value '{scenario.Value}' from select element");
                                break;
                            case "radio":
                                element.Click();
                                scenarioExporter.CaptureTestAction($"Clicked radio button with tag value '{scenario.TagValue}'");
                                break;
                            default:
                                Console.WriteLine($"Unknown command '{scenario.Commande}'");
                                break;
                        }
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("L'élément n'a pas été trouvé.");
                    }
                }

                StopPerformanceMonitoring();

                bool testPassed = PerformTestValidation();
                if (testPassed)
                {
                    Console.WriteLine("Test réussi !");
                }
                else
                {
                    Console.WriteLine("Échec du test.");
                }
                Thread.Sleep(30000); // For demonstration, waiting for 10 seconds

                Console.WriteLine("Test execution completed.");

               
                string jsonOutputPath = $@"E:\StagePFE_TriWeb\PlatformProject\TestingSoft_Backend\TestingSoft_Backend\ReportsVideo\testCase{fakeId}.json";
                scenarioExporter.ExportScenarioCode(jsonOutputPath);

                // Lire les actions depuis le fichier JSON exporté
                List<ScenarioExporter.TestAction> actions = scenarioExporter.ReadTestActions(jsonOutputPath);

                // Générer le code SpecFlow à partir des actions
                string specFlowCode = scenarioExporter.GenerateNUnitTestCode(actions);

                // Exporter le code SpecFlow dans un fichier
                string specFlowOutputPath = $@"E:\StagePFE_TriWeb\PlatformProject\TestingSoft_Backend\TestingSoft_Backend\ReportsVideo\testCase{fakeId}.cs";
                File.WriteAllText(specFlowOutputPath, specFlowCode);

                Console.WriteLine("SpecFlow code generated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during test execution: {ex.Message}");
            }
            finally
            {
                StopVideoRecording();
            }
        }
        private void StartVideoRecording()
        {
            // Specify the ffmpeg command to execute for video recording
            string ffmpegCommand = $"-f gdigrab -t 35 -i desktop {videoFilePath}";

            // Create a new process start info for ffmpeg
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "ffmpeg", // Specify the ffmpeg executable
                Arguments = ffmpegCommand,
                RedirectStandardOutput = true, // Redirect standard output for capturing ffmpeg output
                UseShellExecute = false, // Don't use shell execute
                CreateNoWindow = true // Don't create a window for the ffmpeg process
            };

            // Create and start the ffmpeg process
            ffmpegProcess = new Process
            {
                StartInfo = psi
            };
            ffmpegProcess.Start();
        }

        private void StopVideoRecording()
        {
            // Wait for the ffmpeg process to exit
            ffmpegProcess.WaitForExit();

            // Display a message indicating the completion of the video recording
            Console.WriteLine($"Video recorded to: {videoFilePath}");
        }

        private bool PerformTestValidation()
        {
            return true;
        }
        public void SetTestData(string url, string inputElements)
        {
            this.Url = url;
            this.inputElements = inputElements;
        }

        // Performance Monitoring Methods
        private Stopwatch stopwatch;

        private void StartPerformanceMonitoring()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        private void StopPerformanceMonitoring()
        {
            stopwatch.Stop();
            Console.WriteLine($"Performance Metrics - Total Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}


