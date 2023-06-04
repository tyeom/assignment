using assignment.Models;
using assignment.Repositories;
using System.Collections.ObjectModel;

namespace assignment.Services;

public interface IEmployeeContacService
{
    Task<PagedResult<EmployeeContacModel>> GetAllListAsync(int page, int pageSize);

    Task<IEnumerable<EmployeeContacModel>?> GetFindByNameAsync(string name);

    Task<EmployeeContacModel> Add(EmployeeContacModel employeeContac);

    Task<EmployeeContacModel?> DeleteByEmail(string email);

    Task<EmployeeContacModel?> Delete(long id);

    Task<bool> Update(EmployeeContacModel employeeContacModel);

    Task<bool> Update(long id, EmployeeContacModel employeeContacModel);

    Task<bool> Upload(EmployeeUploadModel employeeUploadModel);
}

public class EmployeeContacService : IEmployeeContacService
{
    private readonly ILoggerService _logger;
    private readonly EmployeeContacRepository _repository;
    public EmployeeContacService(ILoggerService logger, EmployeeContacRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<EmployeeContacModel> Add(EmployeeContacModel employeeContac)
    {
        return await _repository.Add(employeeContac);
    }

    public async Task<PagedResult<EmployeeContacModel>> GetAllListAsync(int page, int pageSize)
    {
        return await _repository.GetByPaged(page, pageSize);
    }

    public async Task<IEnumerable<EmployeeContacModel>?> GetFindByNameAsync(string name)
    {
        return await _repository.FindAll(p => p.Name == name);
    }

    public async Task<EmployeeContacModel?> DeleteByEmail(string email)
    {
        var findByEmail = await _repository.FindAll(p => p.Email == email);
        if(findByEmail is null || findByEmail.Count() <= 0)
            return null;

        /// TODO : email 데이터는 고유한 값으로 찾아 졌으면 무조건 한개만 있다고 간주한다.
        var result = await _repository.Delete(findByEmail.First());
        return result;
    }

    public async Task<EmployeeContacModel?> Delete(long id)
    {
        var result = await _repository.Delete(id);
        return result;
    }

    public async Task<bool> Update(EmployeeContacModel employeeContacModel)
    {
        var findByEmail = await _repository.FindFirst(p => p.Email == employeeContacModel.Email);
        if (findByEmail is null)
            return false;

        findByEmail.Name = employeeContacModel.Name;
        findByEmail.Position = employeeContacModel.Position;
        findByEmail.Tel = employeeContacModel.Tel;
        findByEmail.Joined = employeeContacModel.Joined;

        await _repository.Update(findByEmail);
        return true;
    }

    public async Task<bool> Update(long id, EmployeeContacModel employeeContacModel)
    {
        var findById = await _repository.FindFirst(p => p.Id == id);
        if (findById is null)
            return false;

        findById.Name = employeeContacModel.Name;
        findById.Position = employeeContacModel.Position;
        findById.Tel = employeeContacModel.Tel;
        findById.Joined = employeeContacModel.Joined;

        await _repository.Update(findById);
        return true;
    }

    public async Task<bool> Upload(EmployeeUploadModel employeeUploadModel)
    {
        string? uploadData = null;

        if (employeeUploadModel.File is not null)
        {
            uploadData = await this.ReadAllTextFromFile(employeeUploadModel.File);
        }
        else
        {
            uploadData = employeeUploadModel.TextData;
        }

        if (string.IsNullOrWhiteSpace(uploadData) is true)
        {
            _logger.LogInfo("파일 업로드 내용 없음.");
            return false;
        }

        var employeeContactObj = EmployeeContacModel.FromJson(uploadData);
        // json Deserialize 실패, CSV로 시도
        if (employeeContactObj is null)
        {
            _logger.LogInfo("파일 업로드 - Fail json deserialize.");

            var csvLineArr = uploadData.Split(System.Environment.NewLine);
            await this.ParallelEmployeeContacAdd<String>(csvLineArr);
        }
        else
        {
            await this.ParallelEmployeeContacAdd<EmployeeContacModel>(employeeContactObj);
        }

        return true;
    }

    private async Task ParallelEmployeeContacAdd<T>(IEnumerable<T> data)
    {
        /// TODO : SaveChangesAsync() 호출을 병렬처리로 하면 이전 작업이 완료되기 전에
        /// 컨텍스트에서 새로운 작업이 호출 되었다고 오류가 난다.
        /// 일단 동기로 처리 !
        //await Parallel.ForEachAsync<T>(data, async (employeeContact, cancellationToken) =>
        foreach (var employeeContact in data)
        {
            try
            {
                if(employeeContact is String csv)
                {
                    var employeeContactObj = EmployeeContacModel.FromCsv(csv);
                    if (employeeContactObj is not null)
                    {
                        await this.Add(employeeContactObj);
                    }
                    else
                    {
                        _logger.LogInfo("파일 업로드 - Fail csv deserialize.");
                    }
                }
                else if (employeeContact is EmployeeContacModel employee)
                {
                    await this.Add(employee);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }
        }
    }

    private async Task<string?> ReadAllTextFromFile(IFormFile file)
    {
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(stream))
            {
                string fileContent = await reader.ReadToEndAsync();
                if (string.IsNullOrWhiteSpace(fileContent) is true)
                {
                    return null;
                }
                return fileContent;
            }
        }
    }
}