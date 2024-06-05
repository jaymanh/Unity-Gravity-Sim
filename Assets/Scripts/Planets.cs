using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlanetsInfo
{
    public class Planets
    {
        public string Name { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double PositionZ { get; set; }
        public double Mass { get; set; }
        public double Radius { get; set; }
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
        public double VelocityZ { get; set; }

        // Planets information
        void Start()
        {

        }
        /* 
            Name:
            Position X , Y, Z:
            Mass:
            Radius:
            Velocity: X, Y, Z:
        */

        public Planets(string name, double posX, double posY, double posZ, double mass, double radius, double velX, double velY, double velZ)
        {
            Name = name;
            PositionX = posX;
            PositionY = posY;
            PositionZ = posZ;
            Mass = mass;
            Radius = radius;
            VelocityX = velX;
            VelocityY = velY;
            VelocityZ = velZ;
        }
    }
}