using Newtonsoft.Json.Linq;

namespace Lesson1
{
    public class Post
    {
        public Post(string json)
        {
            JToken jObject = JObject.Parse(json);
            UserId = (int)jObject["userId"];
            Id = (int)jObject["id"];
            Title = (string)jObject["title"];
            Body = jObject["body"].ToString().Replace("\n", "");
        }

        public int UserId { get; private set; }
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }

        public override string ToString()
        {
            return UserId + "\n" + Id + "\n" + Title + "\n" + Body + "\n";
        }
    }
}