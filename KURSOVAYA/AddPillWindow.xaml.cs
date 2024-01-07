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

using KURSOVAYA.Objects;

namespace KURSOVAYA
{
    /// <summary>
    /// Логика взаимодействия для AddPillWindow.xaml
    /// </summary>
    public partial class AddPillWindow : Window {
        public int Code {
            get { return int.Parse(CodeBox.Text); }
        }
        public string Name_ {
            get { return NameBox.Text; }
        }
        public string Desctiption {
            get { return DescriptionBox.Text; }
        }
        public string Unit {
            get { return UnitBox.Text; }
        }
        public int Count {
            get { return (int)CountBox.Value; }
        }
        public int Storage {
            get { return (int)StorageBox.Value; }
        }
        public AddPillWindow(Pill pill)
        {
            InitializeComponent();

            if(pill != null) {
                CodeBox.Text = pill.Code.ToString();
                NameBox.Text = pill.Name.ToString();
                DescriptionBox.Text = pill.Description.ToString();
                UnitBox.Text = pill.Unit.ToString();
                CountBox.Value = pill.Count;
                StorageBox.Value = pill.StorageCount;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {

        }
    }
}
