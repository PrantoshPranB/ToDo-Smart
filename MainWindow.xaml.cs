using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace ToDo_Smart
{
    public partial class MainWindow : Window
    {
        private const string CsvFilePath = @".\TodoItems.csv";
        private ObservableCollection<TodoItem> _todoItems;

        public MainWindow()
        {
            InitializeComponent();

            _todoItems = new ObservableCollection<TodoItem>();
            TodoDataGrid.ItemsSource = _todoItems;

            LoadTodoItemsFromCsv();
        }

        private void LoadTodoItemsFromCsv()
        {
            if (!File.Exists(CsvFilePath)) return;

            DateTime today = DateTime.Today;

            foreach (var line in File.ReadLines(CsvFilePath))
            {
                var parts = line.Split(',');
                if (parts.Length == 4)
                {
                    DateTime.TryParse(parts[0], out var date);
                    DateTime.TryParse(parts[1], out var time);
                    var item = parts[2];
                    var status = parts[3];

                    //MessageBox.Show(item);
                    // Only add items with today's date
                    if (date == today)
                    {
                        _todoItems.Add(new TodoItem { Date = date, Time = time, Item = item, Status = status });
                    }
                }
            }
        }

        private void SaveTodoItemsToCsv()
        {
            var lines = _todoItems.Select(todo => $"{todo.Date:yyyy-MM-dd},{todo.Time:HH:mm:ss},{todo.Item},{todo.Status}");
            File.WriteAllLines(CsvFilePath, lines);
        }

        private void OnAddNewItem(string itemText)
        {
            _todoItems.Add(new TodoItem
            {
                Date = DateTime.Today,
                Time = DateTime.Now,
                Item = itemText,
                Status = "TBD"
            });
        }

        private void OnEndOfDay(object sender, RoutedEventArgs e)
        {
            // Filter and save done/not done items if needed
            var doneItems = _todoItems.Where(t => t.Status == "Done").ToList();
            var notDoneItems = _todoItems.Where(t => t.Status == "Not Done").ToList();

            // Clear completed items from today's list
            _todoItems = new ObservableCollection<TodoItem>(_todoItems.Where(t => t.Status == "TBD").ToList());
            TodoDataGrid.ItemsSource = _todoItems;

            SaveTodoItemsToCsv(); // Save the updated list
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            SaveTodoItemsToCsv();
            base.OnClosing(e);
        }
    }
}
