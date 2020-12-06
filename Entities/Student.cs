using Microsoft.WindowsAzure.Storage.Table;

namespace L02.Entities
{
    public class Student : TableEntity{

        public Student(string university, string cnp)
        {
            this.PartitionKey = university;
            this.RowKey = cnp;
        }

        public Student()
        {

        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Year { get; set; }
        public string Faculty { get; set; }
             
    }
}