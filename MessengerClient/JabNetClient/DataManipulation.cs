using System;
using System.IO;
using System.Collections.Generic;

using static System.Console;


namespace JabNetClient
{
    internal class DataManipulation
    {

        //-----------------------------  Path related functions  ------------------------------------------//


        static public string GetPath(bool custom = false, string subFolder = "\\Gyroscopic\\Unnamed",
            bool showInfo = false, bool engLang = true, string margin = "\t",
            string startLine = "", string endLine = "\n")
        {
            //  Storing the final path here
            string path;

            //  If the path does not exist (Failed to get the users path)
            //  Or we chose to get a system path
            if (!custom)
            {
                //  Get the path to the Users\usernamehere\Documents folder
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                //  If the directory is missing for some reason
                if (!Directory.Exists(path))
                {
                    try
                    {
                        //  Try to create the directory
                        Directory.CreateDirectory(path);

                        //  Show success message (optional)
                        if (showInfo)
                        {
                            //  Write the newline and margin (optional)
                            Write(startLine + margin);

                            //  Write the success message
                            if (engLang) Write("Successfully created the folder: " + path);
                            else Write("Успешно создана папка: " + path);

                            //  Write endline (optional)
                            Write(endLine);
                        }
                    }
                    catch (Exception e)
                    {
                        //  If an unexpected error happens

                        //  Show error message (optional)
                        if (showInfo)
                        {
                            //  Write the newline and margin (optional)
                            Write(startLine + margin);

                            //  Write the error message
                            if (engLang)
                            {
                                Write("Error while creating the folder: " + path + "\n");
                                Write(margin + "Output error: " + e);
                            }
                            else
                            {
                                Write("Ошибка при создании папки: " + path + "\n");
                                Write(margin + "Код ошибки: " + e);
                            }

                            //  Write endline (optional)
                            Write(endLine);
                        }

                        //  If the documents folder is unavailable
                        //  Try to get the path to the LocalAppData folder
                        path = GetLocalAppdataPath(subFolder, showInfo);

                        //  Return the path to the LocalAppData folder (If succeeded)
                        //  Or null (If the process failed)
                        return path;
                    }
                }


                //  If the \Documents folder exists
                try
                {
                    //  Create a new folder for the custom data
                    Directory.CreateDirectory(path + subFolder);

                    //  Change the path to the newly created folder
                    path += subFolder;

                    //  Return the new path
                    return path;
                }
                catch (Exception e)
                {
                    //  Show error output (optional))
                    if (showInfo)
                    {
                        //  Write the newline and margin (optional)
                        Write(startLine + margin);

                        //  Write the error message
                        if (engLang)
                        {
                            Write("Error while creating a subfolder at the path: " + path + "\n");
                            Write(margin + "Output error: " + e);
                        }
                        else
                        {
                            Write("Ошибка при создании подпапки по пути: " + path + "\n");
                            Write(margin + "Код ошибки: " + e);
                        }

                        //  Write endline (optional)
                        Write(endLine);
                    }

                    //  If the documents folder is unavailable
                    //  Try to get the path to the LocalAppData folder
                    path = GetLocalAppdataPath(subFolder, showInfo);

                    //  Return the path to the LocalAppData folder (If succeeded)
                    //  Or null (If failed)
                    return path;
                }
            }




            //  Trying to get the custom path from the user 
            else
            {
                //  Ask the user for a path to save the files
                Write(startLine + margin);
                if (engLang) Write("Enter a path for the files to save sample data: ");
                else Write("Введите путь для сохранения файлов с тестовыми данными: ");

                //  Read the user "path", without the spaces in the start, end, and the " characters
                path = ReadLine().Trim().Replace("\"", "");


                //  Check if the chosen path exists
                if (!Directory.Exists(path) || !Path.IsPathRooted(path))
                {
                    if (Path.IsPathRooted(path))
                    {
                        //  Try create the user chosen path if it is valid
                        try
                        {
                            //  If the user entered a file name to the path
                            if (File.Exists(path) || path.Contains("."))
                            {
                                //  Get the directory of the file
                                path = Path.GetDirectoryName(path);
                            }


                            //  Try to create it
                            Directory.CreateDirectory(path);


                            //  Show success message (optional)
                            if (showInfo)
                            {
                                //  Write the newline and margin (optional)
                                Write(startLine + margin);

                                //  Write the success message
                                if (engLang) Write("Successfully created the folder: " + path);
                                else Write("Успешно создана папка: " + path);

                                //  Write endline (optional)
                                Write(endLine);
                            }


                            //  Return the successfully created user path
                            return path;
                        }
                        catch (Exception e)
                        {
                            //  If we catch an error happens
                            //  (For example Unauthorized access to the folder)
                            //  Show error message (optional)
                            if (showInfo)
                            {
                                //  Write the newline and margin (optional)
                                Write(startLine + margin);

                                //  Write the error message
                                if (engLang)
                                {
                                    Write("Error while creating the folder: " + path + "\n");
                                    Write(margin + "Output error: " + e);
                                }
                                else
                                {
                                    Write("Ошибка при создании папки: " + path + "\n");
                                    Write(margin + "Код ошибки: " + e);
                                }

                                //  Write endline (optional)
                                Write(endLine);
                            }
                        }
                    }

                    //  If the path doesnt exist
                    //  And we can't create it
                    //  Get the stock path (C:\Users\username\Documents\Gyroscopic\Unnamed)
                    //  (Or C:\Users\username\AppData\Local\Gyroscopic\Unnamed)
                    path = GetPath(false, subFolder, showInfo);
                }


                //  Return chosen path
                return path;
            }
        }
        /* Universal function or getting a path
         *     -  User path (custom = true)
         *     -  C:\Users\username\Documents
         *     -  C:\Users\username\AppData\Local
         *  
         *  ACCEPTS:
         *     -  Adding any subfolders into the selected directory path
         *     -  Default input as stated in the function arguments
         *     -  Error outputing                                     */


