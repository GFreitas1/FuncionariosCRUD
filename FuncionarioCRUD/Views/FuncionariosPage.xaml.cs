using FuncionarioCRUD.ViewModels;
using CommunityToolkit.Mvvm.Input;

namespace FuncionarioCRUD.Views
{
    public partial class FuncionariosPage : ContentPage
    {
        private readonly FuncionarioViewModel _viewModel;

        public FuncionariosPage()
        {
            InitializeComponent();
            _viewModel = new FuncionarioViewModel();
            this.BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel.LoadFuncionariosCommand.CanExecute(null))
            {
                await _viewModel.LoadFuncionariosCommand.ExecuteAsync(null);
            }
        }
    }
}