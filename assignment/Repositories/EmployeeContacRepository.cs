using assignment.Base;
using assignment.DataContext;
using assignment.Models;
using assignment.Services;

namespace assignment.Repositories;

public class EmployeeContacRepository : EfCoreRepository<EmployeeContacModel, EmployeeContacDBContext>
{
    public EmployeeContacRepository(ILoggerService logger, EmployeeContacDBContext context)
        : base(logger, context)
    {
    }
}
