using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooterGame
{
    public static class ScalingHelper
    {
        public static double GetGameObjectScale(double windowWidth)
        {
            return windowWidth switch
            {
                <= 300 => 0.60,
                <= 400 => 0.65,
                <= 500 => 0.70,
                <= 700 => 0.75,
                <= 900 => 0.80,
                <= 1000 => 0.85,
                <= 1400 => 0.90,
                <= 2000 => 0.95,
                _ => 1,

                //<= 300 => 0.70,
                //<= 500 => 0.75,
                //<= 700 => 0.85,
                //<= 900 => 0.90,
                //<= 1000 => 1,
                //<= 1400 => 1.1,
                //<= 2000 => 1.2,
                //_ => 1,
            };
        }
    }
}
