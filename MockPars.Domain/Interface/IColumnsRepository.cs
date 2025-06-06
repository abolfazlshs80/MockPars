﻿using MockPars.Domain.Models;

namespace MockPars.Domain.Interface;

public interface IColumnsRepository : IRepository<Columns>
{
    Task<Columns> GetByColumnNameAsync(string name, CancellationToken ct);
    Task<Columns> GetByIdAsync(int id, int tableId,CancellationToken ct);
    Task<IEnumerable<Columns>> GetByTableIdAsync( int tableId,CancellationToken ct);
    Task<List<Columns>> GetAllRowDataAsync(int tableId, CancellationToken ct);
}