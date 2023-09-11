using CommunityToolkit.Mvvm.ComponentModel;

namespace LeitnerBox.Model
{
    public partial class Question : ObservableObject
    {
        [ObservableProperty]
        string q;

        [ObservableProperty]
        string a;

        public Question(string question, string answer) {
            Q = question.Trim('\n');
            A = answer.Trim('\n');
        }
    }
}
