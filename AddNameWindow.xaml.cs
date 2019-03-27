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
using OOP_Project.Person;
using System.Text.RegularExpressions;

namespace FinalGhoulOOPProject
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddNameWindow : Window
    {
        public AddNameWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void AddButton(object sender, RoutedEventArgs e)
        {

            if (txtNameFirst.Text != "" && txtNameMiddle.Text != "" && txtNameLast.Text != "" & txtAddress.Text != "" && txtContactNumber.Text != "")
            {
                bool exist = false;
                PersonClass GetName = new PersonClass(txtNameFirst.Text, txtNameLast.Text, txtNameMiddle.Text);

                //Checking for duplicates
                foreach (string name in DataStorage.customerList)
                {
                    if (name == GetName.GetFullName())
                    {
                        MessageBox.Show("Error! Name already exists in database!");
                        exist = true;
                    }
                }
                if (!exist)
                {
                    //Reserving essential data slots for the newly added user
                    DataStorage.customerList.Add(GetName.GetFullName());
                    DataStorage.address.Add(txtAddress.Text);
                    DataStorage.contactNumber.Add(txtContactNumber.Text);

                    DataStorage.typeOfJewelry.Add("");
                    DataStorage.qualityOfJewelry.Add("");
                    DataStorage.details.Add("");

                    DataStorage.weightOfJewelry.Add(0);
                    DataStorage.discount.Add(0);
                    DataStorage.actualValue.Add(0);

                    DataStorage.dateOfLastPayment.Add("");
                    DataStorage.dateOfTransaction.Add("");
                    DataStorage.dateOfLastPayment.Add("");

                    DataStorage.dateUpdated.Add("");
                    DataStorage.eightDigitPin.Add(0);
                    DataStorage.karatPrice.Add(0);

                    DataStorage.accumulatedAmount.Add(0);
                    DataStorage.amountLoaned.Add(0);
                    DataStorage.interestRate.Add(0);

                    DataStorage.accountBalance.Add(0);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Please fill up the empty boxes!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Preset(object sender, RoutedEventArgs e)
        {
            txtNameFirst.Text = "Brenn";
            txtNameMiddle.Text = "Ching";
            txtNameLast.Text = "Hong";
            txtContactNumber.Text = "222";
            txtAddress.Text = "Ateneo";
        }

        private void txtContactNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
             Regex regex = new Regex("[^0-9]+");
             e.Handled = regex.IsMatch(e.Text);

        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
