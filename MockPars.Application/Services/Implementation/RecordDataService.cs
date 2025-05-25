using ErrorOr;
using MockPars.Application.DTO.RecordData;
using MockPars.Application.Services.Interfaces;
using MockPars.Application.Static.Message;
using MockPars.Domain.Enums;
using MockPars.Domain.Interface;
using MockPars.Domain.Models;

namespace MockPars.Application.Services.Implementation;

public class RecordDataService(IUnitOfWork unitOfWork) : IRecordDataService
{
    public async Task<ErrorOr<int>> CreateRecordData(CreateRecordDataDto model, CancellationToken ct)
    {
        var existsUser = await unitOfWork.ColumnsRepository.ExistsAsync(model.ColumnsId, ct);
        if (!existsUser)
            return ErrorOr.Error.NotFound(description: DatabaseMessage.NotFound);
       
        var RecordData = new RecordData()
        {  //maping
         ColumnsId = model.ColumnsId,
         Value = model.Value,
        };

        await unitOfWork.RecordDataRepository.AddAsync(RecordData, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return RecordData.Id;

    }

    public async Task<ErrorOr<int>> UpdateRecordData(UpdateRecordDataDto model, CancellationToken ct)
    {
        var existsUser = await unitOfWork.ColumnsRepository.ExistsAsync(model.ColumnsId, ct);
        if (!existsUser)
            return ErrorOr.Error.NotFound(description: DatabaseMessage.NotFound);

        var findRecordData = await unitOfWork.RecordDataRepository.GetByIdAsync(model.Id, ct);
        if (findRecordData is null)
            return ErrorOr.Error.NotFound(description: RecordDataMessage.NotFound);
        findRecordData.Value = model.Value;
        findRecordData.ColumnsId=model.ColumnsId;
        //maping
   
        await unitOfWork.RecordDataRepository.UpdateAsync(findRecordData, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return findRecordData.Id;
    }

    public async Task<ErrorOr<bool>> DeleteRecordData(int Id, CancellationToken ct)
    {
        var findRecordData = await unitOfWork.RecordDataRepository.GetByIdAsync(Id, ct);
        if (findRecordData is null)
            return ErrorOr.Error.NotFound(description: RecordDataMessage.NotFound);
        await unitOfWork.RecordDataRepository.DeleteAsync(findRecordData, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<ErrorOr<RecordDataItemDto>> GetRecordDataById(int id, int columnId, CancellationToken ct)
    {
        var findRecordData = await unitOfWork.RecordDataRepository.GetByIdAsync(id, columnId, ct);
        if (findRecordData is null)
            return ErrorOr.Error.NotFound(description: RecordDataMessage.NotFound);

        return new RecordDataItemDto(findRecordData.Id,findRecordData.ColumnsId,findRecordData.Value);
    }

    public async Task<ErrorOr<IEnumerable<RecordDataItemDto>>> GetRecordDataByColumnId(int columnId, CancellationToken ct)
    {
        var findRecordData = await unitOfWork.RecordDataRepository.GetByColumnIdAsync(columnId, ct);
        if (findRecordData is null)
            return ErrorOr.Error.NotFound(description: RecordDataMessage.NotFound);

        return findRecordData.Select(_ => new RecordDataItemDto(_.Id, _.ColumnsId,_.Value)).ToList();
    }
}