﻿using System;
using System.Threading.Tasks;

namespace Lux.Interfaces
{
    public interface IAlertService
    {
        Task ShowAsync(string title, string message, string buttonOk);

        Task<bool> ShowAsyncConfirm(string title, string message, string buttonOk, string buttonCancel);
    }
}
