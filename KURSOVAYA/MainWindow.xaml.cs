using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

using KURSOVAYA.Objects;

namespace KURSOVAYA {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        ObservableCollection<Pill> pills = new ObservableCollection<Pill>();
        Pill? selectedPill = null;
        public MainWindow() {
            InitializeComponent();
            pills.Add(new Pill("Морфин", "Обезбол", "мл", 9400, 10000, 0001));
            pills.Add(new Pill("Фентанил", "Что-то", "мл", 9400, 11000, 0002));
            pills.Add(new Pill("Ибупрофен", "Обезбол", "упаковка", 10, 1000, 0003));
            Pills.ItemsSource = pills;
        }

        private void mainChoise_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ListBox? listBox = sender as ListBox;
            ListBoxItem? choise = listBox.SelectedItem as ListBoxItem;

            groupBox.Header = choise.Content;
            foreach (Grid grid in groupBoxGrid.Children) {
                grid.Visibility = Visibility.Hidden;
            }
            object wantedNode = groupBoxGrid.FindName("box" + (listBox.SelectedIndex + 1));
            if (wantedNode is Grid) {
                Grid? wantedChild = wantedNode as Grid;
                wantedChild.Visibility = Visibility.Visible;
            }
        }

        private void Pills_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            selectedPill = Pills?.SelectedItem as Pill;
            Debug.WriteLine("Код: " + selectedPill?.Code
                + ", Назва: " + selectedPill?.Name
                + ", Опис: " + selectedPill?.Description
                + ", Одиниця виміру: " + selectedPill?.Unit
                + ", Кількість: " + selectedPill?.Count
                + ", На складі: " + selectedPill?.StorageCount);
        }

        private void AddPill_Click(object sender, RoutedEventArgs e) {
            AddPillWindow addPillWindow = new AddPillWindow(null);
            if (addPillWindow.ShowDialog() == true) {
                pills.Add(new Pill(
                    addPillWindow.Name_,
                    addPillWindow.Desctiption,
                    addPillWindow.Unit,
                    addPillWindow.Count,
                    addPillWindow.Storage,
                    addPillWindow.Code));
            }
        }

        private void DeletePill_Click(object sender, RoutedEventArgs e) {
            if (selectedPill != null) { 
                pills.Remove(selectedPill);
            }
        }

        private void EditPill_Click(object sender, RoutedEventArgs e) {
            if (selectedPill != null) {
                AddPillWindow editPillWindow = new AddPillWindow(selectedPill);
                if (editPillWindow.ShowDialog() == true) {
                    var found = pills.FirstOrDefault(x => x.Code == selectedPill.Code);
                    int i = pills.IndexOf(found);
                    pills[i] = new Pill(
                        editPillWindow.Name_,
                        editPillWindow.Desctiption,
                        editPillWindow.Unit,
                        editPillWindow.Count,
                        editPillWindow.Storage,
                        editPillWindow.Code);
                }
            }
        }
    }
}
