using L02.Entities;
using System;
using System.Collections.Generic;  
namespace L02.Repositories{
    public static class StudentRepo{
        public static Student student1 = new Student(1,"Mihai","AC");
        public static Student student2 = new Student(2,"Dan","AC");

        public static List<Student> students;

        static StudentRepo(){
            init();
        }

        public static void init(){
             students = new List<Student>();
            students.Add(student1);
            students.Add(student2);
        }
        public static IEnumerable<Student> GetStudents(){
            return students.ToArray();
        }

        public static Student getStudentOfId(int id){
            return students.Find(x => x.id == id);
        }

        public static void addStudent(Student student){
            students.Add(student);
        }

        public static void updateStudent(Student student){
            Student student1 = students.Find(x => x.id == student.id);
            student1.name = student.name;
            student1.faculty = student.faculty;
        }

        public static void deleteStudent(int id){
            Student student = students.Find(x=>x.id == id);
            if(student != null && student.id > 0)
                students.Remove(student);
        }

    }
}