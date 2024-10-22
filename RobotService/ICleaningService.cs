public interface ICleaningService
{
    Task<CleaningResult> Calculate(CleaningPath path);
}