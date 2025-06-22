using System;
namespace StudentData
{
    [Serializable]
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }

        public override string ToString()
        {
            return $"{Id},{Name}({DOB})";
        }


    }
}
