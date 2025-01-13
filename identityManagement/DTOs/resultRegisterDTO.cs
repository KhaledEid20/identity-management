using System;

namespace identityManagement.DTOs;

public class resultRegisterDTO
{
    public string email { get; set; }
    public string code { get; set; }
    public string error { get; set; }
}
