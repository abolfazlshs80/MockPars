namespace MockPars.Application.DTO.Database;

public class UpdateDatabaseDto
{
    public int Id { get; set; }
    public string DatabaseName { get; set; }

    public string Slug { get; set; }
    public string UserId { get; set; }
}