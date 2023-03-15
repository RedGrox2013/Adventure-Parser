using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

/*
 * Documentation:
 * http://www.spore.com/comm/developer/
 * http://www.spore.com/comm/samples
 */

namespace SporeApi.Creations
{
    public class Creation
    {
        protected Modeltype _modeltype;
        protected readonly long _id;
        protected bool _status;
        protected string _name;
        protected string _author;
        protected long? _authorId;
        protected DateTime _createDate;
        protected string _description;
        protected string[] _tags;
        protected double? _rating;
        protected long? _parent;
        protected List<Comment> _comments;

        public const int ID_LENGTH = 12;

        /// <summary>
        /// Создаёт объект класса Creation
        /// </summary>
        /// <param name="id">ID творения</param>
        /// <exception cref="ArgumentException"></exception>
        public Creation(long id)
        {
            if (id.ToString().Length != ID_LENGTH)
                throw new ArgumentException("ID творения имеет неверный формат.");

            _id = id;
            _comments = new List<Comment>();
            ParseRest();
            if (!_status)
            {
                ParseXML();
                _name = "Deleted creation " + id;
                _author = "Unknown";
            }
        }
        /// <summary>
        /// Создаёт объект класса Creation
        /// </summary>
        /// <param name="creation"></param>
        public Creation(Creation creation)
        {
            _modeltype = creation._modeltype;
            _id = creation._id;
            _status = creation._status;
            _name = creation._name;
            _author = creation._author;
            _authorId = creation._authorId;
            _createDate = creation._createDate;
            _description = creation._description;
            _rating = creation._rating;
            _parent = creation._parent;

            if (creation._tags != null)
            {
                _tags = new string[creation.TagsCount];
                for (int i = 0; i < creation.TagsCount; i++)
                    _tags[i] = creation._tags[i];
            }
            if (creation._comments != null)
                _comments = new List<Comment>(creation._comments);
        }

        public Modeltype ModelType => _modeltype;
        public long ID => _id;
        public bool Status => _status;
        public string Name => _name;
        public string Author => _author;
        public long? AuthorID => _authorId;
        public DateTime CreateDate => _createDate;
        public string Description => _description;
        public double? Rating => _rating;
        public long? Parent => _parent;
        public int CommentsCount => _comments.Count;
        public int TagsCount
        {
            get
            {
                if (_tags == null)
                    return 0;
                return _tags.Length;
            }
        }

        public string XmlUri
        {
            get
            {
                string strId = _id.ToString();
                return "http://www.spore.com/static/model/" + strId.Substring(0, 3) +
                    '/' + strId.Substring(3, 3) + '/' +
                    strId.Substring(6, 3) + '/' + strId + ".xml";
            }
        }
        public string RestUri =>
            "https://www.spore.com/rest/asset/" + _id;
        public string SmallPngUri
        {
            get
            {
                string strId = _id.ToString();
                return "http://www.spore.com/static/thumb/" + strId.Substring(0, 3) +
                    '/' + strId.Substring(3, 3) + '/' +
                    strId.Substring(6, 3) + '/' + strId + ".png";
            }
        }
        public string LargePngUri
        {
            get
            {
                string strId = _id.ToString();
                return "http://www.spore.com/static/image/" + strId.Substring(0, 3) +
                    '/' + strId.Substring(3, 3) + '/' +
                    strId.Substring(6, 3) + '/' + strId + "_lrg.png";
            }
        }
        public string SporepediaSmallUri =>
            "http://www.spore.com/sporepedia#qry=ast-" + _id;
        public string SporepediaLargeUri =>
            $"http://www.spore.com/sporepedia#qry=ast-{_id}:sast-{_id}";

        /// <summary>
        /// Получить автора творения
        /// </summary>
        /// <returns>Объект класса User, являющийся автором творения</returns>
        public User GetUser()
        {
            if (!_status)
                return null;
            return new User(_author);
        }
        /// <summary>
        /// Получить тег по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Тег творения</returns>
        public string GetTagAt(int index) =>
            string.Copy(_tags[index]);
        /// <summary>
        /// Получит комментарий к творению по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Комментарий по индексу</returns>
        public Comment GetCommentAt(int index) =>
            _comments[index];

