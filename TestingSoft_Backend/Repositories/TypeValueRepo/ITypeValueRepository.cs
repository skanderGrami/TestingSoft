namespace TestingSoft_Backend.Repositories.TypeValueRepo
{
    public interface ITypeValueRepository
    {
        Task<TypeValue> AddTypeValue(TypeValue typeValue);
        Task<bool> DeleteTypeValue(int id);
        Task<TypeValue> GetTypeValueById(int id);
        Task<List<TypeValue>> GetTypeValues();
        Task<TypeValue> UpdateTypeValue(int id, TypeValue typeValue);
    }
}
