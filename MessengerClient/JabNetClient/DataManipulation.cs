using System;
using System.IO;
using System.Collections.Generic;

using static System.Console;


namespace JabNetClient
{
    internal class DataManipulation
    {

        //-----------------------------  Path related functions  ------------------------------------------//


        static public string GetPath(bool _custom = false, string _subFolder = "\\Gyroscopic\\Unnamed",
            bool _showInfo = false, bool _engLang = true, string _margin = "\t",
            string _startLine = "", string _endLine = "\n")
        {
            //  Storing the final path here
            string _path;

            //  If the path does not exist (Failed to get the users path)
            //  Or we chose to get a system path
            if (!_custom)
            {
                //  Get the path to the Users\user_name_here\Documents folder
                _path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                //  If the directory is missing for some reason
                if (!Directory.Exists(_path))
                {
                    try
                    {
                        //  Try to create the directory
                        Directory.CreateDirectory(_path);

                        //  Show success message (optional)
                        if (_showInfo)
                        {
                            //  Write the newline and margin (optional)
                            Write(_startLine + _margin);

                            //  Write the success message
                            if (_engLang) Write("Successfully created the folder: " + _path);
                            else Write("Успешно создана папка: " + _path);

                            //  Write endline (optional)
                            Write(_endLine);
                        }
                    }
                    catch (Exception e)
                    {
                        //  If an unexpected error happens

                        //  Show error message (optional)
                        if (_showInfo)
                        {
                            //  Write the newline and margin (optional)
                            Write(_startLine + _margin);

                            //  Write the error message
                            if (_engLang)
                            {
                                Write("Error while creating the folder: " + _path + "\n");
                                Write(_margin + "Output error: " + e);
                            }
                            else
                            {
                                Write("Ошибка при создании папки: " + _path + "\n");
                                Write(_margin + "Код ошибки: " + e);
                            }

                            //  Write endline (optional)
                            Write(_endLine);
                        }

                        //  If the documents folder is unavailable
                        //  Try to get the path to the LocalAppData folder
                        _path = GetLocalAppdataPath(_subFolder, _showInfo);

                        //  Return the path to the LocalAppData folder (If succeeded)
                        //  Or null (If the process failed)
                        return _path;
                    }
                }


                //  If the \Documents folder exists
                try
                {
                    //  Create a new folder for the custom data
                    Directory.CreateDirectory(_path + _subFolder);

                    //  Change the path to the newly created folder
                    _path += _subFolder;

                    //  Return the new path
                    return _path;
                }
                catch (Exception e)
                {
                    //  Show error output (optional))
                    if (_showInfo)
                    {
                        //  Write the newline and margin (optional)
                        Write(_startLine + _margin);

                        //  Write the error message
                        if (_engLang)
                        {
                            Write("Error while creating a subfolder at the path: " + _path + "\n");
                            Write(_margin + "Output error: " + e);
                        }
                        else
                        {
                            Write("Ошибка при создании подпапки по пути: " + _path + "\n");
                            Write(_margin + "Код ошибки: " + e);
                        }

                        //  Write endline (optional)
                        Write(_endLine);
                    }

                    //  If the documents folder is unavailable
                    //  Try to get the path to the LocalAppData folder
                    _path = GetLocalAppdataPath(_subFolder, _showInfo);

                    //  Return the path to the LocalAppData folder (If succeeded)
                    //  Or null (If failed)
                    return _path;
                }
            }




            //  Trying to get the custom path from the user 
            else
            {
                //  Ask the user for a path to save the files
                Write(_startLine + _margin);
                if (_engLang) Write("Enter a path for the files to save sample data: ");
                else Write("Введите путь для сохранения файлов с тестовыми данными: ");

                //  Read the user "path", without the spaces in the start, end, and the " characters
                _path = ReadLine().Trim().Replace("\"", "");


                //  Check if the chosen path exists
                if (!Directory.Exists(_path) || !Path.IsPathRooted(_path))
                {
                    if (Path.IsPathRooted(_path))
                    {
                        //  Try create the user chosen path if it is valid
                        try
                        {
                            //  If the user entered a file name to the path
                            if (File.Exists(_path) || _path.Contains("."))
                            {
                                //  Get the directory of the file
                                _path = Path.GetDirectoryName(_path);
                            }


                            //  Try to create it
                            Directory.CreateDirectory(_path);


                            //  Show success message (optional)
                            if (_showInfo)
                            {
                                //  Write the newline and margin (optional)
                                Write(_startLine + _margin);

                                //  Write the success message
                                if (_engLang) Write("Successfully created the folder: " + _path);
                                else Write("Успешно создана папка: " + _path);

                                //  Write endline (optional)
                                Write(_endLine);
                            }


                            //  Return the successfully created user path
                            return _path;
                        }
                        catch (Exception e)
                        {
                            //  If we catch an error happens
                            //  (For example Unauthorized access to the folder)
                            //  Show error message (optional)
                            if (_showInfo)
                            {
                                //  Write the newline and margin (optional)
                                Write(_startLine + _margin);

                                //  Write the error message
                                if (_engLang)
                                {
                                    Write("Error while creating the folder: " + _path + "\n");
                                    Write(_margin + "Output error: " + e);
                                }
                                else
                                {
                                    Write("Ошибка при создании папки: " + _path + "\n");
                                    Write(_margin + "Код ошибки: " + e);
                                }

                                //  Write endline (optional)
                                Write(_endLine);
                            }
                        }
                    }

                    //  If the path doesnt exist
                    //  And we can't create it
                    //  Get the stock path (C:\Users\user_name\Documents\Gyroscopic\Unnamed)
                    //  (Or C:\Users\user_name\AppData\Local\Gyroscopic\Unnamed)
                    _path = GetPath(false, _subFolder, _showInfo);
                }


                //  Return chosen path
                return _path;
            }
        }
        /* Universal function or getting a path
         *     -  User path (custom = true)
         *     -  C:\Users\user_name\Documents
         *     -  C:\Users\user_name\AppData\Local
         *  
         *  ACCEPTS:
         *     -  Adding any subfolders into the selected directory path
         *     -  Default input as stated in the function arguments
         *     -  Error outputing                                     */


