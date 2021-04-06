﻿namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Catel;
    using Catel.Collections;

    public class CalloutManager : ICalloutManager
    {
        public CalloutManager()
        {
            _callouts = new List<Callout>();
        }

        private IList<Callout> _callouts;

        public IList<Callout> Callouts
        {
            get
            {
                IList<Callout> calloutsCopy = _callouts.ToList();

                return calloutsCopy;
            }
            set
            {
                _callouts = value;
            }
        }

        public void Register(Callout callout)
        {
            Argument.IsNotNull(() => callout);

            _callouts.Add(callout);
        }

        public void Unregister(Callout callout)
        {
            _callouts.Remove(callout);
        }

        public void RemoveAllCallouts()
        {
            _callouts.Clear();
        }

        public void ShowAllCallouts()
        {
            foreach (var callout in Callouts)
            {
                if (callout.ViewModel is CalloutViewModel vm)
                {
                    vm.IsOpen = true;
                }
            }
        }

        public void ShowCallout(UIElement element)
        {
            foreach (var callout in Callouts)
            {
                if (callout.ViewModel is Controls.CalloutViewModel vm)
                {
                    if (vm.PlacementTarget == element)
                    {
                        vm.IsOpen = true;
                    }
                }
            }
        }
    }
}
