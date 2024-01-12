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
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;

using KURSOVAYA.Objects;
using HandyControl.Controls;
using Excel = Microsoft.Office.Interop.Excel;
using static System.Net.Mime.MediaTypeNames;

namespace KURSOVAYA {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        ObservableCollection<Pill> pills = new ObservableCollection<Pill>();
        Pill? selectedPill = null;
        private object m_objOpt = System.Reflection.Missing.Value;
        public MainWindow() {
            InitializeComponent();
            pills.Add(new Pill("Морфин", "Обезбол", "мл", 9400, 10000, "0001"));
            pills.Add(new Pill("Фентанил", "Что-то", "мл", 9400, 11000, "0002"));
            pills.Add(new Pill("Ибупрофен", "Обезбол", "упаковка", 10, 1000, "0003"));
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

        private void import_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = openFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true) {
                pills.Clear();
                XDocument xDocument = XDocument.Load(openFileDialog.FileName);
                XElement root = xDocument?.Element("pills");
                foreach(XElement element in root?.Elements("pill")) {
                    Debug.WriteLine(element?.Attribute("name").Value);
                    pills.Add(new Pill(
                        element.Attribute("name").Value,
                        element.Attribute("desc").Value,
                        element.Attribute("unit").Value,
                        int.Parse(element.Attribute("count").Value),
                        int.Parse(element.Attribute("storage").Value),
                        element.Attribute("code").Value));
                }

            }
        }

        private void export_Click(object sender, RoutedEventArgs e) {
            XDocument xDocument = new XDocument();
            XElement rootElement = new XElement("pills");
            foreach(Pill pill in pills) {
                XElement xElement = new XElement("pill",
                    new XAttribute("name", pill.Name),
                    new XAttribute("desc", pill.Description),
                    new XAttribute("unit", pill.Unit),
                    new XAttribute("count", pill.Count),
                    new XAttribute("storage", pill.StorageCount),
                    new XAttribute("code", pill.Code)
                    );
                rootElement.Add(xElement);
            }
            xDocument.Add(rootElement);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "Data"; // Default file name
            saveFileDialog.DefaultExt = ".xml"; // Default file extension
            saveFileDialog.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true) {
                // Save document
                xDocument.Save(saveFileDialog.FileName);
            }
        }

        private void report_Click(object sender, RoutedEventArgs e) {
            Excel.Application application = new Excel.Application();

            application.Workbooks.Add();
            Excel.Worksheet wsh = application.ActiveSheet;
            for(int i = 0; i < Pills.Columns.Count; i++) {
                wsh.Cells[1, i + 1] = Pills.Columns[i].Header.ToString();

                Excel.Range m_objRange = wsh.get_Range("A1", System.Reflection.Missing.Value);
                m_objRange = m_objRange.get_Resize(1, Pills.Columns.Count);
                Excel.Font f = m_objRange.Font;
                f.Bold = true;
            }

            for (int i = 0; i < Pills.Items.Count; i++) {
                string text = "";
                for (int j = 0; j < Pills.Columns.Count; j++) {
                    var item = Pills.Items[i];
                    var it = Pills.Columns[j].GetCellContent(item) as TextBlock;
                    wsh.Cells[i+2, j + 1] = it.Text;
                }
                //Debug.WriteLine(text);
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "Data"; // Default file name
            saveFileDialog.DefaultExt = ".xlsx"; // Default file extension
            saveFileDialog.Filter = "Excel таблиця (.xls)|*.xlsx"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true) {
                // Save the Workbook and quit Excel.
                application.ActiveWorkbook.SaveAs(saveFileDialog.FileName, m_objOpt, m_objOpt,
                m_objOpt, m_objOpt, m_objOpt, Excel.XlSaveAsAccessMode.xlNoChange,
                m_objOpt, m_objOpt, m_objOpt, m_objOpt);
                application.ActiveWorkbook.Close(false, m_objOpt, m_objOpt);

                application.Quit();
            }
        }
    }
}