        static private string GetLocalAppdataPath(string subFolder = "\\Gyroscopic\\Unnamed",
            bool showInfo = false, bool engLang = true, string margin = "\t",
            string startLine = "", string endLine = "\n")
        {
            //  If we cant get the rights to the documents folder
            try
            {
                //  Get the path to the LocalAppData folder
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                //  Move our path to the subfolder
                path += subFolder;

                //  Try to create the subfolders
                Directory.CreateDirectory(path);

                //  Show success message (optional)
                if (showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(startLine + margin);

                    //  Write the success message
                    if (engLang) Write("Successfully created the subfolder: " + path);
                    else Write("Успешно создана подпапка: " + path);

                    //  Write end line (optional)
                    Write(endLine);
                }

                //  Return the path to the LocalAppData folder
                return path;
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
                if (showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(startLine + margin);

                    //  Write the error message
                    //  If we cant get the rights to the LocalAppData folder
                    if (engLang)
                    {
                        Write("Error while getting the path to the LocalAppData folder\n");
                        Write(margin + "Output error: " + e);
                    }
                    else
                    {
                        Write("Ошибка при получении пути к папке LocalAppData");
                        Write("\n" + margin + "Код ошибки: " + e);
                    }

                    //  Write end line (optional)
                    Write(endLine);
                }

                //  Return null path (fatal error)
                return null;
            }
        }
        //  Getting the path to the LocalAppData folder
        //  Trying to create the selected subfolders there
        //  Return null if the process fails



        //----------------------------  Data manipulation related functions  ---------------------------------//