        static private string GetLocalAppdataPath(string _subFolder = "\\Gyroscopic\\Unnamed",
            bool _showInfo = false, bool _engLang = true, string _margin = "\t",
            string _startLine = "", string _endLine = "\n")
        {
            //  If we cant get the rights to the documents folder
            try
            {
                //  Get the path to the LocalAppData folder
                string _path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                //  Move our path to the subfolder
                _path += _subFolder;

                //  Try to create the subfolders
                Directory.CreateDirectory(_path);

                //  Show success message (optional)
                if (_showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(_startLine + _margin);

                    //  Write the success message
                    if (_engLang) Write("Successfully created the subfolder: " + _path);
                    else Write("Успешно создана подпапка: " + _path);

                    //  Write end line (optional)
                    Write(_endLine);
                }

                //  Return the path to the LocalAppData folder
                return _path;
            }
            catch (Exception e)
            {
                /*  Worst possible scenario:
                 *  -  Possible failed getting the user path
                 *  
                 *  -  Couldnt get the path to the Documents folder
                 *  -  Or could create it / Get the rights to it
                 *  
                 *  -  Couldnt get the path to the LocalAppData folder
                 *  -  Or could create the subfolders / Get the rights to them
                */

                //  Show error message (optional)
                if (_showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(_startLine + _margin);

                    //  Write the error message
                    //  If we cant get the rights to the LocalAppData folder
                    if (_engLang)
                    {
                        Write("Error while getting the path to the LocalAppData folder\n");
                        Write(_margin + "Output error: " + e);
                    }
                    else
                    {
                        Write("Ошибка при получении пути к папке LocalAppData");
                        Write("\n" + _margin + "Код ошибки: " + e);
                    }

                    //  Write end line (optional)
                    Write(_endLine);
                }

                //  Return null path (fatal error)
                return null;
            }
        }
        //  Getting the path to the LocalAppData folder
        //  Trying to create the selected subfolders there
        //  Return null if the process fails



