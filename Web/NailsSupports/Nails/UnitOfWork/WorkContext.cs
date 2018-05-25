using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using NailsFramework.IoC;
using NailsFramework.Logging;
using NailsFramework.Persistence;
using NailsFramework.UnitOfWork.Async;

namespace NailsFramework.UnitOfWork
{
    public class WorkContext
    {
        private readonly object executionPermit = new object();
        private readonly IPersistenceContext persistenceContext;

        public WorkContext(IPersistenceContext persistenceContext)
        {
            Items = new Hashtable();
            this.persistenceContext = persistenceContext;
            Begin += HandleBegin;
            End += HandleEnd;
            UnhandledException += HandleUnhandledException;
        }

        /// <summary>
        ///   The current <see cref = "UnitOfWork">Unit of Work</see>
        /// </summary>
        public UnitOfWork CurrentUnitOfWork { get; private set; }

        /// <summary>
        ///   Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public Hashtable Items { get; private set; }


        /// <summary>
        ///   Returns <c>True</c> if the current Unit of Work is running.
        /// </summary>
        /// <value>&lt;c&gt;True&lt;/c&gt; if the current Unit of Work is running.</value>
        public virtual bool IsUnitOfWorkRunning
        {
            get { return CurrentUnitOfWork != null && CurrentUnitOfWork.Status == UnitOfWorkStatus.Running; }
        }

        [Inject]
        public static ILog Log { protected get; set; }

        /// <summary>
        ///   Run a command in a new Unit Of Work.
        /// </summary>
        /// <seealso cref = "IUnitOfWorkCommand" />
        /// <seealso cref = "UnitOfWorkInfo" />
        /// <param name = "command">Command to execute.</param>
        /// <param name = "info">Unit of work information.</param>
        /// <returns>The return value of the Unit of Work.</returns>
        public object RunUnitOfWork(IUnitOfWorkCommand command, UnitOfWorkInfo info)
        {
            return IsUnitOfWorkRunning ? command.Execute() : ExecuteCommand(command, info);
        }

        /// <summary>
        ///   Run a block of code in a new Unit Of Work
        /// </summary>
        /// <param name = "command">delegate to some block of code</param>
        /// <param name = "info">Unit of work information.</param>
        /// <returns>The return value of the Unit of Work.</returns>
        public TReturn RunUnitOfWork<TReturn>(Func<TReturn> command, UnitOfWorkInfo info)
        {
            return (TReturn) RunUnitOfWork(new UnitOfWorkFunctionCommand<TReturn>(command), info);
        }

        /// <summary>
        ///   Run a block of code in a new Unit Of Work
        /// </summary>
        /// <param name = "command">delegate to some block of code</param>
        /// <param name = "info">Unit of work information.</param>
        public void RunUnitOfWork(Action command, UnitOfWorkInfo info)
        {
            RunUnitOfWork(new UnitOfWorkActionCommand(command), info);
        }
        
        private event EventHandler begin;
        private event EventHandler end;
        private event UnhandledExceptionEventHandler unhandledException;

        /// <summary>
        ///   Event raised when an asynchronous operation begins.
        /// </summary>
        public event EventHandler Begin
        {
            add { begin += value; }
            remove { begin -= value; }
        }

        /// <summary>
        ///   Event raised when an asynchronous operation ends.
        /// </summary>
        public event EventHandler End
        {
            add { end += value; }
            remove { end -= value; }
        }

        /// <summary>
        ///   Event raised when an exception is raised during the 
        ///   asynchronous operation.
        /// </summary>
        public event UnhandledExceptionEventHandler UnhandledException
        {
            add { unhandledException += value; }
            remove { unhandledException -= value; }
        }

        private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Debug(String.Format("Unhandled Exception - {0}", e.ExceptionObject));
        }

        private static void HandleEnd(object sender, EventArgs e)
        {
            Log.Debug("WorkContext - End");
        }

        private static void HandleBegin(object sender, EventArgs e)
        {
            Log.Debug("WorkContext - Begin");
        }

        /// <summary>
        ///   Run a command in a new Unit Of Work.
        /// </summary>
        /// <seealso cref = "IUnitOfWorkCommand" />
        /// <seealso cref = "UnitOfWorkInfo" />
        /// <param name = "command">Command to execute.</param>
        /// <param name = "info">Unit of work information.</param>
        /// <returns>The return value of the Unit of Work.</returns>
        public void RunUnitOfWorkAsync(IUnitOfWorkCommand command, UnitOfWorkInfo info)
        {
            var worker = new BackgroundWorker();

            if (IsUnitOfWorkRunning)
            {
                worker.DoWork += delegate { command.Execute(); };
            }

            CallBegin();
            worker.DoWork += delegate
                                 {
                                     try
                                     {
                                         lock (executionPermit)
                                         {
                                             ExecuteCommand(command, info);
                                         }
                                     }
                                     catch (Exception e)
                                     {
                                         CallUnhandledException(e);
                                     }
                                     finally
                                     {
                                         CallEnd();
                                     }
                                 };
            worker.RunWorkerAsync();
        }

        private void CallUnhandledException(Exception exception)
        {
            var args = new UnhandledExceptionEventArgs(exception, false);

            AsyncExecutor.FireAndForget(unhandledException, this, args);
        }

        /// <summary>
        ///   Run a block of code in a new Unit Of Work
        /// </summary>
        /// <param name = "command">delegate to some block of code</param>
        /// <param name = "info">Unit of work information.</param>
        public void RunUnitOfWorkAsync(Action command, UnitOfWorkInfo info)
        {
            RunUnitOfWorkAsync(new UnitOfWorkActionCommand(command), info);
        }

        private void CallEnd()
        {
            Call(end);
        }

        private void Call(EventHandler eventHandler)
        {
            AsyncExecutor.FireAndForget(eventHandler, this, EventArgs.Empty);
        }

        private void CallBegin()
        {
            Call(begin);
        }

        protected object ExecuteCommand(IUnitOfWorkCommand command, UnitOfWorkInfo info)
        {
            var execution = BeginUnitOfWork(info);

            try
            {
                var result = command.Execute();
                return result;
            }
            catch (Exception e)
            {
                execution.HandleException(e);
                
                if (e is TargetInvocationException)
                    throw e.GetBaseException();
                
                throw;
            }
            finally
            {
                execution.End();
            }
        }

        public UnitOfWorkExecution BeginUnitOfWork(UnitOfWorkInfo unitOfWorkInfo)
        {
            var execution = new UnitOfWorkExecution(persistenceContext, unitOfWorkInfo);
            execution.OnEnd(() => CurrentUnitOfWork = null);
            CurrentUnitOfWork = execution.Begin();
            return execution;
        }
    }
}