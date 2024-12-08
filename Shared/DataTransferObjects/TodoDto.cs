namespace Shared.DataTransferObjects;

public record TodoDto(Guid Id, string Title, string Description, bool IsDone);