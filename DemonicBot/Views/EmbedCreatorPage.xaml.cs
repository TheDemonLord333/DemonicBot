using DemonicBot.ViewModels;

namespace DemonicBot.Views;

public partial class EmbedCreatorPage : ContentPage
{
    private EmbedCreatorViewModel _viewModel;

    public EmbedCreatorPage()
    {
        InitializeComponent();
        _viewModel = BindingContext as EmbedCreatorViewModel;
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