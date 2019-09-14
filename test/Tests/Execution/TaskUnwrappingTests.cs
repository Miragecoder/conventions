﻿using GraphQL;
using GraphQL.Conventions.Relay;
using GraphQL.Conventions.Tests;
using GraphQL.Conventions.Tests.Templates;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests.Execution
{
    public class TaskUnwrappingTests : ConstructionTestBase
    {
        [Test]
        public void Schema_Will_Execute_With_No_Errors_When_A_Type_Is_In_A_Task()
        {
            const string query = @"{
                holders {
                    items {
                        interfaceConnection {
                            test
                        }
                    }
                }
            }";

            var schema = Schema<BugReproSchemaTaskFirst>();
            var result = schema.Execute((e) => e.Query = query);
            AssertNoErrorsInResult(result);

            schema = Schema<BugReproSchemaTaskSecond>();
            result = schema.Execute((e) => e.Query = query);
            AssertNoErrorsInResult(result);
        }

        private void AssertNoErrorsInResult(string result)
        {
            Assert.IsNull(JObject
                .Parse(result)
                .GetValue("errors"));
        }

        private class BugReproSchemaTaskSecond
        {
            public BugReproQueryTaskSecond Query { get; }
        }

        private class BugReproSchemaTaskFirst
        {
            public BugReproQueryTaskFirst Query { get; }
        }

        private class BugReproQueryTaskFirst
        {
            public Task<Broken> AName() => Task.FromResult(new Broken()); // Changes the name of the method to 'Broken' and it will work...

            public Task<Connection<Holder>> Holders() =>
                Task.FromResult(new Connection<Holder>
                {
                    Edges = new List<Edge<Holder>>
                    {
                        new Edge<Holder>
                        {
                            Cursor = Cursor.New<Holder>(0),
                            Node = new Holder
                            {
                            }
                        }
                    },
                    PageInfo = new PageInfo
                    {
                        EndCursor = Cursor.New<Holder>(0),
                        HasNextPage = false,
                        HasPreviousPage = false,
                        StartCursor = Cursor.New<Holder>(0)
                    },
                    TotalCount = 1
                });
        }

        private class BugReproQueryTaskSecond
        {
            public Task<Broken> ZName() => Task.FromResult(new Broken()); // Alpabetic ordering of fields matters for this test!

            public Task<Connection<Holder>> Holders() =>
                Task.FromResult(new Connection<Holder>
                {
                    Edges = new List<Edge<Holder>>
                    {
                        new Edge<Holder>
                        {
                            Cursor = Cursor.New<Holder>(0),
                            Node = new Holder
                            {
                            }
                        }
                    },
                    PageInfo = new PageInfo
                    {
                        EndCursor = Cursor.New<Holder>(0),
                        HasNextPage = false,
                        HasPreviousPage = false,
                        StartCursor = Cursor.New<Holder>(0)
                    },
                    TotalCount = 1
                });
        }

        private class Holder
        {
            public async Task<IEnumerable<ICommonInterface>> InterfaceConnection() => new[] { new Broken() };
        }

        private interface ICommonInterface
        {
            int Test { get; }
        }

        private class Broken : ICommonInterface
        {
            public int Test => 1;
        }
    }
}
