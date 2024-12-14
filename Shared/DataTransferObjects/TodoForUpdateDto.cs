namespace Shared.DataTransferObjects;

public record TodoForUpdateDto : TodoForManipulationDto
{ 
    public bool IsDone { get; init; }
}