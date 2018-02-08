﻿using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using UnityEngine;

namespace com.immortalhydra.gdtb.todos
{
    public static class IO
    {
        #region METHODS

        /// Remove a group of lines from a text file.
        public static void RemoveLinesFromFile(string aFile, int[] aLineNumbers)
        {
            var tempFile = Path.GetTempFileName();
            var currentLineNumber = 0;

            var reader = new StreamReader(aFile);
            var writer = new StreamWriter(tempFile);

            try
            {
                string line;
                var lineIndex = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    // If we found all the QQQ lines we wanted to modify, no reason to check for others.
                    if (lineIndex >= aLineNumbers.Length)
                    {
                        writer.WriteLine(line);
                    }
                    else
                    {
                        // If the line is not the one we want to remove, write it to the temp file.
                        if (currentLineNumber != aLineNumbers[lineIndex])
                        {
                            writer.WriteLine(line);
                        }
                        // Else, we take the QQQ away.
                        else
                        {
                            var lineWithoutQQQ = GetLineWithoutQQQ(line);
                            if (string.IsNullOrEmpty(lineWithoutQQQ))
                            {
                                lineWithoutQQQ = "";
                            }
                            writer.WriteLine(lineWithoutQQQ);
                            lineIndex++;
                        }
                    }
                    currentLineNumber++;
                }
                reader.Close();
                writer.Close();

                // Overwrite the old file with the temp file.
                File.Delete(aFile);
                File.Move(tempFile, aFile);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log(ex.Data);
                Debug.Log(ex.StackTrace);
                reader.Dispose();
                writer.Dispose();
            }
        }


        /// Update the task and priority of a QQQ.
        public static void ChangeQQQ(QQQ anOldQQQ, QQQ aNewQQQ)
        {
            var tempFile = Path.GetTempFileName();
            var currentLineNumber = 0;

            var reader = new StreamReader(anOldQQQ.Script);
            var writer = new StreamWriter(tempFile);
            try
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // If the line is not the one we want to remove, write it to the temp file.
                    if (currentLineNumber != anOldQQQ.LineNumber)
                    {
                        writer.WriteLine(line);
                    }
                    else
                    {
                        // Remove the old QQQ and add the new one, then write the line to file.
                        var lineWithoutQQQ = GetLineWithoutQQQ(line);

                        var slashes = string.IsNullOrEmpty(lineWithoutQQQ) ? "//" : " //";

                        var newLine = lineWithoutQQQ + slashes + Preferences.TODOToken + (int) aNewQQQ.Priority + " " +
                                      aNewQQQ.Task;
                        writer.WriteLine(newLine);
                    }
                    currentLineNumber++;
                }
                reader.Close();
                writer.Close();

