namespace MockPars.Application.DTO.Table;

public record TableItemDto(int Id,int DatabaseId, string TableName, string Slug,bool IsGetAll,bool IsGet,bool IsPut,bool IsPost,bool IsDelete);