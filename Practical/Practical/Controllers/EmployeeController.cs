using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Practical.Context;
using Practical.Models;
using Practical.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Practical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<Club> _clubRepository;
        private readonly IBaseRepository<Department> _departmentRepository;
        private readonly ApplicationContext _context;
        private readonly IHubContext<NotificationHub> _notificationHubContext;


        public EmployeeController(IBaseRepository<Employee> employeeRepository,
            IBaseRepository<Department> departmentRepository,
            IBaseRepository<Club> clubRepository, ApplicationContext context,
            IHubContext<NotificationHub> notificationHubContext)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _clubRepository = clubRepository;
            _context = context;
            _notificationHubContext = notificationHubContext;
        }
        // GET: api/Employee
        [HttpGet]
        public ResponceDetail Get()
        {
            return new ResponceDetail
            {
                Data = _employeeRepository.GetAll(),
                Status = true
            };
        }

        // GET: api/Employee/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Employee
        [HttpPost]
        public ResponceDetail Post(Employee employee)
        {
            _employeeRepository.Insert(employee);
            _employeeRepository.Save();
            NotificationHub objNotifHub = new NotificationHub();
            

            //objNotifHub.SendMessage("Employee Inserted Sucessfully");

            _notificationHubContext.Clients.All.SendAsync("sendToUser", "Employee Inserted Sucessfully");
            return new ResponceDetail
            {
                Status = true,
                Messsage = "Employee inserted Sucessfuly!!"

            };

        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpDelete("{departmenId}")]
        [Route("DepartmentDelete")]
        public ResponceDetail DepartmentDelete(char departmentId)
        {
            var department = _departmentRepository.GetById(departmentId);
            if (department != null)
            {
                var employees = _employeeRepository.GetAll().Where(x => x.DepartmentId == departmentId).ToList();
                foreach(var employee in employees)
                {
                    employee.DepartmentId = null;
                    _employeeRepository.Update(employee);
                }
                _departmentRepository.Delete(department);
                _departmentRepository.Save();
            }

            return new ResponceDetail {
                Status = true,
                Messsage = "Department Deleted Sucessfully!!"
            };
        }

        [HttpGet]
        [Route("GetAvarageAnnualBudget")]
        public ResponceDetail GetAvarageAnnualBudget()
        {
            var data = from e in _employeeRepository.Table
                       join c in _clubRepository.Table on e.ClubId equals c.ClubId
                       join d in _departmentRepository.Table on e.DepartmentId equals d.DepartmentId
                       where c.ClubId == 'A'
                       group d by d.DepartmentId into g
                       select new { Total = g.Count(), Avarage = (g.Average(x => x.AnnualBudget) / g.Count()) };
            return new ResponceDetail
            {
                Status = true,
                Data = data
            };
        }

        [HttpGet]
        [Route("GetAvarageAnnualBudgetLinq")]
        public ResponceDetail GetAvarageAnnualBudgetLinq()
        {
            var data = _employeeRepository.Table.Join(_departmentRepository.Table, x => x.DepartmentId, y => y.DepartmentId, (x, y) => new { x.ClubId, x.DepartmentId, y.AnnualBudget })
                .Join(_clubRepository.Table, y => y.ClubId, x => x.ClubId, (x, y) => x).Where(y => y.ClubId == 'A').GroupBy(z => new { z.DepartmentId }).
                Select(x => new { Total = x.Count(), Avarage = x.Average(x => x.AnnualBudget) });

            return new ResponceDetail
            {
                Status = true,
                Data = data
            };
        }

        [HttpGet]
        [Route("GetAvarageAnnualBudgetDatabase")]
        public async System.Threading.Tasks.Task<ResponceDetail> GetAvarageAnnualBudgetDatabaseAsync()
        {
            //var data = _context.AvarageAnuuaBudget.FromSql().ToList();
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();
            var command = conn.CreateCommand();
            const string query = "select Sum(d.\"AnnualBudget\")/Count(*) as AvarageAnuualBudget from public.\"Employee\" as e inner join public.\"Department\" as d on d.\"DepartmentId\" = e.\"DepartmentId\" inner join public.\"Club\" as c on c.\"ClubId\" = e.\"ClubId\" where c.\"ClubId\" = 'A' group by d.\"DepartmentId\"";
            command.CommandText = query;
            var reader = await command.ExecuteReaderAsync();
            var data = new List<decimal>();
            while (await reader.ReadAsync())
            {
                data.Add(reader.GetDecimal(0));
                // Do whatever you want with title 
            }

            return new ResponceDetail
            {
                Status = true,
                Data = data
            };
        }
    }
}
