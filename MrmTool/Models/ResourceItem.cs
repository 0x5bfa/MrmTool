using MrmLib;
using CommunityToolkit.Mvvm.ComponentModel;
using MrmTool.Common;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace MrmTool.Models
{
    public partial class ResourceItem : ObservableObject
    {
        public ResourceItem(string name, ObservableCollection<ResourceItem> parent)
        {
            Parent = parent;
            Name = name;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayName))]
        public partial string Name { get; set; }

        [ObservableProperty]
        public partial BitmapImage? Icon { get; private set; }

        private int _oldNameLength;

        internal ObservableCollection<ResourceItem> Parent = null!;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFolder))]
        internal partial ResourceType Type { get; private set; }

        partial void OnNameChanging(string value)
        {
            _oldNameLength = Name?.Length ?? 0;
        }

        partial void OnNameChanged(string value)
        {
            foreach (var candidate in Candidates)
            {
                candidate.Candidate.ResourceName = value;
            }

            foreach (var child in Children)
            {
                child.Name = value + child.Name[_oldNameLength..];
            }

            EnsureIconAndType(true);
        }

        public string DisplayName
        {
            get => Name.GetDisplayName();
            set
            {
                if (!string.Equals(DisplayName, value, StringComparison.Ordinal))
                {
                    Name = Name.SetDisplayName(value);
                }
            }
        }

        public ObservableCollection<ResourceItem> Children { get; } = [];

        public ObservableCollection<CandidateItem> Candidates { get; } = [];

        internal bool IsFolder => Type is ResourceType.Folder || Children.Count > 0;

        private void DetermineType()
        {
            if (Children.Count > 0)
            {
                Type = ResourceType.Folder;
            }
            else
            {
                Type = DisplayName.DetermineResourceType();

                if (Type is ResourceType.Unknown &&
                    Candidates.Count > 0 &&
                    Candidates[0].Candidate.ValueType is ResourceValueType.String)
                {
                    Type = ResourceType.Text;
                }
            }
        }

        internal void EnsureIconAndType(bool changed = false)
        {
            if (changed is true || Icon is null || (Type is not ResourceType.Folder && Children.Count > 0))
            {
                DetermineType();

                Icon = Type.GetCorrespondingIcon();
            }
        }

        internal void Delete(PriFile pri, bool isChild = false)
        {
            foreach (var candidate in Candidates)
            {
                pri.ResourceCandidates.Remove(candidate.Candidate);
            }

            Candidates.Clear();

            foreach (var child in Children)
            {
                child.Delete(pri, true);
            }

            if (!isChild)
            {
                Parent.Remove(this);
            }

            Children.Clear();
        }
    }
}
