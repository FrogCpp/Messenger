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
        private Dictionary<UInt64, JN_Chat> _chats = new();

        public Dictionary<UInt64, JN_Chat> Chats
        {
            get => _chats;
            set
            {
                _chats = value;
                OnPropertyChanged();
            }
        }

        private Chat _actualChat = null;
        public Chat ActualChat
        {
            get => _actualChat;
            set
            {
                if (_actualChat != value)
                {
                    _actualChat = value;
                    OnPropertyChanged();
                }
            }
        }


        private Accaunt _user = null;

        public Accaunt User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged();
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


            // тесты

            User = new Accaunt(
                "frogges",
                "name",
                "nothing",
                12336,
                ImageSource.FromFile("C:\\Users\\suzi\\Pictures\\Avatars\\photo_2026-01-17_23-19-08.jpg")
                );



            PageController.AddChat(new Chat(
                new List<UInt64>() { 12, 13, 14 }, 
                ImageSource.FromFile("C:\\Users\\suzi\\Pictures\\Avatars\\photo_2026-01-17_23-19-05.jpg"),
                "bibabobaba",
                PageController.latest
                ));

            OnParentChanged();

        }
        private void OnItemTapped(object sender, EventArgs e)
        {
            if (sender is Border border && border.GestureRecognizers[0] is TapGestureRecognizer tap)
            {
                if (tap.CommandParameter is Chat contact)
                {
                    ActualChat = contact;
                }
            }
        }
        private void SubmitText(object sender, EventArgs e)
        {
            if (ActualChat == null) return;

            UInt64 msgSUID = 0;

            if (ActualChat.latest > 0) msgSUID = ActualChat.latest + 1;

            PageController.SendMesage(ActualChat, new Message(DateTime4b.Now, MessageInputField.Text, User.suid, msgSUID));
            MessageInputField.Text = string.Empty;
        }
    }
}
