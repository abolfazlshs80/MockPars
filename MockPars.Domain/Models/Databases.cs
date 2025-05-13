namespace MockPars.Domain.Models;

public class Databases
{
    public int Id { get; set; }
    public string DatabaseName { get; set; }

    public string Slug { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public HashSet<Tables> Tables { get; set; }
}