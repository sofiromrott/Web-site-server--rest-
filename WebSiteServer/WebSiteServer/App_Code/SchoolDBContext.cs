using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SchoolDBContext
/// </summary>
public class SchoolDBContext : DbContext
{
    public SchoolDBContext() : base("name=SchoolDBContext")
    {
    }

    public virtual DbSet<Student> Students { get; set; }


}