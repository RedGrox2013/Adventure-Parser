namespace SporeApi.Creations
{
	public readonly struct Comment
	{
		private readonly string _message, _sender;

		public string Message => _message;
		public string Sender => _sender;

		public Comment(string message, string sender)
		{
			_message = message;
			_sender = sender;
		}

		public override string ToString() => _sender + ": " + _message;
	}
}
