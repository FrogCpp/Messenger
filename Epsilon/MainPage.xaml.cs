using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Epsilon
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public class Contact : INotifyPropertyChanged
        {
            private string _name = "nameless";
            private ImageSource _avatar;

            public string Name
            {
                get => _name;
                set
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
            public ImageSource Avatar
            {
                get => _avatar;
                set
                {
                    _avatar = value;
                    OnPropertyChanged();
                }
            }

            private ObservableCollection<Message> _messages = new();
            public ObservableCollection<Message> Messages
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
        public class Message : INotifyPropertyChanged
        {

            private string _text;



            public string Text
            {
                get => _text;
                set
                {
                    _text = value;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainPage()
        {
            InitializeComponent();

            BindingContext = this;

            // потом убить ->

            Contacts.Add(new Contact() { Name = "aboba", Avatar = ImageSource.FromFile("C:\\Users\\suzi\\Pictures\\Avatars\\photo_2026-01-17_23-19-06.jpg") });
            Contacts.Add(new Contact() { Name = "aboba1", Avatar = ImageSource.FromFile("C:\\Users\\suzi\\Pictures\\Avatars\\photo_2026-01-17_23-19-08.jpg") });
            Contacts.Add(new Contact() { Name = "aboba2", Avatar = ImageSource.FromFile("C:\\Users\\suzi\\Pictures\\Avatars\\photo_2026-01-17_23-19-05.jpg") });

            Contacts[0].Messages.Add(new Message() { Text = "привет!" });
            Contacts[0].Messages.Add(new Message() { Text = "пашел нахуй!" });
            Contacts[0].Messages.Add(new Message() { Text = "и тебе не хворать" });
            Contacts[0].Messages.Add(new Message() { Text = "О, сарказм. Ценю. Извини, просто день говно. Только что отбил бампер у своего самосвала, пока объезжал идиота на легковушке, который пялился в телефон на кольцевой. Весь рейс насмарку." });
            Contacts[0].Messages.Add(new Message() { Text = "Боже, да это же просто жуть какая-то! Нет, я серьезно, я представляю этот стресс! Ты представляешь, сколько теперь бумажной волокиты? Страховка, акт, возможно, оценка ущерба, простой транспортного средства, срыв графика поставок... Это же цепная реакция! Кстати, на каком именно грузовике работаешь? У нас в семье дядя дальнобойщиком был, так я с детства в Scania, MAN и Volvo чуть ли не разбиралась. Он мне все эти истории рассказывал про рейсы в Уфу или в Питер, как ночью на заправках кофе пьют, как видят самые невероятные рассветы над трассой, а иногда и самые глупые ДТП, когда кто-то пытается втиснуться между двумя фурами, не понимая физики их тормозного пути. Кстати, о физике... помнится, он объяснял, почему нельзя cut-off делать перед фурой. Это ж не просто «ой, простите». Масса-то какая! Инерция! Представь себе стального зверя весом под 40 тонн, мчащегося со скоростью 90 км/ч. Остановить его – не то что легковушку. Это тебе не урок в школе, это реальная, грубая физика на асфальте. 🚛💨☕" });
            Contacts[0].Messages.Add(new Message() { Text = "Ты что, энциклопедия на колесах? Работаю на Volvo FH, седельный. И да, про инерцию я тебе лучше любого учебника расскажу, особенно когда гололед. Но сегодня не про это. После этого инцидента поехал в техцентр, а там очередь. Сижу, жду, и тут начинаю думать... а вообще, какая сложная и на самом деле красивая эта вся система грузоперевозок. Ну вот смотри: стоит где-нибудь в Подмосковье завод, которому нужны детали из Германии. Их грузят в контейнер, контейнер – на шасси, шасси – на паром, паром – через Балтику, потом снова на шасси, и вот он, мой Volvo, уже тянет этот контейнер по МКАДу. Это же как кровеносная система экономики! Артерии – магистрали, капилляры – разъезды по городам. И мы, водители, как эритроциты, тащим кислород-грузы к клеткам-заводам. Понимаешь? А все из-за какого-то клоуна в хэтчбеке, который смотрел тиктоки. Вся эта грандиозная схема дала сбой в одном звене. И ладно бы я один, таких звеньев – тысячи на дороге в любой момент. И каждое – это человек, металл, солярка, законы, погода, эмоции. Забавно, когда об этом думаешь с высоты. Ну или с высоты водительского кресла, пока стоишь в очереди на покраску бампера." });
        }
        private void OnItemTapped(object sender, EventArgs e)
        {
            if (sender is Border border && border.GestureRecognizers[0] is TapGestureRecognizer tap)
            {
                if (tap.CommandParameter is Contact contact)
                {
                    ActualChat = contact;
                }
            }
        }
        private void SubmitText(object sender, EventArgs e)
        {
            if (ActualChat == null) return;

            Console.WriteLine(MessageInputField.Text);
            ActualChat.Messages.Add(new Message() { Text = MessageInputField.Text });
            MessageInputField.Text = string.Empty;
        }
    }
}
