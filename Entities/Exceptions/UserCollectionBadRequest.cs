namespace Entities.Exceptions;

public class UserCollectionBadRequest: BadRequestException
{
    public UserCollectionBadRequest(): base("User collection sent from a client is null.") {}
}