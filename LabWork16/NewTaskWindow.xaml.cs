using System;
using System.Diagnostics;
using System.Windows;

namespace LabWork16
{
    public partial class NewTaskWindow : Window
    {
        public string TaskName { get; private set; } 

        public NewTaskWindow()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Программы (*.exe)|*.exe",
                Title = "Выберите исполняемый файл"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                TaskNameTextBox.Text = openFileDialog.FileName;
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskNameTextBox.Text))
            {
                TaskName = TaskNameTextBox.Text; 
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Введите корректное имя процесса!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
