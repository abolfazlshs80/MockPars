using MockPars.Application.DTO.Table;
using MockPars.Domain.Models;

namespace MockPars.Application.DTO.Database;

public record DatabaseItemDto(int Id, string DatabaseName, string Slug, IEnumerable<TableItemDto>Tables);