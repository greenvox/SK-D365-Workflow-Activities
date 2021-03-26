using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using System;

namespace SKWorkflowActivities
{
    public class GetSingleValueByFetch : CodeActivity
    {
        [RequiredArgument]
        [Input("Search For Value")]
        public InArgument<string> FieldValue { get; set; }

        [RequiredArgument]
        [Input("Search In Field")]
        public InArgument<string> FieldName { get; set; }

        [RequiredArgument]
        [Input("Search In Entity")]
        public InArgument<string> FieldEntity { get; set; }

        [RequiredArgument]
        [Input("Result Field")]
        public InArgument<string> ResultField { get; set; }

        [RequiredArgument]
        [Input("Filter Condition")]
        [Default("and")]
        [ArgumentDescription(@"and OR or")]
        public InArgument<string> Filter { get; set; }

        [Input("Additional Fetch Conditions")]
        [ArgumentDescription(@"<condition attribute='createdon' operator='last-x-hours' value='1' />")]
        public InArgument<string> FetchFilters { get; set; }

        [Input("Order By")]
        [Default("createdon")]
        public InArgument<string> OrderBy { get; set; }

        [Input("Sort Order Descending")]
        public InArgument<bool> DescendingOrder { get; set; }

        [Output("Has Value")]
        public OutArgument<bool> HasValue { get; set; }

        /// <summary>
        /// Variable value as string.
        /// </summary>
        [Output("String Value")]
        public OutArgument<string> StringValue { get; set; }


        /// <summary>
        /// The value of the variable as a boolean value.
        /// </summary>
        [Output("Boolean Value")]
        public OutArgument<bool> BooleanValue { get; set; }


        [Output("Date/Time Value")]
        public OutArgument<DateTime> DateTimeValue { get; set; }


        [Output("Decimal Value")]
        public OutArgument<decimal> DecimalValue { get; set; }


        [Output("Double Value")]
        public OutArgument<double> DoubleValue { get; set; }


        [Output("Integer Value")]
        public OutArgument<int> IntegerValue { get; set; }


        [Output("Money Value")]
        public OutArgument<Money> MoneyValue { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var executionContext = context.GetExtension<IWorkflowContext>();
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(executionContext.UserId);
            //var tracingService = executionContext.GetExtension<ITracingService>();

            var fieldValue = FieldValue.Get(context);
            var fieldName = FieldName.Get(context);
            var entityName = FieldEntity.Get(context);
            var resultField = ResultField.Get(context);
            var fetchFilters = FetchFilters.Get(context);
            var orderBy = OrderBy.Get(context);
            bool descendingOrder = DescendingOrder.Get(context);

            var order = string.Empty;
            if (orderBy != null)
            {
                order = $@"<order attribute='{orderBy}' descending='{descendingOrder}' />";
            }

            var filter = Filter.Get(context);

            var value = CrmUtility.GetStringByValueUsingFetch(service, entityName, resultField, filter, fieldName, fieldValue, fetchFilters, orderBy);

            if (value == null)
                return;

            HasValue.Set(context, true);

            var stringValue = value.ToString().Trim();
            StringValue.Set(context, stringValue);

            var booleanValue = !(stringValue.Equals("false", StringComparison.OrdinalIgnoreCase) || stringValue.Equals("0") || stringValue.Equals("+0") || stringValue.Equals("-0"));
            BooleanValue.Set(context, booleanValue);

            DateTime dateTimeValue;
            if (DateTime.TryParse(stringValue, out dateTimeValue))
                DateTimeValue.Set(context, dateTimeValue);

            decimal decimalValue;
            if (decimal.TryParse(stringValue, out decimalValue))
            {
                DecimalValue.Set(context, decimalValue);
                MoneyValue.Set(context, new Money(decimalValue));
            }

            double doubleValue;
            if (double.TryParse(stringValue, out doubleValue))
                DoubleValue.Set(context, doubleValue);

            int integerValue;
            if (int.TryParse(stringValue, out integerValue))
                IntegerValue.Set(context, integerValue);

        }
    }
}
