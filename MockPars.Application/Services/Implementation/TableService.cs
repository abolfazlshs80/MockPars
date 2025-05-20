using ErrorOr;
using MockPars.Application.DTO.Table;
using MockPars.Application.Services.Interfaces;
using MockPars.Application.Static.Message;
using MockPars.Domain.Interface;
using MockPars.Domain.Models;

namespace MockPars.Application.Services.Implementation;

public class TableService(IUnitOfWork unitOfWork) : ITableService
{
    public async Task<ErrorOr<int>> CreateTable(CreateTableDto model, CancellationToken ct)
    {
        var existsUser = await unitOfWork.DatabasesRepository.ExistsAsync(model.DatabaseId, ct);
        if (!existsUser)
            return ErrorOr.Error.NotFound(description: DatabaseMessage.NotFound);

        var Table = new Tables()
        {
            Slug = model.Slug,
            DatabasesId = model.DatabaseId,
            TableName = model.TableName,
            IsGetAll = model.IsGetAll,
            IsGet = model.IsGet,
            IsPut = model.IsPut,
            IsPost = model.IsPost,
            IsDelete = model.IsDelete,
        };

        await unitOfWork.TablesRepository.AddAsync(Table, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Table.Id;

    }

    public async Task<ErrorOr<int>> UpdateTable(UpdateTableDto model, CancellationToken ct)
    {
        var existsUser = await unitOfWork.DatabasesRepository.ExistsAsync(model.DatabaseId, ct);
        if (!existsUser)
            return ErrorOr.Error.NotFound(description: DatabaseMessage.NotFound);

        var findTable = await unitOfWork.TablesRepository.GetByIdAsync(model.Id, ct);
        if (findTable is null)
            return ErrorOr.Error.NotFound(description: TableMessage.NotFound);

        findTable.Slug = model.Slug;
       findTable.DatabasesId = model.DatabaseId;
       findTable. TableName = model.TableName;
       findTable. IsGetAll = model.IsGetAll;
       findTable. IsGet = model.IsGet;
       findTable. IsPut = model.IsPut;
       findTable. IsPost = model.IsPost;
       findTable. IsDelete = model.IsDelete;

        await unitOfWork.TablesRepository.UpdateAsync(findTable, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return findTable.Id;
    }

    public async Task<ErrorOr<bool>> DeleteTable(int Id, CancellationToken ct)
    {
        var findTable = await unitOfWork.TablesRepository.GetByIdAsync(Id, ct);
        if (findTable is null)
            return ErrorOr.Error.NotFound(description: TableMessage.NotFound);
        await unitOfWork.TablesRepository.DeleteAsync(findTable, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<ErrorOr<TableItemDto>> GetTableById(int id, int databaseId, CancellationToken ct)
    {
        var findTable = await unitOfWork.TablesRepository.GetByIdAsync(id, databaseId, ct);
        if (findTable is null)
            return ErrorOr.Error.NotFound(description: TableMessage.NotFound);

        return new TableItemDto(findTable.Id,findTable.DatabasesId, findTable.TableName, findTable.Slug,findTable.IsGetAll,findTable.IsGet,findTable.IsPut,findTable.IsPost,findTable.IsDelete);
    }

    public async Task<ErrorOr<IEnumerable<TableItemDto>>> GetTablesByDatabaseId(int databaseId, CancellationToken ct)
    {
        var findTable = await unitOfWork.TablesRepository.GetByDatabaseIdAsync(databaseId, ct);
        if (findTable is null)
            return ErrorOr.Error.NotFound(description: TableMessage.NotFound);

        return findTable.Select(_ => new TableItemDto(_.Id, _.DatabasesId, _.TableName, _.Slug, _.IsGetAll, _.IsGet, _.IsPut, _.IsPost, _.IsDelete)).ToList();
    }
}