using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeitnerBox.Model;

namespace LeitnerBox.ViewModel
{
    [QueryProperty("Subject", "Subject")]
    public partial class SubjectViewModel : ObservableObject
    {
        [ObservableProperty]
        Subject subject;

        [ObservableProperty]
        DateTime nextTrainingDate;

        partial void OnSubjectChanged(Subject value)
        {
            NextTrainingDate = value.NextTrainingDate();
        }

        [RelayCommand]
        async Task Add()
        {
            await Shell.Current.GoToAsync($"{nameof(QuestionPage)}",
                new Dictionary<string, object>
                {
                    {"Subject", Subject}
                });
        }


        [RelayCommand]
        async Task Train()
        {
            if (!Subject.HasQuestions())
            {
                await Shell.Current.DisplayAlert("This subject contains no questions!", "Add questions to the subject to train.", "OK");
                return;
            }
            if (Subject.TrainedToday)
            {
                await Shell.Current.DisplayAlert("Subject already trained today!", "Come back another day for the next training.", "OK");
                return;
            }
            if (!Subject.Trainable())
            {
                await Shell.Current.DisplayAlert("You don't have any training to do in this subject.", "Come back another day for the next training.", "OK");
                return;
            }
            Subject.TrainedToday = true;
            await Shell.Current.GoToAsync($"{nameof(TrainPage)}",
                new Dictionary<string, object>
                {
                    {"Subject", Subject}
                });
        }

        [RelayCommand]
        async Task Tap(Drawer d)
        {
            await Shell.Current.GoToAsync($"{nameof(DrawerPage)}",
                new Dictionary<string, object>
                {
                    {"Subject", Subject},
                    {"Drawer", d}
                });
        }
    }
}
