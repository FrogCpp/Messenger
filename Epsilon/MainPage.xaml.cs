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
        /*
        public class Contact : JN_Author, INotifyPropertyChanged
        {
            public Contact(string name, string surname, string bio, ulong suid, ImageSource avatar) : base(name, surname, bio, suid, avatar)
            {
                this.name = name;
                this.surname = surname;
                this.bio = bio;
                this.suid = suid;
                this.avatar = avatar;
            }

            public string Name
            {
                get => name;
                set
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
            public string Surname
            {
                get => surname;
                set
                {
                    surname = value;
                    OnPropertyChanged();
                }
            }
            public ImageSource Avatar
            {
                get => avatar;
                set
                {
                    avatar = value;
                    OnPropertyChanged();
                }
            }

            private ObservableDictionary<UInt64, JN_Message> _messages = new();

            public ObservableCollection<JN_Message> Messages
            {
                get => _messages;
                set
                {
                    _messages = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public class Message : JN_Message, INotifyPropertyChanged
        {
            public string StringSentTime
            {
                get => SentTime.CurrentToStandardString();
            }
            public Message(DateTime4b sentTime, string message, ulong authorSUID) : base(sentTime, message, authorSUID)
            {
                this.sentTime = sentTime;
                this.message = message;
                this.authorSUID = authorSUID;
            }

            public string Text
            {
                get => message;
                set
                {
                    message = value;
                    OnPropertyChanged();
                }
            }

            public DateTime4b SentTime
            {
                get => sentTime;
                set
                {
                    sentTime.CurrentToStandardString();
                    sentTime = value;
                    OnPropertyChanged();
                }
            }


            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<Contact> Contacts { get; } = new();

        private Contact _actualChat = null;
        public Contact ActualChat
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


        private Contact _user = null;

        public Contact User
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
        */


        // пререписываю логику.

        public MainPage()
        {
            InitializeComponent();
            //PageController.Init(User, Contacts);

            BindingContext = this;
            }
        private void OnItemTapped(object sender, EventArgs e)
        {
            if (sender is Border border && border.GestureRecognizers[0] is TapGestureRecognizer tap)
            {
                if (tap.CommandParameter is Contact contact)
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
