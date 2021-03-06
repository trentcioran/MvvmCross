// MvxSpinnerSelectedItemBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxSpinnerSelectedItemBinding : MvxBaseAndroidTargetBinding
    {
        private readonly MvxBindableSpinner _spinner;
        private object _currentValue;

        public MvxSpinnerSelectedItemBinding(MvxBindableSpinner spinner)
        {
            _spinner = spinner;
            _spinner.ItemSelected += SpinnerItemSelected;
        }

        private void SpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var newValue = _spinner.Adapter.GetRawItem(e.Position);
            if (!newValue.Equals(_currentValue))
            {
                _currentValue = newValue;
                FireValueChanged(newValue);
            }
        }

        public override void SetValue(object value)
        {
            if (!value.Equals(_currentValue))
            {
                var index = _spinner.Adapter.GetPosition(value);
                if (index < 0)
                {
                    MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value not found for spinner {0}", value.ToString());
                    return;
                }
                _currentValue = value;
                _spinner.SetSelection(index);
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        public override Type TargetType
        {
            get { return typeof (object); }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _spinner.ItemSelected -= SpinnerItemSelected;
            }
            base.Dispose(isDisposing);
        }
    }
}