        //----------------------------  Data manipulation related functions  ---------------------------------//


        static public List<string> ParseData(List<string> _data, bool _removeEmptyLines = false,
            bool _removeSpace = false, string _spaceRemoveException = "",
            string _lineIgnoreKeys = "", string _ignoreTheseChars = "",
            bool _showInfo = false, bool _engLang = true, string _margin = "\t",
            string _startLine = "", string _endLine = "\n")
        {

            //  Storing the parsed data here
            List<string> _parsedData = new List<string>();


            //  Temporary buffer for easier parsing
            //  (stores the mid data before saving it to the final list)
            string _helper;


            //  Flag for the state of the line saving
            //
            //  Tells if we should ignore it because of a special character
            //  Or if we can save it
            bool _ignoreThisLineFlag;


            //  If the data input is correct (it exists)
            if (_data != null)
            {
                for (int i = 0; i < _data.Count; i++)
                {
                    //  Reset line ignoring flag
                    _ignoreThisLineFlag = false;


                    //  Move to the next line
                    _helper = _data[i];


                    //  ----------------------------------------  White space removal from a line logic
                    if (_removeSpace)
                    {
                        //  Flag that contains the state
                        //
                        //  Whether this particular line
                        //  can have the white space removed from it or not
                        bool _canRemoveFlag = true;


                        //  If the current line isnt empty
                        if (_helper.Length > 0)
                        {

                            //  Check for special characters that prohibits white space cleaning
                            for (int j = 0; j < _spaceRemoveException.Length; j++)
                            {
                                // If the first character of the line
                                // is one of the exceptions or space removal
                                if (_helper[0] == _spaceRemoveException[j])
                                {
                                    //  Disable white space removal for this particular line
                                    _canRemoveFlag = false;

                                    //  Exit the loop
                                    j += _spaceRemoveException.Length;
                                }
                            }
                        }

                        //  If this line is not restricted by any exception characters
                        if (_canRemoveFlag)
                        {
                            //  Remove all the white spaces
                            _helper = _helper.Replace(" ", "");
                        }
                    }


                    //  ---------------------------------------------  Special character ignoring logic
                    for (int j = 0; j < _ignoreTheseChars.Length; j++)
                    {
                        //  remove all the selected special characters
                        _helper = _helper.Replace(_ignoreTheseChars[j].ToString(), "");
                    }


                    //  ---------------------------------------------------  If the line is empty logic
                    //  And the command for removing empty ones is true
                    //  -> Set the _ingoreThisLine flag to true
                    //  And ignore this line
                    if (_helper == "" && _removeEmptyLines) _ignoreThisLineFlag = true;


                    //  -----------------------------------------  Special keys for line ignoring logic
                    //  Check for selected special characters in the line start pos
                    //  That corresponds to line ignoring
                    //
                    //  Else operator used for optimisation
                    //  to skip the loop if the line is already ignored
                    else
                    {
                        //  If the current line isnt empty
                        if (_helper.Length > 0)
                        {
                            //  Check for special characters responsible for line ignoring
                            for (int j = 0; j < _lineIgnoreKeys.Length; j++)
                            {
                                if (_helper[0] == _lineIgnoreKeys[j])
                                {
                                    //  -> Set the _ingoreThisLine flag to true
                                    //  And ignore this line
                                    _ignoreThisLineFlag = true;

                                    //  Exit the loop
                                    j += _lineIgnoreKeys.Length;
                                }
                            }
                        }
                    }


                    //  If the line shouldnt be ignored, we save it (the parsed version)
                    if (!_ignoreThisLineFlag) _parsedData.Add(_helper);
                }

                //  Return the parsed data
                return _parsedData;
            }
            else
            {
                //  Show error message (optional)
                if (_showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(_startLine + _margin);

                    //  Write the error message
                    if (_engLang) Write("Error while parsing the data! Data is null");
                    else Write("Ошибка при парсинге данных! Данные равны null");

                    //  Write endline (optional)
                    Write(_endLine);
                }

                //  Return error
                return null;
            }
        }
        /*  Universal parser for the read data
         *  
         *  ACCEPTED FUNCTIONALITY:
         *     -  Removing  empty lines
         *     -  Removing  white space
         *     -  Exception characters for the white space removal
         *     -  Ignoring  lines with special characters
         *     -  Character ignoring in the whole file
         *     -  Information output                            */


