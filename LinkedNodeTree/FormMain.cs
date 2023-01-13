using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace LinkedNodeTree
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Branch[] branches =
            {
                new Branch { values = new int[] { 1, 2, 3, 4 } },
                new Branch { values = new int[] { 1, 2, 5, 6 } },
                new Branch { values = new int[] { 1, 2, 5, 7 } },
                new Branch { values = new int[] { 1, 2, 8, 9 } },
                new Branch { values = new int[] { 1, 10, 11, 12 } },
                new Branch { values = new int[] { 1, 10, 13, 14 } },
                new Branch { values = new int[] { 1, 10, 11, 15 } },
                new Branch { values = new int[] { 1, 10, 11, 16 } },
                new Branch { values = new int[] { 1, 10, 11, 12, 17 } },
                new Branch { values = new int[] { 18, 19, 20 } },
                new Branch { values = new int[] { 18, 19, 21 } },
                new Branch { values = new int[] { 18, 19, 22 } },
                new Branch { values = new int[] { 18, 23, 24 } },
                new Branch { values = new int[] { 18, 23, 25 } },
                new Branch { values = new int[] { 26, 27, 28, 29 } },
                new Branch { values = new int[] { 26, 27, 28, 30 } },
                new Branch { values = new int[] { 26, 27, 28, 31 } },
                new Branch { values = new int[] { 26, 27, 32, 33  } },
                new Branch { values = new int[] { 26, 27, 32, 34  } },
                new Branch { values = new int[] { 26, 35, 36, 37  } },
                new Branch { values = new int[] { 26, 35, 36, 38  } },
                new Branch { values = new int[] { 26, 39, 40, 41  } },
                new Branch { values = new int[] { 26, 39, 40, 42  } },
                new Branch { values = new int[] { 26, 39, 43, 44  } },
                new Branch { values = new int[] { 26, 39, 43, 45 } }
            };
            LinkedNodeList linkedNodeList = new LinkedNodeList(branches.Length);
            foreach (var branch in branches)
            {
                LinkedNode[] linkedNodes = LinkedNodeList.Convert(branch);
                foreach (var node in linkedNodes)
                {
                    linkedNodeList.Add(node);
                }
            }
            int scope = branches[0].values[0]-1;
            foreach (var branch in branches)
            {
                if (scope != branch.values[0])
                {
                    scope = branch.values[0];
                    TreeNode rootNode = new TreeNode(branch.values[0].ToString());
                    rootNode = RecursiveTreeNodes(rootNode, linkedNodeList);
                    treeView_Example.Nodes.Add(rootNode);
                }
            }
            treeView_Example.ExpandAll();
        }

        private TreeNode RecursiveTreeNodes(TreeNode treeNode, LinkedNodeList linkedNodeList)
        {
            List<LinkedNode> nodes = linkedNodeList.SelectChildren(int.Parse(treeNode.Text));
            foreach (LinkedNode child in nodes)
            {
                TreeNode node = new TreeNode(child.Id.ToString());
                node = RecursiveTreeNodes(node, linkedNodeList);
                treeNode.Nodes.Add(node);
            }
            return treeNode;
        }
    }
}
