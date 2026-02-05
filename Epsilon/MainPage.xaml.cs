using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Shared.Source;
using DateTime4bLib;
using Observables.Specialized.Extensions;

namespace Epsilon
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        internal ObservableCollection<Chat> Chats { get; } = new();

        private Chat _actualChat = null;
        internal Chat ActualChat
        {
            get => _actualChat;
            set
            {
                if (_actualChat != value)
                {
                    _actualChat = value;
                    OnPropertyChanged(nameof(ActualChat));
                }
            }
        }


        private Accaunt _user = null;

        internal Accaunt User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainPage()
        {
            InitializeComponent();
            PageController.Init(User, Chats);

            BindingContext = this;
            }
        private void OnItemTapped(object sender, EventArgs e)
        {
            if (sender is Border border && border.GestureRecognizers[0] is TapGestureRecognizer tap)
            {
                if (tap.CommandParameter is Accaunt contact)
                {
                    
                }
            }
        }
        private void SubmitText(object sender, EventArgs e)
        {
            /*
            if (ActualChat == null) return;

            Console.WriteLine(MessageInputField.Text);
            ActualChat.Messages.Add(new Message(DateTime4b.Now, MessageInputField.Text, 12));
            MessageInputField.Text = string.Empty;
            */
        }
    }
}
