using Personnel.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Core.Contracts
{
    [Serializable]
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        /// <summary>
        /// Ctor (paging in performed inside)
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            Init(source, pageIndex > 0 ? pageIndex - 1 : 0, pageSize);
        }
        public PagedList(List<T> source, int pageIndex, int pageSize, int totalCount)
        {
            InitList(source, pageIndex > 0 ? pageIndex - 1 : 0, pageSize, totalCount);
        }

        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            Init(source, pageIndex > 0 ? pageIndex - 1 : 0, pageSize);
        }

        /// <summary>
        /// Ctor (already paged soure is passed)
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            Init(source, pageIndex, pageSize, totalCount);
        }



        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        private void Init(IQueryable<T> source, int pageIndex, int pageSize, int? totalCount = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (pageIndex == 0 && pageSize == 0)
                pageSize = source.Count();

            if (pageSize <= 0)
            {
                TotalCount = 0;
                TotalPages = 0;
                return;
            }

            TotalCount = totalCount ?? source.Count();
            TotalPages = TotalCount / pageSize;



            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            source = totalCount == null ? source.Skip(pageIndex * pageSize).Take(pageSize) : source;
            AddRange(source);
        }
        private void InitList(List<T> source, int pageIndex, int pageSize, int totalCount)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (source.Any())

                if (pageIndex == 0 && pageSize == 0)
                    pageSize = source.Count();

            if (pageSize <= 0)
            {
                TotalCount = 0;
                TotalPages = 0;
                return;
            }

            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;



            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source);
        }
        private void Init(IList<T> source, int pageIndex, int pageSize, int? totalCount = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (pageIndex == 0 && pageSize == 0)
                pageSize = source.Count();

            if (pageSize <= 0)
            {
                TotalCount = 0;
                TotalPages = 0;
                return;
            }

            TotalCount = totalCount ?? source.Count();
            TotalPages = TotalCount / pageSize;



            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            source = totalCount == null ? source.Skip(pageIndex * pageSize).Take(pageSize).ToList() : source;
            AddRange(source);
        }

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }
    }
}
