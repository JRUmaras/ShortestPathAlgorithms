﻿using ShortestPathAlgorithms.Models;

namespace BruteForceSpaService
{
    public class Request
    {
        public GraphUndirected Graph { get; set; }
        
        public Node Start { get; set; }
        
        public Node End { get; set; }
    }
}