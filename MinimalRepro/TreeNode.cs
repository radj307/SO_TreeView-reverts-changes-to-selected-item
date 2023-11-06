using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MinimalRepro
{
    [AddINotifyPropertyChangedInterface]
    public class TreeNode
    {
        #region Constructor
        public TreeNode(TreeNode? parent, string name)
        {
            Parent = parent;
            Children = new();
            Name = name;
        }
        #endregion Constructor

        #region Properties
        public TreeNode? Parent { get; }
        public ObservableCollection<TreeNode> Children { get; }
        public string Name { get; set; }
        public bool IsSelected
        {
            get;
            set;
        }
        public bool IsExpanded { get; set; }
        #endregion Properties

        #region Methods
        public override string ToString() => Name;
        public T[] GetBranch<T>(Func<TreeNode, T> selector)
        {
            List<T> l = new();

            // get each node all the way up the tree until the root node
            for (TreeNode? node = this; node != null; node = node.Parent)
            {
                l.Add(selector(node));
            }
            // reverse the list so the path starts with the root node
            l.Reverse();

            return l.ToArray();
        }
        public TreeNode[] GetBranch() => GetBranch(node => node);
        public string GetPath() => string.Join('/', GetBranch(node => node.Name));
        public void ForEachBranchNode(Action<TreeNode> action)
        {
            foreach (var node in GetBranch())
            {
                action(node);
            }
        }
        public TreeNode? FindChild(string name, StringComparison stringComparison = StringComparison.Ordinal)
            => Children.FirstOrDefault(child => child.Name.Equals(name, stringComparison));
        #endregion Methods
    }
}
