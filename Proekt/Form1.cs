using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;


namespace Proekt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //create root
            var root = new TreeNode() { Text = "C", Tag = "c:\\" };
            treeView1.Nodes.Add(root);
            Build(root);
            root.Expand();
        }
        private void Build(TreeNode parent)
        {
            var path = parent.Tag as string;
            parent.Nodes.Clear();

            try
            {
                //create dirs
                foreach (var dir in Directory.GetDirectories(path))
                {
                    var node = new TreeNode(Path.GetFileName(dir), new[] { new TreeNode("...") }) { Tag = dir };
                    parent.Nodes.Add(node);
                }

                //create files
                foreach (var file in Directory.GetFiles(path))
                {
                    var node = new TreeNode(Path.GetFileName(file), 1, 1) { Tag = file };
                    parent.Nodes.Add(node);
                }
            }
            catch
            {
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //Проверка, что поле поиска не пустое.
            if (textBox1.Text.Length > 0)
            {
                //Проверка, что поиск не тот же.
                if (LastSearchText != textBox1.Text)
                {
                    //Очищаем переменную,
                    //хранящую найденные элементы.
                    CurrentNodeMatches.Clear();
                    //Сохраняем текущий поиск.
                    LastSearchText = textBox1.Text;
                    //Запускаем метод поиска, с передачей 
                    //в качестве параметров, текст для поиска и 
                    //с какого элемента начинать поиск.
                    SearchNodes(textBox1.Text, treeView1.Nodes[0]);
                }

                //Проверка количества найденных элементов.
                if (CurrentNodeMatches.Count > 0)
                {
                    //Проходимся по найденным элементам.
                    for (int i = 0; i < CurrentNodeMatches.Count; i++)
                    {
                        //Берем текущий элемент
                        TreeNode selectedNode = CurrentNodeMatches[i];
                        //Задаем узел дерева, который в текущий момент будет выбран в элементе
                        //управления иерархического представления.
                        this.treeView1.SelectedNode = selectedNode;
                        //Разворачиваем выбранный узел.
                        this.treeView1.SelectedNode.Expand();
                        //Задаем цвет фона для выбранного узла.
                        this.treeView1.SelectedNode.BackColor = Color.FromArgb(0xCC, 0xFF, 0x00);
                        //Активируем элемент управления.
                        this.treeView1.Select();
                    }
                }
            }
        }

        //Переменная для хранения найденных элементов.
        private List<TreeNode> CurrentNodeMatches = new List<TreeNode>();
        //Переменная для хранения данных последнего поиска.
        private string LastSearchText;

        //Метод поиска, принимающий в качестве входных параметров,
        //текст для поиска и узел с которого надо начинать поиск.
        private void SearchNodes(string SearchText, TreeNode StartNode)
        {
            while (StartNode != null)
            {
                //Проверяем значение поиска, равно ли оно данному экземпляру.
                //Если получено значение true, то параметр value встречается
                //в строке, иначе — false.
                if (StartNode.Text.ToLower().Contains(SearchText.ToLower()))
                {
                    //Добавляем элемент в список найденных.
                    CurrentNodeMatches.Add(StartNode);
                };

                //Если в текущем узле элементы не закончились,
                //то запускам метод поиска.
                if (StartNode.Nodes.Count != 0)
                {
                    SearchNodes(SearchText, StartNode.Nodes[0]);
                };
                //Берем следующий узел для поиска.
                StartNode = StartNode.NextNode;
            };
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            Build(e.Node);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Очищаем поле поиска.
            textBox1.Text = "";

            //Проверяем количество найденных элементов.
            if (CurrentNodeMatches.Count > 0)
            {
                //Проходимся по найденным элементам.
                for (int i = 0; i < CurrentNodeMatches.Count; i++)
                {
                    //Берем текущий элемент.
                    TreeNode selectedNode = CurrentNodeMatches[i];
                    //Задаем узел дерева, который в текущий
                    //момент будет выбран в элементе
                    //управления иерархического представления.
                    this.treeView1.SelectedNode = selectedNode;
                    //Сворачиваем выбранный узел.
                    this.treeView1.SelectedNode.Collapse();
                    //Задаем цвет фона по умолчанию, для выбранного узла.
                    this.treeView1.SelectedNode.BackColor = treeView1.BackColor;
                    //Активируем элемент управления.
                    this.treeView1.Select();
                }
            }
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            var proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = $"C:\\";
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }
    }
}
