
using DemonicBot.ViewModels;

namespace DemonicBot.Views;

public partial class ChannelsPage : ContentPage
{
    private ChannelsViewModel _viewModel;

    public ChannelsPage()
    {
        InitializeComponent();
        _viewModel = BindingContext as ChannelsViewModel;
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