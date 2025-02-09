﻿namespace Orc.Controls
{
    using System;
    using Catel;
    using System.Windows;
    using System.Windows.Controls;
    using UIEventArgs = System.EventArgs;

    /// <summary>
    /// A grid-like control that allows a developer to specify the rows and columns, but gives the freedom
    /// not to define the actual grid and row numbers of the controls inside the <see cref="StackGrid"/>.
    /// <para />
    /// The <see cref="StackGrid"/> automatically builds up the internal grid.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <StackGrid>
    ///   <StackGrid.RowDefinitions>
    ///     <RowDefinition Height="Auto" />
    ///     <RowDefinition Height="*" />
    ///     <RowDefinition Height="Auto" />
    ///   </StackGrid.RowDefinitions>
    /// 
    ///   <StackGrid.ColumnDefinitions>
    ///	    <ColumnDefinition Width="Auto" />
    ///	    <ColumnDefinition Width="*" />
    ///   </StackGrid.ColumnDefinitions>
    /// 
    ///   <!-- Name, will be set to row 0, column 1 and 2 -->
    ///   <Label Content="Name" />
    ///   <TextBox Text="{Bindng Name}" />
    /// 
    ///   <!-- Empty row -->
    ///   <EmptyRow />
    /// 
    ///   <!-- Wrappanel, will span 2 columns -->
    ///   <WrapPanel StackGrid.ColumnSpan="2">
    ///     <Button Command="{Binding OK}" />
    ///   </WrapPanel>
    /// </StackGrid>
    /// ]]>
    /// </code>
    /// </example>
    public class StackGrid : Grid
    {
        private bool _isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackGrid"/> class.
        /// </summary>
        public StackGrid()
        {
            if (CatelEnvironment.IsInDesignMode)
            {
                Loaded += OnInitialized;
            }
            else
            {
                Initialized += OnInitialized;
            }
        }

        /// <summary>
        /// Called when the control is initialized.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// In the non-WPF implementation, this event is actually hooked to the <c>LayoutUpdated</c> event.
        /// </remarks>
        private void OnInitialized(object? sender, UIEventArgs e)
        {
            if (CatelEnvironment.IsInDesignMode)
            {
                Loaded -= OnInitialized;
            }
            else
            {
                Initialized -= OnInitialized;
            }

            FinalInitialize();
        }

        /// <summary>
        /// Final initialize so the <see cref="StackGrid"/> is actually created.
        /// </summary>
        private void FinalInitialize()
        {
            if (_isInitialized)
            {
                return;
            }

            SetColumnsAndRows();

            _isInitialized = true;
        }

        /// <summary>
        /// Sets the columns and rows.
        /// </summary>
        internal void SetColumnsAndRows()
        {
            var columnCount = Math.Max(ColumnDefinitions.Count, 1);
            var currentColumn = 0;
            var currentRow = 0;

            foreach (FrameworkElement child in Children)
            {
                var columnSpan = Grid.GetColumnSpan(child);
                if (child is EmptyRow)
                {
                    // If not yet reached the end of columns, force a new increment anyway
                    if (currentColumn != 0 && currentColumn <= columnCount)
                    {
                        currentRow++;
                    }

                    // The current column for an empty row is alway zero
                    currentColumn = 0;

                    Grid.SetColumn(child, currentColumn);
                    Grid.SetColumnSpan(child, columnCount);
                    Grid.SetRow(child, currentRow);

                    currentRow++;
                    continue;
                }

                Grid.SetColumn(child, currentColumn);
                Grid.SetRow(child, currentRow);

                if (currentColumn + columnSpan >= columnCount)
                {
                    currentColumn = 0;
                    currentRow++;
                }
                else
                {
                    // Increment the current column by the column span
                    currentColumn = currentColumn + columnSpan;
                }
            }
        }
    }
}
