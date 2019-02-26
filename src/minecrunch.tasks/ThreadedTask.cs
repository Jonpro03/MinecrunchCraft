using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace minecrunch.tasks
{
    public class ThreadedTask
    {
        private bool isDone = false;
        private bool isRunning = false;

        private object doneHandle = new object();
        private object runningHandle = new object();

        private Thread thread = null;

        protected virtual void ThreadFunction() { }

        protected virtual void OnFinished() { }

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

        public bool IsRunning
        {
            get
            {
                bool tmp;
                lock (runningHandle)
                {
                    tmp = isRunning;
                }
                return tmp;
            }
            private set
            {
                lock (runningHandle)
                {
                    isRunning = value;
                }
            }
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        public virtual void Start()
        {
            thread = new Thread(Run);
            IsRunning = true;
            thread.Start();
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
                OnFinished();
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
            IsRunning = false;
            IsDone = true;
        }
    }
}
