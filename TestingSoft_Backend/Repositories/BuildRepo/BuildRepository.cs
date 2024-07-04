using Microsoft.EntityFrameworkCore;

namespace TestingSoft_Backend.Repositories.BuildRepo
{
    public class BuildRepository : IBuildRepository
    {
        private readonly TestingSoftContext _context;

        public BuildRepository(TestingSoftContext context)
        {
            _context = context;
        }
        public async Task<Build> AddBuild(Build build)
        {
            _context.Builds.Add(build);
            await _context.SaveChangesAsync();
            return build;
        }

        public async Task<bool> DeleteBuild(int BuildId)
        {
            var build = await _context.Builds.FindAsync(BuildId);
            if (build == null)
            {
                return false;
            }

            _context.Builds.Remove(build);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Build> GetBuildById(int BuildId)
        {
            return await _context.Builds.FindAsync(BuildId);
        }

        public async Task<List<Build>> GetBuilds()
        {
            return await _context.Builds.ToListAsync();
        }

        public async Task<Build> UpdateBuild(int BuildId, Build build)
        {
            if (BuildId != build.BuildId)
            {
                throw new ArgumentException("Id mismatch");
            }

            _context.Entry(build).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildExists(BuildId))
                {
                    throw new KeyNotFoundException($"Build with ID {BuildId} not found");
                }
                else
                {
                    throw;
                }
            }

            return build;
        }
        private bool BuildExists(int id)
        {
            return _context.Builds.Any(b => b.BuildId == id);
        }
    }
}
