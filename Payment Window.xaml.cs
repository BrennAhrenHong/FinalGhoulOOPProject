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
using System.Text.RegularExpressions;

namespace FinalGhoulOOPProject
{
    /// <summary>
    /// Interaction logic for Payment_Window.xaml
    /// </summary>
    public partial class Payment_Window : Window
    {
        //Declarations
        public MainWindow main;
        int index;
        decimal remainingBalance;

        public Payment_Window()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            index = DataStorage.selectedListViewIndex;
            remainingBalance = DataStorage.accountBalance[index];
            txtremainingBalance.Text = "Php " + remainingBalance.ToString("#,##0.00");
        }


        private void Enter(object sender, RoutedEventArgs e)
        {
            decimal paymentInput = Convert.ToDecimal(txtbPayment.Text);
            decimal accountBalance = DataStorage.accountBalance[index];
            string day = DateTime.Now.ToString("MM");
            string month = DateTime.Now.ToString("dd");
            string year = DateTime.Now.ToString("yyyy");
            string currentDate = day + "/" + month + "/" + year;

            if (paymentInput <= accountBalance && paymentInput > 0)
            {
                DataStorage.accountBalance[index] = DataStorage.accountBalance[index] - Convert.ToDecimal(txtbPayment.Text);
                DataStorage.dateOfLastPayment[index] = currentDate;
                MessageBox.Show("Payment Successful!");
                this.Close();
            }
            else if(paymentInput < 0)
            {
                MessageBox.Show("Invalid Input. Input Must be higher than 0", "Invalid Action", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else
            {
                MessageBox.Show("Invalid Input. Input must not exceed the remaining balance.", "Invalid Action", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtbPayment_TextInputDisabler(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^.0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
