using financialHelper1._0.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace financialHelper1._0
{
    /// <summary>
    /// Interaction logic for ExpensesPage1.xaml
    /// </summary>
    public partial class ExpensesPage1 : Window
    {
        FinancesContext db = new FinancesContext();

        private MainWindow _mainWindow;

        public ExpensesPage1(MainWindow mainWindow)
        {

            InitializeComponent();
            _mainWindow = mainWindow;

            loadDataE();

        }
        
        private void loadDataE()
        {
            List<Expense> pExpensesList = db.Expenses.OrderByDescending(x => x.Date).ToList();
            gridExpensesPage.ItemsSource = pExpensesList;
        }

        private void updateBalance(int account, double amount)
        {
            Balance oldBalance = db.Balances
                                  .Where(x => x.Id == account)
                                  .First();

            Balance newBalace = new Balance();

            newBalace.Id = account;
            newBalace.AccountBalance = oldBalance.AccountBalance + amount;
            newBalace.AccountName = oldBalance.AccountName;

            using (FinancesContext updateContext = new FinancesContext())
            {
                updateContext.Balances.Update(newBalace);
                updateContext.SaveChanges();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.loadData();
            _mainWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void btnMoneyReturn_Click(object sender, RoutedEventArgs e)
        {
            List<Expense> moneyReturnsList = (List<Expense>)db.Expenses.Where(x => x.HowMuchReturn != 0 && x.IsSettled == false).ToList();
            gridExpensesPage.ItemsSource = moneyReturnsList;

        }

        private void btnAllExpenses_Click(object sender, RoutedEventArgs e)
        {
            List<Expense> pExpensesList = db.Expenses.OrderByDescending(x => x.Date).ToList();
            gridExpensesPage.ItemsSource = pExpensesList;

            gridExpensesPage.SelectionMode = DataGridSelectionMode.Single;

            btnDelete.Visibility = Visibility.Visible;
            btnExpenseSettle.Visibility = Visibility.Visible;
            
        }

        private void btnOwnExpenses_Click(object sender, RoutedEventArgs e)
        {
            List<Expense> myExpensesList = (List<Expense>)db.Expenses.Where(x => x.HowMuchReturn == 0).ToList();
            gridExpensesPage.ItemsSource = myExpensesList;
        }

        private void btnCategorySym_Click(object sender, RoutedEventArgs e)
        {
            var summedList = db.Expenses.GroupBy(e => e.Category)
                                        .Select(s => new
                                        {
                                            Category = s.Key,
                                            Amount = s.Sum(e => e.Amount)
                                        })
                                        .ToList();
            gridExpensesPage.ItemsSource = summedList;

            btnDelete.Visibility = Visibility.Collapsed;
            btnExpenseSettle.Visibility = Visibility.Collapsed;
        }

        private void btnExpenseSettle_Click(object sender, RoutedEventArgs e)
        {
            if (gridExpensesPage.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an expense");
            }
            else
            {
                Expense settledExpense = (Expense)gridExpensesPage.SelectedItem;

                updateBalance(settledExpense.AccountId, settledExpense.HowMuchReturn);

                settledExpense.HowMuchReturn = 0;
                settledExpense.IsSettled = true;

                db.Expenses.Update(settledExpense);
                db.SaveChanges();

                MessageBox.Show("Expense, settled succesfully");

                loadDataE();

            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gridExpensesPage.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an expense");
            }
            else
            {
                Expense deleteExpense = (Expense)(gridExpensesPage.SelectedItem);

                updateBalance(deleteExpense.AccountId, deleteExpense.Amount);

                db.Expenses.Remove(deleteExpense);
                db.SaveChanges();

                MessageBox.Show("Expense, removed succesfully");

                loadDataE();
            }
        }
    }
}
