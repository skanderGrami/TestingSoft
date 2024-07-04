using System.Formats.Asn1;
using System.Linq;
using Humanizer;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.CodeAnalysis.Scripting;
using OpenQA.Selenium;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestingSoft_Backend.Repositories.ScenarioRepo
{
    public class ScenarioRepository : IScenarioRepository
    {
        private readonly TestingSoftContext _context;
      
        string PathProject = Environment.CurrentDirectory;

        public ScenarioRepository(TestingSoftContext context)
        {
            _context = context;
    
        }
        public async Task<Scenario> AddScenario(Scenario scenario)
        {
            _context.Scenarios.Add(scenario);
            await _context.SaveChangesAsync();
            return scenario;
        }
        public async Task<bool> DeleteScenario(int id)
        {
            var scenario = await _context.Scenarios.FindAsync(id);
            if (scenario == null)
            {
                return false;
            }

            _context.Scenarios.Remove(scenario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Scenario> GetScenarioById(int id)
        {
            return await _context.Scenarios.FindAsync(id);
        }

        public async Task<List<Scenario>> GetScenarios()
        {
            return await _context.Scenarios.ToListAsync();
        }
        public async Task<Scenario> UpdateScenario(int id, Scenario scenario)
        {
            if (id != scenario.ScenarioId)
            {
                throw new ArgumentException("Id mismatch");
            }

            _context.Entry(scenario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScenarioExists(id))
                {
                    throw new KeyNotFoundException($"Scenario with ID {id} not found");
                }
                else
                {
                    throw;
                }
            }

            return scenario;
        }
        private bool ScenarioExists(int id)
        {
            return _context.Scenarios.Any(s => s.ScenarioId == id);
        }

        public async Task<string> CreateXPath(IWebElement element, IWebDriver driver)
        {
            return (string)((IJavaScriptExecutor)driver).ExecuteScript(@"
                function absoluteXPath(element) {
                    var comp, comps = [];
                    var parent = null;
                    var xpath = '';
                    var getPos = function(element) {
                        var position = 1, curNode;
                        if (element.nodeType == Node.ATTRIBUTE_NODE) {
                            return null;
                        }
                        for (curNode = element.previousSibling; curNode; curNode = curNode.previousSibling) {
                            if (curNode.nodeName == element.nodeName) {
                                ++position;
                            }
                        }
                        return position;
                    };
                    if (element instanceof Document) {
                        return '/';
                    }
                    for (; element && !(element instanceof Document); element = element.nodeType == Node.ATTRIBUTE_NODE ? element.ownerElement : element.parentNode) {
                        comp = comps[comps.length] = {};
                        switch (element.nodeType) {
                            case Node.TEXT_NODE:
                                comp.name = 'text()';
                                break;
                            case Node.ATTRIBUTE_NODE:
                                comp.name = '@' + element.nodeName;
                                break;
                            case Node.PROCESSING_INSTRUCTION_NODE:
                                comp.name = 'processing-instruction()';
                                break;
                            case Node.COMMENT_NODE:
                                comp.name = 'comment()';
                                break;
                            case Node.ELEMENT_NODE:
                                comp.name = element.nodeName;
                                break;
                        }
                        comp.position = getPos(element);
                    }
                    for (var i = comps.length - 1; i >= 0; i--) {
                        comp = comps[i];
                        xpath += '/' + comp.name;
                        if (comp.position !== null) {
                            xpath += '[' + comp.position + ']';
                        }
                    }
                    return xpath;
                }
                return absoluteXPath(arguments[0]);
            "
            , element);
            /*Script JavaScript intégré:
            Fonction absoluteXPath :
            Reçoit un élément DOM et génère son XPath absolu.
            Fonction getPos :
            Détermine la position de l'élément parmi ses frères de même type.
            Traversée du DOM:
            Si l'élément est un document, retourne "/".
            Traverse l'arbre DOM depuis l'élément donné jusqu'à la racine, en construisant une liste de composants de chemin (comps).
            Construction du XPath:
            Construit le XPath en partant de la racine jusqu'à l'élément en utilisant les noms des nœuds et leurs positions respectives.*/
        } 

        public async Task<int> NbrMaj(string chaine)
        {
            int compteur = 0;

            foreach (char ch in chaine)
            {
                if (char.IsLower(ch))
                {
                    compteur++;
                }
            }

            return compteur;
        }

        public async Task<int> NbrMin(string chaine)
        {
            int compteur = 0;

            foreach (char ch in chaine)
            {
                if (char.IsUpper(ch))
                {
                    compteur++;
                }
            }

            return compteur;
        }

        public async Task<List<IWebElement>> Rec(string url, IWebDriver driver)
        {
            try
            {
                // Check if the window handle is valid
                if (driver.WindowHandles.Count > 0)
                {
                    driver.Navigate().GoToUrl(url);
                }
                else
                {
                    throw new NoSuchWindowException("The target window is already closed.");
                }

                // Find all elements on the page
                List<IWebElement> elements = driver.FindElements(By.XPath("//*")).ToList();

                return elements;
            }
            catch (NoSuchWindowException ex)
            {
                // Log the error (use a logging framework or write to console for simplicity)
                Console.WriteLine($"Error: {ex.Message}");

                // Optionally, you can rethrow the exception or return an empty list
                throw;
            }
            catch (Exception ex)
            {
                // Log unexpected errors
                Console.WriteLine($"Unexpected error: {ex.Message}");

                // Optionally, you can rethrow the exception or return an empty list
                throw;
            }
        }

        public async Task<Scenario> Recherche(string path, List<Scenario> ab)
        {
            if (ab == null)
            {
                throw new ArgumentNullException(nameof(ab), "La liste de scénarios ne doit pas être nulle.");
            }

            Scenario i1 = new Scenario();

            foreach (var scenario in ab)
            {
                if (scenario?.Path != null && scenario.Path.Equals(path))
                {
                    i1.Commande = scenario.Commande;
                    i1.Path = scenario.Path;
                    i1.Value = scenario.Value;
                    break;
                }
            }

            return i1;
        }

        public async Task<Scenario> Recherche2(string path, List<Scenario> ab, string url)
        {
            if (ab == null)
            {
                throw new ArgumentNullException(nameof(ab), "La liste de scénarios ne doit pas être nulle.");
            }
            Scenario i1 = new Scenario();

            foreach (var scenario in ab)
            {
                if (scenario?.Path != null && scenario.Path.Equals(path) && scenario?.Url != null && scenario.Url.Equals(url))
                {
                    i1.Commande = scenario.Commande;
                    i1.Path = scenario.Path;
                    i1.Value = scenario.Value;
                    i1.Url = scenario.Url;
                    break;
                }
            }

            return i1;
        }

        public async Task<string> RemplirF(Scenario s)
        {
            string instruction = "";
            if (s.Commande.Equals("open"))
            {
                instruction = "When User opens URL \"" + s.Path + "\"";
            }
            else if (s.Path.Contains("dp-date-picker") && s.Path.Contains("input"))
            {
                instruction = "And User clic in dp has xpath \"" + s.Path + "\"";
            }
            else if (s.Path.Equals("/html[1]/body[1]/modal-container[1]/div[1]/div[1]/div[2]/file-drop[1]/div[1]/div[1]/form[1]/div[1]/div[1]/div[1]/div[1]/div[1]/ng-selectize[1]/div[1]/div[1]/input[1]") && s.Value.Equals(""))
            {
                instruction = "And User clic in selectize has xpath \"" + s.Path + "\" and Value as \"Exceptionnel\"";
            }
            else if (s.Path.Contains("/html[1]/body[1]/modal-container[1]/div[1]/div[1]/div[2]/file-drop[1]/div[1]/div[1]/form[1]/div[1]/div[1]/div[2]/div[1]/div[1]/ng-selectize[1]/div[1]/div[1]/input[1]") && s.Value.Equals(""))
            {
                instruction = "And User clic in selectize has xpath \"" + s.Path + "\" and Value as \"Décès conjoint/père/mère/enfant\"";
            }
            else if (s.Path.Contains("selectize") && !s.Value.Equals(""))
            {
                instruction = "And User clic in selectize has xpath \"" + s.Path + "\" and Value as \"" + s.Value + "\"";
            }
            else if (s.Path.Contains("input") && s.Value.Equals("radio"))
            {
                instruction = "And User clic in input radio has xpath \"" + s.Path + "\"";
            }
            else if (s.Path.Contains("input") && !s.Value.Equals(""))
            {
                instruction = "And User clic in input has xpath \"" + s.Path + "\" and Value as \"" + s.Value + "\"";
            }
            else if (s.Path.Contains("textarea") && !s.Value.Equals(""))
            {
                instruction = "And User clic in textarea has xpath \"" + s.Path + "\" and Value as \"" + s.Value + "\"";
            }
            else if (s.Path.Contains("input") && s.Value.Equals(""))
            {
                instruction = "And User clic in input submit has xpath \"" + s.Path + "\"";
            }
            else if (s.Path.Contains("button"))
            {
                instruction = "And User clic in button has xpath \"" + s.Path + "\"";
            }
            else if (s.Path.Contains("select") && !s.Value.Equals(""))
            {
                instruction = "And User clic in select has xpath \"" + s.Path + "\" and Value as \"" + s.Value + "\"";
            }
            else if (s.Path.Contains("a[1]"))
            {
                instruction = "And User clic in lien has xpath \"" + s.Path + "\"";
            }
            else
            {
                instruction = "And User clic on element has xpath \"" + s.Path + "\"";
            }

            return instruction;
        }

        public async Task Traitment(TestCase tc1, string emplacement)
        {
            string instru = "";
            List<Scenario> sce = (List<Scenario>)tc1.Scenarios;
            string nameFile = tc1.TestCaseName.Trim().Replace(' ', '_') + tc1.TestCaseId;
            string filePath = Path.Combine(emplacement, nameFile + ".feature");

            using (var streamWriter = new StreamWriter(filePath))
            {
                streamWriter.WriteLine("Feature: " + tc1.TestCaseName);
                streamWriter.WriteLine();

                streamWriter.WriteLine("Scenario: " + tc1.TestCaseDescription);
                streamWriter.WriteLine("Given User Launch \"" + tc1.Navigator + "\"");

                foreach (var we in sce)
                {
                    instru = await RemplirF(we);
                    streamWriter.WriteLine(instru);
                }

                streamWriter.WriteLine("And close browser");
            }

            Console.WriteLine("Success...");
        }

        public async Task RemplirStep(TestCase tc1, string emplacement)
        {
            string instructionS = "";
            string S = "";
            string func = "";
            string Brow = await ChoixBrowserStep(tc1);
            FindTestCaseScenario(tc1);
            string FolderName = tc1.TestCaseName.Trim().Replace(' ', '_') + tc1.TestCaseId;
            Directory.CreateDirectory(emplacement + FolderName);
            string StepFileName = "Step_" + tc1.TestCaseName.Trim().Replace(' ', '_') + tc1.TestCaseId;
            using (FileStream fout = new FileStream(emplacement + FolderName + "/" + StepFileName + ".cs", FileMode.Append))
            using (StreamWriter writer = new StreamWriter(fout))
            {
                string Debut = "using OpenQA.Selenium;\nusing OpenQA.Selenium.Chrome;\nusing System;\nusing System.Collections.Generic;\nusing System.Threading;\nusing OpenQA.Selenium.Support.UI;\n\nnamespace Step." + FolderName + "\n{\n\tpublic class " + StepFileName + "\n\t{\n\t\tpublic IWebDriver driver;\n\t\tpublic WebDriverWait wait;\n";
                writer.Write(Debut);

                List<string> Tab = new List<string>();
                string NameFile = tc1.TestCaseName.Trim().Replace(' ', '_') + tc1.TestCaseId;

                using (StreamReader lecteurAvecBuffer = new StreamReader("./src/main/java/Test./" + NameFile + "/" + NameFile + ".feature"))
                {
                    Tab.Add("Skander");

                    string line;
                    while ((line = lecteurAvecBuffer.ReadLine()) != null)
                    {
                        byte[] b;
                        if (line.Contains("Given User Launch") && !Tab.Contains("Given User Launch"))
                        {
                            S = "User Launch {string}";
                            func = "public void user_Launch(string stringParam) {\n" + Brow + "\t   \n\t}";
                            instructionS = $"[Given(\"{S}\")]\n{func}\n";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                            Tab.Add("Given User Launch");
                        }
                        else if (line.Contains("When User opens URL") && !Tab.Contains("When User opens URL"))
                        {
                            S = "User opens URL {string}";
                            func = "public void user_opens_URL(string url) {\n\t   \n\t\tdriver.Navigate().GoToUrl(url);\n\t}";
                            instructionS = $"[When(\"{S}\")]\n{func}\n";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                            Tab.Add("When User opens URL");
                        }
                        else if (line.Contains("And User clic in dp has xpath") && !Tab.Contains("And User clic in dp has xpath"))
                        {
                            S = "User clic in dp has xpath {string}";
                            func = "public void user_clic_in_dp_has_xpath(string stringParam) {\n\t\tdriver.FindElement(By.XPath(stringParam)).SendKeys(\"\");\n\t\tThread.Sleep(2000);\n\t}";
                            instructionS = $"[When(\"{S}\")]\n{func}\n";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                            Tab.Add("And User clic in dp has xpath");
                        }
                        else if (line.Contains("And User clic in selectize has xpath") && line.Contains("and Value as") && !Tab.Contains("User clic in selectize has xpath"))
                        {
                            S = "User clic in selectize has xpath {string} and Value as {string}";
                            func = "public void user_clic_in_selectize_has_xpath_and_Value_as(string stringParam1, string stringParam2) {\n\t\tdriver.FindElement(By.XPath(stringParam1)).Clear(); \n\t\tdriver.FindElement(By.XPath(stringParam1)).SendKeys(stringParam2); \n\t\tdriver.FindElement(By.XPath(stringParam1)).SendKeys(Keys.Enter);\n\t}";
                            instructionS = $"[When(\"{S}\")]\n{func}\n";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                            Tab.Add("User clic in selectize has xpath");
                        }
                        else if (line.Contains("And User clic in input has xpath") && line.Contains("and Value as") && !Tab.Contains("User clic in input has xpath"))
                        {
                            S = "User clic in input has xpath {string} and Value as {string}";
                            func = "public void user_clic_in_input_has_xpath_and_Value_as(string stringParam1, string stringParam2) {\n\t\tdriver.FindElement(By.XPath(stringParam1)).Clear(); \n\t\tdriver.FindElement(By.XPath(stringParam1)).SendKeys(stringParam2); \n\t}";
                            instructionS = $"[When(\"{S}\")]\n{func}\n";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                            Tab.Add("User clic in input has xpath");
                        }
                        else if (line.Contains("And User clic in textarea has xpath") && line.Contains("and Value as") && !Tab.Contains("User clic in textarea has xpath"))
                        {
                            S = "User clic in textarea has xpath {string} and Value as {string}";
                            func = "public void user_clic_in_textarea_has_xpath_and_Value_as(string stringParam1, string stringParam2) {\n\t\tWebElement e;\n\t\te = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(stringParam1)));\n\t\te.Clear(); \n\t\te.SendKeys(stringParam2);\n\t}";
                            instructionS = $"[When(\"{S}\")]\n{func}\n";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                            Tab.Add("User clic in textarea has xpath");
                        }
                        else if (line.Contains("And User clic in input radio has xpath") && !Tab.Contains("User clic in input radio has xpath"))
                        {
                            S = "User clic in input radio has xpath {string}";
                            func = "public void user_clic_in_input_radio_has_xpath(string stringParam) {\n\t\tstring s = stringParam.Replace(\"input\", \"label\");\n\t\tWebElement r = driver.FindElement(By.XPath(s));\n\t\tif (r != null) {\n\t\t\tdriver.FindElement(By.XPath(s)).Click();\n\t\t} else {\n\t\t\tWebElement e = driver.FindElement(By.XPath(stringParam));\n\t\t\t((IJavaScriptExecutor)driver).ExecuteScript(\"arguments[0].checked = true;\", e);\n\t\t\te.Click();\n\t\t}\n\t}";
                            instructionS = $"[When(\"{S}\")]\n{func}\n";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                            Tab.Add("User clic in input radio has xpath");
                        }
                        else if (line.Contains("And User clic in input submit has xpath") && !Tab.Contains("User clic in input submit has xpath"))
                        {
                            S = "User clic in input submit has xpath {string}";
                            func = "public void user_clic_in_input_submit_has_xpath(string stringParam) {\n\t\tdriver.FindElement(By.XPath(stringParam)).Click();\n\t\tThread.Sleep(2000);\n\t}";
                            instructionS = $"[When(\"{S}\")]\n{func}\n";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                            Tab.Add("User clic in input submit has xpath");
                        }
                        else if (line.Contains("And User clic in button has xpath") && !Tab.Contains("User clic in button has xpath"))
                        {
                            S = "User clic in button has xpath {string}";
                            func = "public void user_clic_in_button_has_xpath(string stringParam) {\n\t\tWebElement e;\n\t\te = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(stringParam)));\n\t\t((IJavaScriptExecutor)driver).ExecuteScript(\"arguments[0].click();\", e);\n\t}";
                            instructionS = $"[When(\"{S}\")]\n{func}\n";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                            Tab.Add("User clic in button has xpath");
                        }
                        else if (line.Contains("And User clic in select has xpath") && line.Contains("and Value as") && !Tab.Contains("User clic in select has xpath"))
                        {
                            S = "User clic in select has xpath {string} and Value as {string}";
                            func = "public void user_clic_in_select_has_xpath(string stringParam1, string stringParam2) {\n\t\tSelectElement select = new SelectElement(driver.FindElement(By.XPath(stringParam1)));\n\t\tselect.SelectByText(stringParam2);\n\t}";
                            instructionS = $"[When(\"{S}\")]\n{func}\n";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                            Tab.Add("User clic in select has xpath");
                        }
                        else if (line.Contains("And User clic in lien has xpath") && !Tab.Contains("User clic in lien has xpath"))
                        {
                            S = "User clic in lien has xpath {string}";
                            func = "public void user_clic_in_lien_has_xpath(string stringParam) {\n\t\tint pos = stringParam.IndexOf(\"li\");\n\t\tstring s = stringParam.Substring(1, pos + 5);\n\t\tConsole.WriteLine(s);\n\t\tWebElement r = driver.FindElement(By.XPath(s));\n\t\tif (r != null) {\n\t\t\tdriver.FindElement(By.XPath(s)).Click();\n\t\t\tWebElement e;\n\t\t\te = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(stringParam)));\n\t\t\te.Click();\n\t\t} else {\n\t\t\tWebElement e;\n\t\t\te = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(stringParam)));\n\t\t\te.Click();\n\t\t}\n\t}";
                            instructionS = $"[When(\"{S}\")]\n{func}\n";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                            Tab.Add("User clic in lien has xpath");
                        }
                        else if (line.Contains("And close browser") && !Tab.Contains("And close browser"))
                        {
                            S = "close browser";
                            func = "public void close_browser() {\n\t\tdriver.Quit();\n\t}";
                            instructionS = $"[When(\"{S}\")]\n{func}\n";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                            Tab.Add("And close browser");
                        }
                        else if (line.Contains("And User clic on element has xpath") && !Tab.Contains("And User clic on element has xpath"))
                        {
                            S = "User clic on element has xpath {string}";
                            func = "public void user_clic_on_element_has_xpath(string stringParam) {\n\t\tWebElement e;\n\t\te = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(stringParam)));\n\t\te.Click();\n\t}";
                            instructionS = $"[When(\"{S}\")]\n{func}\n";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                            Tab.Add("And User clic on element has xpath");
                        }
                        else if (line.Contains("Feature:") || line.Contains("Scenario:") || line.Contains(""))
                        {
                            S = "";
                            instructionS = "";
                            b = System.Text.Encoding.UTF8.GetBytes(instructionS);
                            writer.Write(b);
                        }
                    }
                }

                instructionS = "}\n}";
                byte[] endBytes = System.Text.Encoding.UTF8.GetBytes(instructionS);
                writer.Write(endBytes);
            }
        }
        public async Task<string> ChoixBrowserStep(TestCase tc1)
        {
            string Browser = tc1.Navigator;
            string Brow = "";
            if (Browser.Equals("Google Chrome"))
            {
                Brow = "\t\tSystem.setProperty(\"webdriver.chrome.driver\",System.getProperty(\"user.dir\")+\"//lib/chromedriver.exe\") ; \r\n\t\tdriver=new ChromeDriver(); \r\ndriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);\r\nwait=new WebDriverWait(driver, TimeSpan.FromSeconds(50));\r\n";
            }

            if (Browser.Equals("Firefox"))
            {
                Brow = "\t\tSystem.setProperty(\"webdriver.gecko.driver\",System.getProperty(\"user.dir\")+\"//lib/geckodriver.exe\") ; \r\n\t\tdriver=new FirefoxDriver(); \r\ndriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);\r\nwait=new WebDriverWait(driver, TimeSpan.FromSeconds(50));\r\n";
            }

            return Brow;
        }

        public async Task<List<Scenario>> FindTestCaseScenario(TestCase tc1)
        {
            return await _context.Scenarios
                           .Where(s => s.TestCaseId == tc1.TestCaseId)
                           .ToListAsync();
        }


        public async Task Runner(string emplacement, TestCase tc1, string emplacementSP)
        {
            FindTestCaseScenario(tc1);
            string folderName1 = tc1.TestCaseName.Trim().Replace(' ', '_') + tc1.TestCaseId;
            string folderName = "Runner" + tc1.TestCaseName.Trim().Replace(' ', '_') + tc1.TestCaseId;
            string filePath = Path.Combine(emplacement, folderName + ".cs");

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                Console.WriteLine("Le fichier a été créé");
            }
            else
            {
                Console.WriteLine("Created");
            }

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                if (emplacement.Contains("E:\\"))
                {
                    emplacement = "./src/main/java/Test.//" + folderName1 + "/";
                }

                int p = emplacement.IndexOf(".", 1);
                emplacementSP = emplacement.Substring(0, p) + emplacement.Substring(p + 1);
                Console.WriteLine(emplacementSP);

                string debut = "using NUnit.Framework;\nusing TechTalk.SpecFlow;\n\nnamespace test." + folderName1 + "\n{\n\t[Binding]\n\tpublic class " + folderName + " : Steps\n\t{\n\t\t[BeforeScenario]\n\t\tpublic void Setup()\n\t\t{\n\t\t\t// Setup code here\n\t\t}\n\n\t\t[AfterScenario]\n\t\tpublic void Teardown()\n\t\t{\n\t\t\t// Teardown code here\n\t\t}\n\t}\n}";
                writer.WriteLine(debut);
            }
        }

        public async Task XMLNuTest(TestCase tc1, string emplacement)
        {
            FindTestCaseScenario(tc1);
            string folderName = "XMLNUnit" + tc1.TestCaseName.Trim().Replace(' ', '_') + tc1.TestCaseId;
            string filePath = Path.Combine(emplacement, folderName + ".xml");

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                Console.WriteLine("Le fichier a été créé");
            }
            else
            {
                Console.WriteLine("Created");
            }

            string nameFileRunner = "Runner" + tc1.TestCaseName.Trim().Replace(' ', '_') + tc1.TestCaseId;
            string nameFolderRunner = tc1.TestCaseName.Trim().Replace(' ', '_') + tc1.TestCaseId;

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                string debut = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!DOCTYPE suite SYSTEM \"https://nunit.org/\">\r\n";
                debut += "<!-- NUnit Test Configuration -->\r\n";
                debut += "<!-- Place your NUnit test configuration here -->\r\n";
                debut += "<!-- Example: -->\r\n";
                debut += "<!-- <NUnitConfiguration>\r\n";
                debut += "<!--   <TestRunners>\r\n";
                debut += "<!--     <TestRunner id=\"NUnit\"></TestRunner>\r\n";
                debut += "<!--   </TestRunners>\r\n";
                debut += "<!--   <TestPackages>\r\n";
                debut += "<!--     <TestPackage id=\"YourTestPackage\">\r\n";
                debut += "<!--       <TestAssembly path=\"YourTestAssembly.dll\"></TestAssembly>\r\n";
                debut += "<!--     </TestPackage>\r\n";
                debut += "<!--   </TestPackages>\r\n";
                debut += "<!-- </NUnitConfiguration> -->\r\n";
                debut += "-->\r\n";

                writer.WriteLine(debut);
            }
        }

        public async Task CopyFolderPrincipal()
        {
            string dirPath = Path.Combine(this.PathProject, "bin");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string sourceFolderTest = Path.Combine(this.PathProject, "target", "classes", "Test");
            string sourceFolderStep = Path.Combine(this.PathProject, "target", "classes", "step");
            string destinationFolderTest = Path.Combine(this.PathProject, "bin", "Test");
            string destinationFolderStep = Path.Combine(this.PathProject, "bin", "step");

            CopyFolder(new DirectoryInfo(sourceFolderTest), new DirectoryInfo(destinationFolderTest));
            CopyFolder(new DirectoryInfo(sourceFolderStep), new DirectoryInfo(destinationFolderStep));
        }
        public async Task CopyFolder(DirectoryInfo sourceFolder, DirectoryInfo destinationFolder)
        {
            if (!destinationFolder.Exists)
            {
                destinationFolder.Create();
                Console.WriteLine("Directory created :: " + destinationFolder.FullName);
            }

            foreach (FileInfo file in sourceFolder.GetFiles())
            {
                string destFile = Path.Combine(destinationFolder.FullName, file.Name);
                file.CopyTo(destFile, true);
            }

            foreach (DirectoryInfo subFolder in sourceFolder.GetDirectories())
            {
                string destFolder = Path.Combine(destinationFolder.FullName, subFolder.Name);
                CopyFolder(subFolder, new DirectoryInfo(destFolder));
            }
        }

        public async Task CreateFile(TestCase tc1)
        {
            string emplacement = "./";
            string filePath = Path.Combine(emplacement, "EnregTS.txt");

            try
            {
                // Création du fichier s'il n'existe pas
                if (!File.Exists(filePath))
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        Console.WriteLine("Le fichier a été créé");
                    }
                }
                else
                {
                    Console.WriteLine("Le fichier existe déjà");
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Erreur lors de la création du fichier");
                return;
            }

            try
            {
                // Ajout du nouvel ID de cas de test à la fin du fichier
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine();
                    sw.Write(tc1.TestCaseId);
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Erreur lors de l'écriture dans le fichier");
            }
        }
    

        public async Task<string> ReadFile(TestCase tc1)
        {
            string emplacement = "./EnregTS.txt";
            string lastLine = "";

            try
            {
                // Lecture du fichier et récupération de la dernière ligne
                using (StreamReader sr = new StreamReader(emplacement))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lastLine = line;
                    }
                }

                Console.WriteLine("La dernière ligne : " + lastLine);
            }
            catch (IOException)
            {
                Console.WriteLine("Erreur lors de la lecture du fichier");
            }

            return lastLine;
        }

        public async Task RemplirBatch(TestCase tc1, string emplacement)
        {
            FindTestCaseScenario(tc1);

            string folderName = "Batch" + tc1.TestCaseName.Trim().Replace(' ', '_') + tc1.TestCaseId;
            string filePath = Path.Combine(emplacement, folderName + ".bat");

            Console.WriteLine(filePath);

            try
            {
                // Création du fichier batch
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine("cd " + this.PathProject);
                    sw.WriteLine("set classpath=" + this.PathProject + "\\lib\\*;" + this.PathProject + "\\bin");

                    string xmlFile = "XMLTestNG" + tc1.TestCaseName.Trim().Replace(' ', '_') + tc1.TestCaseId + ".xml";
                    string emplacementSP = emplacement.Replace("\"", "\\\"");
                    sw.WriteLine("java  org.testng.TestNG " + this.PathProject + emplacementSP + xmlFile);
                    sw.WriteLine("pause");
                }

                Console.WriteLine("Le fichier a été créé");
            }
            catch (IOException)
            {
                Console.WriteLine("Erreur lors de la création du fichier batch");
            }
        }

        public async Task RemplirBatchStandard(TestCase tc1, string emplacement, string emplacementXML)
        {
            FindTestCaseScenario(tc1);
            string folderName1 = tc1.TestCaseName.Trim().Replace(' ', '_') + tc1.TestCaseId;
            string batchFilePath = Path.Combine(emplacement, "Batch.bat");

            try
            {
                // Supprimer le fichier batch s'il existe déjà
                if (File.Exists(batchFilePath))
                {
                    File.Delete(batchFilePath);
                }

                Console.WriteLine(batchFilePath);

                // Création du fichier batch
                using (StreamWriter sw = File.CreateText(batchFilePath))
                {
                    int p = emplacementXML.IndexOf(".", 1);
                    string emplacementSP = emplacementXML.Substring(1, p) + emplacementXML.Substring(p + 1);
                    string e = emplacementSP.Replace("\"", "\\\"");
                    string xmlFile = "XMLUnTest" + tc1.TestCaseName.Trim().Replace(' ', '_') + tc1.TestCaseId + ".xml";

                    sw.WriteLine("cd " + this.PathProject);
                    sw.WriteLine("set classpath=" + this.PathProject + "\\lib\\*;" + this.PathProject + "\\bin");
                    sw.WriteLine("java  org.testng.TestNG " + this.PathProject + e + folderName1 + "/" + xmlFile);
                    sw.WriteLine("pause");
                }

                Console.WriteLine("Le fichier a été créé");
            }
            catch (IOException)
            {
                Console.WriteLine("Erreur lors de la création du fichier batch");
            }
        }

        public async Task<Scenario> Save(Scenario scenario, TestCase testCase)
        {
            Console.WriteLine(scenario.ToString());
            scenario.Commande = scenario.Commande;
            scenario.Path = scenario.Path;
            scenario.Value = scenario.Value;
            scenario.Url = scenario.Url;
            scenario.TestCase = testCase;

            _context.Scenarios.Update(scenario);
            await _context.SaveChangesAsync();

            return scenario;
        }
    }

}
