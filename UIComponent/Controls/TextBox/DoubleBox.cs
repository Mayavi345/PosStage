﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace UIComponent.Controls
{
    //From :https://github.com/NickeManarin/ScreenToGif 
    public class DoubleBox : ExtendedTextBox
    {
        #region Variables

        private bool _ignore;
        private string _baseFormat = "{0:###,###,###,###,##0.";
        private string _format = "{0:###,###,###,###,##0.00}";

        #endregion

        #region Dependency Property

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(DoubleBox),
            new FrameworkPropertyMetadata(double.MaxValue, OnMaximumPropertyChanged));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(DoubleBox),
            new FrameworkPropertyMetadata(0D, OnValuePropertyChanged));

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(DoubleBox),
            new FrameworkPropertyMetadata(0D, OnMinimumPropertyChanged));

        public static readonly DependencyProperty DecimalsProperty = DependencyProperty.Register(nameof(Decimals), typeof(int), typeof(DoubleBox),
            new FrameworkPropertyMetadata(2, OnDecimalsPropertyChanged));

        public static readonly DependencyProperty StepProperty = DependencyProperty.Register(nameof(StepValue), typeof(double), typeof(DoubleBox),
            new FrameworkPropertyMetadata(1D));

        public static readonly DependencyProperty UpdateOnInputProperty = DependencyProperty.Register(nameof(UpdateOnInput), typeof(bool), typeof(DoubleBox),
            new FrameworkPropertyMetadata(false, OnUpdateOnInputPropertyChanged));

        public static readonly DependencyProperty DefaultValueIfEmptyProperty = DependencyProperty.Register(nameof(DefaultValueIfEmpty), typeof(double), typeof(DoubleBox),
            new FrameworkPropertyMetadata(0D));

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(nameof(Scale), typeof(double), typeof(DoubleBox),
            new PropertyMetadata(1D, OnScalePropertyChanged));

        #endregion

        #region Properties

        [Bindable(true), Category("Common")]
        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        [Bindable(true), Category("Common")]
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        [Bindable(true), Category("Common")]
        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        [Bindable(true), Category("Common")]
        public int Decimals
        {
            get => (int)GetValue(DecimalsProperty);
            set => SetValue(DecimalsProperty, value);
        }

        /// <summary>
        /// The Increment/Decrement value.
        /// </summary>
        [Description("The Increment/Decrement value.")]
        public double StepValue
        {
            get => (double)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        [Bindable(true), Category("Common")]
        public bool UpdateOnInput
        {
            get => (bool)GetValue(UpdateOnInputProperty);
            set => SetValue(UpdateOnInputProperty, value);
        }

        [Bindable(true), Category("Common")]
        public double DefaultValueIfEmpty
        {
            get => (double)GetValue(DefaultValueIfEmptyProperty);
            set => SetValue(DefaultValueIfEmptyProperty, value);
        }

        [Bindable(true), Category("Common")]
        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        #endregion

        #region Properties Changed

        private static void OnMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DoubleBox doubleBox))
                return;

            if (doubleBox.Value > doubleBox.Maximum)
                doubleBox.Value = doubleBox.Maximum;
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DoubleBox doubleBox))
                return;

            if (doubleBox.Value > doubleBox.Maximum)
                doubleBox.Value = doubleBox.Maximum;

            else if (doubleBox.Value < doubleBox.Minimum)
                doubleBox.Value = doubleBox.Minimum;

            doubleBox.Value = Math.Round(doubleBox.Value, doubleBox.Decimals);

            if (!doubleBox._ignore)
            {
                var value = string.Format(CultureInfo.CurrentCulture, doubleBox._format, doubleBox.Value * doubleBox.Scale);

                if (!string.Equals(doubleBox.Text, value))
                    doubleBox.Text = value;
            }

            doubleBox.RaiseValueChangedEvent();
        }

        private static void OnMinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DoubleBox doubleBox))
                return;

            if (doubleBox.Value < doubleBox.Minimum)
                doubleBox.Value = doubleBox.Minimum;
        }

        private static void OnDecimalsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DoubleBox doubleBox))
                return;

            doubleBox._format = doubleBox._baseFormat + "".PadRight(doubleBox.Decimals, '0') + "}";

            doubleBox.Value = Math.Round(doubleBox.Value, doubleBox.Decimals);
        }

        private static void OnUpdateOnInputPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DoubleBox)d).UpdateOnInput = (bool)e.NewValue;
        }

        private static void OnScalePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DoubleBox doubleBox)) return;

            //The scale value dictates the value being displayed.
            //For example, The value 600 and the scale 1.25 should display the text 750.
            //Text = Value * Scale.

            doubleBox.Text = string.Format(CultureInfo.CurrentCulture, doubleBox._format, doubleBox.Value * doubleBox.Scale);
        }

        #endregion

        #region Custom Events

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DoubleBox));

        /// <summary>
        /// Event raised when the numeric value is changed.
        /// </summary>
        public event RoutedEventHandler ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }

        public void RaiseValueChangedEvent()
        {
            if (ValueChangedEvent == null)
                return;

            RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }

        #endregion

        static DoubleBox()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleBox), new FrameworkPropertyMetadata(typeof(DoubleBox)));
        }

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AddHandler(DataObject.PastingEvent, new DataObjectPastingEventHandler(OnPasting));

            _format = _baseFormat + "".PadRight(Decimals, '0') + "}";
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Text = string.Format(CultureInfo.CurrentCulture, _format, Value);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //Only sets the focus if not clicking on the Up/Down buttons of a IntegerUpDown.
            if (e.OriginalSource is TextBlock || e.OriginalSource is Border)
                return;

            if (!IsKeyboardFocusWithin)
            {
                e.Handled = true;
                Focus();
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (e.Source is DoubleBox)
                SelectAll();
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Text))
            {
                e.Handled = true;
                return;
            }

            if (!IsEntryAllowed(this, e.Text))
            {
                e.Handled = true;
                return;
            }

            base.OnPreviewTextInput(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (!UpdateOnInput || _ignore)
                return;

            if (string.IsNullOrEmpty(Text)) return;
            if (!IsTextAllowed(Text)) return;

            _ignore = true;

            Value = Math.Round(Convert.ToDouble(Text, CultureInfo.CurrentCulture) / Scale, Decimals);

            _ignore = false;

            base.OnTextChanged(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (!UpdateOnInput)
            {
                if (string.IsNullOrEmpty(Text) || !IsTextAllowed(Text))
                {
                    Value = DefaultValueIfEmpty;
                    return;
                }

                _ignore = true;

                Value = Convert.ToDouble(Text, CultureInfo.CurrentCulture);
                Text = string.Format(CultureInfo.CurrentCulture, _format, Value);

                _ignore = false;
                return;
            }

            Text = string.Format(CultureInfo.CurrentCulture, _format, Value);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                e.Handled = true;
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }

            base.OnKeyDown(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (!IsKeyboardFocusWithin)
                return;

            var step = Keyboard.Modifiers == (ModifierKeys.Shift | ModifierKeys.Control)
                ? 50 : Keyboard.Modifiers == ModifierKeys.Shift
                    ? 10 : Keyboard.Modifiers == ModifierKeys.Control
                        ? 5 : StepValue;

            if (e.Delta > 0)
                Value += step;
            else
                Value -= step;

            e.Handled = true;
        }

        #endregion

        #region Base Properties Changed

        private void OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = e.DataObject.GetData(typeof(string)) as string;

                if (!IsTextAllowed(text))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        #endregion

        #region Methods

        private bool IsEntryAllowed(TextBox textBox, string text)
        {
            //Digits, points or commas.
            var regex = new Regex(@"^[0-9]|\.|\,$"); //TODO: Support for multiple cultures.

            //Checks if it's a valid char based on the context.
            return regex.IsMatch(text) && IsEntryAllowedInContext(textBox, text);
        }

        private bool IsEntryAllowedInContext(TextBox textBox, string next)
        {
            //if number, allow.
            if (char.IsNumber(next.ToCharArray().FirstOrDefault()))
                return true;

            #region Thousands

            var thousands = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            var thousandsChar = thousands.ToCharArray().FirstOrDefault();
            var decimals = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var decimalsChar = decimals.ToCharArray().FirstOrDefault();

            if (next.Equals(thousands))
            {
                var textAux = textBox.Text;

                if (!string.IsNullOrEmpty(textBox.SelectedText))
                    textAux = textAux.Replace(textBox.SelectedText, "");

                var before = textAux.Substring(0, textBox.SelectionStart);
                var after = textAux.Substring(textBox.SelectionStart);

                //If there's no text, is not allowed to add a thousand separator.
                if (string.IsNullOrEmpty(after + before)) return false;

                //Before the carret.
                if (!string.IsNullOrEmpty(before))
                {
                    //You can't add a thousand separator after the decimal.
                    if (before.Contains(decimals)) return false;

                    //Check the previous usage of a thousand separator.
                    if (before.Contains(thousands))
                    {
                        var split = before.Split(thousandsChar);

                        //You can't add a thousand separators closer than 3 chars from each other.
                        if (split.Last().Length != 3) return false;
                    }
                }

                //After the carret.
                if (!string.IsNullOrEmpty(after))
                {
                    var split = after.Split(thousandsChar, decimalsChar);

                    //You can't add a thousand separators closer than 3 chars from another separator, decimal or thousands.
                    if (split.First().Length != 3) return true;
                }

                return false;
            }

            #endregion

            #region Decimal

            if (next.Equals(decimals))
            {
                return !textBox.Text.Any(x => x.Equals(decimalsChar));
            }

            #endregion

            return true;
        }

        private bool IsTextAllowed(string text)
        {
            return double.TryParse(text, out double result);

            //var regex = new Regex(@"^((\d+)|(\d{1,3}(\.\d{3})+)|(\d{1,3}(\.\d{3})(\,\d{3})+))((\,\d{4})|(\,\d{3})|(\,\d{2})|(\,\d{1})|(\,))?$", RegexOptions.CultureInvariant);
            //return regex.IsMatch(text);
        }

        #endregion
    }
}
