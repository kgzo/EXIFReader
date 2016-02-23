using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;

namespace EXIFReader
{
    class EXIFData
    {
        private string picturePath;
        private double lat;
        private bool gpsFound = false;
        public double Lat
        {
            get { return lat; }
            set { lat = value; }
        }

        private double longi;

        public double Longi
        {
            get { return longi; }
            set { longi = value; }
        }

        public string PicturePath
        {
            get { return picturePath; }
            set { picturePath = value; }
        }
        private System.Drawing.Image image;
        Bitmap bitmao;
        public EXIFData(string name)
        {
            this.picturePath = name;
        }

        public void extractInfo(string path)
        {
            try
            {
                image = new Bitmap(path);
                getLatLong(image);
                openWebsite(longi, lat);
                int dsa = 1;
            }

            catch
            {
                Console.WriteLine("You must select a picture");
                Console.ReadLine();
            }
        }



        public void getLatLong(System.Drawing.Image image)
        {
            try
            {
                PropertyItem p1 = image.GetPropertyItem(0x0001);
                byte[] array = new byte[p1.Len];
                for (int i = 0; i < array.Length; i++)
                    array[i] = p1.Value[i];
                string sarray = Encoding.UTF8.GetString(array);
                PropertyItem p2 = image.GetPropertyItem(0x0002);
                lat = convertToCoordinates(p2);
                PropertyItem p3 = image.GetPropertyItem(0x0003);
                PropertyItem p4 = image.GetPropertyItem(0x0004);
                byte[] array1 = new byte[p1.Len];
                for (int i = 0; i < array.Length; i++)
                    array1[i] = p3.Value[i];
                longi = convertToCoordinates(p4);
                string sarray1 = Encoding.UTF8.GetString(array1);
                if (sarray.Contains('S') || sarray.Contains('s'))
                    lat = -lat;
                if (sarray1.Contains('W') || sarray1.Contains('w'))
                    longi = -longi;
                int dasa = 3;
                gpsFound = true;
            }

            catch (Exception e)
            {
                //System.Windows.MessageBox.Show("No data found");
                Console.WriteLine("No GPS data found");
                Console.ReadLine();
                
            }
        }

        private double convertToCoordinates(PropertyItem p1)
        {
            try
            {
                uint degreesNominator = BitConverter.ToUInt32(p1.Value, 0);
                uint degreesDenominator = BitConverter.ToUInt32(p1.Value, 4);
                double degrees = degreesNominator / (double)degreesDenominator;
                uint minutesNominator = BitConverter.ToUInt32(p1.Value, 8);
                uint minutesDenominator = BitConverter.ToUInt32(p1.Value, 12);
                double minutes = minutesNominator / (double)minutesDenominator;
                uint secondsNominator = BitConverter.ToUInt32(p1.Value, 16);
                uint secondsDenominator = BitConverter.ToUInt32(p1.Value, 20);
                double seconds = secondsNominator / (double)secondsDenominator;
                double coordinates = degrees + (minutes / 60d) + (seconds / 3600d);
                return coordinates;
            }

            catch
            {
                //System.Windows.MessageBox.Show("Can't convert to coordinates");
                return 0;
            }
        }

        private void openWebsite(double longi, double latit)
        {
            if (gpsFound)
            {
                string googleMapOptions = ",15z?hl=en";
                string googleMapUrl = "http://maps.google.com/maps?q=loc:";
                string help = longi.ToString();
                string help1 = latit.ToString();
                string finalUrl = googleMapUrl + convertCommaToDot(help1) + "," + convertCommaToDot(help);

                System.Diagnostics.Process.Start(finalUrl);
            }
        }

        private string convertCommaToDot(string url)
        {
            string test = url.Replace(',', '.');
            return test;
        }
    }
}
    


