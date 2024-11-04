namespace RobotService;

public interface ICleaningService
{
    CleaningResult CalculateResult(CleaningPath path);
}