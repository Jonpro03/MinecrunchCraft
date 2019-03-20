using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace minecrunch.tasks
{
    public class ThreadedTask
    {
        private bool isDone = false;

        private object doneHandle = new object();
        private object runningHandle = new object();

        private Thread thread = null;

        protected virtual void ThreadFunction() { }

        public delegate void ThreadCompleteEventHandler(object thing);

        public virtual event ThreadCompleteEventHandler ThreadComplete;

        public Exception e = null;

        /// <summary>
        /// Whether this <see cref="T:minecrunch.tasks.ThreadedTask"/> is done.
        /// </summary>
        /// <value><c>true</c> if is done; otherwise, <c>false</c>.</value>
        public bool IsDone
        {
            get
            {
                bool tmp;
                lock (doneHandle)
                {
                    tmp = isDone;
                }
                return tmp;
            }
            private set
            {
                lock (doneHandle)
                {
                    isDone = value;
                }
            }
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        public virtual void Start()
        {
            if (thread is null)
            {
                thread = new Thread(Run);
                thread.Start();
            }
        }

        public virtual bool IsStarted()
        {
            return thread != null;
        }

        /// <summary>
        /// Abort this instance.
        /// </summary>
        public virtual void Abort()
        {
            thread.Abort();
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        /// <returns>The update.</returns>
        public virtual bool Update()
        {
            if (IsDone)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Wait for task to complete
        /// </summary>
        public IEnumerator WaitFor()
        {
            while (!Update())
            {
                yield return null;
            }
        }

        /// <summary>
        /// Run this instance.
        /// </summary>
        private void Run()
        {
            try
            {
                ThreadFunction();
            }
            catch (Exception e)
            {
                this.e = e;
            }

            IsDone = true;
        }
    }
}
