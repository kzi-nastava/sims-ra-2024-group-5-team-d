using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class ShorctutsViewModel
    {
        public ObservableCollection<ShortcutViewModel> Shortcuts { get; set; }
        private ShortcutService shortcutService;
        public ShorctutsViewModel(User user) {
            shortcutService = new ShortcutService();
            Shortcuts = new ObservableCollection<ShortcutViewModel>();
            shortcutService.GetBasicShortCuts().ForEach(sc => Shortcuts.Add(new ShortcutViewModel(sc.Key, sc.Value)));
        }
    }
}
