using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class ShortcutService
    {
        public List<KeyValuePair<string, string>> GetBasicShortCuts()
        {

            return  new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Home", "H"),
                new KeyValuePair<string, string>("Register", "R"),
                new KeyValuePair<string, string>("Notifications", "N"),
                new KeyValuePair<string, string>("Profile", "P"),
                new KeyValuePair<string, string>("Shortcuts", "I"),
                new KeyValuePair<string, string>("Requests", "Q"),
                new KeyValuePair<string, string>("Renovations", "E"),
                new KeyValuePair<string, string>("Reviews", "V"),
                new KeyValuePair<string, string>("Forum", "F")
            };
        }
    }
}
