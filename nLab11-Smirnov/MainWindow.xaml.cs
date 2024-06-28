using System;
using System.IO;
using System.Windows;

namespace RiverDataApp
{
    public partial class MainWindow : Window
    {
        private string filePath = "rivers.dat";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnAddRiverClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = txtRiverName.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Введите наименование реки.");
                    return;
                }

                if (!double.TryParse(txtLength.Text.Trim(), out double length))
                {
                    MessageBox.Show("Введите корректную длину реки.");
                    return;
                }

                if (!double.TryParse(txtAverageDepth.Text.Trim(), out double averageDepth))
                {
                    MessageBox.Show("Введите корректную среднюю глубину реки.");
                    return;
                }

                // Создаем и записываем информацию о реке в бинарный файл
                WriteRiverToFile(name, length, averageDepth);

                // Очищаем поля ввода после успешной записи
                txtRiverName.Clear();
                txtLength.Clear();
                txtAverageDepth.Clear();

                MessageBox.Show("Река успешно добавлена в файл.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void WriteRiverToFile(string name, double length, double averageDepth)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Append)))
            {
                writer.Write(name);
                writer.Write(length);
                writer.Write(averageDepth);
            }
        }

        private void CalculateTotalLengthOfShallowRivers()
        {
            double totalLength = 0;

            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.OpenOrCreate)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    string name = reader.ReadString();
                    double length = reader.ReadDouble();
                    double depth = reader.ReadDouble();

                    if (depth < 50)
                    {
                        totalLength += length;
                    }
                }
            }

            txtResult.Text = $"Общая длина рек с глубиной менее 50 м: {totalLength} км";
        }
    }
}