        static public List<string> ParseData(List<string> data, bool removeEmptyLines = false,
            bool removeSpace = false, string spaceRemoveException = "",
            string lineIgnoreKeys = "", string ignoreTheseChars = "",
            bool showInfo = false, bool engLang = true, string margin = "\t",
            string startLine = "", string endLine = "\n")
        {

            //  Storing the parsed data here
            List<string> parsedData = new List<string>();


            //  Temporary buffer for easier parsing
            //  (stores the mid data before saving it to the final list)
            string helper;


            //  Flag for the state of the line saving
            //
            //  Tells if we should ignore it because of a special character
            //  Or if we can save it
            bool ignoreThisLineFlag;


            //  If the data input is correct (it exists)
            if (data != null)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    //  Reset line ignoring flag
                    ignoreThisLineFlag = false;


                    //  Move to the next line
                    helper = data[i];


                    //  ----------------------------------------  White space removal from a line logic
                    if (removeSpace)
                    {
                        //  Flag that contains the state
                        //
                        //  Whether this particular line
                        //  can have the white space removed from it or not
                        bool canRemoveFlag = true;


                        //  If the current line isnt empty
                        if (helper.Length > 0)
                        {

                            //  Check for special characters that prohibits white space cleaning
                            for (int j = 0; j < spaceRemoveException.Length; j++)
                            {
                                // If the first character of the line
                                // is one of the exceptions or space removal
                                if (helper[0] == spaceRemoveException[j])
                                {
                                    //  Disable white space removal for this particular line
                                    canRemoveFlag = false;

                                    //  Exit the loop
                                    j += spaceRemoveException.Length;
                                }
                            }
                        }

                        //  If this line is not restricted by any exception characters
                        if (canRemoveFlag)
                        {
                            //  Remove all the white spaces
                            helper = helper.Replace(" ", "");
                        }
                    }


                    //  ---------------------------------------------  Special character ignoring logic
                    for (int j = 0; j < ignoreTheseChars.Length; j++)
                    {
                        //  remove all the selected special characters
                        helper = helper.Replace(ignoreTheseChars[j].ToString(), "");
                    }


                    //  ---------------------------------------------------  If the line is empty logic
                    //  And the command for removing empty ones is true
                    //  -> Set the ingoreThisLine flag to true
                    //  And ignore this line
                    if (helper == "" && removeEmptyLines) ignoreThisLineFlag = true;


                    //  -----------------------------------------  Special keys for line ignoring logic
                    //  Check for selected special characters in the line start pos
                    //  That corresponds to line ignoring
                    //
                    //  Else operator used for optimisation
                    //  to skip the loop if the line is already ignored
                    else
                    {
                        //  If the current line isnt empty
                        if (helper.Length > 0)
                        {
                            //  Check for special characters responsible for line ignoring
                            for (int j = 0; j < lineIgnoreKeys.Length; j++)
                            {
                                if (helper[0] == lineIgnoreKeys[j])
                                {
                                    //  -> Set the ingoreThisLine flag to true
                                    //  And ignore this line
                                    ignoreThisLineFlag = true;

                                    //  Exit the loop
                                    j += lineIgnoreKeys.Length;
                                }
                            }
                        }
                    }


                    //  If the line shouldnt be ignored, we save it (the parsed version)
                    if (!ignoreThisLineFlag) parsedData.Add(helper);
                }

