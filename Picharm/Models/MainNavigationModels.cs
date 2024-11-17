using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Picharm.Models
{
    public class MainNavigationBase { }

    public class MainNavigationItem : MainNavigationBase
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Icon { get; set; }
        public bool IsLeaf { get; set; }
        public ObservableCollection<MainNavigationItem> Children { get; set; } = null;

        public MainNavigationItem(string name, string tag, string icon, ObservableCollection<MainNavigationItem> children = null)
        {
            Name = name;
            Tag = tag;
            Icon = icon;
            IsLeaf = children == null;
            Children = children;
        }
    }

    public class MainNavigationHeader : MainNavigationBase
    {
        public string Name { get; set; }

        public MainNavigationHeader(string name)
        {
            Name = name;
        }
    }

    public class MainNavigationUserItem : MainNavigationBase
    {
        public string Tag { get; set; }

        public MainNavigationUserItem(string tag)
        {
            Tag = tag;
        }
    }

    public class MainNavigationSettingItem : MainNavigationBase
    {
        public string Tag { get; set; }

        public MainNavigationSettingItem(string tag)
        {
            Tag = tag;
        }
    }

    public class MainNavigationSeparator : MainNavigationBase
    {

    }

    class MenuItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ItemTemplate { get; set; }
        public DataTemplate HeaderTemplate { get; set; }
        public DataTemplate UserItemTemplate { get; set; }
        public DataTemplate SettingItemTemplate { get; set; }
        public DataTemplate SeparatorTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return item is MainNavigationItem ? ItemTemplate :
                   item is MainNavigationHeader ? HeaderTemplate :
                   item is MainNavigationUserItem ? UserItemTemplate :
                   item is MainNavigationSettingItem ? SettingItemTemplate : SeparatorTemplate;
        }
    }

}
