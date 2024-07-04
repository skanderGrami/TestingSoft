using Bogus;
using Microsoft.EntityFrameworkCore;

namespace TestingSoft_Backend.Repositories.TestCaseRepo
{
    public class TestCaseRepository : ITestCaseRepository
    {
        private readonly TestingSoftContext _context;
        
        public TestCaseRepository(TestingSoftContext context)
        {
            _context = context;
        }

        public async Task<TestCase> AddTestCase(TestCase testCase)
        {
            _context.TestCases.Add(testCase);
            await _context.SaveChangesAsync();
            Console.WriteLine(testCase);
            return testCase;
            
        }

        public async Task<bool> DeleteTestCase(int id)
        {
            var testCase = await _context.TestCases.FindAsync(id);
            if (testCase == null)
            {
                return false;
            }

            _context.TestCases.Remove(testCase);
            await _context.SaveChangesAsync();
            return true;
        }



        public void ExecuteTestAsync(int inTestCaseId)
        {
            // Utilisez la logique de la classe Test pour exécuter le test
            //var test = new TestingSoft_Backend.Test(@"E:\StagePFE_TriWeb\PlatformProject\TestingSoft_Backend\TestingSoft_Backend\ReportsVideo");
            var test = new TestingSoft_Backend.Test(Path.Combine(Directory.GetCurrentDirectory(), "ReportsVideo")); 
            
            try
            {
                TestCase testcase = _context.TestCases.Find(inTestCaseId);
                List<Scenario> scenarios =  _context.Scenarios.Where(scenario => scenario.TestCaseId== inTestCaseId).ToList();

                TestReport NewTestReport = new TestReport
                {
                    TestCaseId = inTestCaseId,
                    FilePathC = test.bdCsharpFilePath,
                    FilePathJson = test.bdJsonFilePath,
                    FilePathVideo = test.bdVideoFilePath
                };

                _context.TestReports.Add(NewTestReport);
                _context.SaveChanges();


                test.SetUp();
                test.ExecuteTest(testcase, scenarios);
            }
            finally
            {
                test.TearDown();
            }
        }

        public async Task<TestCase> GetTestCaseById(int id)


        {
            TestCase? testCase  =  await _context.TestCases
                .Where(x => x.TestCaseId == id)
                .Select(x => new TestCase()
                {
                    Builds = x.Builds,
                    Scenarios = x.Scenarios,
                    TestCaseId = x.TestCaseId,
                    Navigator = x.Navigator,
                    TestCaseCreatedDate = x.TestCaseCreatedDate,
                    TestCaseDescription = x.TestCaseDescription,
                    TestCaseName = x.TestCaseName,
                    TestCaseUpdatedDate = x.TestCaseUpdatedDate,
                    TestReports = x.TestReports,
                    TestSuite = x.TestSuite,
                    TestSuiteId = x.TestSuiteId,
                    Url = x.Url,
                    User = x.User,
                    UserId = x.UserId,
                    VersionTest = x.VersionTest
                }).FirstOrDefaultAsync();

            if (testCase == null)
            {
                return null!;
            }

            // Lire les fichiers JSON et C# associés au rapport de test le plus récent
            var latestTestReport = testCase.TestReports.OrderByDescending(tr => tr.TestReportId).FirstOrDefault();

            if (latestTestReport != null)
            {
                try
                {
                    testCase.JsonContent = await System.IO.File.ReadAllTextAsync(latestTestReport.FilePathJson);
                    testCase.CsharpContent = await System.IO.File.ReadAllTextAsync(latestTestReport.FilePathC);
                }
                catch (Exception ex)
                {
                    // Log or handle the exception if necessary
                    Console.WriteLine($"Error reading test report files: {ex.Message}");
                }
            }

            return testCase!;
        }

        public async Task<List<TestCase>> GetTestCases()
        {
            return await _context.TestCases.ToListAsync();
        }

        public async Task<TestCase> UpdateTestCase(int id, TestCase testCase)
        {
            if (id != testCase.TestCaseId)
            {
                throw new ArgumentException("Id mismatch");
            }

            _context.Entry(testCase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestCaseExists(id))
                {
                    throw new KeyNotFoundException($"TestCase with ID {id} not found");
                }
                else
                {
                    throw;
                }
            }

            return testCase;        
        }
 

        private bool TestCaseExists(int id)
        {
            return _context.TestCases.Any(t => t.TestCaseId == id);
        }



    }
}
