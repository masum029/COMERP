﻿namespace UI.Services.Interface
{
    public interface ITokenService
    {
        void SaveToken(string token);
        string? GetToken();
        void ClearToken();
    }
}
