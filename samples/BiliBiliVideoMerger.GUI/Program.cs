namespace BiliBiliVideoMerger;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

        appBuilder.Services.AddMasaBlazor();



        appBuilder.RootComponents.Add<App>("app");

        var app = appBuilder.Build();

        app.MainBlazorWindow.Window
            .SetTitle("BiliBiliVideoMerger")
            .SetUseOsDefaultSize(false)
            .SetSize(853, 480);

        app.Run();
    }
}
