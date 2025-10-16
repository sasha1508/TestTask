using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.CadCommands
{
    public partial class Node
    {

        [Teigha.Runtime.CommandMethod("CreateBlockTableRecord")]
        public void CreateBlockTableRecord()
        {
            // Получение ссылки на активный документ
            HostMgd.ApplicationServices.Document doc =
                     HostMgd.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            // Получение ссылки на редактор докумена
            HostMgd.EditorInput.Editor ed = doc.Editor;

            // Получение ссылки на базу данных документа
            Teigha.DatabaseServices.Database db = doc.Database;

            // Создаем примитивы для нового блока
            Teigha.DatabaseServices.Circle crcl = new Teigha.DatabaseServices.Circle();
            crcl.Center = new Teigha.Geometry.Point3d(100, 100, 100);
            crcl.Radius = 100;
            Teigha.DatabaseServices.Polyline pln = new Teigha.DatabaseServices.Polyline();
            pln.AddVertexAt(0, new Teigha.Geometry.Point2d(-10, -10), 0, 0, 0);
            pln.AddVertexAt(1, new Teigha.Geometry.Point2d(210, 210), 0, 0, 0);
            pln.AddVertexAt(2, new Teigha.Geometry.Point2d(210, -10), 0, 0, 0);
            pln.AddVertexAt(3, new Teigha.Geometry.Point2d(-10, 210), 0, 0, 0);
            pln.Closed = true;

            // Создаем новое определение блока
            Teigha.DatabaseServices.BlockTableRecord newBlock = new Teigha.DatabaseServices.BlockTableRecord();
            newBlock.Name = "NewBlock";

            // Добавляем примитивы в блок
            newBlock.AppendEntity(crcl);
            newBlock.AppendEntity(pln);

            // Мы создали определение нового блока. Теперь это определение
            // нужно добавить в базу данных чертежа, чтобы можно было
            // добавлять на листы чертежа вставки нового блока

            // Начало транзакции с базой данных документа
            using (Teigha.DatabaseServices.Transaction trans =
                     db.TransactionManager.StartTransaction())
            {
                // Открываем таблицу блоков для изменения
                Teigha.DatabaseServices.BlockTable blockTable =
                            trans.GetObject(db.BlockTableId, Teigha.DatabaseServices.OpenMode.ForWrite)
                            as Teigha.DatabaseServices.BlockTable;

                // Добавляем новое определение блока в таблицу блоков
                Teigha.DatabaseServices.ObjectId newBlockId = blockTable.Add(newBlock);

                // Открываем пространство модели для записи, чтобы добавить в него вставку блока
                Teigha.DatabaseServices.BlockTableRecord modelSpace =
                                   trans.GetObject(blockTable[Teigha.DatabaseServices.BlockTableRecord.ModelSpace], Teigha.DatabaseServices.OpenMode.ForWrite)
                                   as Teigha.DatabaseServices.BlockTableRecord;

                // Создаем вставку нового определения блока
                Teigha.DatabaseServices.BlockReference newBR =
                   new Teigha.DatabaseServices.BlockReference(new Teigha.Geometry.Point3d(100, 100, 0), newBlockId);

                // Добавляем эту вставку в пространство модели и базу данных чертежа
                modelSpace.AppendEntity(newBR);
                trans.AddNewlyCreatedDBObject(newBR, true);

                // Завершаем транзакцию
                trans.Commit();
            }

            // После того, как мы добавили новое определение блока в базу данных чертежа
            // и вставку этого блока в пространство модели, мы можем проверить, появились ли они там

            // Открываем транзакцию заново, чтобы проверить, как изменилось содержимое пространства модели и базы данных
            using (Teigha.DatabaseServices.Transaction trans =
                     db.TransactionManager.StartTransaction())
            {
                // Открываем таблицу блоков для чтения
                Teigha.DatabaseServices.BlockTable blockTable =
                            trans.GetObject(db.BlockTableId, Teigha.DatabaseServices.OpenMode.ForRead)
                            as Teigha.DatabaseServices.BlockTable;

                // Открываем пространство модели для чтения из таблицы блоков (т.к. пространство модели - тоже блок)
                Teigha.DatabaseServices.BlockTableRecord modelSpace =
                                   trans.GetObject(blockTable[Teigha.DatabaseServices.BlockTableRecord.ModelSpace], Teigha.DatabaseServices.OpenMode.ForRead)
                                   as Teigha.DatabaseServices.BlockTableRecord;

                // Перебираем содержимое пространства модели
                int i = 0;
                foreach (var b in modelSpace) i++;
                ed.WriteMessage($"В пространстве модели сейчас {i} объектов");
                foreach (Teigha.DatabaseServices.ObjectId btrId in modelSpace)
                {
                    // Открываем для чтения каждый объект, входящий в пространство модели
                    Teigha.DatabaseServices.DBObject obj = trans.GetObject(btrId, Teigha.DatabaseServices.OpenMode.ForRead);

                    // Выводим Id и тип каждого объекта в командную строку
                    if (obj != null) ed.WriteMessage($"{btrId} {obj.GetType()}");
                }

                // Открываем для чтения созданный нами блок
                Teigha.DatabaseServices.BlockTableRecord btr = trans.GetObject(blockTable[newBlock.Name], Teigha.DatabaseServices.OpenMode.ForRead) as Teigha.DatabaseServices.BlockTableRecord;

                // Перебираем все его вставки и выводим в командную строку служебный блок, в котором находится вставка, имя и положение каждой вставки
                ed.WriteMessage($"У блока {newBlock.Name} {btr.GetBlockReferenceIds(true, false).Count} вставок");
                foreach (Teigha.DatabaseServices.ObjectId brId in btr.GetBlockReferenceIds(true, false))
                {
                    Teigha.DatabaseServices.BlockReference br = trans.GetObject(brId, Teigha.DatabaseServices.OpenMode.ForRead) as Teigha.DatabaseServices.BlockReference;
                    ed.WriteMessage($"{br.BlockName} {br.Name} X:{br.Position.X} Y:{br.Position.Y} Z:{br.Position.Z}");
                }

                // Завершаем транзакцию
                trans.Abort();
            }
        }
    }
}
