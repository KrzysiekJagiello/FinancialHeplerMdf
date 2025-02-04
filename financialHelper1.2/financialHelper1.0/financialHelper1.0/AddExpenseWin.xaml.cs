using financialHelper1._0.DB;
using Microsoft.Identity.Client.NativeInterop;
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
    /// Interaction logic for AddExpenseWin.xaml
    /// </summary>
    public partial class AddExpenseWin : Window
    {

        private MainWindow _mainWindow;

        FinancesContext db = new FinancesContext();

        public AddExpenseWin(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e) 
        {
            var comboList = db.Balances.ToList().OrderBy(x => x.Id);
            cmbAccount.ItemsSource = comboList;
            cmbAccount.DisplayMemberPath = "AccountName";
            cmbAccount.SelectedValuePath = "ID";
            cmbAccount.SelectedIndex = -1;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            _mainWindow.loadData();
            _mainWindow.Visibility = Visibility.Visible;
        }

        private void savingExpense(Expense newExpense)
        {      
                newExpense.Date = DateOnly.FromDateTime(dateExp.SelectedDate.Value);
                newExpense.Category = txtCategory.Text;
                newExpense.Amount = double.Parse(txtAmount.Text);
                newExpense.AccountId = cmbAccount.SelectedIndex + 1;
                newExpense.HowMuchReturn = int.Parse(txtReturn.Text);
                newExpense.IsSettled = chbSettled.IsChecked ?? false; //if it will be null it is now false
                newExpense.Description = txtDescription.Text;

                db.Expenses.Add(newExpense);
                db.SaveChanges();
        }

        private void changingBalances(Expense newExpense)
        {
            
                int account = cmbAccount.SelectedIndex + 1;
                Balance oldBalance = db.Balances
                                      .Where(x => x.Id == account)
                                      .First();

                Balance newBalace = new Balance();
                newBalace.Id = account;
                newBalace.AccountBalance = oldBalance.AccountBalance - newExpense.Amount;
                newBalace.AccountName = oldBalance.AccountName;

            using (FinancesContext updateContext = new FinancesContext())
            {
                updateContext.Balances.Update(newBalace);
                updateContext.SaveChanges();
            }
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            
            if (dateExp.SelectedDate.HasValue == false || txtCategory.Text.Trim() == "" || !double.TryParse(txtAmount.Text, out _) || cmbAccount.SelectedIndex == -1 || !int.TryParse(txtReturn.Text, out _))
                MessageBox.Show("Please fill areas correctly");
            else
            {
                Expense newExpense = new Expense();

                savingExpense(newExpense);
                
                changingBalances(newExpense);

                
                MessageBox.Show("Expense added succesfuly");
            }
        }
    }
}
