using Dapper;
using DevSchool.Entities;
using DevSchool.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DevSchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly string _connectionString;
        public StudentsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DevSchool");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM Students WHERE IsActive = 1";

                var students = await sqlConnection.QueryAsync<Student>(sql);

                return Ok(students);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM Students WHERE Id = @id";

                var student = await sqlConnection.QuerySingleOrDefaultAsync<Student>(sql, new { id });

                return Ok(student);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(StudentInputModel model)
        {
            var student = new Student(
                model.FullName,
                model.BirthDate,
                model.SchoolClass
            );

            var parameters = new
            {
                student.FullName,
                student.BirthDate,
                student.SchoolClass,
                student.IsActive
            };

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "INSERT INTO Students (FullName, BirthDate, SchoolClass, IsActive) " +
                    "OUTPUT INSERTED.Id VALUES (@FullName, @BirthDate, @SchoolClass, @IsActive)";

                var id = await sqlConnection.ExecuteScalarAsync<int>(sql, parameters);

                return Ok(id);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, StudentInputModel model)
        {
            var parameters = new
            {
                id,
                model.FullName,
                model.BirthDate,
                model.SchoolClass
            };

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "UPDATE Students SET FullName = @FullName, BirthDate = @BirthDate, SchoolClass = @SchoolClass WHERE Id = @id";

                await sqlConnection.ExecuteAsync(sql, parameters);

                return Ok("Resgistro atualizado com sucesso!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "UPDATE Students SET IsActive = 0 WHERE Id = @id";

                await sqlConnection.ExecuteAsync(sql, new { id });

                return Ok("Resgistro deletado com sucesso!");
            }
        }
    }
}
