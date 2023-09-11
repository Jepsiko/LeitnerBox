using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeitnerBox.Model;

namespace LeitnerBox.ViewModel
{
    [QueryProperty("Subject", "Subject")]
    [QueryProperty("Drawer", "Drawer")]
    public partial class DrawerViewModel : ObservableObject
    {
        [ObservableProperty]
        Subject subject;

        [ObservableProperty]
        Drawer drawer;

        [RelayCommand]
        void Delete(Question q)
        {
            if (Drawer.Questions.Contains(q))
            {
                Drawer.RemoveQuestion(q);
            }
            Subject.Save();
        }
    }
}
