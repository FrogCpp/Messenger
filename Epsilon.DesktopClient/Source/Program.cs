using Avalonia;
using System;



namespace Epsilon.DesktopClient.Source
{
    class Program
    {
        // Главная точка входа в приложение
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Настройка Avalonia
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()  // Автоматическое определение платформы
                .WithInterFont()      // Используем Inter шрифт
                .LogToTrace();        // Логирование
    }
}