                //  Return the parsed data
                return parsedData;
            }
            else
            {
                //  Show error message (optional)
                if (showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(startLine + margin);

                    //  Write the error message
                    if (engLang) Write("Error while parsing the data! Data is null");
                    else Write("Ошибка при парсинге данных! Данные равны null");

                    //  Write endline (optional)
                    Write(endLine);
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


        static public List<string> ReadData(string path, string fileName,
            bool showInfo = false, bool engLang = true, string margin = "\t",
            string startLine = "", string endLine = "\n")
        {
            if (File.Exists(Path.Combine(path, fileName)))
            {
                try
                {
                    //  Open the file from the selected path
                    StreamReader dataReader = new StreamReader(Path.Combine(path, fileName));

                    //  Storing the parsed result from the file
                    List<string> foundData = new List<string>();

                    //  Temporary string to hold the data before saving it
                    string helper = dataReader.ReadLine();



                    //  While the end of the document isnt reached - continue reading and saving the info
                    while (helper != null)
                    {
                        //  Save the previous read data
                        foundData.Add(helper);

                        //  Parse the next line in the file
                        helper = dataReader.ReadLine();
                    }

                    //  Close the file manager
                    dataReader.Close();


                    //  Show success message (optional)
                    if (showInfo)
                    {
                        //  Write the newline and margin (optional)
                        Write(startLine + margin);

                        //  Write the success message
                        if (engLang) Write("File >" + fileName + "< was successfully read");
                        else Write("Файл >" + fileName + "< был успешно прочитан");

                        //  Write end line (optional)
                        Write(endLine);
                    }

                    //  Return the found data
                    return foundData;
                }
                catch (Exception e)  // Error exception
                {
                    //  Show error message (optional)
                    if (showInfo)
                    {
                        //  Write the newline and margin (optional)
                        Write(startLine + margin);

                        //  Write the error message
                        if (engLang)
                        {
                            Write("Error while reading the file >" + fileName + "<\n");
                            Write(margin + "Output error: " + e);
                        }
                        else
                        {
                            Write("Ошибка при чтении файла >" + fileName + "<\n");
                            Write(margin + "Код ошибки: " + e);
                        }

                        //  Write end line (optional)
                        Write(endLine);
                    }

                    //  Return error
                    return null;
                }
            }
            else
            {
                //  If the file was not found, output error message (optional)
                if (showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(startLine + margin);

                    //  Write the error message
                    if (engLang) Write("Error while reading the file! File >" + fileName + "< was not found");
                    else Write("Ошибка при чтении файла! Файл >" + fileName + "< не найден");

                    //  Write end line (optional)
                    Write(endLine);
                }

                //  Return error
                return null;
            }
        }
        //  Reading and returning the data inside a file


        static public void SaveData(string path, string fileName, List<string> data,
            bool dontOverwrite, bool showInfo = false, bool engLang = true, string margin = "\t",
            string startLine = "", string endLine = "\n")
        {
            try
            {
                //  Open the file from the selected path
                StreamWriter dataSaver = new StreamWriter(Path.Combine(path, fileName), dontOverwrite);

                if (data != null)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        //  Save the data to the file
                        dataSaver.Write(data[i] + "\n");
                    }

                    //  Show success message (optional)
                    if (showInfo)
                    {
                        //  Write the newline and margin (optional)
                        Write(startLine + margin);

                        //  Write the success message
                        if (engLang) Write("Successfully saved the data to the file >" + fileName + "<");
                        else Write("Успешно сохранены данные в файл >" + fileName + "<");

                        //  Write end line (optional)
                        Write(endLine);
                    }
                }

                //  Show error message (optional)
                else if (showInfo)
                {
                    //  Write the newline and margin if needed
                    Write(startLine + margin);

                    //  Write the error message
                    if (engLang)
                    {
                        Write("Error saving the data to the file >" + fileName + "<\n");
                        Write(margin + "Output error: Data is null");
                    }
                    else
                    {
                        Write("Ошибка сохранения данных в файл >" + fileName + "<\n");
                        Write(margin + "Код ошибки: Данные равны null");
                    }

                    //  Write end line if needed
                    Write(endLine);
                }


                //  Close the file manager
                dataSaver.Close();
            }
            catch (Exception e)  // Error exception
            {
                //  Show error message (optional)
                if (showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(startLine + margin);

                    //  Write the error message
                    if (engLang)
                    {
                        Write("Error saving the data to the file >" + fileName + "<\n");
                        Write(margin + "Output error: " + e);
                    }
                    else
                    {
                        Write("Ошибка сохранения данных в файл >" + fileName + "<\n");
                        Write(margin + "Код ошибки: " + e);
                    }

                    //  Write end line (optional)
                    Write(endLine);
                }
            }
        }
        //  Saving some data to a chosen file, or trying to create it and then save the data



        //  --------------------  File management related functions  --------------------------------------//



        static public string[] GetFiles(string path, bool removePathFromFileNames,
            bool showInfo = false, bool engLang = true, string margin = "\t",
            string startLine = "", string endLine = "\n")
        {
            string[] foundFiles = null;

            try
            {
                //  Get the file names for the chosen directory path
                foundFiles = Directory.GetFiles(path);

                //  Cut the path from the file names (optional)
                if (removePathFromFileNames)
                {
                    //  For every found file
                    for (int i = 0; i < foundFiles.Length; i++)
                    {
                        //  Edit the file name to not include the path
                        foundFiles[i] = Path.GetFileName(foundFiles[i]);
                    }
                }

                //  Show success message (optional)
                if (showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(startLine + margin);

                    //  If the files were found
                    if (foundFiles.Length > 0)
                    {
                        //  Write the success message
                        if (engLang) Write("Successfully got the files from the path >" + path + "<");
                        else Write("Успешно получены файлы по пути >" + path + "<");
                    }

                    //  If no files were found
                    else
                    {
                        //  Write the error message
                        if (engLang) Write("No files found in the path >" + path + "<");
                        else Write("Не найдено ни одного файла по пути >" + path + "<");
                    }


                    //  Write endline (optional)
                    Write(endLine);
                }
            }
            catch (Exception e)
            {
                //  Write the error message (optional)
                if (showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(startLine + margin);

                    //  Write the error message
                    if (engLang)
                    {
                        Write("Error while getting the files from the path >" + path + "<\n");
                        Write(margin + "Output error: " + e);
                    }
                    else
                    {
                        Write("Ошибка при получении файлов по пути >" + path + "<\n");
                        Write(margin + "Код ошибки: " + e);
                    }

                    //  Write endline (optional)
                    Write(endLine);
                }
            }

            //  return the found file names
            return foundFiles;
        }



