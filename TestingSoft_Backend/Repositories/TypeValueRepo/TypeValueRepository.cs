namespace TestingSoft_Backend.Repositories.TypeValueRepo
{
    public class TypeValueRepository : ITypeValueRepository
    {
        private readonly TestingSoftContext _context;

        public TypeValueRepository(TestingSoftContext context)
        {
            _context = context;
        }
        public async Task<TypeValue> AddTypeValue(TypeValue typeValue)
        {
            _context.TypeValues.Add(typeValue);
            await _context.SaveChangesAsync();
            return typeValue;
        }

        public async Task<bool> DeleteTypeValue(int id)
        {
            var typeValue = await _context.TypeValues.FindAsync(id);
            if (typeValue == null)
            {
                return false;
            }

            _context.TypeValues.Remove(typeValue);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TypeValue> GetTypeValueById(int id)
        {
            return await _context.TypeValues.FindAsync(id);
        }

        public async Task<List<TypeValue>> GetTypeValues()
        {
            return await _context.TypeValues.ToListAsync();
        }

        public async Task<TypeValue> UpdateTypeValue(int id, TypeValue typeValue)
        {
            if (id != typeValue.Id)
            {
                throw new ArgumentException("Id mismatch");
            }

            _context.Entry(typeValue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeValueExists(id))
                {
                    throw new KeyNotFoundException($"TypeValue with ID {id} not found");
                }
                else
                {
                    throw;
                }
            }

            return typeValue;
        }

        private bool TypeValueExists(int id)
        {
            return _context.TypeValues.Any(tv => tv.Id == id);
        }
    }

}
