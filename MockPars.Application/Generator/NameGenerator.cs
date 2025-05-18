namespace MockPars.Application.Generator
{
    public static class NameGenerator
    {
        public static string GenerateUniqueCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
