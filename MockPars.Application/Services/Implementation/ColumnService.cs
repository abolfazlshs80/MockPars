using ErrorOr;
using MockPars.Application.DTO.@base;
using MockPars.Application.DTO.Column;
using MockPars.Application.Services.Interfaces;
using MockPars.Application.Static.Message;
using MockPars.Domain.Enums;
using MockPars.Domain.Interface;
using MockPars.Domain.Models;

namespace MockPars.Application.Services.Implementation;

public class ColumnService(IUnitOfWork unitOfWork) : IColumnService
{
    public async Task<List<Dictionary<string, string>>> GetAllRowDataAsync(int tableId, CancellationToken ct)
    {
        var columns = await unitOfWork.ColumnsRepository.GetAllRowDataAsync(tableId, ct);


        // تمام رکوردها رو جمع کن و بر اساس RowIndex گروه‌بندی کن
        var allRecords = columns.SelectMany(c => c.RecordData.Select(r => new
        {
            ColumnName = c.ColumnName,
            r.Value,
            r.RowIndex
        }));

        var grouped = allRecords
            .GroupBy(r => r.RowIndex)
            .Select(group => group.ToDictionary(g => g.ColumnName, g => g.Value))
            .ToList();

        return grouped;
    }

    public async Task<ErrorOr<int>> CreateColumn(CreateColumnDto model, CancellationToken ct)
    {
        var existsUser = await unitOfWork.TablesRepository.ExistsAsync(model.TableId, ct);
        if (!existsUser)
            return ErrorOr.Error.NotFound(description: DatabaseMessage.NotFound);

        var Column = new Columns()
        {  //maping
            TablesId = model.TableId,
            ColumnType = model.ColumnType,
            ColumnName = model.ColumnName,
            FakeDataTypes = (FakeDataTypes)(Enum.IsDefined(model.FakeDataTypes) ? model.FakeDataTypes : FakeDataTypesDto.None)

        };

        await unitOfWork.ColumnsRepository.AddAsync(Column, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Column.Id;

    }

    public async Task<ErrorOr<int>> UpdateColumn(UpdateColumnDto model, CancellationToken ct)
    {
        var existsUser = await unitOfWork.TablesRepository.ExistsAsync(model.TableId, ct);
        if (!existsUser)
            return ErrorOr.Error.NotFound(description: DatabaseMessage.NotFound);

        var findColumn = await unitOfWork.ColumnsRepository.GetByIdAsync(model.Id, ct);
        if (findColumn is null)
            return ErrorOr.Error.NotFound(description: ColumnMessage.NotFound);

        //maping
        findColumn.TablesId = model.TableId;
        findColumn.ColumnType = model.ColumnType;
        findColumn.ColumnName = model.ColumnName;
        findColumn.FakeDataTypes =
            (FakeDataTypes)(Enum.IsDefined(model.FakeDataTypes)
                ? model.FakeDataTypes : FakeDataTypesDto.None);
        await unitOfWork.ColumnsRepository.UpdateAsync(findColumn, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return findColumn.Id;
    }

    public async Task<ErrorOr<bool>> DeleteColumn(int Id, CancellationToken ct)
    {
        var findColumn = await unitOfWork.ColumnsRepository.GetByIdAsync(Id, ct);
        if (findColumn is null)
            return ErrorOr.Error.NotFound(description: ColumnMessage.NotFound);
        await unitOfWork.ColumnsRepository.DeleteAsync(findColumn, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<ErrorOr<ColumnItemDto>> GetColumnById(int id, int tableId, CancellationToken ct)
    {
        var findColumn = await unitOfWork.ColumnsRepository.GetByIdAsync(id, tableId, ct);
        if (findColumn is null)
            return ErrorOr.Error.NotFound(description: ColumnMessage.NotFound);

        return new ColumnItemDto(findColumn.Id, findColumn.ColumnName, findColumn.ColumnType, (FakeDataTypesDto)(findColumn.FakeDataTypes), findColumn.TablesId, null);
    }

    public async Task<ErrorOr<IEnumerable<ColumnItemDto>>> GetColumnsByTableId(int tableId, CancellationToken ct)
    {
        var findColumn = await unitOfWork.ColumnsRepository.GetByTableIdAsync(tableId, ct);
        if (findColumn is null)
            return ErrorOr.Error.NotFound(description: ColumnMessage.NotFound);

        return findColumn.Select(_ => new ColumnItemDto(_.Id, _.ColumnName, _.ColumnType, (FakeDataTypesDto)(_.FakeDataTypes), _.TablesId, null)).ToList();
    }
}