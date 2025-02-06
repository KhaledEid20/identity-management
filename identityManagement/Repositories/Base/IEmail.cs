using System;

namespace identityManagement.Repositories.Base;

public interface IEmail
{
    Task<string> sendEmail(string email , string code);
}
