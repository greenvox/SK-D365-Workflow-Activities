using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;

namespace SKWorkflowActivities
{
    public class ChangeProcessStage : CodeActivity
    {
        [Input("Record Id")]
        public InArgument<string> RecordId { get; set; }

        [Input("Entity Name")]
        public InArgument<string> EntityName { get; set; }

        [Input("Stage Name")]
        [Default("Approve")]
        public InArgument<string> StageName { get; set; }

        [Input("Business Process Flow")]
        [ReferenceTarget("workflow")]
        public InArgument<EntityReference> BPF { get; set; }

        [Input("Going Backwards?")]
        public InArgument<bool> Backwards { get; set; }
        
        [Output("Status")]
        public OutArgument<string> Status { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            //tracingService = executionContext.GetExtension<ITracingService>();

            var recId = RecordId.Get(executionContext);
            var recEntity = EntityName.Get(executionContext);
            var stagename = StageName.Get(executionContext);
            var bpf = BPF.Get(executionContext);
            var backwards = Backwards.Get(executionContext);
            var recEntityIdName = recEntity + "id";
            var outputStatus = "Success";
            //Opportunity
            try
            {
                var qeOpp = new QueryExpression(recEntity);
                qeOpp.ColumnSet.AddColumns(recEntityIdName);
                qeOpp.Criteria.AddCondition(recEntityIdName, ConditionOperator.Equal, recId);
                var record = service.RetrieveMultiple(qeOpp).Entities.FirstOrDefault();


                //Workflow
                var workflow = bpf;
                var bpfName = workflow.Name;
                
                // -- If Workflow were a string field -- //
                //var qeWorkflow = new QueryExpression("workflow");
                //qeWorkflow.ColumnSet.AddColumns("uniquename", "category", "type", "businessprocesstype");
                //qeWorkflow.Criteria.AddCondition("name", ConditionOperator.Equal, bpf);
                //qeWorkflow.Criteria.AddCondition("category", ConditionOperator.Equal, 4);
                //var workflow = service.RetrieveMultiple(qeWorkflow).Entities.FirstOrDefault();
                //var bpfName = workflow["uniquename"].ToString();

                //Stage
                var qeStage = new QueryExpression("processstage");
                qeStage.ColumnSet.AddColumns("processstageid", "processid", "stagename", "primaryentitytypecode");
                qeStage.Criteria.AddCondition("processid", ConditionOperator.Equal, workflow.Id);
                qeStage.Criteria.AddCondition("stagename", ConditionOperator.Equal, stagename);
                var stage = service.RetrieveMultiple(qeStage).Entities.FirstOrDefault();

                var bpfRelatedLookupName = recEntityIdName;
                if (bpfName.StartsWith("bpf_") || bpfName.Contains("bpf_")) bpfRelatedLookupName = "bpf_" + recEntityIdName;

                //BPF
                var qeBPF = new QueryExpression(bpfName);
                qeBPF.ColumnSet.AddColumns("activestageid", "activestagestartedon", "traversedpath", "statecode", "statuscode", "businessprocessflowinstanceid");
                qeBPF.Criteria.AddCondition(bpfRelatedLookupName, ConditionOperator.Equal, record.Id);
                qeBPF.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
                var process = service.RetrieveMultiple(qeBPF).Entities.FirstOrDefault();

                var traversedPath = Convert.ToString(process["traversedpath"]);

                if (backwards)
                {
                    string data = traversedPath;
                    var dataArray = data.Split(',');
                    Console.WriteLine(dataArray.Length);
                    var newPath = string.Empty;
                    if (dataArray.Length < 2)
                    {
                        traversedPath = data;
                    }
                    else
                    {
                        for (int i = 0; i < dataArray.Length - 1; i++)
                        {
                            if (string.IsNullOrEmpty(newPath)) { newPath = dataArray[i]; }
                            else { newPath = newPath + "," + dataArray[i]; }
                        }
                        traversedPath = newPath;
                    }
                }
                else
                {
                    traversedPath = Convert.ToString(process["traversedpath"]) + "," + Convert.ToString(stage.Id);
                }

                var recUpdate = new Entity(process.LogicalName, process.Id);
                recUpdate["activestageid"] = stage.ToEntityReference();
                recUpdate["traversedpath"] = traversedPath;
                recUpdate["activestagestartedon"] = DateTime.UtcNow;
                service.Update(recUpdate);
            }
            catch (InvalidWorkflowException ex)
            {
                outputStatus = ex.Message + ex.ToString();
            }
            Status.Set(executionContext, outputStatus);
        }
    }
}