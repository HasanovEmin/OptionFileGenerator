using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace OptionFileGenerator
{
    public class Item : INotifyPropertyChanged
    {
        private int _id;
        public int Id 
        { 
            get => _id;
            set
            {
                _id = value;
                NotifyPropertyChanged();
            } 
        }
        private double _xCoord;
        public double XCoord
        {
            get => _xCoord;
            set
            {
                _xCoord = value;
                NotifyPropertyChanged();
            }
        }

        private double _yCoord;
        public double YCoord
        {
            get => _yCoord;
            set
            {
                _yCoord = value;
                NotifyPropertyChanged();
            }
        }

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                NotifyPropertyChanged();
            }
        }

        private bool _attribute;
        public bool Attribute
        {
            get => _attribute;
            set
            {
                _attribute = value;
                NotifyPropertyChanged();
            }
        }

        private double _charh = 2.1;
        public double Charh
        {
            get => _charh;
            set
            {
                _charh = value;
                NotifyPropertyChanged();
            }
        }

        private double _angle = 0;
        public double Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