        /// <summary>
        /// Проверяет творение
        /// </summary>
        /// <returns>true, если творение является приключением</returns>
        public bool IsAdventure
        {
            get
            {
                switch (_modeltype)
                {
                    case Modeltype.AdventureAttack:
                    case Modeltype.AdventureCollect:
                    case Modeltype.AdventureDefend:
                    case Modeltype.AdventureExplore:
                    case Modeltype.AdventureSocialize:
                    case Modeltype.AdventureStory:
                    case Modeltype.AdventureNoGenre:
                    case Modeltype.AdventurePuzzle:
                    case Modeltype.AdventureQuest:
                    case Modeltype.AdventureTemplate:
                        return true;
                    default:
                        return false;
                }
            }
        }
        /// <summary>
        /// Проверяет творение
        /// </summary>
        /// <returns>true, если творение является существом</returns>
        public bool IsCreature
        {
            get
            {
                switch (_modeltype)
                {
                    case Modeltype.CreatureAnimal:
                    case Modeltype.CreatureTribal:
                    case Modeltype.CreatureCiv:
                    case Modeltype.CreatureSpace:
                    case Modeltype.CreatureCaptain:
                        return true;
                    default:
                        return false;
                }
            }
        }
        /// <summary>
        /// Проверяет творение
        /// </summary>
        /// <returns>true, если творение является постройкой</returns>
        public bool IsBuilding
        {
            get
            {
                switch (_modeltype)
                {
                    case Modeltype.BuildCityHall:
                    case Modeltype.BuildHouse:
                    case Modeltype.BuildFactory:
                    case Modeltype.BuildEntertainment:
                        return true;
                    default:
                        return false;
                }
            }
        }
        /// <summary>
        /// Проверяет творение
        /// </summary>
        /// <returns>true, если творение является техникой</returns>
        public bool IsVehicle
        {
            get
            {
                switch (_modeltype)
                {
                    case Modeltype.VehicleSpaceship:
                    case Modeltype.VehicleMilitaryLand:
                    case Modeltype.VehicleMilitaryAir:
                    case Modeltype.VehicleMilitarySea:
                    case Modeltype.VehicleEconomicLand:
                    case Modeltype.VehicleEconomicAir:
                    case Modeltype.VehicleEconomicSea:
                    case Modeltype.VehicleReligiousLand:
                    case Modeltype.VehicleReligiousAir:
                    case Modeltype.VehicleReligiousSea:
                    case Modeltype.VehicleColonyLand:
                    case Modeltype.VehicleColonyAir:
                    case Modeltype.VehicleColonySea:
                        return true;
                    default:
                        return false;
                }
            }
        }
        /// <summary>
        /// Проверяет творение
        /// </summary>
        /// <returns>true, если творение является косм. кораблём</returns>
        public bool IsUFO =>
            _modeltype == Modeltype.VehicleSpaceship;

        /// <summary>
        /// Парсит xml-файл творения (http://www.spore.com/static/model/subId1/subId2/subId3/AssetId.xml)
        /// </summary>
        protected virtual void ParseXML()
        {
            XmlReader reader = XmlReader.Create(XmlUri);
            while (!reader.EOF)
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "modeltype")
                {
                    _modeltype = (Modeltype)Convert.ToUInt32(reader.ReadElementContentAsString(), 16);
                    break;
                }
                else
                    reader.Read();
            }

