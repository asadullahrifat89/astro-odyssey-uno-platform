using System;

namespace SpaceShooterGame
{
    public static class PaginationHelper
    {
        public static long GetTotalPageCount(int pageSize, long dataCount)
        {
            var totalPageCount = dataCount < pageSize ? 1 : (long)Math.Ceiling(dataCount / (decimal)pageSize);
            return totalPageCount;
        }

        public static int GetNextPageNumber(
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

        public static int GetPreviousPageNumber(int pageIndex)
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
