using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace MyCustomWorkFlows
{
    internal class GetTaxWorkflow : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);


            // Todo: Complete at a later date

        }
    }
}
