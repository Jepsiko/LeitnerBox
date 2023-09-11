namespace LeitnerBox
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SubjectPage), typeof(SubjectPage));
            Routing.RegisterRoute(nameof(QuestionPage), typeof(QuestionPage));
            Routing.RegisterRoute(nameof(DrawerPage), typeof(DrawerPage));
            Routing.RegisterRoute(nameof(TrainPage), typeof(TrainPage));
        }
    }
}