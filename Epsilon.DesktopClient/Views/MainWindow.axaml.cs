using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Threading.Tasks;



namespace Epsilon.DesktopClient.Views
{
    public partial class MainWindow : Window
    {
        private readonly Button _actionButton;
        private bool _isFlashing = false;

        public MainWindow()
        {
            // 1. Настраиваем окно
            Title = "JabNet Epsilon v0.0";
            Width = 800;
            Height = 600;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // 2. Полноэкранный режим
            WindowState = WindowState.Maximized;

            // 3. Создаем кнопку
            _actionButton = new Button
            {
                Content = "Нажми меня!",
                Width = 200,
                Height = 60,
                FontSize = 18,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = new SolidColorBrush(Color.Parse("#007ACC")),
                Foreground = Brushes.White,
                CornerRadius = new CornerRadius(8)
            };

            // 4. Обработка клика
            _actionButton.Click += OnButtonClick;

            // 5. Подписываемся на изменение темы
            PropertyChanged += (sender, args) =>
            {
                if (args.Property == ActualThemeVariantProperty)
                {
                    UpdateBackground();
                }
            };

            // 6. Создаем контейнер для кнопки
            var grid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = Brushes.Transparent
            };

            grid.Children.Add(_actionButton);

            // 7. Устанавливаем контент
            Content = grid;

            // 8. Обновляем фон при старте
            UpdateBackground();
        }

        private void UpdateBackground()
        {
            if (!_isFlashing)
            {
                Background = ActualThemeVariant == ThemeVariant.Dark
                    ? new SolidColorBrush(Color.FromRgb(0, 0, 0))
                    : new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        private async void OnButtonClick(object? sender, EventArgs e)
        {
            if (_isFlashing) return;

            _isFlashing = true;
            var originalBackground = Background;

            try
            {
                // Мигаем 3 раза
                for (int i = 0; i < 3; i++)
                {
                    // Темно-синий
                    Background = new SolidColorBrush(Color.FromRgb(0, 0, 139));
                    await Task.Delay(100);

                    // Возвращаем исходный цвет
                    Background = originalBackground;

                    if (i < 2) // Не ждем после последнего мигания
                        await Task.Delay(50);
                }
            }
            finally
            {
                _isFlashing = false;
                UpdateBackground();
            }
        }
    }
}