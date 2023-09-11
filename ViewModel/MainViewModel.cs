using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeitnerBox.Model;
using System.Collections.ObjectModel;

namespace LeitnerBox.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel() 
        {
            Subjects = new ObservableCollection<Subject>();

            var path = FileSystem.Current.AppDataDirectory;
            var d = new DirectoryInfo(path);
            foreach (var file in d.GetFiles("*.txt"))
            {
                var fullPath = file.FullName;
                Subjects.Add(Subject.Load(fullPath));
            }
        }

        [ObservableProperty]
        ObservableCollection<Subject> subjects;

        [ObservableProperty]
        string text;

        [RelayCommand]
        void Add()
        {
            if (string.IsNullOrWhiteSpace(Text)) return;

            var s = new Subject(Text);
            s.Save();
            Subjects.Add(s);
            Text = string.Empty;
        }

        [RelayCommand]
        async Task Delete(Subject s)
        {
            bool confirmDelete = await Shell.Current.DisplayAlert("Are you sure you want to delete " + s.Name + "?", "This operation cannot be undone.", "Yes", "No");
            if (confirmDelete && Subjects.Contains(s))
            {
                s.DeleteSave();
                Subjects.Remove(s);
            }
        }

        [RelayCommand]
        async Task Tap(Subject s)
        {
            await Shell.Current.GoToAsync($"{nameof(SubjectPage)}", 
                new Dictionary<string, object>
                {
                    {"Subject", s}
                });
        }
    }
}
