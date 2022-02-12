using System;
using System.Linq;
using System.Threading.Tasks;
using BruteForceSpaService;
using EasyNetQTools;
using EasyNetQTools.NamingConventions.Models;
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
            var graph = CreateBasicGraph();
            var startNode = graph.Nodes.First(n => n.Id == "7");
            var endNode = graph.Nodes.First(n => n.Id == "6");
            
            return new Request
            {
                Graph = graph,
                Start = startNode,
                End = endNode
            };
        }

        private static GraphUndirected CreateBasicGraph()
        {
            var graph = new GraphUndirected
            {
                Nodes = new[]
                {
                    new Node { Id = "0" },
                    new Node { Id = "1" },
                    new Node { Id = "2" },
                    new Node { Id = "3" },
                    new Node { Id = "4" },
                    new Node { Id = "5" },
                    new Node { Id = "6" },
                    new Node { Id = "7" },
                }
            };

            graph.Edges = new[]
            {
                new Edge
                {
                    Node1 = graph.Nodes[0],
                    Node2 = graph.Nodes[1],
                    Weight = 1
                },
                new Edge
                {
                    Node1 = graph.Nodes[0],
                    Node2 = graph.Nodes[2],
                    Weight = 1
                },
                new Edge
                {
                    Node1 = graph.Nodes[1],
                    Node2 = graph.Nodes[4],
                    Weight = 1
                },
                new Edge
                {
                    Node1 = graph.Nodes[1],
                    Node2 = graph.Nodes[5],
                    Weight = 1
                },
                new Edge
                {
                    Node1 = graph.Nodes[2],
                    Node2 = graph.Nodes[3],
                    Weight = 1
                },
                new Edge
                {
                    Node1 = graph.Nodes[2],
                    Node2 = graph.Nodes[4],
                    Weight = 1
                },
                new Edge
                {
                    Node1 = graph.Nodes[3],
                    Node2 = graph.Nodes[7],
                    Weight = 1
                },
                new Edge
                {
                    Node1 = graph.Nodes[4],
                    Node2 = graph.Nodes[5],
                    Weight = 1
                },
                new Edge
                {
                    Node1 = graph.Nodes[4],
                    Node2 = graph.Nodes[6],
                    Weight = 1
                }
            };

            return graph;
        }
    }
}