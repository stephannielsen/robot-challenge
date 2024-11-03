namespace RobotService;

public interface ICleaningService
{
    Task<CleaningResult> CalculateResult(CleaningPath path, RobotDb db);
}