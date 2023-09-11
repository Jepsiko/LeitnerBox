using LeitnerBox.ViewModel;

namespace LeitnerBox;

public partial class QuestionPage : ContentPage
{
	public QuestionPage(QuestionViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}