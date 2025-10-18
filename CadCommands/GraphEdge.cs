using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teigha.BoundaryRepresentation;

namespace TestTask.CadCommands
{
    internal class GraphEdge
    {
        public GraphEdge(GraphNode iNode1, GraphNode iNode2)
        {
            node1 = iNode1;
            node2 = iNode2;
        }

        public GraphNode GetNode1() { return node1; }

        public GraphNode GetNode2() { return node2; }

        public double GetLength()
        {
            return 0.0;
        }

        private readonly GraphNode node1;
        private readonly GraphNode node2;
    }
}
