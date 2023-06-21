using System;

namespace NoName.Helpers{
    public class PagingModel {
        public int currentPage { get; set; }
        public int countPages { get; set; }

        public Func<int?, string> generateUrl { get; set; }
    }
}