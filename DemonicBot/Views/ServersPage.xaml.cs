using DemonicBot.ViewModels;

namespace DemonicBot.Views;

public partial class ServersPage : ContentPage
{
    private ServersViewModel _viewModel;

    public ServersPage()
    {
        InitializeComponent();
        _viewModel = BindingContext as ServersViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel != null)
        {
            await _viewModel.InitializeAsync();
        }
    }
}