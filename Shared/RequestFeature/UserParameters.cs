namespace Shared.RequestFeature;

public class UserParameters : RequestParamters
{
    public int MinAge { get; set; } = 1;

    public int MaxAge { get; set; } = int.MaxValue;

    public bool ValidateMaxAge => MaxAge > MinAge;
}