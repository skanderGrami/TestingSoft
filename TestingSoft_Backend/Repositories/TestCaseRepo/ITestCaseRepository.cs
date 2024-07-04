namespace TestingSoft_Backend.Repositories.TestCaseRepo
{
    public interface ITestCaseRepository
    {
        Task<List<TestCase>> GetTestCases();
        Task<TestCase> GetTestCaseById(int id);
        Task<TestCase> AddTestCase(TestCase testCase);
        Task<TestCase> UpdateTestCase(int id, TestCase testCase);
        Task<bool> DeleteTestCase(int id);
        void  ExecuteTestAsync(int testCase);
    }
}
