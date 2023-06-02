using assignment.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json;

namespace assignment.Models;

public record EmployeeContacModel : IEntity
{
    /// <summary>
    /// 고유 Id
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// 이름
    /// </summary>
    [Required]
    public string? Name { get; set; }

    /// <summary>
    /// 직급
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// E-Mail
    /// </summary>
    [Required]
    public string? Email { get; set; }

    /// <summary>
    /// 전화번호
    /// </summary>
    [Required]
    public string? Tel { get; set; }

    /// <summary>
    /// 등록 날짜
    /// </summary>
    [Required]
    public DateTime Joined { get; set; }

    public static IEnumerable<EmployeeContacModel>? FromJson(string json)
    {
        try
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return JsonSerializer.Deserialize<IEnumerable<EmployeeContacModel>>(json, options);
        }
        catch(Exception ex) {
            return null;
        }
    }

    public static EmployeeContacModel FromCsv(string csvLine)
    {
        string[] values = csvLine.Split(',');
        if (values.Length < 5)
            throw new Exception("잘못된 형식의 CSV 포맷 입니다.");

        EmployeeContacModel employeeContac = new();
        employeeContac.Name = values[0];
        employeeContac.Email = values[1];
        employeeContac.Tel = values[2];

        DateTime joined;
        if (DateTime.TryParseExact(values[3].Trim().Replace(" ",""), "yyyy.MM.dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out joined))
        {
            employeeContac.Joined = joined;
        }
        else
        {
            throw new Exception("잘못된 날짜 형식 입니다. - 유효한 날짜 형식은 yyyy-MM-dd 입니다.");
        }

        employeeContac.Position = values[4];

        return employeeContac;
    }
}