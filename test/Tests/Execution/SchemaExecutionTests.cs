﻿using GraphQL;
using GraphQL.Conventions.Tests;
using GraphQL.Conventions.Tests.Templates;
using GraphQL.Conventions.Tests.Templates.Extensions;
using Newtonsoft.Json.Linq;

namespace Tests.Execution
{
    public class SchemaExecutionTests : ConstructionTestBase
    {
        [Test]
        public void Can_Have_Decimals_In_Schema()
        {
            var schema = Schema<SchemaTypeWithDecimal>();
            schema.ShouldHaveQueries(1);
            schema.ShouldHaveMutations(0);
            schema.Query.ShouldHaveFieldWithName("test");
            var result = schema.Execute((e) => e.Query = "query { test }");
            Assert.IsNull(JObject
                .Parse(result)
                .GetValue("errors"));
        }

        class SchemaTypeWithDecimal
        {
            public QueryTypeWithDecimal Query { get; }
        }

        class QueryTypeWithDecimal
        {
            public decimal Test => 10;
        }
    }
}
