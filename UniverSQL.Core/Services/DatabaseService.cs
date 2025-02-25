using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniverSQL.Core.Data;
using UniverSQL.Core.Models;

namespace UniverSQL.Core.Services
{
    public class DatabaseService
    {
        private readonly SqlServerDbContext _sqlContext;
        private readonly Db2DbContext _db2Context;

        public DatabaseService(SqlServerDbContext sqlContext, Db2DbContext db2Context)
        {
            _sqlContext = sqlContext;
            _db2Context = db2Context;
        }

        public async Task<List<(string EmployeeName, string DepartmentName)>> GetJoinedDataAsync()
        {
            var employees = await _sqlContext.Employees.ToListAsync();
            var departments = await _db2Context.Departments.ToListAsync();

            var result = employees.Join(departments,
                emp => emp.DepartmentId,
                dept => dept.Id,
                (emp, dept) => (emp.Name, dept.Name)).ToList();

            return result;
        }
    }
}
