using System;
namespace ProductsAPI.Services
{
    public interface INotifyService
    {
        void Notify(string userId, string message);
    }
}

