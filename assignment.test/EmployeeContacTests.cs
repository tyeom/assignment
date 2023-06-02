using assignment.Controllers;
using assignment.Models;
using assignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace assignment.test;

public class EmployeeContacTests
{
    [Fact]
    public void JsonToModelTest()
    {
        // Act
        string json = "[\r\n{\"name\":\"�̹���\", \"email\":\"weapon@clovf.com\", \"tel\":\"010-1111-2424\",\"joined\":\"2020-01-05\"},\r\n{\"name\":\"�Ǻ���\", \"email\":\"panv@clovf.com\", \"tel\":\"010-3535-7979\",\"joined\":\"2013-07-01\" },\r\n{\"name\":\"��ȣ��\", \"email\":\"hobread@clovf.com\", \"tel\":\"010-8531-7942\",\"joined\":\"2019-12-05\"}\r\n]";
        var result = EmployeeContacModel.FromJson(json);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void JsonToModelWhenFailTest()
    {
        // Act
        string json = "[\r\n{\"name1\":\"�̹���\", \"email1\":\"weapon@clovf.com\"}\r\n{}]";
        var result = EmployeeContacModel.FromJson(json);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void CsvToModelTest()
    {
        // Act
        string json = "��ö��, charles@clovf.com, 01075312468, 2018.03.07, ����";
        var result = EmployeeContacModel.FromCsv(json);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CsvToModelWhenFailTest()
    {
        // Assert
        Assert.Throws<Exception>(() =>
        {
            // Act
            string json = "��ö��, 01075312468, 2018.03.07, ����";
            var result = EmployeeContacModel.FromCsv(json);
        });
    }
}