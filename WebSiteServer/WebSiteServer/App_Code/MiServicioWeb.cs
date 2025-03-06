using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for MiServicioWeb
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class MiServicioWeb : System.Web.Services.WebService
{
    private readonly SchoolDBContext db = new SchoolDBContext();
    public MiServicioWeb()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod]
    public int Sumar(int a, int b)
    {
        return a + b;
    }

    [WebMethod]
    public Student CreateStudent(string name, int age)
    {
        var student = new Student { Name = name, Age = age };
        db.Students.Add(student);
        db.SaveChanges();
        return student;
    }

    [WebMethod]
    public Student ReadStudent(int id)
    {
        return db.Students.Find(id);
    }

    [WebMethod]
    public List<Student> ReadAllStudents()
    {
        return db.Students.ToList();
    }

    [WebMethod]
    public bool UpdateStudent(int id, string name, int age)
    {
        var student = db.Students.Find(id);
        if (student != null)
        {
            student.Name = name;
            student.Age = age;
            db.SaveChanges();
            return true;
        }
        return false;
    }

    [WebMethod]
    public bool DeleteStudent(int id)
    {
        var student = db.Students.Find(id);
        if (student != null)
        {
            db.Students.Remove(student);
            db.SaveChanges();
            return true;
        }
        return false;
    }

    [WebMethod]
    public string ReadName(int id)
    {
        // Ejecuta el procedimiento almacenado y obtiene el resultado
        var resultados = db.Database.SqlQuery<ReadStudent>(
                            "EXEC ReadStudent @Student_Id",
                            new SqlParameter("@Student_Id", id))
                            .ToList();

        // Devuelve el nombre del primer resultado, si existe
        return resultados.Count > 0 ? resultados[0].Name : null;
    }
}
