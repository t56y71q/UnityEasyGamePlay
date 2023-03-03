using System;
using UnityEngine.InputSystem;

namespace EasyGamePlay
{
    abstract class BaseInputAction
    {
        internal int count = 0;

        public abstract void Start(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext);
        public abstract void Perform(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext);
        public abstract void Cancel(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext);
    }

    class InputAction : BaseInputAction
    {
        internal Action start;
        internal Action perform;
        internal Action cancel;

        static Action defaultAction = delegate () { };

        public InputAction()
        {
            start = defaultAction;
            perform = defaultAction;
            cancel = defaultAction;
        }

        public override void Cancel(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
        {
            start();
        }

        public override void Perform(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
        {
            perform();
        }

        public override void Start(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
        {
            cancel();
        }
    }

    class InputAction<T> : BaseInputAction where T:struct
    {
        internal Action<T> start;
        internal Action<T> perform;
        internal Action<T> cancel;

        static Action<T> defaultAction = delegate (T value) { };

        public InputAction()
        {
            start = defaultAction;
            perform = defaultAction;
            cancel = defaultAction;
        }

        public override void Cancel(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
        {
            start(callbackContext.ReadValue<T>());
        }

        public override void Perform(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
        {
            perform(callbackContext.ReadValue<T>());
        }

        public override void Start(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
        {
            cancel(callbackContext.ReadValue<T>());
        }
    }

    //public interface IInputAction : IBaseInputAction
    //{
    //    void Preform();
    //    void Start();
    //    void Cancel();
    //}

    //public interface IInputAction<T>: IBaseInputAction where T : struct
    //{
    //    void Preform(T param);
    //    void Start(T param);
    //    void Cancel(T param);
    //}
}
