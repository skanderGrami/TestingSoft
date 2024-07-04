namespace TestingSoft_Backend.Repositories.TestTypeRepo
{
    public interface ITestTypeRepository
    {
        Task<List<TestType>> GetTestTypes();
        Task<TestType> GetTestTypeById(int id);
        Task<TestType> AddTestTypec(TestType testType);
        Task<TestType> UpdateTestType(int id, TestType testType);
        Task<bool> DeleteTestType(int id);
    }
}
