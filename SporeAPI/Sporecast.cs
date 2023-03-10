using System;
using System.Xml;

namespace SporeApi
{
    public class Sporecast
    {
        private readonly long _id;
        private string _name = "Unknown";
        private bool _nameInited = false;

        public Sporecast(long id) { _id = id; }

        public string Name
        {
            get
            {
                if (!_nameInited)
                {
                    _nameInited = true;
                    XmlReader reader = XmlReader.Create(GetSporecastRestUri(0));
                    while (!reader.EOF)
                    {
                        try
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "name")
                                _name = reader.ReadElementContentAsString();
                            else
                                reader.Read();
                        }
                        catch { break; }
                    }
                    reader.Close();
                    reader.Dispose();
                }

                return _name;
            }
        }
        public long ID => _id;

        public string GetSporecastRestUri(int length) =>
            GetSporecastRestUri(0, length);
        public string GetSporecastRestUri(int startIndex, int length) =>
            "http://www.spore.com/rest/assets/sporecast/" +
            _id + '/' + startIndex + '/' + length;
        /// <summary>
        /// Читает творения из споркаста
        /// </summary>
        /// <param name="length">Кол-во творений</param>
        /// <returns>Массив с ID творений</returns>
        /// <exception cref="XmlException">Недопустимый знак для указанной кодировки</exception>
        public long[] GetSporecastAssetsIDs(int length) =>
            GetSporecastAssetsIDs(0, length);
        /// <summary>
        /// Читает творения из споркаста
        /// </summary>
        /// <param name="startIndex">Индекс творения, с которого нужно начать</param>
        /// <param name="length">Кол-во творений</param>
        /// <returns>Массив с ID творений</returns>
        /// <exception cref="XmlException">Недопустимый знак для указанной кодировки</exception>
        public long[] GetSporecastAssetsIDs(int startIndex, int length)
        {
            long[] assets = null;
            XmlReader reader = XmlReader.Create(
                GetSporecastRestUri(startIndex, length));

            int indx = 0;
            while (!reader.EOF)
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "count")
                {
                    int count = reader.ReadElementContentAsInt();
                    if (count < length)
                        assets = new long[count];
                    else
                        assets = new long[length];
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.Name == "name" &&
                    !_nameInited)
                {
                    _nameInited = true;
                    _name = reader.ReadElementContentAsString();
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.Name == "id")
                    assets[indx++] = reader.ReadElementContentAsLong();
                else
                    reader.Read(); // System.Xml.XmlException: Недопустимый знак для указанной кодировки
            }

            reader.Close();
            reader.Dispose();
            return assets;
        }

        public override string ToString() => $"{Name} ({_id})";
    }
}
