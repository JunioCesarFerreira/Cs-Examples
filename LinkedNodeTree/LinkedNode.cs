using System.Collections.Generic;

namespace LinkedNodeTree
{
    struct Branch
    {
        public int[] values;
    }

    class LinkedNode
    {
        public int Id;
        public int IdParent;
    }

    class LinkedNodeList
    {
        public List<LinkedNode> Nodes { get; private set; }

        public LinkedNodeList(int capacity)
        {
            Nodes = new List<LinkedNode>(capacity);
        }

        public static LinkedNode[] Convert(Branch branch)
        {
            LinkedNode[] nodes = new LinkedNode[branch.values.Length - 1];
            for (var i = 1; i < branch.values.Length; i++)
            {
                nodes[i-1] = new LinkedNode
                {
                    IdParent = branch.values[i - 1],
                    Id = branch.values[i]
                };
            }
            return nodes;
        }

        public void Add(LinkedNode newNode)
        {
            bool isNew = true;
            foreach (LinkedNode node in Nodes)
            {
                if (node.Id == newNode.Id)
                {
                    isNew = false;
                    break;
                }
            }
            if (isNew)
            {
                Nodes.Add(newNode);
            }
        }

        public List<LinkedNode> SelectChildren(int parentId)
        {
            List<LinkedNode> result = new List<LinkedNode>(Nodes.Count / 2);
            foreach(LinkedNode linkedNode in Nodes)
            {
                if (linkedNode.IdParent == parentId)
                {
                    result.Add(linkedNode);
                }
            }
            return result;
        }
    }
}
