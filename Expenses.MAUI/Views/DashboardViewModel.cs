using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Expenses.Common.DTO;
using Expenses.MAUI.Interfaces;

namespace Expenses.MAUI.Views
{
    public partial class DashboardViewModel : VmBase
    {
        private readonly IExpenseService _expenseService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        [ObservableProperty]
        private bool isRefreshing;

        public DashboardViewModel(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        public ObservableCollection<ExpenseDTO> RecentExpenses { get; } = new();
        public bool IsNotBusy => !IsBusy;

        public override async Task OnInitializedAsync()
        {
            await GetExpensesAsync();
        }

        [RelayCommand]
        private async Task RefreshExpensesAsync()
        {
            await GetExpensesAsync();
        }

        private async Task GetExpensesAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var expenses = await _expenseService.GetExpensesAsync();

                if (expenses is not null)
                {
                    if (RecentExpenses.Count > 0)
                    {
                        RecentExpenses.Clear();
                    }

                    foreach (var item in expenses)
                    {
                        RecentExpenses.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"Unable to get expenses: {ex.Message}", "Understood");
            }
            finally
            {
                IsBusy = false;
                if (IsRefreshing)
                {
                    IsRefreshing = false;
                }
            }
        }

        [RelayCommand]
        public Task GoToExpenseAsync()
        {
            return Task.CompletedTask;
        }

        [RelayCommand]
        public Task GoToRecentExpensesAsync()
        {
            return Task.CompletedTask;
        }
    }
}
