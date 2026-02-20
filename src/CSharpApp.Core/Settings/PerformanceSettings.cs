using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpApp.Core.Settings
{
    public class PerformanceSettings
    {
        public int SlowRequestThresholdMs { get; set; } = 500;
    }
}
