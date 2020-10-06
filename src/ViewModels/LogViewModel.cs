using System;
using Prism.Mvvm;
using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.ViewModels
{
    public class LogViewModel: BindableBase, ILogViewModel
    {
        public LogViewModel(Exception exception)
        {
            Type = "Exception";
            Message = exception.Message;
        }

        public string Message { get; set; }
        public string Type { get; set; }
    }
}