using Microsoft.Xrm.Sdk;
using System;
using System.ServiceModel;

namespace MyPlugins
{
    public class OnCreateContactCreateTaskExample : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Extract the tracing service for use in debugging sandboxed plug-ins.  
            // If you are not registering the plug-in in the sandbox, then you do  
            // not have to add any tracing service related code.  
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference which you will need for  
            // web service calls.  
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);



            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity contact = (Entity)context.InputParameters["Target"];

                try
                {
                    // Plug-in business logic goes here.  
                    Entity taskRecord = new Entity("task");

                    // string
                    taskRecord.Attributes.Add("subject", "Follow up");
                    taskRecord.Attributes.Add("description", "This contact needs to be followed up and status set");
                    
                    // date
                    taskRecord.Attributes.Add("scheduledend", DateTime.Now.AddDays(7));

                    // parent object or lookup - contact ID is this case
                    taskRecord.Attributes.Add("regardingobjectid", contact.ToEntityReference());
                    
                    // options
                    taskRecord.Attributes.Add("actualdurationminutes", 90);
                    taskRecord.Attributes.Add("prioritycode", new OptionSetValue(1)); // normal

                    Guid taskGuid = service.Create(taskRecord);

                    // testing of shared variables from another plugin
                    string sharedValue = context.SharedVariables["KeyAbc"].ToString();
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("MyPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}