            reader.Close();
        }
        protected void ParseRest()
        {
            XmlReader reader = null;
            try
            {
                reader = XmlReader.Create(RestUri);
                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "status":
                                _status = reader.ReadElementContentAsBoolean();
                                break;
                            case "name":
                                _name = reader.ReadElementContentAsString();
                                break;
                            case "author":
                                _author = reader.ReadElementContentAsString();
                                break;
                            case "authorid":
                                _authorId = reader.ReadElementContentAsLong();
                                break;
                            case "created":
                                _createDate = DateTime.Parse(
                                    reader.ReadElementContentAsString());
                                break;
                            case "description":
                                _description = reader.ReadElementContentAsString();
                                if (_description == "NULL")
                                    _description = null;
                                break;
                            case "tags":
                                string tagsStr = reader.ReadElementContentAsString();
                                if (tagsStr == "NULL")
                                    _tags = null;
                                else
                                {
                                    _tags = tagsStr.Split(',');
                                    for (int i = 0; i < _tags.Length; i++)
                                        _tags[i] = _tags[i].Trim(' ');
                                }
                                break;
                            case "subtype":
                                _modeltype = (Modeltype)Convert.ToUInt32(
                                    reader.ReadElementContentAsString(), 16);
                                break;
                            case "rating":
                                _rating = reader.ReadElementContentAsDouble();
                                break;
                            case "parent":
                                string parentStr = reader.ReadElementContentAsString();
                                if (parentStr == "NULL")
                                    _parent = null;
                                else
                                    _parent = long.Parse(parentStr);
                                break;
                            case "comment":
                                reader.Read();
                                _comments.Add(new Comment(
                                    reader.ReadElementContentAsString(),
                                    reader.ReadElementContentAsString()));
                                break;
                            default:
                                reader.Read();
                                break;
                        }
                    }
                    else
                        reader.Read();
                }
            }
            catch
            {
                if (_name == null)
                    _name = "???";
                if (_author == null)
                    _author = "???";
                if (_description == null)
                    _description = "???";
            }

            reader?.Close();
        }

        /// <summary>
        /// Конверитрует ссылку или ID творения типа string в объект класса Creation
        /// </summary>
        /// <param name="uri">Ссылка на творение или его ID</param>
        /// <returns>Объект класса Creation</returns>
        public static Creation Parse(string uri)
        {
            if (uri.Length == ID_LENGTH)
                return new Creation(long.Parse(uri));

            if (uri.Count(c => c == ' ') >= 2)
            {
                string[] temp = uri.Split(' ');
                return new Creation(long.Parse(
                    temp[temp.Length - 1]
                    .TrimStart('(').TrimEnd(')')
                    ));
            }

            long id = 0;
            string[] uriElements = uri.Split('/');
            foreach (string el in uriElements)
            {
                // http://www.spore.com/static/<DataType>/<subId1>/<subId2>/<subId3>/<AssetId>.<format>
                if (el == "static")
                {
                    id = long.Parse(
                        uriElements[uriElements.Length - 1] // <AssetId>.<format>
                        .Substring(0, ID_LENGTH));
                    break;
                }
                // http://www.spore.com/rest/asset/<AssetId>
                else if (el == "rest")
                {
                    id = long.Parse(uriElements[uriElements.Length - 1]);
                    break;
                }
                // http://www.spore.com/sporepedia#qry=ast-<AssetId>%3Asast-<AssetId>
                // http://www.spore.com/sporepedia#qry=ast-<AssetId>
                // http://www.spore.com/sporepedia#qry=usr-<User>%7C<UserId>%3Asast-<AssetId>
                // http://www.spore.com/sporepedia#qry=usr-<User>%7C<UserId>%3Asast-<AssetId>%3Apg-<PageId>
                else if (el.Count(c => c == '#') >= 1)
                {
                    // sporepedia#qry=ast-<AssetId>%3Asast-<AssetId>
                    // sporepedia#qry=ast-<AssetId>
                    // sporepedia#qry=usr-<User>%7C<UserId>%3Asast-<AssetId>
                    // sporepedia#qry=usr-<User>%7C<UserId>%3Asast-<AssetId>%3Apg-<PageId>
                    string[] temp = el.Split('-');
                    if (temp[0] == "sporepedia#qry=ast" || temp[0] == "sporepedia#qry=sast")
                    {
                        // <AssetId>%3Asast-<AssetId>
                        // <AssetId>
                        id = long.Parse(temp[1]
                            .Substring(0, ID_LENGTH));
                    }
                    else if (temp[0] == "sporepedia#qry=usr")
                    {
                        if (temp[temp.Length - 1].Length < ID_LENGTH)
                        {
                            // <User>%7C<UserId>%3Asast-<AssetId>%3Apg-<PageId>
                            id = long.Parse(temp[temp.Length - 2] // <AssetId>%3Apg
                                .Substring(0, ID_LENGTH));
                        }
                        else
                        {
                            // <User>%7C<UserId>%3Asast-<AssetId>
                            id = long.Parse(temp[temp.Length - 1]); // <AssetId>
                        }
                    }
                    break;
                }
            }

            return new Creation(id);
        }
        /// <summary>
        /// Конверитрует ссылку или ID творения типа string в объект класса Creation
        /// </summary>
        /// <param name="uri">Ссылка на творение или его ID</param>
        /// <param name="result">Результат</param>
        /// <returns>false, если было поймано исключение</returns>
        public static bool TryParse(string uri, out Creation result)
        {
            try
            {
                result = Parse(uri);
            }
            catch
            {
                result = null;
                return false;
            }
            return true;
        }

        public override string ToString() =>
            $"{_modeltype} {_name} ({_id})";
    }
}
