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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SierpinskiAttractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Usage_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult usage = MessageBox.Show("");
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult about = MessageBox.Show("");
        }
    }
}
