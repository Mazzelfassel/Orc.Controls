﻿namespace Orc.Automation.Tests
{
    using System.Linq;
    using System.Windows.Automation;

    public class TestHostAutomationControl : AutomationControl
    {
        public TestHostAutomationControl(AutomationElement element) 
            : base(element)
        {
        }

        public string TryLoadControl(string fullName, string assemblyLocation, params string[] resources)
        {
            if (!Access.Execute<bool>(nameof(TestHostAutomationPeer.LoadAssembly), assemblyLocation))
            {
                return $"Error! Can't load control assembly from: {assemblyLocation}";
            }

            foreach (var resource in resources ?? Enumerable.Empty<string>())
            {
                if (!Access.Execute<bool>(nameof(TestHostAutomationPeer.LoadResources), resource))
                {
                    return $"Error! Can't load control resource: {resource}";
                }
            }

            var testedControlAutomationId = Access.Execute<string>(nameof(TestHostAutomationPeer.PutControl), fullName);
            if (string.IsNullOrWhiteSpace(testedControlAutomationId) || testedControlAutomationId.StartsWith("Error"))
            {
                return $"Error! Can't put control inside test host control: {fullName}";
            }

            return testedControlAutomationId;
        }
    }
}
