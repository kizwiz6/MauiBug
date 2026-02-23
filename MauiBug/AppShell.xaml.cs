namespace MauiBug
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(VideoPage), typeof(VideoPage));
        }
    }
}
