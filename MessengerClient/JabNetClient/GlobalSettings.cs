using System.IO;
using System.Collections.Generic;


using GyroscopicDataLibrary;


namespace JabNetClient
{
    internal class GlobalSettings
    {
        //  Path for storing the custom settings 
        //
        //  Путь для хранения файла с пользовательскими настройками
        static public string gPathForCustomSettings;


        //  Path for the auth key and encryption key (for the auto auth process)
        //
        //  Путь для файла с ключом авторизации и специальным ключом шифрования (для авто авторизации)
        static public string gPathForStoredAuthKey;


        //  Try to auto authorise the user when opening the program
        //  true = try to auto authorise,  false = don't auto authorise
        //  
        //  Настройка отвечает за попытку автоматической авторизации пользователя при запуске программы
        //  true = да, пытайся автоматом авторизировать,     false = нет, не пытайся
        static public bool gAutoAuthorise;


        //  Parameter for the RE key generation
        //  true  = include extra unicode character pack in the key generation
        //  false = don't include
        //
        //  Параметр для случайной генерации надёжноро РЕ ключа
        //  true  = да, включи в генерацию ключа дополнительный набор символов юникода
        //  false = нет, не включай
        static public bool gGenerateExtraUnicode;


        //  Parameter for the enryption process
        //  Resembles the used cipher version
        //  (By default is set to: 4 = RE4 algorithm)
        //
        //  Параметр для шифрования данных
        //  Отвечает за используемую в шифре версию алгоритма
        //  (По стандарту используется: 4 = алгоритм РЕ4)
        static public byte gCipherVersion;

        static public void ApplySettings()
        {
            //  Read data from the chosen file
            //  False = silent (without printing any info)
            //
            //  Прочесть сохранённые настройки из указанного файла
            //  False = сделать это тихо (без какого либо вывода информации))
            List<string> data = BetterDataIO.ReadData(gPathForCustomSettings, "Settings.txt", false);

            if (data != null)
            {
                //  Add logic for parsing the data 
                //  And extracting settings from them
                //
                //  Заметки на будующее:
                //  Добавить логику преобразования прочитанной информации
                //  в настройки, и изменить их соответствующе
                //
                //  Лёш, я сам это сделаю
                //
                //ParseData(data, true, true, "*", "", "*", true);
            }
        }
             //  Apply user settings
             //  Изменить настройки на пользовательские

        static public void ResetSettings()
        {
            //  Set currently used cipher version to RE4
            //  Установить версию испоьзуемого шифра на алгоритм РЕ4
            gCipherVersion = 4;

            //  Enable auto authorisation
            //  Разрешить автоматическую авторизацию
            gAutoAuthorise = true;

            //  Dont include extra unicode character pack to the key generation function
            //  Не включать дополнительный набор символов юникода для генерации ключа шифрования
            gGenerateExtraUnicode = false;

            //  Set path for storing the custom settings
            //  Установливаем путь для хранения пользовательских настроек
            gPathForCustomSettings = Directory.GetCurrentDirectory();

            //  Set path for storing the auth key, and special encryption key
            //
            //  Установливаем путь для хранения специального ключа для автоматического входа
            //  А также специального ключа шифрования для него
            gPathForStoredAuthKey = Directory.GetCurrentDirectory();
        }
             //  Reseting the settings to the default
             //  Сбрасываем настройки до изначальных / системных


        static public void ChangeSettings()
        {
            bool confirmNewSettings = false;

            while (!confirmNewSettings)
            {


                //  Change settings according to user input
                //  Settings changing logic here
            }

            List<string> data = EncodeCurrentSettings();

            //  False1 = overwrite current stored settings file
            //  False2 = perform the saving silent (dont show any info)
            //  
            //  False1 = перезаписывать файл с настройками
            //  False2 = делать это тихо (без какого либо вывода информации)
            BetterDataIO.SaveData(gPathForCustomSettings, "Settings.txt", data, false, "\n", false);
        }
             //  Changing the settings for the user's choice
             //  Изменяем настройки на пользовательские

        static private List<string> EncodeCurrentSettings()
        {
            //  We will be storing the encoded settings data here
            //  Мы будем хранить закодированные настройки здесь
            List<string> encodedData = new List<string>();


            return encodedData;
        }
             //  Encode the current settings by transforming them into a txt file
             //  Закодируем текущие настройки, превратя их в данные для .тхт файла
    }
}