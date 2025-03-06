using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplicationREST.Data;
using WebApplicationREST; // O WebApplicationREST.DTOs


namespace WebApplicationREST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly WebApplicationRESTContext _context;

        public StudentsController(WebApplicationRESTContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("HelloWorld")]
        public ActionResult<string> HelloWorld()
        {
            return Ok("Hello World");
        }

        [HttpGet]
        [Route("ReadAllStudents")]
        public ActionResult<IEnumerable<Student>> ReadAllStudents()
        {
            var students = _context.Students.ToList();
            return Ok(students);
        }

        [HttpGet]
        [Route("ReadStudent/{id}")]
        public ActionResult<Student> ReadStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        [Route("CreateStudent")]
        public ActionResult<Student> CreateStudent([FromBody] Student student)
        {
            try
            {
                if (student == null)
                {
                    return BadRequest("Invalid data.");
                }

                _context.Students.Add(student);
                _context.SaveChanges();

                // Usar CreatedAtAction para retornar la URL del recurso creado
                return CreatedAtAction(nameof(ReadStudent), new { id = student.Id }, student);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error saving student");
            }
        }

        [HttpPut]
        [Route("UpdateStudent/{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Student updatedStudent)
        {
            if (updatedStudent == null)
            {
                return BadRequest("Invalid data.");
            }

            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        [Route("DeleteStudent/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet("ReadName/{id:int}")]
        public async Task<ActionResult<string>> ReadName(int id)
        {
            try
            {
                // Ejecuta el procedimiento almacenado y mapea el resultado a StudentNameDTO
                var resultado = await _context.Set<ReadStudent>()
                                              .FromSqlRaw("EXEC ReadStudent @Student_Id", new SqlParameter("@Student_Id", id))
                                              .AsNoTracking()
                                              .FirstOrDefaultAsync();

                if (resultado == null)
                {
                    return NotFound($"No se encontró el nombre del estudiante con ID {id}");
                }

                return Ok(resultado.Name);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error fetching student name: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetStudentStatistics")]
        public ActionResult<StudentStatistics> GetStudentStatistics()
        {
            try
            {
                // Obtener todos los estudiantes
                var students = _context.Students.ToList();

                if (!students.Any())
                {
                    return Ok(new StudentStatistics
                    {
                        TotalStudents = 0,
                        AverageAge = 0,
                        Under18Count = 0,
                        Between18And25Count = 0,
                        Over25Count = 0
                    });
                }

                // Calcular estadísticas
                var totalStudents = students.Count;
                var averageAge = students.Average(s => s.Age);
                var under18Count = students.Count(s => s.Age < 18);
                var between18And25Count = students.Count(s => s.Age >= 18 && s.Age <= 25);
                var over25Count = students.Count(s => s.Age > 25);

                var statistics = new StudentStatistics
                {
                    TotalStudents = totalStudents,
                    AverageAge = averageAge,
                    Under18Count = under18Count,
                    Between18And25Count = between18And25Count,
                    Over25Count = over25Count
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error calculating student statistics: " + ex.Message);
            }
        }

    }
}