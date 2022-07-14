using System;

namespace AstroOdyssey
{
    public class HighScore
    {
        public DateTime Date { get; set; } = DateTime.Now;

        public GameScore GameScore { get; set; }
    }
}
