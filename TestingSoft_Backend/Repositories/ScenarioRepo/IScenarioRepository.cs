using OpenQA.Selenium;

namespace TestingSoft_Backend.Repositories.ScenarioRepo
{
    public interface IScenarioRepository
    {
        Task<List<Scenario>> GetScenarios();
        Task<Scenario> GetScenarioById(int id);
        Task<Scenario> AddScenario(Scenario scenario);
        Task<Scenario> UpdateScenario(int id, Scenario scenario);
        Task<bool> DeleteScenario(int id);
        Task<Scenario> Save(Scenario scenario, TestCase testCase);
        Task<string> CreateXPath(IWebElement element, IWebDriver driver);
        Task<List<IWebElement>> Rec(string url, IWebDriver driver);
        Task<Scenario> Recherche(string path, List<Scenario> ab);
        Task<Scenario> Recherche2(string path, List<Scenario> ab, string url);
        Task<int> NbrMaj(string chaine);
        Task<int> NbrMin(string chaine);
        Task Traitment(TestCase tc1, string Emplacement);
        Task<string> RemplirF(Scenario s);
        Task RemplirStep(TestCase tc1, string emplacement);
        Task <string> ChoixBrowserStep(TestCase tc1);
        Task<List<Scenario>> FindTestCaseScenario(TestCase tc1);
        Task Runner(string emplacement, TestCase tc1, string emplacementSP);
        Task XMLNuTest(TestCase tc1, string emplacement);
        Task CopyFolderPrincipal();
        Task CopyFolder(DirectoryInfo sourceFolder, DirectoryInfo destinationFolder);
        Task CreateFile(TestCase tc1);
        Task<string> ReadFile(TestCase tc1);
        Task RemplirBatch(TestCase tc1, string emplacement);
        Task RemplirBatchStandard(TestCase tc1, string emplacement, string emplacementXML);
    }
}
