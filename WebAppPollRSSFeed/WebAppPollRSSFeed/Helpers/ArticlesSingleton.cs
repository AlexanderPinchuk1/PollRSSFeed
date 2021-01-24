
namespace WebAppPollRSSFeed.Helpers
{
    public class ArticlesSingleton
    {
        private static ArticlesSingleton ArticlesText;

        public string Text { get; set; } = "";

        public static ArticlesSingleton GetInstance()
        {
            if (ArticlesText == null)
            {
                ArticlesText = new ArticlesSingleton();    
            }

            return ArticlesText;
        }
    }
}
