using HostMgd.EditorInput;
using HostMgd.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Teigha.DatabaseServices;

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
            // Получение ссылки на активный документ
            HostMgd.ApplicationServices.Document doc = HostMgd.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            // Получение ссылки на редактор докумена
            HostMgd.EditorInput.Editor ed = doc.Editor;

            HostMgd.EditorInput.PromptSelectionResult res = ed.GetSelection();


            if (res.Status == PromptStatus.OK)
            {
                foreach (SelectedObject selObj in res.Value)
                {
                    
                    //selObj.GetType();
                    ed.WriteMessage("Выбран объект: " );

                   //doc.Database.obj
                    //Entity ent = selObj.Object as Entity;
                    //if (ent != null)
                    //    selectedEntities.Add(ent);
                }
            }



        }

        private Graph? graph;
    }
}
