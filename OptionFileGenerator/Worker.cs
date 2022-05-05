using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace OptionFileGenerator
{
    internal static class Worker
    {
        public static double BackingsheetHeight { get; set; }
        public static double BackingsheetWidth { get; set; }
        
        public static double CoefficientHeight { get; set; }
        public static double CoefficientWidth { get; set; }

        
        public static List<string> ListSizes { get; set; } = new List<string>()
        {
            BackSizes.A0.ToString(),
            BackSizes.A1.ToString(),
            BackSizes.A2.ToString(),
            BackSizes.A3.ToString(),
            BackSizes.A4.ToString()
        };
        
        static double[] A0 = { 841, 1189 };
        static double[] A1 = { 594, 841 };
        static double[] A2 = { 594, 420 };
        static double[] A3 = { 297, 420 };
        static double[] A4 = { 210, 297 };

        public static double CanvasChangedHeight { get; set; } = A2[0];
        public static double CanvasChangedWidth { get; set; } = A2[1];
        public static Dictionary<string, double[]> SizesDictionary { get; set; } = new Dictionary<string, double[]>()
        {
            { BackSizes.A0.ToString(), A0 },
            { BackSizes.A1.ToString(), A1 },
            { BackSizes.A2.ToString(), A2 },
            { BackSizes.A3.ToString(), A3 },
            { BackSizes.A4.ToString(), A4 }
        };

        public static double XCoord { get; private set; }
        public static double YCoord { get; private set; }
        public static string FileName { get; internal set; }

        // Calculates XCoord and YCoord
        internal static void CalculateCoordinate(Point pos)
        {
            XCoord = Math.Round(pos.X / CoefficientWidth, 3);
            YCoord = pos.Y / CoefficientHeight;
            YCoord = Math.Round(BackingsheetHeight - YCoord, 3);
        }

        // Executes from MainWindow
        // Create and add new Item to collection
        public static void CreateNewItem(string txtId, string txtText, bool isattribute, double charh, double angle)
        {
            ItemHolder.itemCollection.Add(
                    new Item()
                    {
                        Id = int.Parse(txtId),
                        XCoord = XCoord,
                        YCoord = YCoord,
                        Text = txtText,
                        Attribute = isattribute,
                        Charh = charh,
                        Angle = angle
                    }
                );
        }

        // Executes from ImportFile
        // Create and add new Item to collection
        public static void CreateNewItem(int id, string txtText, bool isattribute, double xCoord, double yCoord, double charh)
        {
            XCoord = xCoord;
            YCoord = yCoord;
            ItemHolder.itemCollection.Add(
                    new Item()
                    {
                        Id = id,
                        XCoord = XCoord,
                        YCoord = YCoord,
                        Text = txtText,
                        Attribute = isattribute,
                        Charh = charh,

                    }
                );
        }




        // Modify existing Item
        internal static void EditItem(int id, string text, bool isChecked, double xCoord, double yCoord, double charh, double angle)
        {
            ItemHolder.itemCollection[id].Text = text;
            ItemHolder.itemCollection[id].Attribute = isChecked;
            ItemHolder.itemCollection[id].XCoord = xCoord;
            ItemHolder.itemCollection[id].YCoord = yCoord;
            ItemHolder.itemCollection[id].Charh = charh;
            ItemHolder.itemCollection[id].Angle = angle;
        }


        // Delete Item
        internal static void DeleteItem(int oldId)
        {
            int idValue = ItemHolder.itemCollection[oldId].Id;
            foreach (Item item in ItemHolder.itemCollection)
            {
                if (item.Id > idValue)
                {
                    item.Id -= 1;
                }
            }

            ItemHolder.itemCollection.RemoveAt(oldId);

        }

        // Delete all Items
        internal static void DeleteAllProgress()
        {
            ItemHolder.itemCollection.Clear();
        }


        // Sets Sheets coefficients for mapping
        internal static void SetBackendSheetCoefficients(double canvasChangedHeight, double canvasChangedWidth)
        {
            if (canvasChangedHeight != 0 && canvasChangedWidth != 0)
            {
                CanvasChangedHeight = canvasChangedHeight;
                CanvasChangedWidth = canvasChangedWidth;
            }
            CoefficientHeight = CanvasChangedHeight / BackingsheetHeight;
            CoefficientWidth = CanvasChangedWidth / BackingsheetWidth;
        }


        // Change Size of backingsheet
        internal static void SetBacksheetSize(string size)
        {
            SizesDictionary.TryGetValue(size, out double[] newSize);
            BackingsheetWidth = newSize[0];
            BackingsheetHeight = newSize[1];
        }

        // Generates and returns list of formated imported line 
        internal static List<string> GetFormattedList(List<string> temp, string textPosition)
        {
            
            List<string> list = new List<string>();
            string textPos = "textpos";
            string x = "x";
            string y = "y";
            string charh = "charh";
            

            //int index = temp.IndexOf("Textposition");
            for(int i = 0; i < temp.Count; i++)
            {
                if (temp[i].ToLower().StartsWith(textPos))
                {
                    if (textPosition != null && textPosition != string.Empty)
                    {
                        list.Insert(0, textPosition);
                    }
                    else
                    {
                        list.Insert(0, temp[i + 1]);
                    }
                    
                }
                else if (temp[i].ToLower().StartsWith(x))
                {
                    try
                    {
                        if (temp[i + 1].Contains("mm"))
                        {
                            list.Insert(1, temp[i + 1].Substring(0, temp[i + 1].IndexOf("mm")));
                        }
                        else
                        {
                            list.Insert(1, temp[i + 1]);
                        }

                    }
                    catch (Exception)
                    {
                        string errorResult = string.Empty;
                        foreach (var item in temp)
                        {
                            errorResult += item.ToString();
                        }
                        MessageBox.Show($"line {errorResult} is wrong format - Programm skips this line");
                        return null;
                    }
                }
                else if (temp[i].ToLower().StartsWith(y))
                {
                    try
                    {
                        if (temp[i + 1].Contains("mm"))
                        {
                            list.Insert(2, temp[i + 1].Substring(0, temp[i + 1].IndexOf("mm")));
                        }
                        else
                        {
                            list.Insert(2, temp[i + 1]);
                        }

                    }
                    catch (Exception)
                    {
                        string errorResult = string.Empty;
                        foreach (var item in temp)
                        {
                            errorResult += item.ToString();
                        }
                        MessageBox.Show($"line {errorResult} is wrong format - Programm skips this line");
                        return null;
                    }


                }
                else if (temp[i].ToLower().StartsWith(charh))
                {
                    try
                    {
                        if (temp[i + 1].Contains("mm"))
                        {
                            list.Insert(3, temp[i + 1].Substring(0, temp[i + 1].IndexOf("mm")));
                        }
                        else
                        {
                            list.Insert(3, temp[i + 1]);
                        }
                        
                    }
                    catch (Exception)
                    {
                        
                        return null;
                    }
                    
                    
                }
            }
            if (textPosition != null && textPosition != string.Empty)
            {
                list.Insert(4, "false");
            }
            else
            {
                list.Insert(4, "true");
            }
            return list;
        }

        public static double ConverToDouble(string value)
        {
            value = value.Replace(',', '.');
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            double convertedValue = Convert.ToDouble(value, provider);
            
            return convertedValue;
        }

        public static string ConverToString(double value)
        {
            
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            string convertedValue = Convert.ToString(value, provider);

            return convertedValue;
        }
    }
}