        static public List<string> ReadData(string _path, string _fileName,
            bool _showInfo = false, bool _engLang = true, string _margin = "\t",
            string _startLine = "", string _endLine = "\n")
        {
            if (File.Exists(Path.Combine(_path, _fileName)))
            {
                try
                {
                    //  Open the file from the selected path
                    StreamReader _dataReader = new StreamReader(Path.Combine(_path, _fileName));

                    //  Storing the parsed result from the file
                    List<string> _foundData = new List<string>();

                    //  Temporary string to hold the data before saving it
                    string _helper = _dataReader.ReadLine();



                    //  While the end of the document isnt reached - continue reading and saving the info
                    while (_helper != null)
                    {
                        //  Save the previous read data
                        _foundData.Add(_helper);

                        //  Parse the next line in the file
                        _helper = _dataReader.ReadLine();
                    }

                    //  Close the file manager
                    _dataReader.Close();


                    //  Show success message (optional)
                    if (_showInfo)
                    {
                        //  Write the newline and margin (optional)
                        Write(_startLine + _margin);

                        //  Write the success message
                        if (_engLang) Write("File >" + _fileName + "< was successfully read");
                        else Write("Файл >" + _fileName + "< был успешно прочитан");

                        //  Write end line (optional)
                        Write(_endLine);
                    }

                    //  Return the found data
                    return _foundData;
                }
                catch (Exception e)  // Error exception
                {
                    //  Show error message (optional)
                    if (_showInfo)
                    {
                        //  Write the newline and margin (optional)
                        Write(_startLine + _margin);

                        //  Write the error message
                        if (_engLang)
                        {
                            Write("Error while reading the file >" + _fileName + "<\n");
                            Write(_margin + "Output error: " + e);
                        }
                        else
                        {
                            Write("Ошибка при чтении файла >" + _fileName + "<\n");
                            Write(_margin + "Код ошибки: " + e);
                        }

                        //  Write end line (optional)
                        Write(_endLine);
                    }

                    //  Return error
                    return null;
                }
            }
            else
            {
                //  If the file was not found, output error message (optional)
                if (_showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(_startLine + _margin);

                    //  Write the error message
                    if (_engLang) Write("Error while reading the file! File >" + _fileName + "< was not found");
                    else Write("Ошибка при чтении файла! Файл >" + _fileName + "< не найден");

                    //  Write end line (optional)
                    Write(_endLine);
                }

                //  Return error
                return null;
            }
        }
        //  Reading and returning the data inside a file


