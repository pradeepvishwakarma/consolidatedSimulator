using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationService.Entities
{

    public class Coordinate
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class DistanceConverter:Coordinate
    {

        
        //distance in meter and angle in degree
        public void ComputeCoordinates( double lat, double lng,double distance,double angle)
        {
            double R = 6371;
            //double lat = 41.7109798584497;
            //double lng = -90.2952135273637;
            //double dist = 0.01; // kilo meters   1 meter =  0.001 km
            //double degree = 90;
            // convert to radians
            distance = distance * 0.001;
            lat = lat * (Math.PI / 180);
            lng = lng * (Math.PI / 180);
            angle = angle * (Math.PI / 180);
            
            var lat2 = Math.Asin(Math.Sin(lat) * Math.Cos(distance / R) +
              Math.Cos(lat) * Math.Sin(distance / R) * Math.Cos(angle));

            var lon2 = lng + Math.Atan2(Math.Sin(angle) * Math.Sin(distance / R) * Math.Cos(lat),
                     Math.Cos(distance / R) - Math.Sin(lat) * Math.Sin(lat2));

            // convert to degree
            latitude = lat2 / (Math.PI / 180);
            longitude = lon2 / (Math.PI / 180);
        }

        public double GetDistanceBetweenTwoPoints(Coordinate sourcePoint, Coordinate destinationPoint)
        {
            double diffX = destinationPoint.latitude - sourcePoint.latitude;
            double diffY = destinationPoint.longitude - sourcePoint.latitude;
            double value = Math.Sqrt(diffX * diffX + diffY * diffY);
            return value;

        }


    }
}
