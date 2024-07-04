namespace TestingSoft_Backend.Repositories.TestSuiteRepo
{
    public interface ITestSuiteRepository
    {
        Task<List<TestSuite>> GetTestSuites();
        Task<TestSuite> GetTestSuiteById(int id);
        Task<TestSuite> AddTestSuite(TestSuite testSuite);
        Task<TestSuite> UpdateTestSuite(int id, TestSuite testSuite);
        Task<bool> DeleteTestSuite(int id);
    }
}
