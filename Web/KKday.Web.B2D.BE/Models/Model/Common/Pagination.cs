using System;
namespace KKday.Web.B2D.BE.Models.Model.Common
{
    [Serializable]
    public class Pagination
    {
        public int current_page { get; set; }
        public int total_count { get; set; }
        public int total_pages { get; set; }
        public int page_size { get; set; }
    }
}
