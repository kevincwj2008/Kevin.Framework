using System.Collections.Generic;

namespace Kevin.Framework.Infrastructure.PagingQuery
{
    public class PageResult<T> : IPageResult<T>
    {

        public PageResult()
        {
            this.Data = new List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items">结果</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页条数</param>
        public PageResult(IEnumerable<T> data, int totalCount, int pageIndex, int pageSize)
        {
            Data = data;
            TotalCount = totalCount;
            CurrentPage = pageIndex;
            TotalPage = TotalCount / pageSize + (TotalCount % pageSize == 0 ? 0 : 1);
        }

        /// <summary>
        /// 列表总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 列表项
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; set; }
    }
}
