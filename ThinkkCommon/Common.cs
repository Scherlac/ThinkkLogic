using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ThinkkCommon
{

    public enum Colors
    {
        Red = 0x01,
        Yellow = 0x02,
        White = 0x03,
        Blue = 0x04,
        Green = 0x05,
        Black = 0x06,
        Grey = 0x08,
    }

    [Flags]
    public enum States : uint
    {
        Empty = 0x00,
        Red = 0x01,
        Yellow = 0x02,
        White = 0x03,
        Blue = 0x04,
        Green = 0x05,
        Black = 0x06,

        ColorMod = 0x07,

        Grey = 0x08,

        ColorMask = 0x0F,

        Hidden = 0x10,
        RightColor = 0x20,
        RigthPlace = 0x40,
        Unknown = 0x80,
    }


    public class RelayCommand : ICommand
    {
        public Func<object, bool> Function { get; private set; }
        public Action<object> Action { get; private set; }

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Func<object, bool> func, Action<object> act)
        {
            Function = func;
            Action = act;
        }

        //public void Disable()
        //{
        //    Function = (o) => { return false; };
        //    Action = null;
        //    CanExecuteChanged.Invoke(this, new EventArgs());
        //}

        public bool CanExecute(object parameter)
        {
            return Function.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            Action?.Invoke(parameter);
        }
    }

    public class Common
    {

    }

    /// <summary>
    /// This is the base implementation os the INotifyPropertyChanged. It is also used in the NetBSI Library in order to have a common and easy to use implementation.  
    /// </summary>
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        private event PropertyChangedEventHandler propertychanged;

        /// <summary>
        /// Occurs when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This method raise the event.
        /// </summary>
        /// <param name="propertyName">The name of the property has changed</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = propertychanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// This generic method is used to update the field behind the property.  
        /// </summary>
        /// <typeparam name="T">The type of the property. This is evaluated automatically</typeparam>
        /// <param name="field">This is the reference to the variable.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">The name of the property. This is evaluated automatically</param>
        /// <returns>This method return true on the case the field was updated.</returns>
        protected bool SetField<T>(ref T field, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// This generic method is used to update the field behind the property.  
        /// </summary>
        /// <typeparam name="T">The type of the property. This is evaluated automatically</typeparam>
        /// <param name="field">This is the reference to the variable.</param>
        /// <param name="value">The value.</param>
        /// <param name="Hi">The high limit.</param>
        /// <param name="Lo">The low limit.</param>
        /// <param name="propertyName">The name of the property. This is evaluated automatically</param>
        /// <returns>This method return true on the case the field was updated.</returns>
        protected bool SetFieldHiLo<T>(ref T field, T value, T Hi, T Lo,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            if (Comparer<T>.Default.Compare(value, Hi) > 0)
            {
                throw new ArgumentException(string.Format("The value has to be lower or equal to  {0}", Hi));
            }
            if (Comparer<T>.Default.Compare(value, Lo) < 0)
            {
                throw new ArgumentException(string.Format("The value has to be greater or equal to {0}", Lo));
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }


    public static class LocalExtensions
    {


        public static bool IsAssignableFrom(this Type value, Type referece)
        {
            return value.GetTypeInfo().IsAssignableFrom(referece.GetTypeInfo());
        }

        public static T RemoveFlag<T>(this Enum value, T f)
        {
            if (typeof(T).IsAssignableFrom(value.GetType()))
            {
                var vv = (uint)(object)value;
                var ff = (uint)(object)f;

                return (T)(object)(vv & ~ff);
            }

            throw new ArgumentException(string.Format("Invalid argument: Cannot convert type {0} to {1}", value.GetType(), typeof(T)));
        }

        public static T AddFlag<T>(this Enum value, T f)
        {
            if (typeof(T).IsAssignableFrom(value.GetType()))
            {
                var vv = (uint)(object)value;
                var ff = (uint)(object)f;

                return (T)(object)(vv | ff);
            }

            throw new ArgumentException(string.Format("Invalid argument: Cannot convert type {0} to {1}", value.GetType(), typeof(T)));
        }


    }

}
