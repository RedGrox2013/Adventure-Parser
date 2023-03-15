using System.Xml;

namespace SporeApi
{
    public static class RestService
    {
        public const string STATS_URI = "http://www.spore.com/rest/stats";
        public static long TotalUploads => GetContent("totalUploads");
        public static long DayUploads => GetContent("dayUploads");
        public static long TotalUsers => GetContent("totalUsers");
        public static long DayUsers => GetContent("dayUsers");

        private static long GetContent(string name)
        {
            long content = 0;
            XmlReader reader = XmlReader.Create(STATS_URI);
            while (!reader.EOF)
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == name)
                {
                    content = reader.ReadElementContentAsLong();
                    break;
                }
                else
                    reader.Read();
            }

            reader.Close();
            return content;
        }
    }
}
