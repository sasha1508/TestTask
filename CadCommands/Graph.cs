using HostMgd.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Teigha.DatabaseServices;
using Teigha.DatabaseServices.Filters;

namespace TestTask.CadCommands
{
    public class Graph
    {
       

        public Graph()
        {
            nodeSet = new HashSet<GraphNode>();
            edgeSet = new HashSet<GraphEdge>();

            nodeSet.Add(CreateNode(0));
        }

        public GraphNode CreateNode(int id)
        {
            int newID = id;
            bool existsID = true;
            while (existsID)
            {
                existsID = false;
                //Проверяем существует ли узел с таким ID:
                foreach (GraphNode node in nodeSet)
                {
                    int nodeID = node.GetID();
                    if (nodeID == newID)
                    {
                        existsID = true;
                        newID++;
                        break;
                    }
                }
            }

            //Узла с таки ID нет, создаём его:
            GraphNode newNode = new(newID);
            nodeSet.Add(newNode);

            return newNode;
        }
        
        private HashSet<GraphNode> nodeSet;
        private HashSet<GraphEdge> edgeSet;
    }
}
