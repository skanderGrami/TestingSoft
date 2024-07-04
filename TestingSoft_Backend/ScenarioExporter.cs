using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestingSoft_Backend
{
    public class ScenarioExporter
    {
        private List<TestAction> testActions = new List<TestAction>(); // Liste pour stocker les actions de test

        // Méthode pour capturer et exporter le code de scénario
        public void ExportScenarioCode(string outputPath)
        {
            // Convertir la liste d'actions en JSON
            string json = JsonConvert.SerializeObject(testActions, Formatting.Indented);
            // Écrire le JSON dans un fichier
            File.WriteAllText(outputPath, json);
        }

        // Méthode pour capturer une action de test
        public void CaptureTestAction(string actionDescription)
        {
            TestAction action = new TestAction
            {
                Timestamp = DateTime.Now,
                ActionDescription = actionDescription
            };
            testActions.Add(action);
        }

        // Classe pour représenter une action de test
        public class TestAction
        {
            public DateTime Timestamp { get; set; }
            public string ActionDescription { get; set; }
        }
        // Méthode pour générer du code SpecFlow à partir des actions de test
        public string GenerateSpecFlowCode(List<TestAction> actions)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using TechTalk.SpecFlow;");
            sb.AppendLine("using OpenQA.Selenium;");
            sb.AppendLine("using OpenQA.Selenium.Chrome;");
            sb.AppendLine();
            sb.AppendLine("[Binding]");
            sb.AppendLine("public class WebTestSteps");
            sb.AppendLine("{");
            sb.AppendLine("    private IWebDriver driver;");
            sb.AppendLine();
            sb.AppendLine("    [Given(\"I have opened the browser\")]");
            sb.AppendLine("    public void GivenIHaveOpenedTheBrowser()");
            sb.AppendLine("    {");
            sb.AppendLine("        driver = new ChromeDriver();");
            sb.AppendLine("        driver.Manage().Window.Maximize();");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    [When(\"I perform the test actions\")]");
            sb.AppendLine("    public void WhenIPerformTheTestActions()");
            sb.AppendLine("    {");

            foreach (var action in actions)
            {
                if (action.ActionDescription.StartsWith("Navigated to"))
                {
                    var url = action.ActionDescription.Split('\'')[1];
                    sb.AppendLine($"        driver.Navigate().GoToUrl(\"{url}\");");
                }
                else if (action.ActionDescription.StartsWith("Entered text"))
                {
                    var parts = action.ActionDescription.Split('\'');
                    var text = parts[1];
                    var element = parts[3];
                    sb.AppendLine($"        driver.FindElement(By.Id(\"{element}\")).SendKeys(\"{text}\");");
                }
                else if (action.ActionDescription.StartsWith("Clicked button with tag value"))
                {
                    var tagValue = action.ActionDescription.Split('\'')[1];
                    sb.AppendLine($"        driver.FindElement(By.XPath(\"{tagValue}\")).Click();");
                }
                // Ajoutez d'autres actions au besoin
            }

            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    [Then(\"I close the browser\")]");
            sb.AppendLine("    public void ThenICloseTheBrowser()");
            sb.AppendLine("    {");
            sb.AppendLine("        driver.Quit();");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        // Méthode pour générer du code de test NUnit à partir des actions de test
        public string GenerateNUnitTestCode(List<TestAction> actions)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using NUnit.Framework;");
            sb.AppendLine("using OpenQA.Selenium;");
            sb.AppendLine("using OpenQA.Selenium.Chrome;");
            sb.AppendLine();
            sb.AppendLine("[TestFixture]");
            sb.AppendLine("public class WebTests");
            sb.AppendLine("{");
            sb.AppendLine("    private IWebDriver driver;");
            sb.AppendLine();
            sb.AppendLine("    [SetUp]");
            sb.AppendLine("    public void SetUp()");
            sb.AppendLine("    {");
            sb.AppendLine("        driver = new ChromeDriver();");
            sb.AppendLine("        driver.Manage().Window.Maximize();");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    [Test]");
            sb.AppendLine("    public void PerformTestActions()");
            sb.AppendLine("    {");

            foreach (var action in actions)
            {
                if (action.ActionDescription.StartsWith("Navigated to"))
                {
                    var url = action.ActionDescription.Split('\'')[1];
                    sb.AppendLine($"        driver.Navigate().GoToUrl(\"{url}\");");
                }
                else if (action.ActionDescription.StartsWith("Entered text"))
                {
                    var parts = action.ActionDescription.Split('\'');
                    var text = parts[1];
                    var element = parts[3];
                    sb.AppendLine($"        driver.FindElement(By.Id(\"{element}\")).SendKeys(\"{text}\");");
                }
                else if (action.ActionDescription.StartsWith("Clicked button with tag value"))
                {
                    var tagValue = action.ActionDescription.Split('\'')[1];
                    sb.AppendLine($"        driver.FindElement(By.XPath(\"{tagValue}\")).Click();");
                }
                // Ajoutez d'autres actions au besoin
            }

            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    [TearDown]");
            sb.AppendLine("    public void TearDown()");
            sb.AppendLine("    {");
            sb.AppendLine("        driver.Quit();");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        // Méthode pour lire le fichier JSON exporté
        public List<TestAction> ReadTestActions(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<TestAction>>(json);
        }
    }
}
