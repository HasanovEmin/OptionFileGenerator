using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace OptionFileGenerator
{
    internal class ExportFile
    {
        public string Filename { get; set; }
        internal void Export(ItemCollection collectionFromList)
        {          
            
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (.txt)|*.txt";

            bool? result = saveFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                Filename = saveFileDialog.FileName;
                using (FileStream fs = new FileStream(Filename, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {

                        foreach (var row in collectionFromList)
                        {
                            Item item = row as Item;
                            string text = string.Empty;
                            if (!item.Attribute)
                            {
                                text = $@"'{item.Text}'";
                            }
                            else
                                text = $"{item.Text}";

                            string angle = string.Empty;
                            if (item.Angle != 0)
                            {
                                angle = $"Angle {Worker.ConverToString(item.Angle)}";
                            }
                            

                            string xcoord = Worker.ConverToString(item.XCoord);
                            string ycoord = Worker.ConverToString(item.YCoord);
                            string charh = Worker.ConverToString(item.Charh);
                            sw.WriteLine($"TextPosition {text} X {xcoord}mm Y {ycoord}mm CharHeight {charh}mm {angle}");




                            //sw.WriteLine($"TextPosition {text} X {item.XCoord}mm Y {item.YCoord}mm CharHeight {item.Charh}mm {angle}");
                        }

                    }
                }
            }
        }
    }
}
