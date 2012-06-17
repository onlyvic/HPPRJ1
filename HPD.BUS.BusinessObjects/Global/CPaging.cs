using System;
using System.Collections.Generic;
using System.Text;

namespace HPD.BUS.BusinessObjects.Global
{
    [Serializable]
    public class CPaging
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int RowPerPage { get; set; }
        public string SortColumn { get; set; }
        public string SortType { get; set; }
        public int TotalRows { get; set; }
        public CPaging() { }
        public CPaging(int totalPage, int currentPage, int rowPerPage)
        {
            this.TotalPage = totalPage;
            this.CurrentPage = currentPage;
            this.RowPerPage = rowPerPage;
        }
    }
}
