namespace TestingSoft_Backend.Repositories.TestSuiteRepo
{
    public class TestSuiteRepository : ITestSuiteRepository
    {
        private readonly TestingSoftContext _context;

        public TestSuiteRepository(TestingSoftContext context)
        {
            _context = context;
        }
        public async Task<TestSuite> AddTestSuite(TestSuite testSuite)
        {
            _context.TestSuites.Add(testSuite);
            await _context.SaveChangesAsync();
            return testSuite;
        }

        public async Task<bool> DeleteTestSuite(int id)
        {
            var testSuite = await _context.TestSuites.FindAsync(id);
            if (testSuite == null)
            {
                return false;
            }

            _context.TestSuites.Remove(testSuite);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TestSuite> GetTestSuiteById(int id)
        {
            return await _context.TestSuites.FindAsync(id);
        }

        public async Task<List<TestSuite>> GetTestSuites()
        {
            return await _context.TestSuites.ToListAsync();
        }

        public async Task<TestSuite> UpdateTestSuite(int id, TestSuite testSuite)
        {
            if (id != testSuite.TestSuiteId)
            {
                throw new ArgumentException("Id mismatch");
            }

            _context.Entry(testSuite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestSuiteExists(id))
                {
                    throw new KeyNotFoundException($"TestSuite with ID {id} not found");
                }
                else
                {
                    throw;
                }
            }

            return testSuite;
        }
        private bool TestSuiteExists(int id)
        {
            return _context.TestSuites.Any(t => t.TestSuiteId == id);
        }
    }
}
