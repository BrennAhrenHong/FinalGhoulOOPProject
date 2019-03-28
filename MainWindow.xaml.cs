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

namespace FinalGhoulOOPProject
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Declaring essential variables
        public SettingsWindow settings;
        Payment_Window paymentWindow;
        LoanTransaction addLoanTransaction;
        List<CustomerDetails> itemList;
        public List<int> listItemIndex;

        bool listIndexChecker;
        bool hasSomething;
        bool searchBarState;

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.WindowState = WindowState.Maximized;
            //DebugList();
        }

        //Opens Settings Window
        private void btnOpenPriceSettings(object sender, RoutedEventArgs e)
        {
            //Instantiates Settings Window
            if (settings == null)
            {
                settings = new SettingsWindow();
                settings.main = this;
                settings.Show();
            }
            //Reopens settings if instantiated
            else
            {
                settings.Show();
            }
        }

        //Opens LoanWindow
        private void btnOpenLoanTransaction(object sender, RoutedEventArgs e)
        {
            if (settings != null)
            {
                addLoanTransaction = new LoanTransaction();
                addLoanTransaction.main = this;
                listIndexChecker = false;
                listViewMasterList.SelectedIndex = -1;
                searchBarState = false;
                txtbSearchBar.Text = "";
                addLoanTransaction.ShowDialog();
                listViewMasterList.ItemsSource = null;
                listIndexChecker = true;
                ListDetails();
                listViewMasterList.ItemsSource = itemList;
            }
            else
            {
                MessageBox.Show("Please configure the price settings first!", "Invalid Action", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        //Open Payment Window
        private void btnOpenPayWindow(object sender, RoutedEventArgs e)
        {
            paymentWindow = new Payment_Window();
            paymentWindow.main = this;
            listIndexChecker = false;
            paymentWindow.ShowDialog();
            listViewMasterList.ItemsSource = null;
            ListDetails();
            listViewMasterList.ItemsSource = itemList;
            TransactionDetailsVisibility(-1);
            listIndexChecker = true;
            searchBarState = false;
            txtbSearchBar.Clear();
        }

        //Exit Button
        private void btnExitProgram(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
        }

        //Reveals customer details when clicked from the listview
        private void listViewMasterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listIndexChecker && listViewMasterList.SelectedIndex >= 0 && searchBarState != true)
            {
                //listViewMasterList.SelectedItem
                TransactionDetailsVisibility(listItemIndex[listViewMasterList.SelectedIndex]);
                DataStorage.selectedListViewIndex = listItemIndex[listViewMasterList.SelectedIndex];
            }
            else if(searchBarState && listViewMasterList.SelectedIndex >= 0)
            {
                var selectedIndexWithSearchBar = listViewMasterList.SelectedItems[listViewMasterList.SelectedIndex] as CustomerDetails;
                TransactionDetailsVisibility(listItemIndex[DataStorage.DataIndex(selectedIndexWithSearchBar.Name)]);
                DataStorage.selectedListViewIndex = listItemIndex[DataStorage.DataIndex(selectedIndexWithSearchBar.Name)];
            }
        }


        //Logic to reveal customer details if selected in the List
        private void TransactionDetailsVisibility(int index)
        {
            if (index >= 0)
            {
                gridCustomerDetails.Visibility = Visibility.Visible;
                txtbName.Text = DataStorage.customerList[index];
                txtbAddress.Text = DataStorage.address[index];
                txtbContactNumber.Text = DataStorage.contactNumber[index];
                txtbFirstTransactionDate.Text = DataStorage.dateOfTransaction[index];
                txtbLastTransactionDate.Text = DataStorage.dateOfLastPayment[index];
                txtbAmountLoaned.Text = DataStorage.amountLoaned[index].ToString("#,##0.00");
                txtbInterestRate.Text = Convert.ToString(DataStorage.interestRate[index]);
                txtbAccumulatedAmount.Text = DataStorage.accumulatedAmount[index].ToString("#,##0.00");
                txtbRemainingBalance.Text = DataStorage.accountBalance[index].ToString("#,##0.00");
                txtbTypeOfJewelry.Text = DataStorage.typeOfJewelry[index];
                txtbQualityOfJewelry.Text = DataStorage.qualityOfJewelry[index];
                txtbWeight.Text = Convert.ToString(DataStorage.weightOfJewelry[index]);
                txtbActualValue.Text = DataStorage.actualValue[index].ToString("#,##0.00");
                txtbKaratPrice.Text = DataStorage.karatPrice[index].ToString("#,##0.00");
                txtbDiscount.Text = Convert.ToString(DataStorage.discount[index]);
                rtxtbDetails.Text = Convert.ToString(DataStorage.details[index]);
                btnPay.IsEnabled = true;
            }
            else
            {
                gridCustomerDetails.Visibility = Visibility.Hidden;
                btnPay.IsEnabled = false;
            }
        }



        //Used to add data into the List and update outdated customer balances
        public void ListDetails()
        {
            itemList = new List<CustomerDetails>();
            listItemIndex = new List<int>();
            int maxIndex = DataStorage.customerList.Count;

            //Logic for adding customer into the list and updating outdated accounts
            for (int index = 0; index < maxIndex; index++)
            {
                if (DataStorage.accountBalance[index] != 0)
                {
                    //Temporary variables
                    string day = DateTime.Now.ToString("MM");
                    string month = DateTime.Now.ToString("dd");
                    string year = DateTime.Now.ToString("yyyy");
                    string currentDate = day + "/" + month + "/" + year;

                    int monthsAccrued = AccruedCalculations.MonthsAccrued(DataStorage.dateOfLastPayment[index]);
                    decimal balance = DataStorage.accountBalance[index];
                    decimal interestRate = DataStorage.interestRate[index];
                    decimal accumlatedAmount = DataStorage.accumulatedAmount[index];

                    decimal AccruedAmount = AccruedCalculations.AccruedAmount(interestRate, balance, monthsAccrued);

                    //Updates customer balances if outdated
                    if (DataStorage.dateUpdated[index] != currentDate)
                    {
                        DataStorage.accountBalance[index] = balance + AccruedAmount;
                        DataStorage.accumulatedAmount[index] = accumlatedAmount + AccruedAmount;
                        DataStorage.dateUpdated[index] = currentDate;
                    }

                    //Adds basic customer data to list
                    itemList.Add(new CustomerDetails()
                    {
                        ID = DataStorage.eightDigitPin[index],
                        Name = DataStorage.customerList[index],
                        TransactionDate = DataStorage.dateOfTransaction[index],
                        AmountLoaned = DataStorage.amountLoaned[index].ToString("#,##0.00"),
                        InterestRate = DataStorage.interestRate[index].ToString() + "%",
                        Balance = DataStorage.accountBalance[index].ToString("#,##0.00")
                    });

                    listItemIndex.Add(index);
                }
            }
        }

        //Logic for Search Bar //HASSOMETHING = TRUE MUST HAVE SEPARATE LOGIC FOR CHECKING INDEX
        private void txtbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchBarState = false;
            if (txtbSearchBar.Text != "")
            {
                searchBarState = true;
            }
            if (DataStorage.accountBalance != null)
            {
                foreach (decimal account in DataStorage.accountBalance)
                {
                    if (account != 0)
                    {
                        hasSomething = true;
                        break;
                    }
                }
            }
            if (listViewMasterList.HasItems != false)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listViewMasterList.ItemsSource);
                view.Filter = nameFilter;

                CollectionViewSource.GetDefaultView(listViewMasterList.ItemsSource).Refresh();
            }
            else if (hasSomething)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listViewMasterList.ItemsSource);
                view.Filter = nameFilter;

                CollectionViewSource.GetDefaultView(listViewMasterList.ItemsSource).Refresh();
            }
        }

        //Name Filter Logic
        private bool nameFilter(object name)
        {
            if (String.IsNullOrEmpty(txtbSearchBar.Text))
                return true;
            else
            {
               return ((name as CustomerDetails).Name.IndexOf(txtbSearchBar.Text, StringComparison.OrdinalIgnoreCase) >= 0);     
            }
        }

        //Adding Sample Items to the List
        public void DebugList()
        {
            DataStorage.customerList.Add("Arisa");
            DataStorage.address.Add("Home");
            DataStorage.contactNumber.Add("09329580029");
            DataStorage.dateOfTransaction.Add("3/17/2019");
            DataStorage.typeOfJewelry.Add("Rings");
            DataStorage.qualityOfJewelry.Add("10k");
            DataStorage.dateOfTransaction.Add("2/12/2019");
            DataStorage.dateOfLastPayment.Add("3/12/2019");
            DataStorage.dateUpdated.Add("5/13/19");
            DataStorage.details.Add("qwerty");
            DataStorage.discount.Add(2);
            DataStorage.weightOfJewelry.Add(5000);
            DataStorage.actualValue.Add(25000);
            DataStorage.accumulatedAmount.Add(3000);
            DataStorage.karatPrice.Add(2500);
            DataStorage.amountLoaned.Add(1000);
            DataStorage.interestRate.Add(5);
            DataStorage.accountBalance.Add(500);
            DataStorage.dateUpdated.Add("3/18/2019");
            DataStorage.eightDigitPin.Add(000000001);

            DataStorage.customerList.Add("Ben");
            DataStorage.dateOfTransaction.Add("2/13/2015");
            DataStorage.amountLoaned.Add(1000);
            DataStorage.interestRate.Add(10);
            DataStorage.accountBalance.Add(500);
            DataStorage.dateUpdated.Add("3/23/2019");
            DataStorage.address.Add("Home");
            DataStorage.contactNumber.Add("09329580029");
            DataStorage.typeOfJewelry.Add("Rings");
            DataStorage.qualityOfJewelry.Add("10k");
            DataStorage.dateOfTransaction.Add("2/12/2019");
            DataStorage.dateOfLastPayment.Add("4/03/2019");
            DataStorage.dateUpdated.Add("5/13/19");
            DataStorage.details.Add("qwerty");
            DataStorage.discount.Add(2);
            DataStorage.weightOfJewelry.Add(5000);
            DataStorage.actualValue.Add(25000);
            DataStorage.accumulatedAmount.Add(1500);
            DataStorage.karatPrice.Add(2500);
            DataStorage.eightDigitPin.Add(000000002);

            DataStorage.customerList.Add("AhBrenn");
            DataStorage.dateOfTransaction.Add("3/13/2017");
            DataStorage.amountLoaned.Add(3000);
            DataStorage.interestRate.Add(20);
            DataStorage.accountBalance.Add(2000);
            DataStorage.dateUpdated.Add("3/28/2019");
            DataStorage.address.Add("Home");
            DataStorage.contactNumber.Add("09329580029");
            DataStorage.typeOfJewelry.Add("Rings");
            DataStorage.qualityOfJewelry.Add("10k");
            DataStorage.dateOfTransaction.Add("2/12/2019");
            DataStorage.dateOfLastPayment.Add("4/03/2019");
            DataStorage.dateUpdated.Add("5/13/19");
            DataStorage.details.Add("qwerty");
            DataStorage.discount.Add(2);
            DataStorage.weightOfJewelry.Add(5000);
            DataStorage.actualValue.Add(25000);
            DataStorage.accumulatedAmount.Add(3500);
            DataStorage.karatPrice.Add(2500);
            DataStorage.eightDigitPin.Add(000000002);

            List<CustomerDetails> itemList = new List<CustomerDetails>();
            ListDetails();
            //listViewMasterList.ItemsSource = itemList;
        }
    }


    //Used for inserting items into the masterlist
    public class CustomerDetails
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string TransactionDate { get; set; }
        public string AmountLoaned { get; set; }
        public string InterestRate { get; set; }
        public string Balance { get; set; }
    }

    //Class for storing data
    public static class DataStorage
    {
        //Used to get the index in Loan Transaction
        public static int DataIndex(string input)
        {
            int counter = 0;
            foreach (string name in customerList)
            {
                if (name == input)
                {

                    break;
                }
                counter++;
            }
            return counter;
        }

        //Other Essential data
        public static decimal[] priceArray = new decimal[3];
        public static int selectedListViewIndex;

        //String type data list
        public static List<string> address = new List<string>();
        public static List<string> customerList = new List<string>();
        public static List<string> contactNumber = new List<string>();
        public static List<string> dateOfTransaction = new List<string>();
        public static List<string> dateOfLastPayment = new List<string>();
        public static List<string> dateUpdated = new List<string>();
        public static List<string> details = new List<string>();
        public static List<string> qualityOfJewelry = new List<string>();
        public static List<string> typeOfJewelry = new List<string>();
        public static List<string> transactionHistoryList = new List<string>();

        //Int type data list
        public static List<int> eightDigitPin = new List<int>();

        //Decimal type data list
        public static List<decimal> actualValue = new List<decimal>();
        public static List<decimal> accountBalance = new List<decimal>();
        public static List<decimal> accumulatedAmount = new List<decimal>();
        public static List<decimal> amountLoaned = new List<decimal>();
        public static List<decimal> discount = new List<decimal>();
        public static List<decimal> interestRate = new List<decimal>();
        public static List<decimal> karatPrice = new List<decimal>();
        public static List<decimal> weightOfJewelry = new List<decimal>();
    }
}
