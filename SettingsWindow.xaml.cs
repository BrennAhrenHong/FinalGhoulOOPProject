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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public MainWindow main;


        public SettingsWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }


        //Data Saving and Selection Logic
        private void Button_Save(object sender, RoutedEventArgs e)
        {

            switch (selectedCarat.Text)
            {
                case "10k":
                    if (priceGram.Text == "")
                        priceGram.Text = "0";
                    DataStorage.priceArray[0] = Convert.ToDecimal(priceGram.Text);
                    txtTenK.Text = priceGram.Text;
                    break;

                case "18k":
                    if (priceGram.Text == "")
                        priceGram.Text = "0";
                    DataStorage.priceArray[1] = Convert.ToDecimal(priceGram.Text);
                    txtEightteenK.Text = priceGram.Text;
                    break;

                case "21k":
                    if (priceGram.Text == "")
                        priceGram.Text = "0";
                    DataStorage.priceArray[2] = Convert.ToDecimal(priceGram.Text);
                    txtTwentyoneK.Text = priceGram.Text;
                    break;

                default:
                    break;
            }
            selectedCarat.Text = null;
            priceGram.Text = null;
        }

        //Cancel button
        private void btnCancel(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
