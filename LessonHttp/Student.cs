public class Student
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Branch { get; set; }
    public int Age { get; set; }
    public static List<Student> GetAllStudents()
    {
        return new List<Student>()
        {
            new Student { ID = 1, Name = "Abdulatif", Gender = "Male", Branch = "Tuman", Age = 19 },
            new Student { ID = 2, Name = "Elyor", Gender = "Male", Branch = "Shahar", Age = 25 },
            new Student { ID = 3, Name = "Xurshid", Gender = "Male", Branch = "Viloyat", Age = 20},
            new Student { ID = 4, Name = "Jamshid", Gender = "Male", Branch = "Tuman", Age = 18 }
        };
    }
}