using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MinimalRepro
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        #region Constructor
        public MainWindowVM(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            RootNode = new(null!, "Root");
            // populate the tree with placeholder nodes
            for (int i = 0, i_max = 4; i < i_max; ++i)
            {
                var childNode = new TreeNode(RootNode, $"Node{i}");

                for (int j = 0, j_max = 4; j < j_max; ++j)
                {
                    var subChildNode = new TreeNode(childNode, $"Node{j}");

                    for (int k = 0, k_max = 4; k < k_max; ++k)
                    {
                        subChildNode.Children.Add(new TreeNode(subChildNode, $"Node{k}"));
                    }

                    childNode.Children.Add(subChildNode);
                }

                RootNode.Children.Add(childNode);
            }
        }
        #endregion Constructor

        #region Fields
        private MainWindow _mainWindow;
        #endregion Fields

        #region Properties
        public TreeNode RootNode { get; }
        public string CurrentPath
        {
            get => _currentPath;
            set
            {
                _currentPath = value;

                if (FindNode(_currentPath) is TreeNode targetNode)
                {
                    _mainWindow.TreeView.SelectedItemChanged -= this.TreeView_SelectedItemChanged;
                    targetNode.IsSelected = true;
                    _mainWindow.TreeView.SelectedItemChanged += this.TreeView_SelectedItemChanged;
                    targetNode.ForEachBranchNode(node => node.IsExpanded = true);
                }

                NotifyPropertyChanged();
            }
        }
        private string _currentPath;
        #endregion Properties

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new(propertyName));
        #endregion Events

        #region Methods

        #region PostInitializeComponent
        internal void PostInitializeComponent()
        {
            _mainWindow.TreeView.SelectedItemChanged += this.TreeView_SelectedItemChanged;
        }
        #endregion PostInitializeComponent

        #region FindNode
        public TreeNode? FindNode(string stringPath)
        {
            if (stringPath.Length == 0)
                return null;

            var path = stringPath.Split('/');

            if (path.Length == 0 || !path[0].Equals(RootNode.Name, StringComparison.OrdinalIgnoreCase))
                return null;
            else if (path.Length == 1)
                return RootNode;

            TreeNode? node = RootNode;

            int i = 1;
            for (int i_max = path.Length;
                i < i_max && node != null;
                node = node.FindChild(path[i++], StringComparison.OrdinalIgnoreCase))
            { }

            if (i != path.Length)
                return null;

            return node;
        }
        #endregion FindNode

        #endregion Methods

        #region EventHandlers

        #region TreeView
        private void TreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            var oldNode = (TreeNode?)e.OldValue;
            var newNode = (TreeNode?)e.NewValue;

            if (newNode != null)
            {
                _currentPath = newNode.GetPath();
                NotifyPropertyChanged(nameof(CurrentPath));
            }
            else
            {
                _currentPath = "";
                NotifyPropertyChanged(nameof(CurrentPath));
            }
        }
        #endregion TreeView

        #endregion EventHandlers
    }
}
