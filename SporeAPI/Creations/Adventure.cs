using System;
using System.Collections.Generic;
using System.Xml;

namespace SporeApi.Creations
{
	public class Adventure : Creation
	{
		private List<long> _assets;
		private bool _isAvatarLocked;
		private int _numAllowedPosseMembers;
		private string _winText;
		private string _loseText;
		private string _introText;
		private long? _avatarAsset;

        /// <summary>
        /// Создаёт объект класса Adventure
        /// </summary>
        /// <param name="id">ID творения</param>
        /// <exception cref="ArgumentException"></exception>
        public Adventure(long id) : base(id)
		{
			ParseXML();
		}
		/// <summary>
		/// Создаёт объект класса Adventure из объекта Creation
		/// </summary>
		/// <param name="creation"></param>
		/// <exception cref="ArgumentException"></exception>
		public Adventure(Creation creation) : base(creation)
		{
			if (!creation.IsAdventure)
				throw new ArgumentException("Творение не является приключением.");

			ParseXML();
		}

		public int? AssetsCount => _assets?.Count;
		public long GetAssetAt(int index) => _assets[index];
		public Creation GetCreationFromAssetAt(int index) => new Creation(_assets[index]);
		public bool IsAvatarLocked => _isAvatarLocked;
		public int NumAllowedPosseMembers => _numAllowedPosseMembers;
		public string WinText => _winText;
		public string LoseText => _loseText;
		public string IntroText => _introText;
		public long? AvatarAsset => _avatarAsset;

        /// <summary>
        /// Парсит xml-файл творения (http://www.spore.com/static/model/subId1/subId2/subId3/AssetId.xml)
        /// </summary>
        /// <exception cref="InvalidCastException"></exception>
        protected override void ParseXML()
		{
			XmlReader reader = XmlReader.Create(XmlUri);
			while (!reader.EOF)
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					switch (reader.Name)
					{
						case "modeltype":
							_modeltype = (Modeltype)Convert.ToUInt32(reader.ReadElementContentAsString(), 16);
							if (!IsAdventure)
								throw new InvalidCastException("Творение не является приключением.");
							break;
						case "asset":
							if (_assets == null)
								_assets = new List<long>();
							_assets.Add(reader.ReadElementContentAsLong());
							break;
						case "bAvatarLocked":
							_isAvatarLocked = reader.ReadElementContentAsBoolean();
							break;
						case "numAllowedPosseMembers":
							_numAllowedPosseMembers = reader.ReadElementContentAsInt();
							break;
						case "winText":
                            _winText = reader.ReadElementContentAsString();
							break;
						case "loseText":
                            _loseText = reader.ReadElementContentAsString();
							break;
						case "introText":
							_introText = reader.ReadElementContentAsString();
							break;
						case "mAvatarAsset":
							reader.Read();
							if (reader.Name == "ID")
                                _avatarAsset = reader.ReadElementContentAsLong();
							else
								_avatarAsset = null;
                            break;
						// Доделать акты
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
	}
}
