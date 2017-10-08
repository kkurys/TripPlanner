using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using TripPlanner.Business;

namespace TripPlanner.Core
{
    public class DataReader
    {
        public List<Hotel> ReadHotels(string filename)
        {
            List<Hotel> hotels = new List<Hotel>();
            int id = 301;
            string[] lines = File.ReadAllLines(filename, Encoding.GetEncoding("Windows-1250"));
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parsedLine = lines[i].Split(';');
                var newHotel = new Hotel()
                {
                    Id = id++,
                    Category = "Hotel",
                    City = parsedLine[0],
                    Name = parsedLine[1],
                    Lat = double.Parse(parsedLine[2].Split(' ')[0], CultureInfo.InvariantCulture),
                    Long = double.Parse(parsedLine[2].Split(' ')[1], CultureInfo.InvariantCulture),
                };
                hotels.Add(newHotel);
            }
            return hotels;
        }
        public List<Attraction> ReadAttractions(string filename)
        {
            List<Attraction> attractions = new List<Attraction>();
            int id = 1;
            string[] lines = File.ReadAllLines(filename, Encoding.GetEncoding("UTF-8"));
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parsedLine = lines[i].Split(';');
                parsedLine[2] = parsedLine[2].Replace('.', ',');
                parsedLine[3] = parsedLine[3].Replace('.', ',');
                var newAttraction = new Attraction()
                {
                    Id = id++,
                    Name = parsedLine[0],
                    Category = parsedLine[1],
                    Lat = double.Parse(parsedLine[2].Split(' ')[0], CultureInfo.InvariantCulture),
                    Long = double.Parse(parsedLine[2].Split(' ')[1], CultureInfo.InvariantCulture),
                    Profit = parsedLine[3] == "" ? 2.5 : double.Parse(parsedLine[3])
                };
                attractions.Add(newAttraction);
            }

            return attractions;
        }
    }
}
