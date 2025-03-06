using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Student
/// </summary>
public class Student
{
    public Student()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

}

public class ReadStudent
{
    public int StudentId { get; set; }
    public string Name { get; set; }
}