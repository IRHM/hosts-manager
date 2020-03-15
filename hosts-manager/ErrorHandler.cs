using System;
using System.Diagnostics;
using System.Windows;

namespace hosts_manager
{
    class ErrorHandler
    {
        public void ShowError(Exception ex, string customMessage = "")
        {
            // Get line that threw exception
            int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();

            // Show error in messagebox
            MessageBox.Show($"Error: {ex.Message} L:{line}\n{customMessage}");
        }
    }
}
