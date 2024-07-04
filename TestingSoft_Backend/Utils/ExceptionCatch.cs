using Microsoft.Data.SqlClient;

namespace TestingSoft_Backend.Utils
{
    public class ExceptionCatch
    {
        public static void InsertDataException(string connectionString, ExceptionDb ex)
        {
            string qs = @"INSERT INTO ExceptionDb(
                [Error],
                [Repository],
                [Fonction],
                [CreateDate]) 
            VALUES(@Error, @Repository, @Fonction,  @CreateDate)";
            CreateCommand(qs, connectionString, ex);
        }

        private static void CreateCommand(string queryString, string connectionString, ExceptionDb ex)
        {
            using SqlConnection connection = new(connectionString);
            connection.Open();
            SqlCommand command = new(queryString, connection);
            command.Parameters.Add(new SqlParameter("@Error", ex.Error));
            command.Parameters.Add(new SqlParameter("@Repository", ex.Repository));
            command.Parameters.Add(new SqlParameter("@Fonction", ex.Fonction));
            command.Parameters.Add(new SqlParameter("@CreateDate", ex.CreateDate));
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static ExceptionDb CreateInstanceExceptionDb(Exception ex, string Repositorys, string Fonctions)
        {
            ExceptionDb exceptionDb = new()
            {
                CreateDate = DateTime.Now,
                Error = ex.ToString(),
                Repository = Repositorys,
                Fonction = Fonctions
            };

            return exceptionDb;
        }
    }
}
