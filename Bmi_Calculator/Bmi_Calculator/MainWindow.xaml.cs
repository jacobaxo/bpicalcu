using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Xml;
using System.Xml.Serialization;

namespace Bmi_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [XmlRoot("Bmi Calculator", Namespace = "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"")]
    public partial class MainWindow : Window
    {
        
        public string FilePath = "C:\\Users\\mays_jacobj\\source\\repos\\Bmi_Calculator\\Bmi_Calculator";
        public string FileName = "yourBMI.xml";

        public class Customer
        {
            [XmlAttribute ("Last Name")]
            public string lastName { get; set; }
            [XmlAttribute("First Name")]
            public string firstName { get; set; }
            [XmlAttribute("Phone Number")]
            public string phoneNumber { get; set; }
            [XmlAttribute("Height")]
            public int heightInches { get; set; }
            [XmlAttribute("Weight")]
            public int weightLbs { get; set; }
            [XmlAttribute("Customer BMI")]
            public int customerBMI { get; set; }
            [XmlAttribute("Status")]
            public string statusTitle { get; set; }

        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            xPhoneBox.Text = "";
            xFirstNameBox.Text = "";
            xLastNameBox.Text = "";
            xHeightInchesBox.Text = "";
            xWeightLbsBox.Text = "";
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            Customer customer1 = new Customer();

            customer1.lastName = xLastNameBox.Text;
            customer1.firstName = xFirstNameBox.Text;
            customer1.phoneNumber = xPhoneBox.Text;

            int currentWeight = 0;
            int currentHeight = 0;
            Int32.TryParse(xWeightLbsBox.Text, out currentWeight);
            Int32.TryParse(xHeightInchesBox.Text, out currentHeight);
            customer1.weightLbs = currentWeight;
            customer1.heightInches = currentHeight;

            int bmi;
            bmi = 703 * customer1.weightLbs / (customer1.heightInches * customer1.heightInches);
            customer1.customerBMI = bmi;

            string yourBMIStatus = "NA";
            customer1.statusTitle = yourBMIStatus;

            MessageBox.Show($"The Customer's name is {customer1.firstName} {customer1.lastName} and they can be reached at {customer1.phoneNumber}. They are {customer1.heightInches} inches tall. Their weight is {customer1.weightLbs}. Their BMI is {bmi}");

            if (bmi < 18.5)
            {
                yourBMIStatus = "According to CDC.gov BMI Calculator\n you are underweight.";
                customer1.statusTitle = "Underweight";
            }
            else if (bmi < 24.9)
            {
                yourBMIStatus = "According to CDC.gov BMI Calculator\n you have a normal body weight.";
                customer1.statusTitle = "Normal";
            }
            else if (bmi < 29.9)
            {
                yourBMIStatus = "According to CDC.gov BMI Calculator\n you are overweight.";
                customer1.statusTitle = "Overweight";
            }
            else
            {
                yourBMIStatus = "According to CDC.gov BMI Calculator\n you are obese.";
                customer1.statusTitle = "Obese";
            }

            xYourBMIResults.Text = customer1.customerBMI.ToString();
            xBMIMessage.Text = yourBMIStatus;

            TextWriter writer = new StreamWriter(FilePath + FileName);
            XmlSerializer ser = new XmlSerializer(typeof(Customer));
            ser.Serialize(writer, customer1);
            writer.Close();

        }


        private void OnLoadStats()
        {
            Customer customer = new Customer();

            XmlSerializer des = new XmlSerializer(typeof(Customer));
            using (XmlReader reader = XmlReader.Create(FilePath + FileName))
            {
                customer = (Customer)des.Deserialize(reader);

                xLastNameBox.Text = customer.lastName;
                xFirstNameBox.Text = customer.firstName;
                xPhoneBox.Text = customer.phoneNumber;
            }

            DataSet xmlData = new DataSet();
            xmlData.ReadXml(FilePath + FileName, XmlReadMode.Auto);
            xDataGrid.ItemsSource = xmlData.Tables[0].DefaultView;
        }


    }
}
