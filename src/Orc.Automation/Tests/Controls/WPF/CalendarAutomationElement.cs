﻿namespace Orc.Automation.Tests
{
    using System.Windows.Automation;


    public class CalendarAutomationElement : ProtectedByControlTypeAutomationControl
    {
        public CalendarAutomationElement(AutomationElement element) 
            : base(element, ControlType.Calendar)
        {
        }

        //public void Some()
        //{
        //    Element.RunPatternFunc<TablePattern>(x => x.GetItem(2, 2));
        //}

        //public void Some2()
        //{

        //    var selection = Element.RunPatternFunc<SelectionPattern, AutomationElement[]>(x => x.Current.GetSelection());

        //    var automationElement = Element.RunPatternFunc<TablePattern, AutomationElement>(x => x.GetItem(2, 2));

        //    var automationElements = Element.RunPatternFunc<TablePattern, AutomationElement[]>(x => x.Current.GetColumnHeaders());
        //}
    }
}
