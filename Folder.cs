using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    internal class Folder : INotifyPropertyChanged
    {

        private string? _ListBoxName;
        public event PropertyChangedEventHandler? PropertyChanged;
        public string? FullName { get; set; }

        public string? ListBoxName
        {
            get { return _ListBoxName; }
            set
            {
                _ListBoxName = value;
                OnPropertyChanged("LB name");
            }
        }


        protected void OnPropertyChanged([CallerMemberName] string arg = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(arg));
        }
    }
}
