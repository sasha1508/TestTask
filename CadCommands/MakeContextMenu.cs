using HostMgd.EditorInput;
using HostMgd.Windows;
using Multicad.DatabaseServices;
using Multicad.DatabaseServices.StandardObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Teigha.DatabaseServices;
using static System.Net.Mime.MediaTypeNames;

namespace TestTask.CadCommands
{
    internal class MakeContextMenu
    {
        /// <summary>
        /// Добавляет строку в контекстное меню
        /// </summary>
        [Teigha.Runtime.CommandMethod("pdCreateMenu")]
        public void PdCreateMenu()
        {
            ContextMenuExtension menu = new()  { Title = "Создание графа" };
            HostMgd.ApplicationServices.Application.AddDefaultContextMenuExtension(menu);

            HostMgd.Windows.MenuItem menuCreateNode = new("Создать узел");
            menuCreateNode.Click += new EventHandler(MekeNodeMenuClick);
            menu.MenuItems.Add(menuCreateNode);

            HostMgd.Windows.MenuItem menuChangeNodeView = new("Изменить внешний вид узла");
            menuChangeNodeView.Click += new EventHandler(ChangeNodeView);
            menu.MenuItems.Add(menuChangeNodeView);
        }

        private void MekeNodeMenuClick(object? sender, EventArgs e)
        {
            if (graph == null)  graph = new();
            else  graph.CreateNode(1);
        }

        private void ChangeNodeView(object? sender, EventArgs e)
        {
            Multicad.McObjectId objId = Multicad.DatabaseServices.McObjectManager.SelectObject("Выбери узел графа: ");
            if (objId.GetObject().IsKindOf(McBlockRef.TypeID))
            {
                McBlockRef ent = (McBlockRef)objId.GetObject();
                string blockName = ent.BlockName;
                Multicad.Geometry.Point3d insertPoint = ent.InsertPoint;
                Teigha.Geometry.Point3d blockInsertPoint = new(insertPoint.X, insertPoint.Y, insertPoint.Z);
                
                if (blockName.Contains("CircleNode_"))
                {
                    string idString = blockName.Substring(11);
                    int id = int.Parse(idString);

                    if (graph != null)
                    {
                        GraphNode? node = graph.FindNode(id);
                        node?.ChengeViewNode(blockInsertPoint, GraphNode.NodeType.Circle);
                    }

                    return;
                }
                else if (blockName.Contains("TriangleNode_"))
                {
                    string idString = blockName.Substring(13);
                    int id = int.Parse(idString);

                    if (graph != null)
                    {
                        GraphNode? node = graph.FindNode(id);
                        node?.ChengeViewNode(blockInsertPoint, GraphNode.NodeType.Triangle);
                    }

                    return;
                }
            }
            
            MessageBox.Show("Выбранный объект не является узлом графа");
        }

        private Graph? graph;
    }
}
