namespace L02.Entities
{
    public class Student{
        public int id {get; set;}
        public string name {get; set;}
        public string faculty{get; set;}
        public Student(int id, string name,string faculty){
            this.id = id;
            this.name = name;
            this.faculty = faculty;
        }

        public Student(){
            
        }

    }
}