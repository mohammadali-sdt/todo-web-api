using System;

namespace Entities.Exceptions;

public class PasswordsDoesNotMatch : BadRequestException
{
    public PasswordsDoesNotMatch() : base("Password and PasswordConfirm doesn't match.")
    {

    }
}
