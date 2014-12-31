using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace RedCell.UI.WPF
{
    /// <summary>
    /// Interaction logic for FileSystemTree
    /// </summary>
    public partial class FileSystemTree : TreeView
    {
        private readonly object _dummyNode = null;
        private DirectoryInfo _rootPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemTree"/> class.
        /// </summary>
        public FileSystemTree()
        {
            InitializeComponent();
            ShowDirectories = true;
            ShowDrives = false;
            ShowFiles = true;
        }

        /// <summary>
        /// Gets or sets the selected path.
        /// </summary>
        /// <value>The selected path.</value>
        public string SelectedPath { get; set; }

        /// <summary>
        /// Gets or sets the root path.
        /// </summary>
        /// <value>The root path.</value>
        public string RootPath
        {
            get { return _rootPath == null ? null : _rootPath.FullName; }
            set
            {
                _rootPath = value == null ? null : new DirectoryInfo(value);
                UpdateView();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show directories.
        /// </summary>
        /// <value><c>true</c> to show directories; otherwise, <c>false</c>.</value>
        public bool ShowDirectories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show files.
        /// </summary>
        /// <value><c>true</c> to show files; otherwise, <c>false</c>.</value>
        public bool ShowFiles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show drives if RootPath is null.
        /// </summary>
        /// <value><c>true</c> to show drives; otherwise, <c>false</c>.</value>
        public bool ShowDrives { get; set; }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        private void UpdateView()
        {
            this.Items.Clear();

            // Show top-most point in filesystems.
            if (RootPath == null && ShowDrives)
            {
                foreach (string s in Directory.GetLogicalDrives())
                {
                    var item = new TreeViewItem {Header = s, Tag = s, FontWeight = FontWeights.Bold};
                    item.Items.Add(_dummyNode);
                    item.Expanded += Folder_Expanded;
                    this.Items.Add(item);
                }
            }

            // Show specified filesystem.
            else
            {
                var item = new TreeViewItem { Header = _rootPath.Name, Tag = _rootPath, FontWeight = FontWeights.Bold };
                item.Items.Add(_dummyNode);
                item.Expanded += Folder_Expanded;
                this.Items.Add(item);
            }
        }

        /// <summary>
        /// Handles the Expanded event of the Folder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = sender as TreeViewItem;
            Expand(item);
        }

        /// <summary>
        /// Expands the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        private void Expand(TreeViewItem node)
        {
            if (node.Items.Count == 1 && node.Items[0] == _dummyNode)
            {
                node.Items.Clear();
                try
                {
                    var directory = node.Tag as DirectoryInfo;
                    foreach (var item in directory.GetFileSystemInfos().OrderBy(f => f.GetType().Name))
                    {
                        if (item is FileInfo && !ShowFiles) continue;
                        if (item is DirectoryInfo && !ShowDirectories) continue;

                        var subitem = new TreeViewItem
                        {
                            Header = item.Name,
                            Tag = item,
                            FontWeight = FontWeights.Normal,
                            AllowDrop = item is DirectoryInfo,
                        };
                        
                        if(item is DirectoryInfo)
                            subitem.Items.Add(_dummyNode);
                        subitem.Expanded += Folder_Expanded;
                        subitem.Drop += Subitem_Drop;
                        subitem.DragOver += Subitem_DragOver;
                        subitem.GiveFeedback += Subitem_GiveFeedback;

                        node.Items.Add(subitem);
                    }
                }
                catch (Exception)
                {
                    
                }
            }           
            
        }

        private void Subitem_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
        }

        private void Subitem_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent("FileDrop") ? DragDropEffects.Copy : DragDropEffects.None;
        }

        void Subitem_Drop(object sender, DragEventArgs e)
        {
            var item = sender as TreeViewItem;
            var destination = item.Tag as DirectoryInfo;
            if(e.Data.GetDataPresent("FileDrop"))
            {
                var items = e.Data.GetData("FileDrop") as string[];
                foreach (string fi in items)
                {
                    var file = new FileInfo(fi);
                    var directory = new DirectoryInfo(fi);
                    if (file.Exists) file.CopyTo(Path.Combine(destination.FullName, file.Name));
                    if (directory.Exists) DirectoryCopy(directory, destination, true);
                }
            }
        }


        /// <summary>
        /// Copies the directory
        /// </summary>
        /// <param name="sourceDir">Name of the source dir.</param>
        /// <param name="destDir">Name of the dest dir.</param>
        /// <param name="copySubDirs">The copy sub dirs.</param>
        /// <remarks>See http://msdn.microsoft.com/en-us/library/bb762914(v=vs.110).aspx</remarks>
        private static void DirectoryCopy(DirectoryInfo sourceDir, DirectoryInfo destDir, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = sourceDir;
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: "+ sourceDir);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDir.FullName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
                foreach (DirectoryInfo subdir in dirs)
                    DirectoryCopy(subdir, destDir.CreateSubdirectory(subdir.Name), copySubDirs);
        }
        /// <summary>
        /// Folderses the item_ selected item changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void FoldersItem_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = sender as TreeView;
            var item = tree.SelectedItem as TreeViewItem;
            SelectedPath = item == null ? null : (item.Tag as FileSystemInfo).FullName;
        }
    }
}
