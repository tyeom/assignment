using assignment.DataContext;
using assignment.Models;
using assignment.Services;
using assignment.Extensions;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace assignment.Controllers;

[Route("api")]
[ApiController]
public class EmployeeController : Controller
{
    private readonly ILoggerService _logger;
    private readonly IEmployeeContacService _employeeContactService;

    public EmployeeController(ILoggerService logger, IEmployeeContacService employeeContactService)
    {
        _logger = logger;
        _employeeContactService = employeeContactService;
    }

    [HttpGet("employee")]
    public async Task<IActionResult> Employee(int page, int pageSize)
    {
        //var testData = new EmployeeContacModel()
        //{
        //    Name = "테스트",
        //    Email = "emailddd",
        //    Joined = DateTime.Now,
        //    Phone = "111",
        //    Position = "과장"
        //};

        //await _employeeContactService.Add(testData);

        if (pageSize <= 0)
            return BadRequest("paseSize는 0 일 수 없습니다.");

        var employeeContactList = await _employeeContactService.GetAllListAsync(page, pageSize);
        return Ok(employeeContactList);
    }

    [HttpGet("employee/{name}")]
    public async Task<IActionResult> EmployeeFindByName(string name)
    {
        var employeeContactList = await _employeeContactService.GetFindByNameAsync(name);
        return Ok(employeeContactList);
    }

    [HttpPost("employee")]
    public async Task<IActionResult> UploadEmployee([FromForm] EmployeeUploadModel employeeUpload)
    {
        var result = await _employeeContactService.Upload(employeeUpload);
        if(result)
        {
            var createdResource = new { page = 1, pageSize = 30 };
            return CreatedAtAction(nameof(EmployeeController.Employee),
                new { page = createdResource.page, pageSize = createdResource.pageSize},
                createdResource);
        }
        else
        {
            return BadRequest("업로드 실패 - 파일 포맷이 잘못 되었습니다.");
        }
    }

    // 임의로 추가 했습니다.
    [HttpDelete("employee/delete")]
    public async Task<IActionResult> DeleteByEmail(string email)
    {
        var employeeContactList = await _employeeContactService.DeleteByEmail(email);
        return Ok(employeeContactList);
    }

    // 임의로 추가 했습니다.
    [HttpPost("employee/update")]
    public async Task<IActionResult> UpdateByEmail([FromBody]EmployeeContacModel employeeContacModel)
    {
        var result = await _employeeContactService.Update(employeeContacModel);
        if (result is true)
        {
            return Ok("Sucess");
        }
        else
        {
            return Ok("Fail");
        }
    }

    // 임의로 추가 했습니다.
    [HttpPost("employee/update/{id}")]
    public async Task<IActionResult> UpdateByEmail(long id, [FromBody] EmployeeContacModel employeeContacModel)
    {
        var result = await _employeeContactService.Update(id, employeeContacModel);
        if (result is true)
        {
            return Ok("Sucess");
        }
        else
        {
            return Ok("Fail");
        }
    }
}
