﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceUtilities;
using SevriceContract;
using ServiceUtilities.OperationsManager;

namespace ServiceImplementation
{
    public class DoSomethingService : IDoSomethingService
    {
        private readonly IOperationsManager _operationsManager;

        public DoSomethingService(IOperationsManager operationsManager)
        {
            _operationsManager = operationsManager;
        }

        public OperationResult<SomeResult> DoSomething(SomeParameters parmaters)
        {
            var operationId = _operationsManager.RegistrOperation("Do something");
            var operation = new DoSomethingOperation(_operationsManager, operationId, parmaters);
            var handler = operation.RunAsync();
            handler.WaitOne();
            return DoSomethingGetResult(operationId);
        }

        public OperationStartInformation DoSomethingAsync(SomeParameters parmaters)
        {
            var guid = _operationsManager.RegistrOperation("Do something");
            var operation = new DoSomethingOperation(_operationsManager, guid, parmaters);
            operation.RunAsync();
            return new OperationStartInformation
            {
                OperationGuid = guid,
                IsReportingProgress = false,
                IsSupportingCancel = false,
            };
        }

        public OperationResult<SomeResult> DoSomethingGetResult(Guid operationId)
        {
            var objectResult = _operationsManager.GetOperationResult(operationId);
            return new OperationResult<SomeResult>(
                (SomeResult)objectResult.Result,
                objectResult.Info
                );
        }
    }
}
