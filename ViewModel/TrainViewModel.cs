using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeitnerBox.Model;

namespace LeitnerBox.ViewModel
{
    [QueryProperty("Subject", "Subject")]
    public partial class TrainViewModel : ObservableObject
    {
        [ObservableProperty]
        Subject subject;

        [ObservableProperty]
        Drawer drawer;

        [ObservableProperty]
        Question current;

        [ObservableProperty]
        int index = 1;

        [ObservableProperty]
        int size;

        [ObservableProperty]
        string answer;

        int drawerIndex = 0;
        int[] sizes;

        int correct = 0;
        int failed = 0;
        int total = 0;

        partial void OnSubjectChanged(Subject value)
        {
            sizes = new int[Subject.NUM_DRAWERS];
            for (int i = 0; i < Subject.NUM_DRAWERS; i++)
            {
                sizes[i] = Subject.Drawers[i].Size;
            }

            GoToNextTrainableDrawer();
            Drawer = value.Drawers[drawerIndex];
            Current = Drawer.Questions[0];
            Size = sizes[drawerIndex];
        }

        [RelayCommand]
        void Show()
        {
            Answer = Current.A;
        }

        [RelayCommand]
        async Task Validate()
        {
            correct++;
            Drawer.RemoveQuestion(Current);
            if (drawerIndex < Subject.NUM_DRAWERS-1) 
                Subject.Drawers[drawerIndex+1].AddQuestion(Current);
            else
            {
                bool remove = await Shell.Current.DisplayAlert("You mastered this question!", "Do you want to remove it from your training ?", "Yes", "No");
                if (!remove)
                {
                    Drawer.AddQuestion(Current);
                }
            }
            Subject.Save();
            await NextQuestion();
        }

        [RelayCommand]
        async Task Fail()
        {
            failed++;
            Drawer.RemoveQuestion(Current);
            Subject.Drawers.First().AddQuestion(Current);
            Subject.Save();
            await NextQuestion();
        }

        async Task NextQuestion()
        {
            total++;
            if (Index < Size)
                Index++;
            else
                await NextDrawer();
            if (Drawer.Size > 0) Current = Drawer.Questions[0];
            Answer = "";
        }

        void GoToNextTrainableDrawer()
        {
            while (drawerIndex < Subject.NUM_DRAWERS && (sizes[drawerIndex] == 0 || !Subject.Drawers[drawerIndex].Trainable(Subject.CreationDate)))
                drawerIndex++;
        }

        async Task NextDrawer()
        {
            Index = 1;
            Subject.Drawers[drawerIndex].Trained();
            drawerIndex++;
            GoToNextTrainableDrawer();

            if (drawerIndex == Subject.NUM_DRAWERS)
            {
                await Shell.Current.DisplayAlert("Training finished!",
                    "• " + correct + " questions answered correctly\n"+
                    "• " + failed + " questions failed", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                Drawer = Subject.Drawers[drawerIndex];
                Size = sizes[drawerIndex];
            }
        }
    }
}
