using financialHelper1._0.DB;
using System.Windows;

namespace financialHelper1._0
{
    /// <summary>
    /// Interaction logic for AddIncomeWin.xaml
    /// </summary>
    public partial class AddIncomeWin : Window
    {
        private MainWindow _mainWindow;
        FinancesContext db = new FinancesContext();

        public AddIncomeWin(MainWindow mainWindow)
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

        private void savingIncome(Income newIncome)
        {
            newIncome.Date = DateOnly.FromDateTime(dateExp.SelectedDate.Value);
            newIncome.Category = txtCategory.Text;
            newIncome.Amount = double.Parse(txtAmount.Text);
            newIncome.AccountId = cmbAccount.SelectedIndex + 1;
            newIncome.Description = txtDescription.Text;

            db.Incomes.Add(newIncome);
            db.SaveChanges();
        }

        private void changingBalancesIncome(Income newIncome)
        {

            int account = cmbAccount.SelectedIndex + 1;
            Balance oldBalance = db.Balances
                                  .Where(x => x.Id == account)
                                  .First();

            Balance newBalace = new Balance();
            newBalace.Id = account;
            newBalace.AccountBalance = oldBalance.AccountBalance + newIncome.Amount;
            newBalace.AccountName = oldBalance.AccountName;

            using (FinancesContext updateContext = new FinancesContext())
            {
                updateContext.Balances.Update(newBalace);
                updateContext.SaveChanges();
            }
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (dateExp.SelectedDate.HasValue == false || txtCategory.Text.Trim() == "" || !double.TryParse(txtAmount.Text, out _) || cmbAccount.SelectedIndex == -1)
                MessageBox.Show("Please fill areas correctly");
            else
            {
                Income newIncome = new Income();

                savingIncome(newIncome);

                changingBalancesIncome(newIncome);


                MessageBox.Show("Income added succesfuly");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            _mainWindow.loadData();
            _mainWindow.Visibility = Visibility.Visible;
        }

        
    }
}
