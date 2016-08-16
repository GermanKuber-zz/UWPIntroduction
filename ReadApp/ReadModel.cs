namespace ReadApp
{
    public class ReadModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Img { get; set; }
        public string CompleteName => $"{Name} - {LastName}";
    }
}
