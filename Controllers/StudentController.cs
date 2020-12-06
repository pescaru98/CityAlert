using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using L02.Entities;
using L02.Repositories;

namespace L02.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    { 
        private readonly ILogger<StudentController> _logger;
        private IStudentRepository _studentRepository;
/*        public StudentController(ILogger<StudentController> logger)
        {        

            _logger = logger;
        }*/

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Student>> Get(){
            return await _studentRepository.GetAllStudents();
        }

        [HttpGet("{partitionKey}/{rowKey}")]
        public async Task<List<Student>> GetStudent(string partitionKey, string rowKey)
        {
            return await _studentRepository.GetStudent(partitionKey,rowKey);
        }

        [HttpPost]
        public async Task CreateStudent([FromBody] Student student)
        {
            try
            {
               await _studentRepository.CreateStudent(student);
            }catch(System.Exception)
            {
                throw;
            }
        }

        [HttpDelete("delete/{partitionKey}/{rowKey}")]
        public async Task DeleteStudent(string partitionKey, string rowKey)
        {
            try
            {
                await _studentRepository.DeleteStudent(partitionKey, rowKey);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /*        [HttpGet("{id}")]
               public Student GetStudentOf(int id){
                   return StudentRepo.getStudentOfId(id);
               }

               [HttpGet("{id}/{name}/{faculty}")]
               public void addStudentGet(int id, string name,string faculty){
                   Student student = new Student(id,name,faculty);
                   StudentRepo.addStudent(student);
               }

               [HttpPost("add")]
               public void addStudent(Student student){
                   StudentRepo.addStudent(student);
               }

               [HttpPut("update")]
               public void updateStudent(Student student){
                   StudentRepo.updateStudent(student);
               }

               [HttpDelete("delete/{id}")]
               public void deleteStudent(int id){
                   StudentRepo.deleteStudent(id);
               }*/

    }
}