                // Overwrite the old file with the temp file.
                File.Delete(anOldQQQ.Script);
                File.Move(tempFile, anOldQQQ.Script);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log(ex.Data);
                Debug.Log(ex.StackTrace);
                reader.Dispose();
                writer.Dispose();
            }
        }


        /// Add a QQQ to a script.
        public static void AddQQQ(QQQ aQQQ)
        {
            var tempFile = Path.GetTempFileName();
            var lines = File.ReadAllLines(aQQQ.Script);

            if (lines.Length < aQQQ.LineNumber - 1)
            {
                var oldLines = lines;
                lines = new string[aQQQ.LineNumber];
                for (var i = 0; i < oldLines.Length; i++)
                {
                    lines[i] = oldLines[i];
                }
            }

            var currentLineNumber = 0;
            var writer = new StreamWriter(tempFile);
            try
            {
                while (currentLineNumber < lines.Length)
                {
                    // Add the new QQQ as the first line in the file.
                    if (currentLineNumber == aQQQ.LineNumber - 1 ||
                        (currentLineNumber == aQQQ.LineNumber && aQQQ.LineNumber == 0))
                    {
                        var newQQQ = "//" + Preferences.TODOToken + (int) aQQQ.Priority + " " + aQQQ.Task;


                        writer.WriteLine(newQQQ);
                        if (currentLineNumber == lines.Length - 1)
                        {
                            writer.Write(lines[currentLineNumber]);
                        }
                        else
                        {
                            writer.WriteLine(lines[currentLineNumber]);
                        }
                    }
                    else
                    {
                        if (lines[currentLineNumber] != null)
                        {
                            writer.WriteLine(lines[currentLineNumber]);
                        }
                        else
                        {
                            writer.WriteLine();
                        }
                    }
                    currentLineNumber++;
                }
                writer.Close();

                // Overwrite the old file with the temp file.
                File.Delete(aQQQ.Script);
                File.Move(tempFile, aQQQ.Script);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log(ex.Data);
                Debug.Log(ex.StackTrace);
                writer.Dispose();
            }
        }


        /// Populate a list with the files (and folders) in the "exclude.txt" doc.
        public static List<string> GetExcludedScripts()
        {
            var excludeDoc = GetFilePath("TODOs/exclude.txt");

            // Parse the document for exclusions.
            var excludedScripts = new List<string>();
            var reader = new StreamReader(excludeDoc);
            try
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("#") || String.IsNullOrEmpty(line) ||
                        line == " ") // If the line is a comment, is empty, or is a single space, ignore them.
                    {
                    }
                    else
                    {
                        excludedScripts.Add(line);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log(ex.Data);
                Debug.Log(ex.StackTrace);
                reader.Dispose();
            }
            return excludedScripts;
        }


        /// Load scripts stored in the "script db".
        public static void LoadScripts()
        {
            if (QQQOps.AllScripts != null)
            {
                QQQOps.AllScripts.Clear();
            }
            else
            {
                QQQOps.AllScripts = new List<string>();
            }

            var scriptsFile = GetPathRelativeToExtension("scripts.gdtb");

            if (File.Exists(scriptsFile))
            {
                // Parse the document for exclusions.
                var reader = new StreamReader(scriptsFile);
                try
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        QQQOps.AllScripts.Add(line);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    Debug.Log(ex.Data);
                    Debug.Log(ex.StackTrace);
                    reader.Dispose();
                }
            }
        }


        /// Load the QQQs saved in qqqs.bak.
        public static List<QQQ> LoadStoredQQQs()
        {
            var backedQQQs = new List<QQQ>();
            var bakFile = GetPathRelativeToExtension("bak.gdtb");

            if (File.Exists(bakFile))
            {
                // Parse the document for exclusions.
                var reader = new StreamReader(bakFile);
                try
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("#") || String.IsNullOrEmpty(line) ||
                            line == " ") // If the line is a comment, is empty, or is a single space, ignore them.
                        {
                        }
                        else
                        {
                            backedQQQs.Add(ParseQQQ(line));
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    Debug.Log(ex.Data);
                    Debug.Log(ex.StackTrace);
                    reader.Dispose();
                }
            }
            return backedQQQs;
        }


        /// Write QQQs in memory to the backup file.
        public static void WriteQQQsToFile(List<QQQ> qqqs)
        {
            var tempFile = Path.GetTempFileName();
            var bakFile = GetPathRelativeToExtension("bak.gdtb");

            var writer = new StreamWriter(tempFile, false);
            try
            {
                foreach (var qqq in qqqs)
                {
                    var priority = QQQOps.PriorityToInt(qqq.Priority);
                    var task =
                        qqq.Task.Replace("|",
                            "(U+007C)"); // Replace pipes so that the parser doesn't get confused on reimport.
                    var line = priority + "|" + task + "|" + qqq.Script + "|" + qqq.LineNumber + "|" + qqq.IsPinned;
                    writer.WriteLine(line);
                }
                writer.Close();

                // Overwrite the old file with the temp file.
                if (File.Exists(bakFile))
                {
                    File.Delete(bakFile);
                }
                File.Move(tempFile, bakFile);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log(ex.Data);
                Debug.Log(ex.StackTrace);
                writer.Dispose();
            }
        }


        /// Overwrite the shortcut flag in WindowMain.cs to use the provided one.
        public static void OverwriteShortcut(string aShortcut)
        {
            var tempFile = Path.GetTempFileName();
            var file = GetFilePath("Gamedev Toolbelt/Editor/TODOs/WindowMain.cs");

            var writer = new StreamWriter(tempFile, false);
            var reader = new StreamReader(file);

            try
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("[MenuItem"))
                    {
                        writer.WriteLine("        [MenuItem(" + '"' + "Window/Gamedev Toolbelt/TODOs/Open TODO " +
                                         aShortcut + '"' + ", false, 1)]");
                    }
                    else
                    {
                        writer.WriteLine(line);
                    }
                }
                reader.Close();
                writer.Close();

                // Overwrite the old file with the temp file.
                File.Delete(file);
                File.Move(tempFile, file);
                UnityEditor.AssetDatabase.ImportAsset(file);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log(ex.Data);
                Debug.Log(ex.StackTrace);
                reader.Dispose();
                writer.Dispose();
            }
        }


        /// Write to file the list of scripts in the extension.
        public static void SaveScriptList()
        {
            var tempFile = Path.GetTempFileName();
            var scriptsFile = GetPathRelativeToExtension("scripts.gdtb");

            if (QQQOps.AllScripts == null)
            {
                QQQOps.AllScripts = new List<string>();
            }

            var writer = new StreamWriter(tempFile, false);
            try
            {
                foreach (var script in QQQOps.AllScripts)
                {
                    writer.WriteLine(script);
                }
                writer.Close();

                // Overwrite the old file with the temp file.
                if (File.Exists(scriptsFile))
                {
                    File.Delete(scriptsFile);
                }
                File.Move(tempFile, scriptsFile);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log(ex.Data);
                Debug.Log(ex.StackTrace);
                writer.Dispose();
            }
        }


        /// Create a path relative to the Extension root.
        public static string GetPathRelativeToExtension(string aName)
        {
            return GetFirstInstanceOfFolder("TODOs") + "/" + aName;
        }


        /// Remove all deleted QQQs from files in one to go prevent mismatches between QQQ and line.
        public static void PersistCompletions(TODO todos)
        {
            // Group line numbers by script.
            var groupedQQQs = new Dictionary<string, List<int>>();
            foreach (var qqq in todos.CompletedQQQs)
            {
                if (!groupedQQQs.ContainsKey(qqq.Script))
                {
                    groupedQQQs[qqq.Script] = new List<int>();
                }
                groupedQQQs[qqq.Script].Add(qqq.LineNumber);
            }

            // Sort them (ascending script line number).
            foreach (var qqq in groupedQQQs)
            {
                var orderedLines = qqq.Value.OrderBy(o => o).ToList();
                qqq.Value.Clear();
                qqq.Value.AddRange(orderedLines);
            }

            // Finally, we remove the lines.
            try
            {
                foreach (var qqq in groupedQQQs)
                {
                    RemoveLinesFromFile(qqq.Key, qqq.Value.ToArray());
                }
                WriteQQQsToFile(todos.QQQs);
                todos.CompletedQQQs.Clear();
                groupedQQQs.Clear();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log(ex.Data);
                Debug.Log(ex.StackTrace);
            }
            WindowMain.QQQsChanged = true;
        }


        /// Return the first instance of the given folder.
        /// This is a non-recursive, breadth-first search algorithm.
        private static string GetFirstInstanceOfFolder(string aFolderName)
        {
            var projectDirectoryPath = Directory.GetCurrentDirectory();
            var projectDirectoryInfo = new DirectoryInfo(projectDirectoryPath);
            var listOfAssetsDirs = projectDirectoryInfo.GetDirectories("Assets");
            var assetsDir = "";
            foreach (var dir in listOfAssetsDirs)
            {
            #if UNITY_EDITOR_WIN
                if (dir.FullName.EndsWith("\\Assets"))
                {
                    assetsDir = dir.FullName;
                }
            #elif UNITY_EDITOR_OSX
                if (dir.FullName.EndsWith("/Assets"))
                {
                    assetsDir = dir.FullName;
                }
            #endif
            }
            var path = assetsDir;

            var q = new Queue<string>();
            q.Enqueue(path);
            var absolutePath = "";
            while (q.Count > 0)
            {
                path = q.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        q.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    Debug.Log(ex.Data);
                    Debug.Log(ex.StackTrace);
                }

                string[] folders = null;
                try
                {
                    folders = Directory.GetDirectories(path);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    Debug.Log(ex.Data);
                    Debug.Log(ex.StackTrace);
                }

                if (folders != null)
                {
                    foreach (var folder in folders)
                    {
                        if (folder.EndsWith(aFolderName))
                        {
                            absolutePath = folder;
                        }
                    }
                }
            }
            var relativePath = absolutePath.Remove(0, projectDirectoryPath.Length + 1);
            return relativePath;
        }


        /// Check for character before the QQQ to see if they are spaces or backslashes. If they are, remove them.
        /// This is to remove the whole QQQ without removing anything else of importance (including stuff in a comment BEFORE a QQQ).
        private static string GetLineWithoutQQQ(string aLine)
        {
            var qqqIndex = aLine.IndexOf(Preferences.TODOToken, StringComparison.Ordinal);
            qqqIndex = qqqIndex < 1 ? 1 : qqqIndex;

            var j = qqqIndex - 1;
            while (j >= 0 && (aLine[j] == ' ' || aLine[j] == '/'))
            {
                if (j > 0)
                {
                    j--;
                    qqqIndex--;
                }
                else
                {
                    return null;
                }
            }
            var lineWithoutQQQ = aLine.Substring(0, aLine.Length - (aLine.Length - qqqIndex));

            return lineWithoutQQQ;
        }


        /// Get the path of a file based on the ending provided.
        private static string GetFilePath(string aPathEnd)
        {
            var assetsPaths = UnityEditor.AssetDatabase.GetAllAssetPaths();
            var filePath = "";
            foreach (var path in assetsPaths)
            {
                if (path.EndsWith(aPathEnd))
                {
                    filePath = path;
                    break;
                }
            }
            return filePath;
        }


        /// Parse a line in the backup file.
        private static QQQ ParseQQQ(string aString)
        {
            var parts = aString.Split('|');

            // Make sure that priority is assigned.
            int priority;
            if (Int32.TryParse(parts[0], out priority) == false)
            {
                priority = 2;
            }

            // Restore any pipe sign in the task.
            var task = parts[1].Replace("(U+007C)", "|");

            // Make sure that line number is assigned.
            int lineNumber;
            if (Int32.TryParse(parts[3], out lineNumber) == false)
            {
                lineNumber = 0;
            }

            // Check whether the QQQ is pinned or not.
            bool isPinned;
            if (Boolean.TryParse(parts[4], out isPinned) == false)
            {
                isPinned = false;
            }

            var qqq = QQQ.Create(priority, task, parts[2], lineNumber, isPinned);
            return qqq;
        }

        #endregion
    }
}