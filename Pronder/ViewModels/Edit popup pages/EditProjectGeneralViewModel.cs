using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Pronder.Models;
using System.Diagnostics;
using System.IO;

namespace Pronder.ViewModels
{
    public partial class EditProjectPagesGeneralViewModel : ObservableRecipient
    {
        public static Project? _project => Project.GlobalInstance;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private string _tag;

        [ObservableProperty]
        private string? _iconPath;

        [ObservableProperty]
        private string? _bannerPath;

        [ObservableProperty]
        private bool _isIconExpanderExpanded = false;

        [ObservableProperty]
        private bool _isBannerExpanderExpanded = false;

        [ObservableProperty]
        private string _iconFileName;

        [ObservableProperty]
        private string _bannerFileName;

        public EditProjectPagesGeneralViewModel()
        {
            _name = _project.Name;
            _description = _project.About;
            _tag = _project.Tag;

            _iconPath = string.IsNullOrEmpty(_project.Icon) ? "" : _project.Icon;
            _bannerPath = string.IsNullOrEmpty(_project.Banner) ? null : _project.Banner;

            _isIconExpanderExpanded = !string.IsNullOrEmpty(_project.Icon);
            _isBannerExpanderExpanded = !string.IsNullOrEmpty(_project.Banner);

            _iconFileName = !string.IsNullOrEmpty(_project.Icon) ? Path.GetFileName(_project.Icon) : null;
            _bannerFileName = !string.IsNullOrEmpty(_project.Banner) ? Path.GetFileName(_project.Banner) : null;

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName is nameof(Name) or nameof(Description) or nameof(Tag) or nameof(IconPath) or nameof(BannerPath) or nameof(IconFileName) or nameof(BannerFileName))
                {
                    UpdateProject();
                    SaveProjectToFile();
                }

                if (e.PropertyName == nameof(IconPath))
                {
                    IsIconExpanderExpanded = !string.IsNullOrEmpty(IconPath);
                }

                if (e.PropertyName == nameof(BannerPath))
                {
                    IsBannerExpanderExpanded = !string.IsNullOrEmpty(BannerPath);
                }
            };
        }

        private void UpdateProject()
        {
            _project.Name = Name;
            _project.About = Description;
            _project.Tag = Tag;
            _project.Icon = IconPath;
            _project.Banner = BannerPath;

            _isIconExpanderExpanded = !string.IsNullOrEmpty(_project.Icon);
            _isBannerExpanderExpanded = !string.IsNullOrEmpty(_project.Banner);

            _iconFileName = !string.IsNullOrEmpty(_project.Icon) ? Path.GetFileName(_project.Icon) : null;
            _bannerFileName = !string.IsNullOrEmpty(_project.Banner) ? Path.GetFileName(_project.Banner) : null;
        }

        private void SaveProjectToFile()
        {
            if (!string.IsNullOrEmpty(Project.ProjectPath))
            {
                File.WriteAllText(Project.ProjectPath, JsonConvert.SerializeObject(_project, Formatting.Indented));
            }
        }
    }
}
