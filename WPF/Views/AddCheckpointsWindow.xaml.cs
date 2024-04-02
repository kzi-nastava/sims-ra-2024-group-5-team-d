using BookingApp.Domain.Models;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BookingApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for AddCheckpointsWindow.xaml
    /// </summary>
    public partial class AddCheckpointsWindow : Window
    {
        private List<TextBox> textBoxes = new List<TextBox>(); // List to store dynamically added TextBoxes
        private CheckPointRepository _repository;
        private TourRepository _tourRepository;
        public AddCheckpointsWindow(int numberOfCheckpoints)
        {
            InitializeComponent();
            DataContext = this;
            _repository = new CheckPointRepository();
            _tourRepository = new TourRepository();

            // Add textboxes based on the number of checkpoints specified
            AddTextBoxes(numberOfCheckpoints);
        }

        // Event handler for the "Add" button click event
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> checkpointNames = new List<string>();

            // Iterate through all dynamically added TextBoxes and get their text
            foreach (TextBox textBox in textBoxes)
            {
                Debug.WriteLine(textBox.Text);
                checkpointNames.Add(textBox.Text);
            }

            // Save the checkpoint names to the CSV file
            SaveCheckpointsToCSV(checkpointNames);
        }

        // Method to save checkpoint names to the CSV file
        private void SaveCheckpointsToCSV(List<string> checkpointNames)
        {
            // Get the tour ID from somewhere (e.g., pass it as a parameter to the window constructor)
            int tourId = _tourRepository.NextIdForTour(); // Example tour ID

            // Create CheckPoint objects for each checkpoint name
            List<CheckPoint> checkpoints = new List<CheckPoint>();
            foreach (string name in checkpointNames)
            {
                CheckPoint checkpoint = new CheckPoint(name, tourId, false); // Assuming IsChecked is initially false
                Debug.WriteLine(name);
                _repository.Save(checkpoint);
            }

            MessageBox.Show("Checkpoints added successfully.");
            Close();
        }

        // Method to dynamically add TextBoxes for checkpoints
        private void AddTextBoxes(int count)
        {
            for (int i = 0; i < count; i++)
            {
                TextBox textBox = new TextBox();
                textBox.Margin = new Thickness(0, 5, 0, 5);
                textBoxes.Add(textBox); // Add TextBox to the list
                TextBoxPanel.Children.Add(textBox); // Add TextBox to the StackPanel
            }
        }

    }
}

