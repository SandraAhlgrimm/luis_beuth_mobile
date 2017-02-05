
using luis_beuth.Models.Data;
using luis_beuthluis_beuth_mobile.Models.Data;
using System.Collections.Generic;

namespace luis_beuth_mobile.Models.Data
{
    public enum Period
    {
        First,
        Second
    }
    public class Exam 
    {
        public int Id { get; set; }
        public string Semester { get; set;}
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public int CourseId { get; set; } 
        public Course Course { get; set; }
        public Period Period { get; set; }
        public double Grade { get; set; }
        public List<Rent> Rents { get; set; }
    }
}