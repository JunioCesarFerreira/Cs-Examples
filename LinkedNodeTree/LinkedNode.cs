using System.Collections.Generic;

namespace LinkedNodeTree
{
    class LinkedNode
    {
        public string Id;
        public string IdParent;
    }

    class LinkedNodeList
    {
        public List<LinkedNode> Nodes { get; private set; }

        public LinkedNodeList(int capacity)
        {
            Nodes = new List<LinkedNode>(capacity);
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

        public void AddRange(LinkedNode[] range)
        {
            foreach (LinkedNode node in range) Add(node);
        }

        public List<LinkedNode> SelectChildren(string parentId)
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
