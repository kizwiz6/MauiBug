namespace MauiBug
{
    public partial class VideoPage : ContentPage
    {
        public VideoPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                // Peel through the wrapper exceptions to find the real trimmer error.
                // The JavaProxyThrowable hides the actual MissingMethodException or
                // XamlParseException — this prints it to the Debug Output window.
                System.Diagnostics.Debug.WriteLine("=== TRIMMER CRASH DETECTED ===");
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                var inner = ex.InnerException;
                var depth = 1;
                while (inner != null)
                {
                    System.Diagnostics.Debug.WriteLine($"=== INNER EXCEPTION (depth {depth}) ===");
                    System.Diagnostics.Debug.WriteLine(inner.ToString());
                    inner = inner.InnerException;
                    depth++;
                }

                System.Diagnostics.Debug.WriteLine("=== END TRIMMER CRASH ===");
                throw; // Re-throw so the crash still surfaces
            }
        }

        private async void OnGoBackClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
