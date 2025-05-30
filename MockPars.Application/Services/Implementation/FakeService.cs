using ErrorOr;
using MockPars.Application.DTO.@base;
using MockPars.Application.Extention;
using MockPars.Application.Services.Interfaces;
using MockPars.Application.Static.FakeValue;
using MockPars.Application.Static.Message;
using MockPars.Domain.Interface;
using MockPars.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MockPars.Application.Services.Implementation
{
    class FakeService(IUnitOfWork unitOfWork, ISqlProvider sqlProvider) : IFakeService
    {
        public async Task<ErrorOr<int>> GenerateFakeData(int tableId, int count, CancellationToken ct)
        {
            var find_table = await unitOfWork.TablesRepository.GetColumnsByIdAsync(tableId, ct);
            if (find_table == null)
                return Error.NotFound(TableMessage.NotFound);


            foreach (var item in find_table?.Columns)
            {
                int rowIndex = await unitOfWork.RecordDataRepository.GetLastRowByColumnIdAsync(item.Id, ct);
                for (int i = 0; i < count; i++)
                {
                    await unitOfWork.RecordDataRepository.AddAsync(new RecordData()
                    {
                        ColumnsId = item.Id,
                        Value = sqlProvider.GenerateFake(Enum.Parse<FakeDataTypesDto>(item.FakeDataTypes.ToString())).ToString(),
                        RowIndex = rowIndex
                    }
                        , ct);

                    rowIndex++;
                }

            }

            await unitOfWork.SaveChangesAsync(ct);

            return 1;

        }


    }
}
