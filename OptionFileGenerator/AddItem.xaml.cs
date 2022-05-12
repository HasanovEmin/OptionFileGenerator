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
            TxtXCoord.Text = Worker.ConvertToString(Worker.XCoord);
            TxtYCoord.Text = Worker.ConvertToString(Worker.YCoord);
            Item item = new Item();
            TxtCharh.Text = Worker.ConvertToString(item.Charh);
            TxtAngle.Text = Worker.ConvertToString(item.Angle);
            
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            double xcoord = Worker.ConvertToDouble(TxtXCoord.Text);
            double ycoord = Worker.ConvertToDouble(TxtYCoord.Text);
            double charh = Worker.ConvertToDouble(TxtCharh.Text);
            double angle = Worker.ConvertToDouble(TxtAngle.Text);

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


            TxtXCoord.Text = Worker.ConvertToString(xCoord);
            TxtYCoord.Text = Worker.ConvertToString(yCoord);
            TxtCharh.Text = Worker.ConvertToString(charh);
            TxtAngle.Text = Worker.ConvertToString(angle);

        }

        private void btnEditCoords_Click(object sender, RoutedEventArgs e)
        {
            EditCanvas editCanvas = new EditCanvas();
            editCanvas.ShowDialog();
            TxtXCoord.Text = Worker.ConvertToString(Worker.XCoord);
            TxtYCoord.Text = Worker.ConvertToString(Worker.YCoord);
        }
    }
}
