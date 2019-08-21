﻿using GraphQL;
using GraphQL.Conventions.Tests;
using GraphQL.Conventions.Tests.Templates.Extensions;

namespace Tests.Execution
{
    public class SchemaExecutionTests
    {
        [Test]
        public void Can_Have_Decimals_In_Schema()
        {
            var schema = SchemaBuilderHelpers.Schema<SchemaTypeWithDecimal>();
            schema.ShouldHaveQueries(1);
            schema.ShouldHaveMutations(0);
            schema.Query.ShouldHaveFieldWithName("test");
            var res = schema.Execute((e) => e.Query = "query { test }");
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