using DemonicBot.ViewModels;

namespace DemonicBot.Views;

public partial class LoginPage : ContentPage
{
    private LoginViewModel _viewModel;

    public LoginPage()
    {
        InitializeComponent();
        _viewModel = BindingContext as LoginViewModel;
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