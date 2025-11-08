public class AIApiInput
    {
        public string prompt { get; set; }
        public int userid { get; set; }
    }

    public class AIApiResponse
    {
        public List<AIResposneList> suggestions { get; set; }
        public bool isSuccess { get; set; }
        public string errorText { get; set; }
    }

public class AIResposneList
{
    public string question { get; set; }
    public string answer { get; set; }
}