        static public void DeleteFile(string path, string fileName,
            bool showInfo = false, bool engLang = true, string margin = "\t",
            string startLine = "", string endLine = "\n")
        {
            try
            {
                //  Check if the file exists
                if (File.Exists(Path.Combine(path, fileName)))
                {
                    //  Deleting the file
                    File.Delete(Path.Combine(path, fileName));

                    //  Show success message (optional)
                    if (showInfo)
                    {
                        //  Write the newline and margin (optional)
                        Write(startLine + margin);

                        //  Write the success message
                        if (engLang) Write("File >" + fileName + "< was successfully deleted");
                        else Write("Файл >" + fileName + "< был успешно удалён");

                        //  Write end line (optional)
                        Write(endLine);
                    }
                }

                //  Show error message (optional)
                else if (showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(startLine + margin);

                    //  Write the error message
                    if (engLang) Write("Error! File >" + fileName + "< was not found");
                    else Write("Ошибка! Файл >" + fileName + "< не найден");

                    //  Write end line (optional)
                    Write(endLine);
                }
            }
            catch (Exception e)  // Error exception
            {
                //  Show error message (optional)
                if (showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(startLine + margin);

                    //  Write the error message
                    if (engLang)
                    {
                        Write("Error while deleting the file >" + fileName + "<\n");
                        Write(margin + "Output error: " + e);
                    }
                    else
                    {
                        Write("Ошибка при удалении файла >" + fileName + "<\n");
                        Write(margin + "Код ошибки: " + e);
                    }

                    //  Write end line (optional)
                    Write(endLine);
                }
            }
        }
        //  Deleting a file


        static public void ClearFile(string path, string fileName,
            bool showInfo = false, bool engLang = true, string margin = "\t",
            string startLine = "", string endLine = "\n")
        {
            try
            {
                //  Check if the file exists
                if (File.Exists(Path.Combine(path, fileName)))
                {
                    //  New file manager to clear the file
                    StreamWriter clearFile = new StreamWriter(Path.Combine(path, fileName), false);

                    //  Show success message (optional)
                    if (showInfo)
                    {
                        //  Write the newline and margin (optional)
                        Write(startLine + margin);

                        //  Write the success message
                        if (engLang) Write("File >" + fileName + "< was successfully cleared");
                        else Write("Файл >" + fileName + "< был успешно очищен");

                        //  Write end line (optional)
                        Write(endLine);
                    }

                    //  Close the file manager
                    clearFile.Close();
                }

                //  Show error message (optional)
                else if (showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(startLine + margin);

                    //  Write the error message
                    if (engLang) Write("Error! File >" + fileName + "< was not found");
                    else Write("Ошибка! Файл >" + fileName + "< не найден");

                    //  Write end line (optional)
                    Write(endLine);
                }

            }
            catch (Exception e)  // Error exception
            {
                //  Show error message (optional)
                if (showInfo)
                {
                    //  Write the newline and margin (optional)
                    Write(startLine + margin);

                    //  Write the error message
                    if (engLang)
                    {
                        Write("Error while clearing the file >" + fileName + "<\n");
                        Write(margin + "Output error: " + e);
                    }
                    else
                    {
                        Write("Ошибка при очистке файла >" + fileName + "<\n");
                        Write(margin + "Код ошибки: " + e);
                    }

                    //  Write end line (optional)
                    Write(endLine);
                }
            }
        }
        //  Clearing all the contents inside the chosen file



        //-----------------------------  For demo only functions  ----------------------------------------------------//

        static public void WaitForAnyKey(bool clearAfter = false,
            bool engLang = true, string margin = "\t",
            string startLine = "\n", string endLine = "\n\n")
        {
            //  Write the newline and margin (optional)
            Write(startLine + margin);

            //  Write the message
            if (engLang) Write("Press any key to continue ");
            else Write("Нажмите любую клавишу для продолжения ");

            //  Wait for the user input
            ReadKey();

            //  Clearing the console (optional)
            if (clearAfter) Clear();

            //  Write the endline (optional)
            Write(endLine);
        }
        //  Demo function, otherwise unnecessary
    }
}