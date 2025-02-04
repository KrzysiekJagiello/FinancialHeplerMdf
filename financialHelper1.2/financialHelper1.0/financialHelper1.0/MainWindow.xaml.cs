using financialHelper1._0.DB;
using financialHelper1._0.ViewModels;
using Microsoft.Identity.Client.NativeInterop;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace financialHelper1._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        FinancesContext db = new FinancesContext();

        public void loadBalances()
        {
            double IKOSum = db.Balances
                              .Where(x => x.Id == 1)
                              .Select(x => x.AccountBalance)
                              .First();

            double cashSum = db.Balances
                                  .Where(x => x.Id == 2)
                                  .Select(x => x.AccountBalance)
                                  .First();

            double revolutSum = db.Balances
                                  .Where(x => x.Id == 3)
                                  .Select(x => x.AccountBalance)
                                  .First();

            double totalSum = IKOSum + cashSum + revolutSum;

            txtIKO.Text = IKOSum.ToString();

            txtCash.Text = cashSum.ToString();

            txtRevolut.Text = revolutSum.ToString();

            txtSumTotal.Text = totalSum.ToString();
        }

        double monthExpense(int month, int year)
        {
            return db.Expenses
                     .Where(e => e.Date.Month == month && e.Date.Year == year)
                     .Sum(e => e.Amount);
        }
        double monthIncome(int month, int year)
        {
            return db.Incomes
                     .Where(e => e.Date.Month == month && e.Date.Year == year)
                     .Sum(e => e.Amount);
        }

        public void loadMonths()
        {
            int thisMonth = DateTime.Now.Month;
            int thisYear = DateTime.Now.Year;

            int prevMonth = thisMonth - 1;
            int prevYear = thisYear;

            if(prevMonth == 0)
            {
                prevMonth = 12;
                prevYear = thisYear - 1;
            }

            double thisIncome = monthIncome(thisMonth, thisYear);
            double thisExpense = monthExpense(thisMonth, thisYear);
            double thisSum = monthIncome(thisMonth, thisYear) - monthExpense(thisMonth, thisYear);
            double prevSum = monthIncome(prevMonth, prevYear) - monthExpense(prevMonth, prevYear);

            txtThisMonthIn.Text = "+" + thisIncome.ToString();
            txtThisMonthEx.Text = "-" + thisExpense.ToString();
            txtThisMonthSum.Text = "=" + thisSum.ToString();

            if (thisSum >= 0)
            {
                txtThisMonthSum.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF50F743"));
            }
            else if (thisSum == 0)
            {
                txtThisMonthSum.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEAEAEA"));
            }
            else
            {
                txtThisMonthSum.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF2828"));
            }



            if (prevSum > 0)
            {
                txtPrevMonth.Text = "+" + prevSum.ToString();
                txtPrevMonth.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF50F743"));
            }
            else if(prevSum == 0)
            {
                txtPrevMonth.Text = prevSum.ToString();
                txtPrevMonth.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEAEAEA"));
            }
            else
            {
                txtPrevMonth.Text = prevSum.ToString();
                txtPrevMonth.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF2828"));
            }

        }


        public void loadData()
        {
        
            List<Expense> expensesList = db.Expenses.OrderByDescending(x => x.Date).ToList();

            dGridExpesnses.ItemsSource = expensesList.Take(5);

            List<Income> incomesList = db.Incomes.OrderByDescending(x => x.Date).ToList();
            dGridIncomes.ItemsSource = incomesList.Take(5);

            loadBalances();

            loadMonths();


        
        }

        public MainWindow()
        {
            InitializeComponent();
            
            loadData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private void btnExpenses_Click(object sender, RoutedEventArgs e)
        {
            ExpensesPage1 pageExpenses = new ExpensesPage1(this);
            this.Visibility = Visibility.Collapsed;
            pageExpenses.ShowDialog();

        }

        private void btnIncomes_Click(object sender, RoutedEventArgs e)
        {
            IncomesPage pageIncomes = new IncomesPage(this);
            this.Visibility = Visibility.Collapsed;
            pageIncomes.ShowDialog();
        }

        private void btnAddEnpese_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseWin pageAddE = new AddExpenseWin(this);
            this.Visibility = Visibility.Collapsed;
            pageAddE.ShowDialog();
        }

        private void btnAddIncome_Click(object sender, RoutedEventArgs e)
        {
            AddIncomeWin pageAddI = new AddIncomeWin(this);
            this.Visibility = Visibility.Collapsed;
            pageAddI.ShowDialog();
        }

        private void btnSumTotal_Click(object sender, RoutedEventArgs e)
        {
            allAccounts pageAllAc = new allAccounts(this);
            this.Visibility = Visibility.Collapsed;
            pageAllAc.ShowDialog();
        }

        private void btnIKO_Click(object sender, RoutedEventArgs e)
        {
            IKOAccount iKOAccount = new IKOAccount(this);
            this.Visibility = Visibility.Collapsed;
            iKOAccount.ShowDialog();
        }

        private void btnCash_Click(object sender, RoutedEventArgs e)
        {
            CashAccount cashAccount = new CashAccount(this);
            this.Visibility = Visibility.Collapsed;
            cashAccount.ShowDialog();
        }

        private void btnRevolut_Click(object sender, RoutedEventArgs e)
        {
            RevolutAccount revolutAccount = new RevolutAccount(this);
            this.Visibility = Visibility.Collapsed;
            revolutAccount.ShowDialog();
        }
    }
}