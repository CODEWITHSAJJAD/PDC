using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab_Intro
{
    public delegate double MyFuncA(int v1, float v2, double v3, short v4);


    public delegate T MyFunc<T1, T2, T3, T4, T>(T1 v1, T2 v2, T3 v3, T4 v4);
    public delegate T MyFunc<T1, T2, T3, T>(T1 v1, T2 v2, T3 v3);
    public delegate T MyFunc<T1, T2, T>(T1 v1, T2 v2);


    class Student
    {
        public string Name { get; set; }
        public float Score { get; set; }
        public char Gender { get; set; }

        public override string ToString()
        {
            return $"{Name},{Gender},{Score}";
        }
    }
    class MainClass
    {
        /// <summary>
        /// will input a number and will return true if number is 3-digit, else false.
        /// </summary>
        /// <param name="num">Your Number</param>
        /// <returns>True:if a 3-Digit number</returns>
        public static bool Is3DigitNumber(int num)
        {
            return num >= 100 && num <= 999;
        }

        public static bool Is2DigitNumber(int num)
        {
            return num >= 10 && num <= 99;
        }

        public static bool IsFemale(Student s)
        {
            return s.Gender == 'F';
        }
        public static bool IsFemaleWithGoodScore(Student s)
        {
            return s.Gender == 'F' && s.Score>3.5;
        }


        public static  double Sum(int v1,float v2, double v3,short v4 )
        {
            return v1 + v2 + v3 + v4;
        }


        public static void DisplayMessage(MyFuncA f)
        {
            var ans = f(2,4.6F,4.9,3);
            if(ans <10)
                Console.WriteLine("Average");
            else
                Console.WriteLine("Good");
        }




        public static void Main(string[] args)
        {


            DisplayMessage(Sum);

            
            MyFuncA f1 = Sum;


           Double ans = f1(2, 4.5F, 4.9, 2);
            Console.WriteLine(ans);
            Console.ReadKey();


            var students = new List<Student> {
                new Student{ Name="A", Gender='M', Score=2.5F },
                new Student{ Name="B", Gender='F', Score=3.5F },
                new Student{ Name="C", Gender='M', Score=2.5F },
                new Student{ Name="D", Gender='F', Score=3.9F },
                new Student{ Name="E", Gender='M', Score=3.0F },
                new Student{ Name="F", Gender='M', Score=2.5F },
            };

            var femaleOnly = students.FindAll(IsFemaleWithGoodScore);
            foreach (var s in femaleOnly)
            {
                Console.WriteLine(s);
            }

            Console.ReadKey();





            var data = new List<int> {2,3,4,6,99,80,990,123,556,78 };
           var result= data.FindAll(Is2DigitNumber);
            foreach (var d in result)
            {
                Console.WriteLine(d);
            }
            Console.ReadKey();
        }
    }
}
