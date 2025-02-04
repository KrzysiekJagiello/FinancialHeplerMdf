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
    /// Interaction logic for allAccounts.xaml
    /// </summary>
    /// 



    public partial class allAccounts : Window
    {

        FinancesContext db = new FinancesContext();

        private MainWindow _mainWindow;

        public allAccounts(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            loadData();
        }

        private void loadData()
        {
            List<Transaction> allTransactions = db.Expenses  // TransactionSource: 1 -> income 2-> expense
                                        .Select(e => new Transaction
                                        {
                                            Date = e.Date,
                                            Category = e.Category,
                                            Amount = e.Amount,
                                            AccountId = e.AccountId,
                                            Description = e.Description,
                                            TransactionSource = 2

                                        })
                                        .Concat(db.Incomes
                                                  .Select(i => new Transaction
                                                  {
                                                      Date = i.Date,
                                                      Category = i.Category,
                                                      Amount = i.Amount,
                                                      AccountId = i.AccountId,
                                                      Description = i.Description,
                                                      TransactionSource = 1
                                                      
                                                  }))
                                                  .OrderByDescending(x => x.Date)
                                                  .ToList();

            gridAllEntries.ItemsSource = allTransactions;

        }

        private void gridAllEntries_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var item = e.Row.Item as Transaction;

            if (item != null)
            {
                if (item.TransactionSource == 1)
                {
                    e.Row.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF50F743"));
                }
                else
                {
                    e.Row.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF2828"));
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.loadData();
            _mainWindow.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}
