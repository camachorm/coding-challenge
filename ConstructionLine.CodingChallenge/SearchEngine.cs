using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _sizeIndex;
        private readonly List<Shirt> _colorIndex;

        public SearchEngine(List<Shirt> shirts)
        {
            _sizeIndex = shirts.OrderBy(shirt => shirt.Size.Id).ThenBy(shirt => shirt.Color.Id).ToList();
            _colorIndex = shirts.OrderBy(shirt => shirt.Color.Id).ThenBy(shirt => shirt.Size.Id).ToList();
        }


        public SearchResults Search(SearchOptions options)
        {
            var result = new SearchResults();

            // TODO: search logic goes here.
            if (!options.Colors.Any() && !options.Sizes.Any())
                return result;
            
            var useColorAsRoot = options.Colors.Count > options.Sizes.Count;
            var idList = useColorAsRoot
                ? options.Colors.OrderBy(color => color.Id).Select(color => color.Id)
                : options.Sizes.OrderBy(size => size.Id).Select(size => size.Id);

            var rootQueryable = useColorAsRoot
                ? _colorIndex.AsQueryable()
                : _sizeIndex.AsQueryable();

            var secondaryQueryable = (useColorAsRoot
                ? options.Sizes.Select(size => size.Id)
                : options.Colors.Select(color => color.Id))
                .ToList();

            var hasSecondaryQuery = secondaryQueryable.Any();

            var maxId = useColorAsRoot
                ? options.Colors.Max(color => color.Id)
                : options.Sizes.Max(size => size.Id);

            var minId = idList.First();

            foreach (var shirt in rootQueryable)
            {
                var rootComparisonId = ReadId(useColorAsRoot, shirt);
                if (rootComparisonId.CompareTo(maxId) > 0)
                {
                    break;
                }
                if (rootComparisonId.CompareTo(minId) < 0)
                {
                    continue;
                }

                if (!hasSecondaryQuery ||
                    secondaryQueryable.Any(guid => guid.CompareTo(ReadId(!useColorAsRoot, shirt)) == 0))
                {
                    result.Shirts.Add(shirt);
                    result.ColorCounts.First(count => count.Color.Id.CompareTo(shirt.Color.Id) == 0).Count++;
                    result.SizeCounts.First(count => count.Size.Id.CompareTo(shirt.Size.Id) == 0).Count++;
                }
            }


            return result;
        }

        private Guid ReadId(bool useColorAsRoot, Shirt shirt)
        {
            return useColorAsRoot ? shirt.Color.Id : shirt.Size.Id;
        }
    }
}