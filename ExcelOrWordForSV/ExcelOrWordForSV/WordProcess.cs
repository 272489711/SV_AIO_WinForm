using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace ExcelOrWordForSV
{
    class WordProcess
    {
        object missing = System.Reflection.Missing.Value;
        object fileName;
        Word.Application wordApp;
        Word.Document wordDoc;

        public WordProcess(object fileName)
        {
            this.fileName = fileName;
            wordApp = new Word.ApplicationClass();
            wordDoc = wordApp.Documents.Add(ref missing, ref missing, ref missing, ref missing);
        }

        public bool CreateWordFile()
        {
            //添加页眉
            wordApp.ActiveWindow.View.Type = Word.WdViewType.wdOutlineView;
            wordApp.ActiveWindow.View.SeekView = Word.WdSeekView.wdSeekPrimaryHeader;
            wordApp.ActiveWindow.ActivePane.Selection.InsertAfter("SV文本数据显示");
            wordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;//右对齐
            wordApp.ActiveWindow.View.SeekView = Word.WdSeekView.wdSeekMainDocument;//跳出页眉设置
            wordApp.Selection.ParagraphFormat.LineSpacing = 15f;//设置文档行间距

            //移动焦点并换行
            object count = 14;
            object WdLine = Word.WdUnits.wdLine;//换行
            wordApp.Selection.MoveDown(ref WdLine, ref count, ref missing);
            wordApp.Selection.TypeParagraph();//插入段落

            //文档中创建表格
            Word.Table table = wordDoc.Tables.Add(wordApp.Selection.Range, 12, 3, ref missing, ref missing);

            //设置表格样式
            table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleDashLargeGap;
            table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            table.Columns[1].Width = 100f;
            table.Columns[2].Width = 220f;
            table.Columns[3].Width = 105f;

            //填充表格内容
            table.Cell(1, 1).Range.Text = "产品详细信息表";
            table.Cell(1, 1).Range.Bold = 2;//设置单元格中的字体为粗体

            //合并单元格
            table.Cell(1, 1).Merge(table.Cell(1, 3));
            wordApp.Selection.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;//垂直居中
            wordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;//水平居中

            //填充表格内容
            table.Cell(2, 1).Range.Text = "产品基本信息";
            table.Cell(2, 1).Range.Font.Color = Word.WdColor.wdColorAqua;//设置单元格字体颜色；
            //合并单元格
            table.Cell(2, 1).Merge(table.Cell(2, 3));
            wordApp.Selection.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;//垂直居中

            //填充表格内容
            table.Cell(3, 1).Range.Text = "品牌名称";
            table.Cell(3, 2).Range.Text = "BrandName";
            //纵向合并单元格
            table.Cell(3, 3).Select();//选中一行
            object moveUnit = Word.WdUnits.wdLine;
            object moveCount = 5;
            object moveExtend = Word.WdMovementType.wdExtend;
            wordApp.Selection.MoveDown(ref moveUnit, ref moveCount, ref moveExtend);
            wordApp.Selection.Cells.Merge();

            //在表格中增加行
            wordDoc.Content.Tables[1].Rows.Add(ref missing);

            wordDoc.Paragraphs.Last.Range.Text = "文档创建时间：" + DateTime.Now.ToString();//落款

            //文件保存
            object pass = 123456;
            object readOnly = true;
            object format = Word.WdSaveFormat.wdFormatDocument;
            try
            {
                wordDoc.SaveAs(ref fileName,ref format, ref missing, ref missing   , ref missing, ref missing,ref readOnly     , ref missing,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,ref missing);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            wordDoc.Close(ref missing, ref missing, ref missing);
            wordApp.Quit(ref missing, ref missing, ref missing);
            Console.WriteLine("创建成功！");
            return true;
        }
    }
}
