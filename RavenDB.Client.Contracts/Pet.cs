namespace RavenDB.Client.Contracts
{
    public class Pet
    {
        public string Color { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, a {1} {2}", Name, Color, Race);
        }
    }
}
