using System;
using System.Collections;

namespace EasyGamePlay
{
    enum CoroutineState
    {
        runing,
        pausing,
        finsih
    }

    public class CoroutineTask
    {
        private IEnumerator enumerator;
        private CoroutineState coroutineState;
        internal bool isStoped;

        private static Action<CoroutineTask> remove;

        public CoroutineTask(IEnumerator enumerator)
        {
            this.enumerator = enumerator;
            coroutineState = CoroutineState.runing;
            isStoped = false;
        }

        internal IEnumerator CallWarpper()
        {
            yield return null;
            while (!isStoped)
            {
                switch (coroutineState)
                {
                    case CoroutineState.runing:
                        if (enumerator != null && enumerator.MoveNext())
                        {
                            yield return enumerator.Current;
                        }  
                        else
                            coroutineState = CoroutineState.finsih;
                        break;
                    case CoroutineState.pausing:
                        yield return null;
                        break;
                    case CoroutineState.finsih:
                        isStoped=true;
                        break;
                }
            }
            remove(this);
        }

        public void Pause()
        {
            coroutineState = CoroutineState.pausing;
        }

        public void Resume()
        {
            coroutineState = CoroutineState.runing;
        }

        public static void SetAction(Action<CoroutineTask> remove)
        {
            CoroutineTask.remove = remove;
        }
    }
}
