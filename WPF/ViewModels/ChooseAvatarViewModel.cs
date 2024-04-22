using BookingApp.WPF.Commands;
using BookingApp.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels
{
    public class ChooseAvatarViewModel
    {
        public ICommand AvatarClickedCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ObservableCollection<string> AvatarPaths { get; set; }
        private string selectedAvatarPath;
        public ChooseAvatarViewModel()
        {
            AvatarPaths = new ObservableCollection<string>();
            AvatarClickedCommand = new RelayParameterCommand(AvatarClicked);
            NextCommand = new RelayCommand(Next);
            ShowAvatars();
        }
        private void ShowAvatars()
        {
            string folderPath = @"../../../Resources/Avatars/";
            if (Directory.Exists(folderPath))
            {
                string[] filePaths = Directory.GetFiles(folderPath);
                foreach (string filePath in filePaths)
                {
                    AvatarPaths.Add(filePath);
                }
            }
        }
        private void AvatarClicked(object obj)
        {
            if (obj is string avatarPath)
            {
                Debug.WriteLine(avatarPath);
                selectedAvatarPath = avatarPath;
            }
        }
        private void Next()
        {
            if (selectedAvatarPath != null)
            {
                 LoginScreen.contentControl.Content = new CreateAnAccountUserControl(selectedAvatarPath);
            }
        }
    }
}
