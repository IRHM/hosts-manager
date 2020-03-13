﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace hosts_manager
{
    class ErrorHandler
    {
        public void showError(Exception ex, string customMessage = "")
        {
            // Get line that threw exception
            int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();

            // Show error in messagebox
            MessageBox.Show($"Error: {ex.Message} L:{line}\n{customMessage}");
        }
    }
}