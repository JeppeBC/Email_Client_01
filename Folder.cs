using MailKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{

    // A Class representation of a Folder


    // By implementing the INotifyPropertyChanged interface we can raise and handle events whenver a property 
    // of a datasource is changed (for example changing the listbox name because filter moved a mail to that folder)
    // We want this to automatically get updated. 
    internal class Folder : INotifyPropertyChanged
    {

        private string? _ListBoxName;                                 
        public event PropertyChangedEventHandler? PropertyChanged;
        public string? FullName { get; set; }

        public string? ListBoxName
        {
            get { return _ListBoxName; }
            set                                 // whenver we set the listbox name, call OnPropertyChanged()
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
