using System;
using System.IO;
using System.Collections.Generic;

using static System.Console;



namespace JabNetClient
{
    internal class DataManipulation
    {
        static public string GetPath(bool _custom = false)
        {
            if (!_custom)
            {
                //  Return the path to the project folder
                return Directory.GetCurrentDirectory();
            }
            else
            {
                //  Ask the user for a path to save the files
                Write("\n\tEnter a path for the files to save sample data: ");

                //  Read the user path
                string _path = ReadLine();

                //  Check if the chosen path exists
                if (!Directory.Exists(_path))
                {
                    //  If the path does not exist, return the path to the project folder
                    return Directory.GetCurrentDirectory();
                }

                //  Return chosen path
                return _path;
            }
        }
             //  Getting the path to the project folder

        static public List<string> ReadSavedData(string _path, string _fileName, bool _showInfo = false)
        {
            if (File.Exists(Path.Combine(_path, _fileName)))
            {
                //  Open the file from the selected path
                StreamReader _dataReader = new StreamReader(Path.Combine(_path, _fileName));

                //  Storing the parsed result from the file
                List<string> _foundData = new List<string>();

                //  Temporary string to hold the data before saving it
                string _helper;

                for (int i = 0; i > -1; i++)
                {
                    //  Parse the user input
                    _helper = _dataReader.ReadLine();

                    //  If this is not the end of the database (just an regular string)
                    if (_helper != null)
                    {
                        //  Remove all the white spaces
                        _helper = _helper.Replace(" ", "");

                        //  Save the parsed data
                        _foundData.Add(_helper);

                        //  Check for empty strings and comments
                        if (_foundData[i] == "\n" || _foundData[i] == "" ||
                            (_foundData[i][0] == '/' && _foundData[i][1] == '/'))
                        {
                            //  Remove the empty strings and comments
                            _foundData.RemoveAt(i);

                            //  Return to the previous index, bcs wee removed the current one
                            i--;
                        }
                    }
                    else  // If we have reached the end of the database (found a null string)
                    {
                        //   Exit the data parsing loop
                        i = -2;
                    }
                }

                //  Close the file manager
                _dataReader.Close();

                //  If wanted, output success message
                if (_showInfo) Write("\tFile >" + _fileName + "< was successfully read");

                //  Return the parsed data
                return _foundData;
            }
            else
            {
                //  If the file was not found, output error message (optional)
                if (_showInfo) Write("\tError while reading the file! File >" + _fileName + "< was not found");
                return null;  //  Return error
            }
        }
             //  Reading and parsing the data inside a file => outputting the parsed data

        static public void SaveData(string _path, string _fileName, List<string> _data, bool _dontOverwrite, bool _showInfo = false)
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

                    //  Show success message if wanted
                    if (_showInfo) Write("\tSuccessfully saved the data to the file >" + _fileName + "<");
                }
                else
                {
                    //  Show error message if wanted
                    if (_showInfo) Write("\tError saving the data to the file >" + _fileName + "<\n\tOutput error: Data is null");
                }

                //  Close the file manager
                _dataSaver.Close();
            }
            catch (Exception e)  // Error exception
            {
                //  Showing error message if needed
                if (_showInfo) Write("\tError saving the data to the file >" + _fileName + "<\n\tOutput error: " + e);
            }
        }
             //  Saving some data to a chosen file, or trying to create it and then save the data

        static public void DeleteData(string _path, string _fileName, bool _showInfo = false)
        {
            try
            {
                //  Check if the file exists
                if (File.Exists(Path.Combine(_path, _fileName)))
                {
                    //  Deleting the file
                    File.Delete(Path.Combine(_path, _fileName));

                    //  Showing additional info if needed
                    if (_showInfo) Write("\tFile >" + _fileName + "< was successfully deleted");
                }
                else
                {
                    //  Showing error message if needed
                    if (_showInfo) Write("\tError! File >" + _fileName + "< was not found");
                }
            }
            catch (Exception e)  // Error exception
            {
                //  Showing error message if needed
                if (_showInfo) Write("\tError while deleting the file >" + _fileName + "<\n\tOutput error: " + e);
            }
        }
             //  Deleting a file

        static public void ClearData(string _path, string _fileName, bool _showInfo)
        {
            try
            {
                //  Check if the file exists
                if (File.Exists(Path.Combine(_path, _fileName)))
                {
                    //  New file manager to clear the file
                    StreamWriter _clearFile = new StreamWriter(Path.Combine(_path, _fileName), false);

                    //  Showing additional info if needed
                    if (_showInfo) Write("\tFile >" + _fileName + "< was successfully cleared");

                    //  Close the file manager
                    _clearFile.Close();
                }
                else
                {
                    //  Showing error message if needed
                    if (_showInfo) Write("\tError! File >" + _fileName + "< was not found");
                }
            }
            catch (Exception e)  // Error exception
            {
                //  Showing error message if needed
                if (_showInfo) Write("\tError while clearing the file >" + _fileName + "<\n\tOutput error: " + e);
            }
        }
             //  Clearing all the contents inside the chosen file

        static public void WaitForKey()
        {
            Write("\n\tPress any key to continue ");
            ReadKey();
            Write("\n\n");
        }
             //  Temporary placeholder useless procedure
    }
}