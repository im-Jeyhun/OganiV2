using OganiWebApp.Database.Models;

namespace OganiWebApp.Areas.Client.ViewModels
{
    public class BlogViewModel
    {
        public BlogViewModel(int id, string thumbNailImgName, string thumbNailImgUrl, User? user, int blogCategoryId, BlogCategory? blogCategory, string title, string content, DateTime created)
        {
            Id = id;
            ThumbNailImgName = thumbNailImgName;
            ThumbNailImgUrl = thumbNailImgUrl;
            User = user;
            BlogCategoryId = blogCategoryId;
            BlogCategory = blogCategory;
            Title = title;
            Content = content;
            Created = created;
        }

        public int Id { get; set; }
        public string ThumbNailImgName { get; set; }
        public string ThumbNailImgUrl { get; set; }
        public User? User { get; set; }
        public int BlogCategoryId { get; set; }
        public BlogCategory? BlogCategory { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}
