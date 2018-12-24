using System.Collections.Generic;

namespace DjUtil.Tools.DataStructure
{
    public abstract class TreeNode<T> where T :class 
    {
        protected TreeNode(T currentObj, TreeNode<T> parentNode = null)
        {
            CurrentObj = currentObj;
            ParentNode = parentNode;
        }
        private T CurrentObj { get; set; }
        private TreeNode<T> RootNode { get; set; }
        private TreeNode<T> ParentNode { get; set; }
        private List<TreeNode<T>> ChildNodes { get; set; } = new List<TreeNode<T>>();
        public void AddChild(TreeNode<T> child)
        {
            ChildNodes.Add(child);
        }
        public void RemoveChild(TreeNode<T> child)
        {
            ChildNodes.Remove(child);
        }
        public bool Move2OtherParent(TreeNode<T> parent)
        {
            if (!ChildNodes.Contains(parent))
            {
                ParentNode = parent;
                return true;
            }
            return false;
        }
        public override bool Equals(object obj)
        {
            if (obj is T)
            {
                return CurrentObj == obj;
            }
            if(obj is TreeNode<T>)
            {
                return this == obj;
            }
            return false;
        }
    }
}
