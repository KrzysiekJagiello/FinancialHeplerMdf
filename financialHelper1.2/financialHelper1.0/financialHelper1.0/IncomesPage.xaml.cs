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
    /// Interaction logic for IncomesPage.xaml
    /// </summary>
    public partial class IncomesPage : Window
    {

        FinancesContext db = new FinancesContext();

        private MainWindow _mainWindow;

        public IncomesPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            loadDataI();
        }

        private void loadDataI()
        {
            List<Income> pIncomeList = db.Incomes.OrderByDescending(x => x.Date).ToList();
            gridIncomesPage.ItemsSource = pIncomeList;
        }

        private void updateBalance(int account, double amount)
        {
            Balance oldBalance = db.Balances
                                  .Where(x => x.Id == account)
                                  .First();

            Balance newBalace = new Balance();

            newBalace.Id = account;
            newBalace.AccountBalance = oldBalance.AccountBalance - amount;
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

        private void btnDeleteI_Click(object sender, RoutedEventArgs e)
        {
            if (gridIncomesPage.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an expense");
            }
            else
            {
                Income deleteIncone = (Income)(gridIncomesPage.SelectedItem);

                updateBalance(deleteIncone.AccountId, deleteIncone.Amount);

                db.Incomes.Remove(deleteIncone);
                db.SaveChanges();

                MessageBox.Show("Income, removed succesfully");

                loadDataI();
            }
        }

        private void btnCategorySymI_Click(object sender, RoutedEventArgs e)
        {
            var summedList = db.Incomes
                                        .GroupBy(e => e.Category)
                                        .Select(s => new
                                        {
                                            Category = s.Key,
                                            Amount = s.Sum(e => e.Amount)
                                        })
                                        .ToList();
            gridIncomesPage.ItemsSource = summedList;

            btnCategorySymI.Visibility = Visibility.Collapsed;
            btnDeleteI.Visibility = Visibility.Collapsed;
            btnShowAllI.Visibility = Visibility.Visible;
        }

        private void btnShowAllI_Click(object sender, RoutedEventArgs e)
        {
            loadDataI();

            btnShowAllI.Visibility= Visibility.Collapsed;
            btnCategorySymI.Visibility = Visibility.Visible;
            btnDeleteI.Visibility = Visibility.Visible;
        }
    }
}
