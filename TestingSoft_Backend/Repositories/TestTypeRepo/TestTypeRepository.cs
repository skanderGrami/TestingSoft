using Microsoft.EntityFrameworkCore;

namespace TestingSoft_Backend.Repositories.TestTypeRepo
{
    public class TestTypeRepository : ITestTypeRepository
    {
        private readonly TestingSoftContext _context;

        public TestTypeRepository(TestingSoftContext context)
        {
            _context = context;
        }
        public async Task<TestType> AddTestTypec(TestType testType)
        {
            _context.TestTypes.Add(testType);
            await _context.SaveChangesAsync();
            return testType;
        }

        public async Task<bool> DeleteTestType(int id)
        {
            var testType = await _context.TestTypes.FindAsync(id);
            if (testType == null)
            {
                return false;
            }

            _context.TestTypes.Remove(testType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TestType> GetTestTypeById(int id)
        {
            return await _context.TestTypes.FindAsync(id);
        }

        public async Task<List<TestType>> GetTestTypes()
        {
            return await _context.TestTypes.ToListAsync();
        }

        public async Task<TestType> UpdateTestType(int id, TestType testType)
        {
            if (id != testType.TestId)
            {
                throw new ArgumentException("Id mismatch");
            }

            _context.Entry(testType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestTypeExists(id))
                {
                    throw new KeyNotFoundException($"TestType with ID {id} not found");
                }
                else
                {
                    throw;
                }
            }

            return testType;
        }
        private bool TestTypeExists(int id)
        {
            return _context.TestTypes.Any(t => t.TestId == id);
        }
    }
}
