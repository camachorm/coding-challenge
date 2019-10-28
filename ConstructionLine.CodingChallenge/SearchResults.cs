using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchResults
    {
        public List<Shirt> Shirts { get; set; } = new List<Shirt>();


        public List<SizeCount> SizeCounts { get; set; } = Size.All.Select(size => new SizeCount { Size = size, Count = 0 }).ToList();


        public List<ColorCount> ColorCounts { get; set; } = Color.All.Select(color => new ColorCount { Color = color, Count = 0 }).ToList();
    }
}