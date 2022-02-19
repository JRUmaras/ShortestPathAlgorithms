using System;
using System.Linq;
using System.Threading.Tasks;
using BruteForceSpaService;
using EasyNetQTools;
using EasyNetQTools.NamingConventions.Models;
using Graphs.Factories;
using Graphs.Models;
using ShortestPathAlgorithms.Models;

namespace ClientApp
{
    internal static class Program
    {
        private static async Task Main()
        {
            var customNaming = new CustomNaming
            {
                ExchangeName = "bruteforce.exchange",
                RequestQueueName = "bruteforce.queue",
            };
            
            var client = new Client<Request, Response>(customNaming);

            var request = CreateRequest();

            while (true)
            {
                Console.WriteLine("Calling the service...");
                try
                {
                    var response = await client.MakeRequestAsync(request);
                    Console.WriteLine(response.Path.Distance);
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("The request was cancelled. Maybe a time-out?");
                }

                Console.WriteLine("Press <enter> to continue or <esc> to terminate...");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape) break;
            }
        }

        private static Request CreateRequest()
        {
            var graph = DirectedGraphFactory.CreateBasicGraph();
            var startNode = graph.Nodes.First(n => n.Id == "7");
            var endNode = graph.Nodes.First(n => n.Id == "6");
            
            return new Request
            {
                Graph = graph,
                Start = startNode,
                End = endNode
            };
        }
    }
}