using DemonicBot.ViewModels;

namespace DemonicBot.Views;

public partial class SettingsPage : ContentPage
{
    private SettingsViewModel _viewModel;

    public SettingsPage()
    {
        InitializeComponent();
        _viewModel = BindingContext as SettingsViewModel;
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