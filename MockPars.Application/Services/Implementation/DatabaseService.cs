using ErrorOr;
using MockPars.Application.DTO.Column;
using MockPars.Application.DTO.Database;
using MockPars.Application.DTO.Table;
using MockPars.Application.DTO.Users;
using MockPars.Application.Generator;
using MockPars.Application.Services.Interfaces;
using MockPars.Application.Static.Message;
using MockPars.Domain.Interface;
using MockPars.Domain.Models;
using System.Linq;
using System.Reflection;
using MockPars.Application.DTO.RecordData;

namespace MockPars.Application.Services.Implementation;

public class DatabaseService(IUnitOfWork unitOfWork) : IDatabaseService
{
    public async Task<ErrorOr<int>> CreateDatabase(CreateDatabaseDto model, CancellationToken ct)
    {
        var existsUser = await unitOfWork.UserRepository.ExistsAsync(model.UserId, ct);
        if (!existsUser)
            return ErrorOr.Error.NotFound(description: UserMessageStatic.NotFound_User);

        var database = new Databases()
        {
            Slug = model.Slug,
            UserId = model.UserId,
            DatabaseName = model.DatabaseName
        };

        await unitOfWork.DatabasesRepository.AddAsync(database, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return database.Id;

    }

    public async Task<ErrorOr<int>> UpdateDatabase(UpdateDatabaseDto model, CancellationToken ct)
    {
        var existsUser = await unitOfWork.UserRepository.ExistsAsync(model.UserId, ct);
        if (!existsUser)
            return ErrorOr.Error.NotFound(description: UserMessageStatic.NotFound_User);

        var findDatabase = await unitOfWork.DatabasesRepository.GetByIdAsync(model.Id, ct);
        if (findDatabase is null)
            return ErrorOr.Error.NotFound(description: DatabaseMessage.NotFound);

        findDatabase.Slug = model.Slug;
        findDatabase.UserId = model.UserId;
        findDatabase.DatabaseName = model.DatabaseName;

        await unitOfWork.DatabasesRepository.UpdateAsync(findDatabase, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return findDatabase.Id;
    }

    public async Task<ErrorOr<bool>> DeleteDatabase(int Id, CancellationToken ct)
    {
        var findDatabase = await unitOfWork.DatabasesRepository.GetByIdAsync(Id, ct);
        if (findDatabase is null)
            return ErrorOr.Error.NotFound(description: DatabaseMessage.NotFound);
        await unitOfWork.DatabasesRepository.DeleteAsync(findDatabase, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<ErrorOr<DatabaseItemDto>> GetDatabaseById(int id,string userId, CancellationToken ct)
    {
        var findDatabase = await unitOfWork.DatabasesRepository.GetByIdAsync(id,userId, ct);
        if (findDatabase is null)
            return ErrorOr.Error.NotFound(description: DatabaseMessage.NotFound);

        return new DatabaseItemDto(findDatabase.Id, findDatabase.DatabaseName, findDatabase.Slug,
            findDatabase.Tables.Select(_=> new TableItemDto(_.Id, _.DatabasesId, _.TableName, _.Slug, _.IsGetAll, _.IsGet, _.IsPut, _.IsPost, _.IsDelete,
                      _.Columns.Select(c => new ColumnItemDto(c.Id, c.ColumnName, c.ColumnType, (FakeDataTypesDto)(c.FakeDataTypes),c.TablesId
                          , c.RecordData.Select(r => new RecordDataItemDto(r.Id, r.ColumnsId, r.Value)))
                      ).ToList())));
    }

    public async Task<ErrorOr<IEnumerable<DatabaseItemDto>>> GetDatabasesByUserId(string userId, CancellationToken ct)
    {
        var findDatabase = await unitOfWork.DatabasesRepository.GetByUserIdAsync( userId, ct);
        if (findDatabase is null)
            return ErrorOr.Error.NotFound(description: DatabaseMessage.NotFound);


        var a= findDatabase.Select(_ => new DatabaseItemDto(_.Id, _.DatabaseName, _.Slug,null)).ToList();
      
        return findDatabase.Select(_=> new DatabaseItemDto(_.Id, _.DatabaseName, _.Slug,null)).ToList();
    }
}