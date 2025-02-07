using System.Diagnostics;
using System.Windows;

namespace LabWork16
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadProcesses();
            LoadApplications();
        }

        public class ProcessInfo
        {
            public string ProcessName { get; set; }
            public int Id { get; set; }
            public string MemoryUsage { get; set; }
        }

        public class ApplicationInfo
        {
            public string Title { get; set; }
            public string StartTime { get; set; }
        }

        private void LoadProcesses()
        {
            ProcessesListView.Items.Clear();
            var processes = Process.GetProcesses()
                .Select(p => new ProcessInfo
                {
                    ProcessName = p.ProcessName,
                    Id = p.Id,
                    MemoryUsage = $"{p.WorkingSet64 / 1024 / 1024} MB"
                }).ToList();

            foreach (var process in processes)
            {
                ProcessesListView.Items.Add(process);
            }

            StatusTextBlock.Text = $"Процессов: {ProcessesListView.Items.Count}";
        }

        private void LoadApplications()
        {
            ApplicationsListView.Items.Clear();
            var applications = Process.GetProcesses()
                .Where(p => !string.IsNullOrWhiteSpace(p.MainWindowTitle))
                .Select(p => new ApplicationInfo
                {
                    Title = p.MainWindowTitle,
                    StartTime = p.StartTime.ToString()
                }).ToList();

            foreach (var app in applications)
            {
                ApplicationsListView.Items.Add(app);
            }
        }

        private void RefreshProcesses_Click(object sender, RoutedEventArgs e)
        {
            LoadProcesses();
            LoadApplications();
        }

        private void EndTask_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessesListView.SelectedItem is ProcessInfo selectedProcess)
            {
                try
                {
                    Process process = Process.GetProcessById(selectedProcess.Id);
                    process.Kill();
                    process.WaitForExit(); 
                    LoadProcesses(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка завершения процесса: {ex.Message}");
                }
            }
        }

        private void KillProcessTree_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessesListView.SelectedItem is ProcessInfo selectedProcess)
            {
                try
                {
                    Process process = Process.GetProcessById(selectedProcess.Id);
                    process.Kill(true); 
                    process.WaitForExit();
                    LoadProcesses();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка завершения дерева процессов: {ex.Message}");
                }
            }
        }

        private void ShowNewTaskPanel(object sender, RoutedEventArgs e)
        {
            var newTaskWindow = new NewTaskWindow();
            if (newTaskWindow.ShowDialog() == true) 
            {
                try
                {

                    string processName = newTaskWindow.TaskName;
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = processName,
                        UseShellExecute = true
                    };
                    Process.Start(startInfo); 
                    LoadProcesses(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка запуска задачи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
