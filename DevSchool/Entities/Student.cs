namespace DevSchool.Entities
{
    public class Student
    {
        protected Student()
        {
        }

        public Student(string fullName, DateTime birthDate, string schoolClass) 
        { 
            FullName = fullName;
            BirthDate = birthDate;
            SchoolClass = schoolClass;
            IsActive = true;
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string SchoolClass { get; set; }
        public bool IsActive { get; set; }
    }
}
