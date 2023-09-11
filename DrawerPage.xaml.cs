using LeitnerBox.ViewModel;

namespace LeitnerBox;

public partial class DrawerPage : ContentPage
{
	public DrawerPage(DrawerViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}