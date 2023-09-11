using LeitnerBox.ViewModel;

namespace LeitnerBox;

public partial class SubjectPage : ContentPage
{
	public SubjectPage(SubjectViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}