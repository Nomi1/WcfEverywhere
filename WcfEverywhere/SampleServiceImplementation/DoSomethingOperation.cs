using System;
using System.Threading;
using ServiceUtilities;
using SevriceContract;
using ServiceUtilities.OperationsManager;

namespace ServiceImplementation
{
    class DoSomethingOperation : OperationBase<SomeResult>
    {
        const int SLEEP_TIME_MILISECONDS = 1000;
        private const int PROCESS_STAGES = 10;

        private readonly SomeParameters _parmaters;

        public DoSomethingOperation(IOperationsManager operationsManager, Guid operationGuid, SomeParameters parmaters)
            : base(operationsManager, operationGuid)
        {
            _parmaters = parmaters;
        }

        protected override SomeResult Run()
        {
            for (int i = 0; i < PROCESS_STAGES; i++)
            {
                Thread.Sleep(SLEEP_TIME_MILISECONDS);
                if (OperationsManager.GetIsOperationFlagedToCancel(OperationGuid))
                {
                    throw new OperationCanceledException();
                }
                OperationsManager.SetOperationProgress(OperationGuid, Convert.ToInt32((((double)i) / ((double)PROCESS_STAGES)) * 100));
            }
            return new SomeResult
                       {
                           Result = string.Format("The number provided was '{0}'.", _parmaters.Parameter)
                       };
        }
    }
}