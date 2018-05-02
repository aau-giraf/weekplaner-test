﻿using System;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace WeekPlanner.Services
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string message, string btnText = "OK", string title = null)
        {
            var config = new AlertConfig
            {
                Title = title,
                OkText = btnText,
                Message = message,
            };

            return UserDialogs.Instance.AlertAsync(config);
        }

        public Task<bool> ConfirmAsync(string message, string title = null, string cancelText = "Nej", string okText = "Ja")
        {
            ConfirmConfig config = new ConfirmConfig
            {
                Message = message,
                OkText = okText,
                Title = title,
                CancelText = cancelText,
            };

            return UserDialogs.Instance.ConfirmAsync(config);
        }

        public Task<string> ActionSheetAsync(string title, string cancel = "Annuller", string destructive = null, params string[] buttons)
        {
            return UserDialogs.Instance.ActionSheetAsync(title, cancel, destructive, null, buttons);
        }
    }
}
