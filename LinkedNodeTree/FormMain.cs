using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace LinkedNodeTree
{
    public struct Branch
    {
        public int[] values;
    }

    public partial class FormMain : Form
    {
        private readonly Branch[] branches =
        {
                new Branch { values = new int[] { 1, 2, 3, 4 } },
                new Branch { values = new int[] { 1, 2, 5, 6 } },
                new Branch { values = new int[] { 1, 2, 5, 7 } },
                new Branch { values = new int[] { 1, 2, 8, 9 } },
                new Branch { values = new int[] { 1, 10, 11, 16 } },
                new Branch { values = new int[] { 1, 10, 11, 12, 17 } },
                new Branch { values = new int[] { 18, 19, 20 } },
                new Branch { values = new int[] { 26, 27, 32, 34  } },
                new Branch { values = new int[] { 26, 35, 36, 37  } },
                new Branch { values = new int[] { 26, 35, 36, 38  } },
                new Branch { values = new int[] { 18, 19, 21 } },
                new Branch { values = new int[] { 18, 19, 22 } },
                new Branch { values = new int[] { 18, 23, 24 } },
                new Branch { values = new int[] { 18, 23, 25 } },
                new Branch { values = new int[] { 1, 10, 11, 12 } },
                new Branch { values = new int[] { 1, 10, 13, 14 } },
                new Branch { values = new int[] { 1, 10, 11, 15 } },
                new Branch { values = new int[] { 26, 27, 28, 29 } },
                new Branch { values = new int[] { 26, 27, 28, 30 } },
                new Branch { values = new int[] { 26, 27, 28, 31 } },
                new Branch { values = new int[] { 26, 27, 32, 33  } },
                new Branch { values = new int[] { 26, 39, 40, 41  } },
                new Branch { values = new int[] { 26, 39, 40, 42  } },
                new Branch { values = new int[] { 26, 39, 43, 44  } },
                new Branch { values = new int[] { 26, 39, 43, 45 } }
        };

        public FormMain()
        {
            InitializeComponent();
        }

        #region Build a tree using recursion and the idea of links between nodes
        private void BuildTree_1(Branch[] branches)
        {
            LinkedNodeList linkedNodeList = Convert(branches);
            TreeNode rootNode = new TreeNode("root");
            rootNode = RecursiveTreeNodes(rootNode, linkedNodeList);
            treeView_Example.Nodes.Add(rootNode);
        }

        private LinkedNode[] Convert(Branch branch)
        {
            LinkedNode[] nodes = new LinkedNode[branch.values.Length];
            nodes[0] = new LinkedNode
            {
                IdParent = "root",
                Id = branch.values[0].ToString()
            };
            for (var i = 1; i < branch.values.Length; i++)
            {
                nodes[i] = new LinkedNode
                {
                    IdParent = branch.values[i - 1].ToString(),
                    Id = branch.values[i].ToString()
                };
            }
            return nodes;
        }

        private LinkedNodeList Convert(Branch[] branches)
        {
            LinkedNodeList linkedNodeList = new LinkedNodeList(branches.Length);
            foreach (var branch in branches)
            {
                LinkedNode[] linkedNodes = Convert(branch);
                linkedNodeList.AddRange(linkedNodes);
            }
            return linkedNodeList;
        }

        private TreeNode RecursiveTreeNodes(TreeNode treeNode, LinkedNodeList linkedNodeList)
        {
            List<LinkedNode> nodes = linkedNodeList.SelectChildren(treeNode.Text);
            foreach (LinkedNode child in nodes)
            {
                TreeNode node = new TreeNode(child.Id.ToString());
                node = RecursiveTreeNodes(node, linkedNodeList);
                treeNode.Nodes.Add(node);
            }
            return treeNode;
        }
        #endregion

        #region Build a tree using only branches
        private void BuildTree_2(Branch[] branches)
        {
            treeView_Example.Nodes.Add("root");
            TreeNode currentNode = treeView_Example.Nodes[0];
            foreach (Branch branch in branches)
            {
                for (int i = 0; i < branch.values.Length; i++)
                {
                    IEnumerable<TreeNode> existsBranch = currentNode.Nodes
                                    .Cast<TreeNode>()
                                    .Where((n) => n.Text == branch.values[i].ToString());

                    if (existsBranch.Count() > 0)
                    {
                        currentNode = existsBranch.First();
                    }
                    else
                    {
                        currentNode.Nodes.Add(branch.values[i].ToString());
                        currentNode = currentNode.Nodes[currentNode.Nodes.Count - 1];
                    }
                }
                currentNode = treeView_Example.Nodes[0];
            }
        }
        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {
            treeView_Example.Nodes.Clear();
            long time = DateTime.Now.Ticks;

            BuildTree_1(branches);

            time = DateTime.Now.Ticks - time;
            treeView_Example.ExpandAll();
            MessageBox.Show("rutime:" + (time / 10).ToString() + " μs");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            treeView_Example.Nodes.Clear();
            long time = DateTime.Now.Ticks;

            BuildTree_2(branches);

            time = DateTime.Now.Ticks - time;
            treeView_Example.ExpandAll();
            MessageBox.Show("rutime:" + (time/10).ToString() + " μs");
        }
    }
}
