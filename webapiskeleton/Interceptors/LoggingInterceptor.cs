using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace webapiskeleton.Interceptors
{
    public class LoggingInterceptor : IInterceptionBehavior
    {
        /// <summary>
        /// Implement this method to execute your behavior processing.
        /// </summary>
        /// <param name="input">Inputs to the current call to the target.</param>
        /// <param name="getNext">Delegate to execute to get the next delegate in the behavior chain.</param>
        /// <returns>
        /// Return value from the target.
        /// </returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var methodInfo = string.Format("{0}.{1}",
                input.MethodBase.DeclaringType != null ? input.MethodBase.DeclaringType.FullName : string.Empty,
                input.MethodBase.Name);

            LogManager.GetLogger("webapiskeleton.logger").Info(string.Format("Entered {0}", methodInfo));
            var ret = getNext().Invoke(input, getNext);

            //Need to do this or else it loses the exception
            if (ret.Exception != null)
                ret.Exception = new Exception(string.Format("Exception in {0}", methodInfo), ret.Exception);

            LogManager.GetLogger("webapiskeleton.logger").Info(string.Format("Exited {0}", methodInfo));
            return ret;
        }

        /// <summary>
        /// Returns the interfaces required by the behavior for the objects it intercepts.
        /// </summary>
        /// <returns>
        /// The required interfaces.
        /// </returns>
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        /// <summary>
        /// Returns a flag indicating if this behavior will actually do anything when invoked.
        /// </summary>
        /// <remarks>
        /// This is used to optimize interception. If the behaviors won't actually
        /// do anything (for example, PIAB where no policies match) then the interception
        /// mechanism can be skipped completely.
        /// </remarks>
        public bool WillExecute
        {
            get { return true; }
        }
    }
}