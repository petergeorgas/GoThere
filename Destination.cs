using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;

namespace GoThere
{
    public class Destination
    {
        public Vector3 location = new Vector3();
        public float heading;
        public String name;

        public Destination(String destinationName, Vector3 destinationLocation, float destinationHeading)
        {
            name = destinationName;
            location = destinationLocation;
            heading = destinationHeading;
        }

        // Will return the name of the Destination object
        public String getName()
        {
            return name;
        }

        //Will return the location of the Destination object
        public Vector3 getLocation()
        {
            return location;
        }

        //Will return the heading of the Destination object
        public float getHeading()
        {
            return heading;
        }

        //Converts the Destination object to a string
        public String toString()
        {
            String destinationString = name + "\n" + location.ToString() + "\n Heading: " + heading;
            return destinationString;
        }
    }
}
