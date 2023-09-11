using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeitnerBox.Model;

namespace LeitnerBox.ViewModel
{
    [QueryProperty("Subject", "Subject")]
    public partial class QuestionViewModel : ObservableObject
    {
        [ObservableProperty]
        Subject subject;

        [ObservableProperty]
        string q;

        [ObservableProperty]
        string a;

        [RelayCommand]
        async Task Add()
        {
            if (string.IsNullOrWhiteSpace(Q)) return;
            if (string.IsNullOrWhiteSpace(A)) return;

            Subject.AddQuestion(new Question(Q, A));
            Subject.Save();

            await Shell.Current.GoToAsync("..");
        }
    }
}
