using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SKWorkflowActivities
{
    /// <summary>
    /// Get a value from a JSON string.
    /// </summary>
    public class GetJSONValueByPath : CodeActivity
    {
        [Input("JSON")]
        [RequiredArgument]
        public InArgument<string> Json { get; set; }


        [Input("Value Path")]
        [RequiredArgument]
        [Default("Manufacturers[0].Products[1].Price")]
        public InArgument<string> ValuePath { get; set; }


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
            HasValue.Set(context, false);
            StringValue.Set(context, null);
            BooleanValue.Set(context, default(bool));
            DateTimeValue.Set(context, default(DateTime));
            DecimalValue.Set(context, default(decimal));
            MoneyValue.Set(context, default(Money));
            DoubleValue.Set(context, default(double));
            IntegerValue.Set(context, default(int));

            var json = Json.Get(context);
            JObject jValue = JObject.Parse("{ }");
            try
            {
                jValue = JObject.Parse(json);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"Failed to parse JSON string. {ex.Message}", ex);
            }
            var valuePath = ValuePath.Get(context);
            JToken pathValue = jValue.SelectToken(valuePath);

            // name of all products enumerated
            //IEnumerable<JToken> jResult = jValue.SelectTokens(valuePath);

            //foreach (JToken item in jResult)
            //{
            //    Console.WriteLine(item);
            //}

            var value = Convert.ToString(pathValue);
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