namespace TestingSoft_Backend.Repositories.BuildRepo
{
    public interface IBuildRepository
    {
        Task<List<Build>> GetBuilds();
        Task<Build> GetBuildById(int BuildId);
        Task<Build> AddBuild(Build build);
        Task<Build> UpdateBuild(int BuildId, Build build);
        Task<bool> DeleteBuild(int BuildId);
    }
}
