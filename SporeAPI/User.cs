using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SporeApi
{
    public class User
    {
        private readonly string _name;
        private bool _status;
        private long? _id;
        private string _image;
        private string _tagline;
        private DateTime _createDate;

        private bool _sporecastsInited = false;
        private List<long> _sporecasts;

        /// <summary>
        /// Создаёт объект класса User
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <exception cref="ArgumentNullException"></exception>
        public User(string userName)
        {
            _name = userName ?? throw new ArgumentNullException();
            ParseRest();
            if (!_status)
                _name = "Deleted user";
        }

        public string Name => _name;
        public bool Status => _status;
        public long? ID => _id;
        public string Image => _image;
        public string Tagline => _tagline;
        public DateTime CreateDate => _createDate;
        public int SporecastsCount
        {
            get
            {
                SetSporecasts();
                if (_sporecasts == null)
                    return 0;
                return _sporecasts.Count;
            }
        }
        public long GetSporecastIDAt(int index)
        {
            SetSporecasts();
            return _sporecasts[index];
        }
        public Sporecast GetSporecastAt(int index) =>
            new Sporecast(GetSporecastIDAt(index));

        public string RestUri =>
            "http://www.spore.com/rest/user/" + _name;
        public string ProfileUri =>
            "http://www.spore.com/view/myspore/" + _name;

        private void ParseRest()
        {
            XmlReader reader = XmlReader.Create(RestUri);
            while (!reader.EOF)
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "status":
                            _status = reader.ReadElementContentAsBoolean();
                            break;
                        case "id":
                            _id = reader.ReadElementContentAsLong();
                            break;
                        case "image":
                            _image = reader.ReadElementContentAsString();
                            break;
                        case "tagline":
                            _tagline = reader.ReadElementContentAsString();
                            break;
                        case "creation":
                            _createDate = DateTime.Parse(
                                reader.ReadElementContentAsString());
                            break;
                        default:
                            reader.Read();
                            break;
                    }
                }
                else
                    reader.Read();
            }

            reader.Close();
            reader.Dispose();
        }

        private void SetSporecasts()
        {
            if (_sporecastsInited)
                return;

            _sporecastsInited = true;
            XmlReader reader = XmlReader.Create(GetSporecastsRestUri());
            while (!reader.EOF)
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "length")
                {
                    int len = reader.ReadElementContentAsInt();
                    if (len == 0)
                        break;
                    _sporecasts = new List<long>(len);
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "id")
                    _sporecasts.Add(reader.ReadElementContentAsLong());
                else
                    reader.Read();
            }

            reader.Close();
            reader.Dispose();
        }

        public string GetSporecastsRestUri() =>
            "http://www.spore.com/rest/sporecasts/" + _name;

        public string GetAssetsRestUri(int length) =>
            GetAssetsRestUri(0, length);
        public string GetAssetsRestUri(int startIndex, int length) =>
            "https://www.spore.com/rest/assets/user/" +
                _name + '/' + startIndex + '/' + length;
        public long[] GetAssetsIDs(int length) =>
            GetAssetsIDs(0, length);
        public long[] GetAssetsIDs(int startIndex, int length)
        {
            long[] assets = null;
            XmlReader reader = XmlReader.Create(
                GetAssetsRestUri(startIndex, length));

            int indx = 0;
            while (!reader.EOF)
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "count")
                {
                    int count = reader.ReadElementContentAsInt();
                    if (count == 0)
                        break;
                    if (length > count)
                        assets = new long[count];
                    else
                        assets = new long[length];
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.Name == "id")
                    assets[indx++] = reader.ReadElementContentAsLong();
                else
                    reader.Read();
            }

            reader.Close();
            reader.Dispose();
            return assets;
        }

        public override string ToString() => $"{_name} ({_id})";
    }
}

// Доделать Achievements for User