namespace MauiBug
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void OnGoToVideoClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(VideoPage));
        }

        private void OnForceGCClicked(object? sender, EventArgs e)
        {
            // Force garbage collection to trigger the MauiMediaElement finalizer.
            // If the trimmer stripped the (Context) constructor, this will cause
            // the MissingMethodException / JavaProxyThrowable crash.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private async void OnAutoTestClicked(object? sender, EventArgs e)
        {
            // Programmatically create, display, and dispose the VideoPage.
            // This avoids Shell caching and ensures the page can be GC'd.
            var page = new VideoPage();
            await Navigation.PushModalAsync(page);
            await Task.Delay(3000); // Let the MediaElement fully initialize
            await Navigation.PopModalAsync();
            page = null;

            // Aggressive GC to trigger the MauiMediaElement finalizer
            await Task.Delay(500);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
