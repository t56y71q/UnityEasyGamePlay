using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EasyGamePlay
{
    public class EInput
    {
        private InputActionAsset inputAsset;
        private SortedList<string, BaseInputAction> inputActions = new SortedList<string, BaseInputAction>(10);

        public void LoadAsset(InputActionAsset inputAsset)
        {
            this.inputAsset = inputAsset;
            inputAsset.Enable();
        }

        public void UnloadAsset()
        {
            if(inputAsset!=null)
            {
                inputAsset.Disable();
            }
        }

        public void Bind(string name,Action start,Action perform,Action cancel)
        {
            if(!inputActions.TryGetValue(name,out BaseInputAction baseInputAction))
            {
                var action= inputAsset.FindAction(name);
                if (action == null)
                {
                    Debug.Log("Not find InputAction");
                    return;
                }
                    

                baseInputAction = new InputAction();
                inputActions.Add(name, baseInputAction);

                action.started += baseInputAction.Start;
                action.performed += baseInputAction.Perform;
                action.canceled += baseInputAction.Cancel;
            }

            baseInputAction.count++;
            InputAction inputAction = baseInputAction as InputAction;
            inputAction.start += start;
            inputAction.perform += perform;
            inputAction.cancel += cancel;
        }

        public void UnBind(string name, Action start, Action perform, Action cancel)
        {
            if (inputActions.TryGetValue(name, out BaseInputAction baseInputAction))
            {
                InputAction inputAction = baseInputAction as InputAction;
                inputAction.start -= start;
                inputAction.perform -= perform;
                inputAction.cancel -= cancel;

                if (--baseInputAction.count==0)
                {
                    inputActions.Remove(name);

                    var action = inputAsset.FindAction(name);

                    action.started -= baseInputAction.Start;
                    action.performed -= baseInputAction.Perform;
                    action.canceled -= baseInputAction.Cancel;
                }
            }
        }

        public void Bind<T>(string name, Action<T> start, Action<T> perform, Action<T> cancel) where T:struct
        {
            if (!inputActions.TryGetValue(name, out BaseInputAction baseInputAction))
            {
                var action = inputAsset.FindAction(name);
                if (action == null)
                {
                    Debug.Log("Not find InputAction");
                    return;
                }

                baseInputAction = new InputAction<T>();
                inputActions.Add(name, baseInputAction);

                action.started += baseInputAction.Start;
                action.performed += baseInputAction.Perform;
                action.canceled += baseInputAction.Cancel;
            }

            baseInputAction.count++;
            InputAction<T> inputAction = baseInputAction as InputAction<T>;
            inputAction.start += start;
            inputAction.perform += perform;
            inputAction.cancel += cancel;
        }

        public void UnBind<T>(string name, Action<T> start, Action<T> perform, Action<T> cancel) where T : struct
        {
            if (inputActions.TryGetValue(name, out BaseInputAction baseInputAction))
            {
                InputAction<T> inputAction = baseInputAction as InputAction<T>;
                inputAction.start -= start;
                inputAction.perform -= perform;
                inputAction.cancel -= cancel;

                if (--baseInputAction.count == 0)
                {
                    inputActions.Remove(name);

                    var action = inputAsset.FindAction(name);

                    action.started -= baseInputAction.Start;
                    action.performed -= baseInputAction.Perform;
                    action.canceled -= baseInputAction.Cancel;
                }
            }
        }
    }
}
