using System;

namespace SpaceShooterGame
{
    public class PaginationHelper : IPaginationHelper
    {
        public long GetTotalPageCount(int pageSize, long dataCount)
        {
            var totalPageCount = dataCount < pageSize ? 1 : (long)Math.Ceiling(dataCount / (decimal)pageSize);
            return totalPageCount;
        }

        public int GetNextPageNumber(
            long totalPageCount,
            int pageIndex)
        {
            pageIndex++;

            if (pageIndex > totalPageCount)
            {
                pageIndex = (int)totalPageCount;
            }

            return pageIndex;
        }

        public int GetPreviousPageNumber(int pageIndex)
        {
            pageIndex--;

            if (pageIndex < 0)
            {
                pageIndex = 0;
            }

            return pageIndex;
        }
    }
}
