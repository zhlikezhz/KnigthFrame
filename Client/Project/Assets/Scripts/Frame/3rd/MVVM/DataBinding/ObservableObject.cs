using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Huge.MVVM.DataBinding
{
    [Serializable]
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        private static readonly PropertyChangedEventArgs NULL_EVENT_ARGS = new PropertyChangedEventArgs(null);
        private static readonly Dictionary<string, PropertyChangedEventArgs> PROPERTY_EVENT_ARGS = new Dictionary<string, PropertyChangedEventArgs>();

        private static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
        {
            if (propertyName == null)
                return NULL_EVENT_ARGS;

            PropertyChangedEventArgs eventArgs;
            if (PROPERTY_EVENT_ARGS.TryGetValue(propertyName, out eventArgs))
                return eventArgs;

            eventArgs = new PropertyChangedEventArgs(propertyName);
            PROPERTY_EVENT_ARGS[propertyName] = eventArgs;
            return eventArgs;
        }

        private PropertyChangedEventHandler propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { this.propertyChanged += value; }
            remove { this.propertyChanged -= value; }
        }

        /// <summary>
        /// Raises the PropertyChanging event.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected virtual void RaisePropertyChanged(string propertyName = null)
        {
            RaisePropertyChanged(GetPropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the PropertyChanging event.
        /// </summary>
        /// <param name="eventArgs">Property changed event.</param>
        protected virtual void RaisePropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            try
            {
                if (propertyChanged != null)
                {
                    propertyChanged(this, eventArgs);
                }
            }
            catch (Exception e)
            {
                Huge.Debug.LogWarningFormat("Set property '{0}', raise PropertyChanged failure.Exception:{1}", eventArgs.PropertyName, e);
            }
        }

        /// <summary>
        /// Raises the PropertyChanging event.
        /// </summary>
        /// <param name="eventArgs"></param>
        protected virtual void RaisePropertyChanged(params PropertyChangedEventArgs[] eventArgs)
        {
            foreach (var args in eventArgs)
            {
                try
                {
                    if (propertyChanged != null)
                    {
                        propertyChanged(this, args);
                    }
                }
                catch (Exception e)
                {
                    Huge.Debug.LogWarningFormat("Set property '{0}', raise PropertyChanged failure.Exception:{1}", args.PropertyName, e);
                }
            }
        }

        /// <summary>
        ///  Set the specified propertyName, field, newValue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        ///  Set the specified propertyName, field, newValue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="newValue"></param>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        protected bool Set<T>(ref T field, T newValue, PropertyChangedEventArgs eventArgs)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            RaisePropertyChanged(eventArgs);
            return true;
        }

        /// <summary>
        ///  Set the specified propertyName, field, newValue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        [Obsolete]
        protected bool SetValue<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null) where T : IEquatable<T>
        {
            if ((field != null && field.Equals(newValue)) || (field == null && newValue == null))
            {
                return false;
            }

            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        ///  Set the specified propertyName, field, newValue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="newValue"></param>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        [Obsolete]
        protected bool SetValue<T>(ref T field, T newValue, PropertyChangedEventArgs eventArgs) where T : IEquatable<T>
        {
            if ((field != null && field.Equals(newValue)) || (field == null && newValue == null))
            {
                return false;
            }

            field = newValue;
            RaisePropertyChanged(eventArgs);
            return true;
        }
    }
}
