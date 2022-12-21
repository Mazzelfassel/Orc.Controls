﻿namespace Orc.Controls
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catel.Data;
    using Catel.Logging;

    public class LogFilterGroup : ModelBase
    {
        #region Properties
        public string Name { get; set; }

        public bool IsRuntime { get; set; }

        public bool IsEnabled { get; set; } = true;

        public ObservableCollection<LogFilter> LogFilters { get; set; } = new ObservableCollection<LogFilter>();
        #endregion

        #region Methods
        public bool Pass(LogEntry logEntry)
        {
            return LogFilters.All(filter => filter.Pass(logEntry));
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
