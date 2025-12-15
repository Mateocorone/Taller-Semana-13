namespace Farmacia_V_M.R.E.A_.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnIngresarClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuPage());
    }
}
