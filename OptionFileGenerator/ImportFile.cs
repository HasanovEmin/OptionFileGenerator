using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace OptionFileGenerator
{
    internal class ImportFile
    {
        internal void Import()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";

            bool? result = openFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {

                var tempFile = File.ReadLines(openFileDialog.FileName);
                foreach (var line in tempFile)
                {
                    if (line.ToLower().StartsWith("textpos"))
                    {
                        List<string> temp = new List<string>();
                        string textPosition = string.Empty;
                        if (line.Contains("'"))
                        {
                            temp = line.Split("'").ToList();
                            textPosition = temp[1];
                        }



                        temp = line.Split().ToList();

                        List<string> formatedList = Worker.GetFormattedList(temp, textPosition);
                        

                        
                        if (formatedList != null)
                        {
                            

                            double xcoord = Worker.ConvertToDouble(formatedList[1]);
                            double ycoord = Worker.ConvertToDouble(formatedList[2]);
                            double charh = Worker.ConvertToDouble(formatedList[3]);
                            //
                            try
                            {
                                Worker.CreateNewItem(ItemHolder.itemCollection.Count + 1, formatedList[0], bool.Parse(formatedList[4]), xcoord, ycoord, charh);
                            }
                            catch (Exception)
                            {
                                string errorResult = string.Empty;
                                foreach (var str in temp)
                                {
                                    errorResult += str;
                                }
                                MessageBox.Show($"line {errorResult} is wrong format - Programm skips this line");
                            }

                            //try
                            //{
                            //    Worker.CreateNewItem(ItemHolder.itemCollection.Count + 1, formatedList[0], bool.Parse(formatedList[4]), double.Parse(formatedList[1]), double.Parse(formatedList[2]), double.Parse(formatedList[3]));
                            //}
                            //catch (Exception)
                            //{
                            //    string errorResult = string.Empty;
                            //    foreach (var str in temp)
                            //    {
                            //        errorResult += str;
                            //    }
                            //    MessageBox.Show($"line {errorResult} is wrong format - Programm skips this line");
                            //}

                        }



                    }
                }

            }
        }
    }
}
