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
    /// Interaction logic for LoanTransaction.xaml
    /// </summary>
    public partial class LoanTransaction : Window
    {
        public MainWindow main;
        decimal initialValue;

        public LoanTransaction()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            foreach (string item in DataStorage.customerList)
            {
                cmbNameList.Items.Add(item);
            }
        }

        private void CalculationLogic()
        {
            decimal weight;
            decimal discount;           

            switch (cmbJewelryQuality.Text)
            {
                case "10k":
                    if (txtbWeight.Text == "")
                        weight = 0;

                    else
                        weight = Convert.ToDecimal(txtbWeight.Text);

                    if (txtbDiscount.Text == "")
                        discount = 0;

                    else
                        discount = Convert.ToDecimal(txtbDiscount.Text);
                    initialValue = DataStorage.priceArray[0] * (weight - discount);
                    txtblockActualValue.Text = "Php " + initialValue.ToString("#,##0.00");
                    if (discount > weight)
                        txtblockActualValue.Text = "Php 0.00";
                    break;

                case "18k":
                    if (txtbWeight.Text == "")
                        weight = 0;

                    else
                        weight = Convert.ToDecimal(txtbWeight.Text);

                    if (txtbDiscount.Text == "")
                        discount = 0;

                    else
                        discount = Convert.ToDecimal(txtbDiscount.Text);

                    initialValue = DataStorage.priceArray[1] * (weight - discount);
                    if (discount > weight)
                        txtblockActualValue.Text = "Php 0.00";
                    break;

                case "21k":
                    if (txtbWeight.Text == "")
                        weight = 0;

                    else
                        weight = Convert.ToDecimal(txtbWeight.Text);

                    if (txtbDiscount.Text == "")
                        discount = 0;
                    else
                        discount = Convert.ToDecimal(txtbDiscount.Text);

                    initialValue = DataStorage.priceArray[2] * (weight - discount);
                    txtblockActualValue.Text = "Php " + initialValue.ToString("#,##0.00");
                    if (discount > weight)
                        txtblockActualValue.Text = "Php 0.00";
                    break;

                default:
                        txtblockActualValue.Text = "Php 0.00";
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void txtbWeight_TextInputDisabler(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtbDiscount_TextInputDisabler(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtbAmountLoaned_TextInputDisabler(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtbInterestRate_TextInputDisabler(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void WeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculationLogic();
        }

        private void txtDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculationLogic();
        }

        private void cmbJewelryQuality_DropDownClosed(object sender, EventArgs e)
        {
            CalculationLogic();
            switch (cmbJewelryQuality.Text)
            {
                case "10k":
                    txtbKaratPrice.Text = DataStorage.priceArray[0].ToString("#,##0.00");
                    break;

                case "18k":
                    txtbKaratPrice.Text = DataStorage.priceArray[1].ToString("#,##0.00");
                    break;

                case "21k":
                    txtbKaratPrice.Text = DataStorage.priceArray[2].ToString("#,##0.00");
                    break;

                default:
                    break;
            }
        }

        private void btnAddName_Click(object sender, RoutedEventArgs e)
        {
            AddNameWindow openAddNameWindow = new AddNameWindow();
            openAddNameWindow.ShowDialog();
            cmbNameList.Items.Clear();
            foreach (string item in DataStorage.customerList)
            {
                cmbNameList.Items.Add(item);
            }
        }



        private void btnAddTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (cmbNameList.Text != "" && cmbTypeOfJewelry.Text != "" && cmbJewelryQuality.Text != "" && txtbWeight.Text != "" && txtbDiscount.Text != "" && txtbAmountLoaned.Text != "" && txtbInterestRate.Text != "" && rtbDetails.Text != "")
            {
                int index = DataStorage.DataIndex(cmbNameList.Text);
                decimal actualValue = Convert.ToDecimal(initialValue);
                decimal amountLoaned = Convert.ToDecimal(txtbAmountLoaned.Text);

                string day = DateTime.Now.ToString("MM");
                string month = DateTime.Now.ToString("dd");
                string year = DateTime.Now.ToString("yyyy");
                string currentDate = day + "/" + month + "/" + year;

                if (DataStorage.accountBalance[index] == 0 && Convert.ToDecimal(txtbAmountLoaned.Text) <= Convert.ToDecimal(initialValue) && Convert.ToDecimal(initialValue) > 0 && Convert.ToDecimal(txtbInterestRate.Text) > 0)
                {

                    //Data storing
                    DataStorage.typeOfJewelry[index] = cmbTypeOfJewelry.Text;
                    DataStorage.qualityOfJewelry[index] = cmbJewelryQuality.Text;
                    DataStorage.weightOfJewelry[index] = Convert.ToDecimal(txtbWeight.Text);

                    DataStorage.discount[index] = Convert.ToInt32(txtbDiscount.Text);
                    DataStorage.actualValue[index] = Convert.ToDecimal(initialValue);
                    DataStorage.dateOfTransaction[index] = currentDate;

                    DataStorage.amountLoaned[index] = Convert.ToDecimal(txtbAmountLoaned.Text);
                    DataStorage.interestRate[index] = Convert.ToDecimal(txtbInterestRate.Text);
                    DataStorage.accumulatedAmount[index] = Convert.ToDecimal(txtbAmountLoaned.Text);

                    DataStorage.dateOfLastPayment[index] = currentDate;
                    DataStorage.details[index] = rtbDetails.Text;
                    DataStorage.dateUpdated[index] = currentDate;

                    DataStorage.amountLoaned[index] = Convert.ToDecimal(txtbAmountLoaned.Text);
                    DataStorage.accountBalance[index] = Convert.ToDecimal(txtbAmountLoaned.Text);
                    DataStorage.karatPrice[index] = Convert.ToDecimal(txtbKaratPrice.Text);
                    //Generating of unique pin
                    Random pin = new Random();

                    int codeListSize = DataStorage.eightDigitPin.Count - 1;
                    int counter = 0;
                    int randomPin = pin.Next(00000000, 100000000);

                    bool checking = true;

                    //Duplicate Checker
                    while (checking && DataStorage.eightDigitPin.Count != 0)
                    {
                        foreach (int codes in DataStorage.eightDigitPin)
                        {
                            if (randomPin == codes)
                            {
                                randomPin = pin.Next(00000000, 100000000);
                                break;
                            }
                            counter++;
                            if (codeListSize < counter)
                            {
                                checking = false;
                                break;
                            }
                        }
                    }

                    //Adding Pin to Storage
                    DataStorage.eightDigitPin[index] = randomPin;

                    this.Close();

                    //index = main
                    main.Show();

                }
                else if (DataStorage.accountBalance[index] > 0)
                {
                    MessageBox.Show("Error! Customer still has " + "Php " + DataStorage.accountBalance[index] + " remaining in his account.", "Invalid Action", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {
                    MessageBox.Show("Error! Some fields have incorrect values or are blank.", "Invalid Action", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            else
            {
                MessageBox.Show("Error! Some fields have incorrect values or are blank.", "Invalid Action", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }


        private void cancelButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
