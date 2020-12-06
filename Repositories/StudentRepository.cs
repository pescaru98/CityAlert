using System;
using System.Collections.Generic;
using L02.Entities;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace L02.Repositories
{
    public class StudentRepository : IStudentRepository{

        private CloudTableClient _tableClient;
        private CloudTable _studentsTable;
        private string _connectionString;

        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue(typeof(string), "AzureStorageConnectionString").ToString();
            Task.Run(async () => { await InitializeTable(); }).GetAwaiter().GetResult();
        }
         
        private async Task InitializeTable()
        {
           

            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();

            _studentsTable = _tableClient.GetTableReference("studenti");

            await _studentsTable.CreateIfNotExistsAsync();
        }

        async Task<List<Student>> IStudentRepository.GetAllStudents(){
            var students = new List<Student>();

            TableQuery<Student> query = new TableQuery<Student>();
            TableContinuationToken token = null;

            do
            {
                TableQuerySegment<Student> resultSegment = await _studentsTable.ExecuteQuerySegmentedAsync(query, token);

                token = resultSegment.ContinuationToken;

                students.AddRange(resultSegment.Results);
            } while (token != null);

            return students;
        }

        async Task IStudentRepository.CreateStudent(Student student)
        {
            var insertOperation = TableOperation.Insert(student);

            await _studentsTable.ExecuteAsync(insertOperation);
        }


        async Task IStudentRepository.DeleteStudent(string partitionKey, string rowKey)
        {

            Task<List<Student>> studentTask = Task<List<Student>>.Factory.StartNew(async () => await GetStudent(partitionKey, rowKey));
            List<Student> student = studentTask.Result;
            var deleteOperation = TableOperation.Delete(student.First());
            await _studentsTable.ExecuteAsync(deleteOperation);
        }


        public async Task<List<Student>> GetStudent(string partitionKey, string rowKey)
        {
            var students = new List<Student>();

            TableQuery<Student> query = new TableQuery<Student>()
               .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey.ToString()))
               .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey.ToString()));
            TableContinuationToken token = null;

            do
            {
                TableQuerySegment<Student> resultSegment = await _studentsTable.ExecuteQuerySegmentedAsync(query, token);

                token = resultSegment.ContinuationToken;

                students.AddRange(resultSegment.Results);
            } while (token != null);

            return students;
        }
    }
}