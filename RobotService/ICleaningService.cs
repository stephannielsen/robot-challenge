namespace RobotService;

public interface ICleaningService
{
    Task<CleaningResult> Calculate(CleaningPath path, RobotDb db);
}