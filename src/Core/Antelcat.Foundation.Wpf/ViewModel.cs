﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Antelcat.Core.Extensions;

namespace Feast.Foundation.Wpf
{
    public partial class ViewModel : ObservableObject
    {
        [ObservableProperty] private string? message;

        public int Index => index++;
        private int index;
        private CancellationTokenSource? canceler;

        private void ShowMessage()
        {
            canceler?.Cancel();
            Message = $"This is message {Index}";
            var c = canceler = new();
            Task.Run(async() =>
            {
                if (c.Token.IsCancellationRequested) return;
                Message = null;
            }, c.Token);
        }

        [RelayCommand]
        private async Task Show()
        {
            ShowMessage();
        }
    }
}
