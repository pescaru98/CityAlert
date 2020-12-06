
using System.Collections.Generic;
using System.Threading.Tasks;
using L02.Entities;

public interface IStudentRepository
{
    public Task<List<Student>> GetAllStudents();

    public Task CreateStudent(Student student);

    public Task DeleteStudent(string partitionKey, string rowKey);

    public Task<List<Student>> GetStudent(string partitionKey, string rowKey);
} 