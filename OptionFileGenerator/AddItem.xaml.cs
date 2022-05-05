using System.Globalization;
using System.Windows;

namespace OptionFileGenerator
{
    /// <summary>
    /// Interaction logic for AddItem.xaml
    /// </summary>
    public partial class AddItem : Window
    {
        public string OldTxtText { get; set; }
        public bool OldAttribute { get; set; }
        public int OldId { get; private set; }
        public int Id { get; set; }

        private double oldXcoord;
        private double oldYcoord;
        private double oldCharh;
        private double oldAngle;

        public AddItem()
        {
            InitializeComponent();
            //NumberFormatInfo provider = new NumberFormatInfo();
            //provider.NumberDecimalSeparator = ".";
            TxtId.Text = (ItemHolder.itemCollection.Count + 1).ToString();
            TxtXCoord.Text = Worker.ConverToString(Worker.XCoord);
            TxtYCoord.Text = Worker.ConverToString(Worker.YCoord);
            Item item = new Item();
            TxtCharh.Text = Worker.ConverToString(item.Charh);
            TxtAngle.Text = Worker.ConverToString(item.Angle);
            
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            double xcoord = Worker.ConverToDouble(TxtXCoord.Text);
            double ycoord = Worker.ConverToDouble(TxtYCoord.Text);
            double charh = Worker.ConverToDouble(TxtCharh.Text);
            double angle = Worker.ConverToDouble(TxtAngle.Text);

            if (TxtText.Text == OldTxtText 
                && chkAtrrib.IsChecked == OldAttribute 
                && xcoord == oldXcoord 
                && ycoord == oldYcoord 
                && charh == oldCharh 
                && angle == oldAngle) 
            {
                this.Close();
                return;
            }



            if (OldId == int.Parse(TxtId.Text) && 
                (TxtText.Text != OldTxtText 
                || chkAtrrib.IsChecked != OldAttribute 
                || xcoord != oldXcoord 
                || ycoord != oldYcoord
                || charh != oldCharh 
                || angle != oldAngle))
            {

                Worker.EditItem(Id, TxtText.Text, (bool)chkAtrrib.IsChecked, xcoord, ycoord, charh, angle );
                
            }
            else
            {
                
                Worker.CreateNewItem(TxtId.Text, TxtText.Text, (bool)chkAtrrib.IsChecked, charh, angle);
            }
            
                

            this.Close();
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        internal void FillEditableProperties(int id, string text, bool attribute, double xCoord, double yCoord, double charh, double angle)
        {
            TxtId.Text = id.ToString();
            TxtText.Text = text.ToString();
            chkAtrrib.IsChecked = attribute;
            
            OldTxtText = text.ToString();
            OldAttribute = attribute;
            OldId = id;
            Id = id - 1;

            oldXcoord = xCoord;
            oldYcoord = yCoord;
            oldAngle = angle;
            oldCharh = charh;


            TxtXCoord.Text = Worker.ConverToString(xCoord);
            TxtYCoord.Text = Worker.ConverToString(yCoord);
            TxtCharh.Text = Worker.ConverToString(charh);
            TxtAngle.Text = Worker.ConverToString(angle);

        }

        private void btnEditCoords_Click(object sender, RoutedEventArgs e)
        {
            EditCanvas editCanvas = new EditCanvas();
            editCanvas.ShowDialog();
            TxtXCoord.Text = Worker.ConverToString(Worker.XCoord);
            TxtYCoord.Text = Worker.ConverToString(Worker.YCoord);
        }
    }
}
