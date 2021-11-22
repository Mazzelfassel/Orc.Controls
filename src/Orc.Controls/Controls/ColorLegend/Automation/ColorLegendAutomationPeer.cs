﻿namespace Orc.Automation
{
    using System;
    using System.Linq;
    using Controls;

    public class ColorLegendAutomationPeer : ControlCommandAutomationPeerBase<ColorLegend>
    {
        public ColorLegendAutomationPeer(ColorLegend owner)
            : base(owner)
        {
            owner.SelectionChanged += OwnerOnSelectionChanged;
        }

        private void OwnerOnSelectionChanged(object sender, EventArgs e)
        {
            RaiseEvent(nameof(ColorLegend.SelectionChanged), 10);
        }

        [CommandRunMethod]
        public void ChangeItemState(int index)
        {
            var item = Control.ItemsSource.ElementAt(index);

            item.IsChecked = !item.IsChecked;
        }
    }
}
