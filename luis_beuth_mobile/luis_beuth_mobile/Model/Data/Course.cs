namespace luis_beuth.Models.Data
{
    public enum Degree
    {
        Bachelor,
        Master
    }
    public class Course 
    {    
        public int Id { get; set; }
        public string Name { get; set; }
        public string StudyPath { get; set; }
        public Degree Degree { get; set; }
        public string CourseNumber { get; set; }
    }

}