using System;
using System.Diagnostics;

namespace ProductsAPI.Services
{
	public class NotifyService: INotifyService
	{
        public void Notify(string userId, string message)
        {
            Debug.WriteLine($"Sending message...{userId} - {message}");
        }

    }
}

