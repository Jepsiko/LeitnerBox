using LeitnerBox.ViewModel;

namespace LeitnerBox;

public partial class TrainPage : ContentPage
{
	public TrainPage(TrainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}