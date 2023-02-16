using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public class Exception
    {
        private  Dictionary<Type, Action<System.Exception>> exceptions;
        private  Action<System.Exception> globalExceptions=null;

        public  void Init()
        {
            exceptions = new Dictionary<Type, Action<System.Exception>>();
            globalExceptions = null;
        }

        public void Destroy()
        {
            exceptions.Clear();
        }

        public void AddException<T>(Action<System.Exception> action) where T: System.Exception
        {
            if(exceptions.TryGetValue(typeof(T),out Action<System.Exception> value))
            {
                value += action;
            }
            else
            {
                exceptions.Add(typeof(T), action);
            }
        }

        public void AddGlobalException(Action<System.Exception> action)
        {
            globalExceptions += action;
        }

        internal void SendException(System.Exception exception)
        {
            if (exceptions.TryGetValue(exception.GetType(), out Action<System.Exception> value))
            {
                value.Invoke(exception);
            }

            if(globalExceptions!=null)
            {
                globalExceptions(exception);
            }
        } 
    }
}
