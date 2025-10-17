using HostMgd.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ContextMenuExtension menu = new()
            {
                Title = "Создание графа"
            };
            HostMgd.ApplicationServices.Application.AddDefaultContextMenuExtension(menu);

            MenuItem menuCreateWallGrid = new("Создать узел");
            menuCreateWallGrid.Click += new EventHandler(MekeNodeMenuClick);

            menu.MenuItems.Add(menuCreateWallGrid);
        }

        void MekeNodeMenuClick(object? sender, EventArgs e)
        {
            Graph graph = new();
            Node node = graph.CreateNode();
        }
    }
}