        static public void SaveData(string _path, string _fileName, List<string> _data,
            bool _dontOverwrite, bool _showInfo = false, bool _engLang = true, string _margin = "\t",
            string _startLine = "", string _endLine = "\n")
        {
            try
            {
                //  Open the file from the selected path
                StreamWriter _dataSaver = new StreamWriter(Path.Combine(_path, _fileName), _dontOverwrite);

                if (_data != null)
                {
                    for (int i = 0; i < _data.Count; i++)
                    {
                        //  Save the data to the file
                        _dataSaver.Write(_data[i] + "\n");
                    }

                    //  Show success message (optional)
                    if (_showInfo)
                    {
                        //  Write the newline and margin (optional)
                        Write(_startLine + _margin);

                        //  Write the success message
                        if (_engLang) Write("Successfully saved the data to the file >" + _fileName + "<");
                        else Write("Успешно сохранены данные в файл >" + _fileName + "<");

                        //  Write end line (optional)
                        Write(_endLine);
                    }
                }

                //  Show error message (optional)
                else if (_showInfo)
                {
                    //  Write the newline and margin if needed
                    Write(_startLine + _margin);

                    //  Write the error message
                    if (_engLang)
                    {
                        Write("Error saving the data to the file >" + _fileName + "<\n");
                        Write(_margin + "Output error: Data is null");
                    }
                    else
                    {
                        Write("Ошибка сохранения данных в файл >" + _fileName + "<\n");
                        Write(_margin + "Код ошибки: Данные равны null");
                    }

                    //  Write end line if needed
                    Write(_endLine);
                }


                //  Close the file manager
                _dataSaver.Close();
            }
            catch (Exception e)  // Error exception
            {
                //  Show error message (optional)
                if (_showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(_startLine + _margin);

                    //  Write the error message
                    if (_engLang)
                    {
                        Write("Error saving the data to the file >" + _fileName + "<\n");
                        Write(_margin + "Output error: " + e);
                    }
                    else
                    {
                        Write("Ошибка сохранения данных в файл >" + _fileName + "<\n");
                        Write(_margin + "Код ошибки: " + e);
                    }

                    //  Write end line (optional)
                    Write(_endLine);
                }
            }
        }
        //  Saving some data to a chosen file, or trying to create it and then save the data



        //  --------------------  File management related functions  --------------------------------------//



        static public string[] GetFiles(string _path, bool _removePathFromFileNames,
            bool _showInfo = false, bool _engLang = true, string _margin = "\t",
            string _startLine = "", string _endLine = "\n")
        {
            string[] _foundFiles = null;

            try
            {
                //  Get the file names for the chosen directory path
                _foundFiles = Directory.GetFiles(_path);

                //  Cut the path from the file names (optional)
                if (_removePathFromFileNames)
                {
                    //  For every found file
                    for (int i = 0; i < _foundFiles.Length; i++)
                    {
                        //  Edit the file name to not include the path
                        _foundFiles[i] = Path.GetFileName(_foundFiles[i]);
                    }
                }

                //  Show success message (optional)
                if (_showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(_startLine + _margin);

                    //  If the files were found
                    if (_foundFiles.Length > 0)
                    {
                        //  Write the success message
                        if (_engLang) Write("Successfully got the files from the path >" + _path + "<");
                        else Write("Успешно получены файлы по пути >" + _path + "<");
                    }

                    //  If no files were found
                    else
                    {
                        //  Write the error message
                        if (_engLang) Write("No files found in the path >" + _path + "<");
                        else Write("Не найдено ни одного файла по пути >" + _path + "<");
                    }


                    //  Write endline (optional)
                    Write(_endLine);
                }
            }
            catch (Exception e)
            {
                //  Write the error message (optional)
                if (_showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(_startLine + _margin);

                    //  Write the error message
                    if (_engLang)
                    {
                        Write("Error while getting the files from the path >" + _path + "<\n");
                        Write(_margin + "Output error: " + e);
                    }
                    else
                    {
                        Write("Ошибка при получении файлов по пути >" + _path + "<\n");
                        Write(_margin + "Код ошибки: " + e);
                    }

                    //  Write endline (optional)
                    Write(_endLine);
                }
            }

            //  return the found file names
            return _foundFiles;
        }



