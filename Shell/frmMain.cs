using Core.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.FCA;
using DevExpress.Spreadsheet;

namespace Shell
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Worksheet worksheet;
        private Dictionary<int, List<int>> SelectedData;

        public frmMain()
        {
            InitializeComponent();
            worksheet = shcMain.ActiveWorksheet;
            CSVImport csvImport = new CSVImport(@"E:\Магистратура\5\test2.csv", ";");
            worksheet.Load(csvImport);
        }

        private void shcMain_CustomDrawCell(object sender, DevExpress.XtraSpreadsheet.CustomDrawCellEventArgs e)
        {
            if (SelectedData==null)
            {
               return;
            }
            if (e.Cell.Value != null)
            {
                foreach (var data in SelectedData)
                {
                    if (data.Key+1==e.Cell.RowIndex&&data.Value.Contains(e.Cell.ColumnIndex+1))
                    {
                        var shift = 2;
                        var rect = e.FillBounds;
                        rect.X = rect.X + shift;
                        rect.X = rect.X + shift;rect.Height = rect.Height - shift;
                        rect.Width = rect.Width - shift;
                        e.Graphics.FillRectangle(Brushes.MediumBlue, rect);
                    }    
                }
            }
        }

        public void LoadTree(TreeNode<NodeData> tree)
        {
            tlData.BeginUnboundLoad();
            tlData.ClearNodes();
            var test = tree.Children;
            foreach (var child in tree.Children)
            {
                var rootNode = tlData.AppendNode(new object[] {child.Value.Text}, -1);
                rootNode.Tag = child.Value.Data;
                foreach (var childChild in child.Children)
                {
                    var subRoot = tlData.AppendNode(new object[] {childChild.Value.Text}, rootNode);
                    subRoot.Tag = childChild.Value.Data;
                    foreach (var childChildChild in childChild.Children)
                    {
                        var subSubRoot = tlData.AppendNode(new object[] {childChildChild.Value.Text}, subRoot);
                        subSubRoot.Tag = childChild.Value.Data;
                    }
                }
            }
            tlData.ExpandAll();
            tlData.EndUnboundLoad();
        }


        private async void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var test = worksheet.Export();
            FCAAlgoritm algoritm = new FCAAlgoritm(test);
            await algoritm.RunFCAAsync().ContinueWith(task =>
            {
                var tree = task.Result.GetTree();
                tlData.Invoke((Action)(() =>
                {
                    LoadTree(tree);
                }));                
            });
            
        }

        private void tlData_NodeChanged(object sender, DevExpress.XtraTreeList.NodeChangedEventArgs e)
        {
        }

        private void tlData_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node==null)
            {
                return;
            }
            SelectedData = e.Node.Tag as Dictionary<int, List<int>>;
            if (SelectedData != null)
            {
                shcMain.Refresh();
            }          
        }
    }
}