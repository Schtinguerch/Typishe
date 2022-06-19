using System;

namespace Typishe.Service
{
    public struct HsvColor
    {
        private double _hue, _saturation, _brightness;

        public double Hue
        {
            get => _hue;
            set
            {
                if (value < 0 || value > 360)
                {
                    throw new ArgumentException("Hue value is out of [0; 360]");
                }

                _hue = value;
            }
        }

        public double Saturation
        {
            get => _saturation;
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentException("Saturation value is out of [0; 1]");
                }

                _saturation = value;
            }
        }

        public double Brightness
        {
            get => _brightness;
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentException("Brightness value is out of [0; 1]");
                }

                _brightness = value;
            }
        }
    }
}
