using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.ServiceModel;

namespace MyPlugins
{
    public class OnCreateUpdateContactCheckDuplicateEmail : IPlugin
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
                     
                    // get contact email
                    string email = contact.Attributes.Contains("emailaddress1") ? contact.Attributes["emailaddress1"].ToString() : "";

                    // check duplicate
                    if (!string.IsNullOrEmpty(email))
                    {
                        // select emailaddress1 from contact where emailaddress1 = 'email'
                        QueryExpression q = new QueryExpression("contact")
                        {
                            ColumnSet = new ColumnSet(new string[] { "emailaddress1" }),
                        };
                        q.Criteria.AddCondition("emailaddress1", ConditionOperator.Equal, email);

                        EntityCollection collection = service.RetrieveMultiple(q);

                        // if duplicate show error message
                        if (collection.Entities.Count > 0)
                        {
                            throw new InvalidPluginExecutionException("Duplicate email detected!");
                        }

                    }

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