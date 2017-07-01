namespace NPAInspectionWriter.DTO
{
    public class PagingInfoDTO
    {
        public string CurPage { get; set; }
        public string MaxPage { get; set; }
        public string ItemsPerPage { get; set; }
        public string CurStartItemNum { get; set; }
        public string CurEndItemNum { get; set; }
        public string MaxItemNum { get; set; }

        public PagingInfoDTO()
        {
            CurPage = string.Empty;
            MaxPage = string.Empty;
            ItemsPerPage = string.Empty;
            CurStartItemNum = string.Empty;
            CurEndItemNum = string.Empty;
            MaxItemNum = string.Empty;
        }
    }
}