        static public void DeleteFile(string _path, string _fileName,
            bool _showInfo = false, bool _engLang = true, string _margin = "\t",
            string _startLine = "", string _endLine = "\n")
        {
            try
            {
                //  Check if the file exists
                if (File.Exists(Path.Combine(_path, _fileName)))
                {
                    //  Deleting the file
                    File.Delete(Path.Combine(_path, _fileName));

                    //  Show success message (optional)
                    if (_showInfo)
                    {
                        //  Write the newline and margin (optional)
                        Write(_startLine + _margin);

                        //  Write the success message
                        if (_engLang) Write("File >" + _fileName + "< was successfully deleted");
                        else Write("Файл >" + _fileName + "< был успешно удалён");

                        //  Write end line (optional)
                        Write(_endLine);
                    }
                }

                //  Show error message (optional)
                else if (_showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(_startLine + _margin);

                    //  Write the error message
                    if (_engLang) Write("Error! File >" + _fileName + "< was not found");
                    else Write("Ошибка! Файл >" + _fileName + "< не найден");

                    //  Write end line (optional)
                    Write(_endLine);
                }
            }
            catch (Exception e)  // Error exception
            {
                //  Show error message (optional)
                if (_showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(_startLine + _margin);

                    //  Write the error message
                    if (_engLang)
                    {
                        Write("Error while deleting the file >" + _fileName + "<\n");
                        Write(_margin + "Output error: " + e);
                    }
                    else
                    {
                        Write("Ошибка при удалении файла >" + _fileName + "<\n");
                        Write(_margin + "Код ошибки: " + e);
                    }

                    //  Write end line (optional)
                    Write(_endLine);
                }
            }
        }
        //  Deleting a file


        static public void ClearFile(string _path, string _fileName,
            bool _showInfo = false, bool _engLang = true, string _margin = "\t",
            string _startLine = "", string _endLine = "\n")
        {
            try
            {
                //  Check if the file exists
                if (File.Exists(Path.Combine(_path, _fileName)))
                {
                    //  New file manager to clear the file
                    StreamWriter _clearFile = new StreamWriter(Path.Combine(_path, _fileName), false);

                    //  Show success message (optional)
                    if (_showInfo)
                    {
                        //  Write the newline and margin (optional)
                        Write(_startLine + _margin);

                        //  Write the success message
                        if (_engLang) Write("File >" + _fileName + "< was successfully cleared");
                        else Write("Файл >" + _fileName + "< был успешно очищен");

                        //  Write end line (optional)
                        Write(_endLine);
                    }

                    //  Close the file manager
                    _clearFile.Close();
                }

                //  Show error message (optional)
                else if (_showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(_startLine + _margin);

                    //  Write the error message
                    if (_engLang) Write("Error! File >" + _fileName + "< was not found");
                    else Write("Ошибка! Файл >" + _fileName + "< не найден");

                    //  Write end line (optional)
                    Write(_endLine);
                }

            }
            catch (Exception e)  // Error exception
            {
                //  Show error message (optional)
                if (_showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(_startLine + _margin);

                    //  Write the error message
                    if (_engLang)
                    {
                        Write("Error while clearing the file >" + _fileName + "<\n");
                        Write(_margin + "Output error: " + e);
                    }
                    else
                    {
                        Write("Ошибка при очистке файла >" + _fileName + "<\n");
                        Write(_margin + "Код ошибки: " + e);
                    }

                    //  Write end line (optional)
                    Write(_endLine);
                }
            }
        }
        //  Clearing all the contents inside the chosen file



        //-----------------------------  For demo only functions  ----------------------------------------------------//

        static public void WaitForAnyKey(bool _clearAfter = false,
            bool _engLang = true, string _margin = "\t",
            string _startLine = "\n", string _endLine = "\n\n")
        {
            //  Write the newline and margin (optional)
            Write(_startLine + _margin);

            //  Write the message
            if (_engLang) Write("Press any key to continue ");
            else Write("Нажмите любую клавишу для продолжения ");

            //  Wait for the user input
            ReadKey();

            //  Clearing the console (optional)
            if (_clearAfter) Clear();

            //  Write the endline (optional)
            Write(_endLine);
        }
        //  Demo function, otherwise unnecessary
    }
}