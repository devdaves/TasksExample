using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Castle.Core.Internal;
using MediatR;
using TasksExample.Api.Models;

namespace TasksExample.Api.Features.Health
{
    public class HealthItemsList
    {
        public class QueryAsync : IRequest<List<HealthItem>>
        {
            public string Key { get; private set; }

            public QueryAsync(string key = "")
            {
                this.Key = key;
            }
        }

        public class HandlerAsync : IAsyncRequestHandler<QueryAsync, List<HealthItem>>
        {
            public async Task<List<HealthItem>> Handle(QueryAsync message)
            {
                var items = new Dictionary<string, Func<Task<string>>>()
                {
                    {"test1", this.Test1},
                    {"test2", this.Test2},
                    {"test3", this.Test3},
                };

                var results = new List<HealthItem>();

                if (string.IsNullOrEmpty(message.Key))
                {
                    // execute all health item functions
                    items.Keys.ForEach(async k => 
                        results.Add(new HealthItem() { Key = k, Value = await items[k].Invoke() }));
                }
                else if(items.ContainsKey(message.Key))
                {
                    // execute the single health item function
                    results.Add(new HealthItem() {Key = message.Key, Value = await items[message.Key].Invoke()});
                }
                
                return results;
            }

            private async Task<string> Test1()
            {
                return await Task.FromResult("test1");
            }

            private async Task<string> Test2()
            {
                return await Task.FromResult("test2");
            }

            private async Task<string> Test3()
            {
                return await Task.FromResult("test3");
            }
        }       
    }
}