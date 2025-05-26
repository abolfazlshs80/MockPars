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
    class FakeService(IUnitOfWork unitOfWork) : IFakeService
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
                        Value = GenerateFake(Enum.Parse<FakeDataTypesDto>(item.FakeDataTypes.ToString())),
                        RowIndex = rowIndex
                    }
                        , ct);

                    rowIndex++;
                }

            }

            await unitOfWork.SaveChangesAsync(ct);

            return 1;

        }

        string GenerateFake(FakeDataTypesDto type)
        {
            switch (type)
            {
                case FakeDataTypesDto.Name:
                    return FakeValue.Names[Random.Shared.Next(0, FakeValue.Names.Count - 1)];
                    // Generate fake name logic
                    break;
                case FakeDataTypesDto.City:
                    // Generate fake city logic
                    return FakeValue.Cities[Random.Shared.Next(0, FakeValue.Cities.Count - 1)];
                    break;
                case FakeDataTypesDto.Phone:
                    // Generate fake phone logic
                    return FakeValue.PhoneNumbers[Random.Shared.Next(0, FakeValue.PhoneNumbers.Count - 1)];
                    break;
                case FakeDataTypesDto.Date:
                    return DateTime.Now.AddDays(Random.Shared.Next(-365, 0)).ToShamsi(); // Generate a random date within the last year
                    // Generate fake date logic
                    break;
                case FakeDataTypesDto.Time:
                    // Generate fake time logic
                    // Generate a random time within the day
                    return DateTime.Now.AddHours(Random.Shared.Next(0, 23)).ToString("HH:mm:ss"); // Generate a random time within the day
                    break;
                default:
                    throw new ArgumentException("Invalid fake data type", nameof(type));
            }
        }
    }
}
