using System;
using System.IO;
using System.Collections.Generic;

using static System.Console;



namespace JabNetClient
{
    internal class DataManipulation
    {
        static public string GetPath(bool _custom = false, bool _showInfo = false)
        {
            //  Storing the final path here
            string _path;

            //  If the path does not exist
            if (!_custom)
            {
                try
                {
                    //  Get the path to the program folder
                    _path = Directory.GetCurrentDirectory();

                    //  Find the index of the "users" folder in the path
                    int _usersFolderID = _path.IndexOf("\\Users\\");

                    //  Exit up to the \Users\user_name_here  folder
                    _path = _path.Remove(_usersFolderID + 4 + _path.IndexOf("\\", _usersFolderID + 1));

                    //  Enter the \Users\user_name_here\Documents folder
                    _path += "\\Documents";

                    //  Create a new folder for the custom data
                    Directory.CreateDirectory(_path + "\\Gyroscopic\\JabNet\\TestDirectory");

                    //  Change the path to the newly created folder
                    _path += "\\Gyroscopic\\JabNet\\TestDirectory";

                    //  Return the new path
                    return _path;
                }
                catch (Exception e)
                {
                    //  Show error output if wanted
                    if (_showInfo) Write("\n\tError while getting the path to the program folder\n\tOutput error: " + e);

                    //  If the path was not found, return null for the path
                    return null;
                }
            }
            else
            {
                //  Ask the user for a path to save the files
                Write("\n\tEnter a path for the files to save sample data: ");

                //  Read the user path
                _path = ReadLine();

                //  Check if the chosen path exists
                if (!Directory.Exists(_path))
                {
                    //  If the path does not exist, return stock path
                    _path = GetPath(false);
                }

                //  Return chosen path
                return _path;
            }
        }
             //  Getting the path to the project folder


        static public List<string> ParseData(List<string> _data, bool _removeEmptyLines = false,
            bool _removeSpace = false, string _spaceRemoveException = "",
            string _lineIgnoreKeys = "", string _ignoreTheseChars = "", bool _showInfo = false)
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


            //  If the data input is correct
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
                if (_showInfo) Write("\tError while parsing the data! Data is null");
                return null;
            }
        }
             //  Parsing the data by input criterias

        static public List<string> ReadData(string _path, string _fileName, bool _showInfo = false)
        {
            if (File.Exists(Path.Combine(_path, _fileName)))
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
             //  Reading and returning the data inside a file

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