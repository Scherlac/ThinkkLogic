using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Windows.Input;

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
