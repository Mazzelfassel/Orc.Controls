﻿namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using Automation;

    /// <summary>
    /// The color picker.
    /// </summary>
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public class ColorPicker : Control
    {
        #region Fields
        /// <summary>
        /// The color board.
        /// </summary>
        private ColorBoard _colorBoard;

        /// <summary>
        /// The popup.
        /// </summary>
        private Popup _popup;
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class.
        /// </summary>
        public ColorPicker()
        {
            DefaultStyleKey = typeof(ColorPicker);
        }
        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// The on apply template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _popup = (Popup)GetTemplateChild("PART_Popup");

            _colorBoard = new ColorBoard();
            _colorBoard.SetCurrentValue(ColorBoard.ColorProperty, Color);
            _colorBoard.SizeChanged += OnColorBoardSizeChanged;

            _popup.SetCurrentValue(Popup.ChildProperty, _colorBoard);
            _colorBoard.DoneClicked += OnColorBoardDoneClicked;
            _colorBoard.CancelClicked += OnColorBoardCancelClicked;

            var b = new Binding(nameof(ColorBoard.Color))
            {
                Mode = BindingMode.TwoWay,
                Source = _colorBoard
            };
            SetBinding(CurrentColorProperty, b);

            KeyDown += OnColorPickerKeyDown;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// The color property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            nameof(Color), typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.White, OnColorChanged));


        /// <summary>
        /// Gets or sets the current color.
        /// </summary>
        public Color CurrentColor
        {
            get { return (Color)GetValue(CurrentColorProperty); }
            set { SetValue(CurrentColorProperty, value); }
        }

        /// <summary>
        /// The current color property.
        /// </summary>
        public static readonly DependencyProperty CurrentColorProperty = DependencyProperty.Register(
            nameof(CurrentColor), typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.White));

        /// <summary>
        /// Gets or sets a value indicating whether is drop down open.
        /// </summary>
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        /// <summary>
        /// The is drop down open property.
        /// </summary>
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            nameof(IsDropDownOpen),
            typeof(bool),
            typeof(ColorPicker),
            new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the popup placement.
        /// </summary>
        public PlacementMode PopupPlacement
        {
            get { return (PlacementMode)GetValue(PopupPlacementProperty); }
            set { SetValue(PopupPlacementProperty, value); }
        }

        /// <summary>
        /// The popup placement property.
        /// </summary>
        public static readonly DependencyProperty PopupPlacementProperty = DependencyProperty.Register(
            nameof(PopupPlacement), typeof(PlacementMode), typeof(ColorPicker), new PropertyMetadata(PlacementMode.Bottom));
        #endregion

        #region Public Events
        /// <summary>
        /// The color changed.
        /// </summary>
        public event EventHandler<ColorChangedEventArgs> ColorChanged;
        #endregion

        #region Methods
        /// <summary>
        /// The on color property chaged.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cp = (ColorPicker)d;

            cp?.RaiseColorChanged((Color)e.NewValue, (Color)e.OldValue);
        }

        /// <summary>
        /// The color picker_ key down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnColorPickerKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && IsDropDownOpen)
            {
                _colorBoard.OnDoneClicked();
            }
        }

        /// <summary>
        /// The on color changed.
        /// </summary>
        /// <param name="newColor">The new color.</param>
        /// <param name="oldColor">The old color.</param>
        private void RaiseColorChanged(Color newColor, Color oldColor)
        {
            ColorChanged?.Invoke(this, new ColorChangedEventArgs(newColor, oldColor));
        }

        /// <summary>
        /// The color board_ done clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnColorBoardDoneClicked(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(ColorProperty, _colorBoard.Color);
            _popup.SetCurrentValue(Popup.IsOpenProperty, false);
        }

        /// <summary>
        /// The color board_ cancel clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnColorBoardCancelClicked(object sender, RoutedEventArgs e)
        {
            var color = Color;

            SetCurrentValue(CurrentColorProperty, color);
            _colorBoard.SetCurrentValue(ColorBoard.ColorProperty, color);
            _popup.SetCurrentValue(Popup.IsOpenProperty, false);
        }

        /// <summary>
        /// The color board_ size changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnColorBoardSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (PopupPlacement == PlacementMode.Bottom)
            {
                _popup.SetCurrentValue(Popup.VerticalOffsetProperty, ActualHeight);
            }

            if (PopupPlacement == PlacementMode.Top)
            {
                _popup.SetCurrentValue(Popup.VerticalOffsetProperty, -1 * _colorBoard.ActualHeight);
            }

            if (PopupPlacement == PlacementMode.Right)
            {
                _popup.SetCurrentValue(Popup.HorizontalOffsetProperty, ActualWidth);
            }

            if (PopupPlacement == PlacementMode.Left)
            {
                _popup.SetCurrentValue(Popup.HorizontalOffsetProperty, -1 * _colorBoard.ActualWidth);
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ColorPickerAutomationPeer(this);
        }
        #endregion
    }
}
