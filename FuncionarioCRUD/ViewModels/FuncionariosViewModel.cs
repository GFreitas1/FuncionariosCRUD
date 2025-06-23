using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using FuncionarioCRUD.Models;
using FuncionarioCRUD.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FuncionarioCRUD.ViewModels
{
    public partial class FuncionarioViewModel : ObservableObject
    {
        private readonly FuncionarioService _funcionarioService;

        public IAsyncRelayCommand LoadFuncionariosCommand { get; }
        public IAsyncRelayCommand AddFuncionarioCommand { get; }
        public IAsyncRelayCommand UpdateFuncionarioCommand { get; }
        public IAsyncRelayCommand DeleteFuncionarioCommand { get; }
        public IRelayCommand ClearSelectionCommand { get; }

        [ObservableProperty]
        private ObservableCollection<Funcionario> _funcionarios;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddFuncionarioCommand))]
        private string _novoNome;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddFuncionarioCommand))]
        private string _novaSenha;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UpdateFuncionarioCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteFuncionarioCommand))]
        [NotifyCanExecuteChangedFor(nameof(ClearSelectionCommand))]
        private Funcionario _selectedFuncionario;

        [ObservableProperty]
        private bool _isBusy;

        public FuncionarioViewModel()
        {
            _funcionarioService = new FuncionarioService();
            Funcionarios = new ObservableCollection<Funcionario>();

            LoadFuncionariosCommand = new AsyncRelayCommand(LoadFuncionariosAsync);
            AddFuncionarioCommand = new AsyncRelayCommand(AddFuncionarioAsync, CanAddFuncionario);
            UpdateFuncionarioCommand = new AsyncRelayCommand(UpdateFuncionarioAsync, CanUpdateFuncionario);
            DeleteFuncionarioCommand = new AsyncRelayCommand(DeleteFuncionarioAsync, CanDeleteFuncionario);
            ClearSelectionCommand = new RelayCommand(ClearSelection);

            LoadFuncionariosCommand.Execute(null);
        }

        private async Task LoadFuncionariosAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var fetchedFuncionarios = await _funcionarioService.GetFuncionariosAsync();
                Funcionarios.Clear();
                foreach (var func in fetchedFuncionarios)
                {
                    Funcionarios.Add(func);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddFuncionarioAsync()
        {
            if (IsBusy || !CanAddFuncionario()) return;
            IsBusy = true;
            try
            {
                var novoFuncionario = new Funcionario
                {
                    Nome = NovoNome,
                    Senha = NovaSenha
                };

                var funcionarioAdicionado = await _funcionarioService.AddFuncionarioAsync(novoFuncionario);
                if (funcionarioAdicionado != null)
                {
                    Funcionarios.Add(funcionarioAdicionado);
                    NovoNome = string.Empty;
                    NovaSenha = string.Empty;
                    await Shell.Current.DisplayAlert("Sucesso", "Funcionário adicionado com sucesso!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erro", "Falha ao adicionar funcionário.", "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanAddFuncionario()
        {
            return !string.IsNullOrWhiteSpace(NovoNome) && !string.IsNullOrWhiteSpace(NovaSenha);
        }

        private async Task UpdateFuncionarioAsync()
        {
            if (IsBusy || !CanUpdateFuncionario()) return;
            IsBusy = true;
            try
            {
                if (SelectedFuncionario != null)
                {
                    bool success = await _funcionarioService.UpdateFuncionarioAsync(SelectedFuncionario);
                    if (success)
                    {
                        await Shell.Current.DisplayAlert("Sucesso", "Funcionário atualizado com sucesso!", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Erro", "Falha ao atualizar funcionário.", "OK");
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanUpdateFuncionario()
        {
            return SelectedFuncionario != null;
        }

        private async Task DeleteFuncionarioAsync()
        {
            if (IsBusy || !CanDeleteFuncionario()) return;
            IsBusy = true;
            try
            {
                if (SelectedFuncionario != null)
                {
                    bool confirm = await Shell.Current.DisplayAlert("Confirmar", $"Tem certeza que deseja deletar {SelectedFuncionario.Nome}?", "Sim", "Não");
                    if (confirm)
                    {
                        bool success = await _funcionarioService.DeleteFuncionarioAsync(SelectedFuncionario.Id);
                        if (success)
                        {
                            Funcionarios.Remove(SelectedFuncionario);
                            SelectedFuncionario = null;
                            await Shell.Current.DisplayAlert("Sucesso", "Funcionário deletado com sucesso!", "OK");
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Erro", "Falha ao deletar funcionário.", "OK");
                        }
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanDeleteFuncionario()
        {
            return SelectedFuncionario != null;
        }

        private void ClearSelection()
        {
            SelectedFuncionario = null;
        }
    }
}