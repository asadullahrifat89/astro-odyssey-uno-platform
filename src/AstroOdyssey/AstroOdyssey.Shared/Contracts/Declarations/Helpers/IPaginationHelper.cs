namespace AstroOdyssey
{
    public interface IPaginationHelper
    {
        long GetTotalPageCount(int pageSize, long dataCount);

        int GetNextPageNumber(
            long totalPageCount,
            int pageIndex);

        int GetPreviousPageNumber(int pageIndex);
    }
}
