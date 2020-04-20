using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 图虫.ViewModels
{
    public class SitesSearchViewModel
    {
        public string Background { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Intro { get; set; }
        public string Bio { get; set; }
        public string Id { get; set; }

        public SitesSearchViewModel(Models.SitesSearchResult.Site_List site)
        {
            this.Name = site.name;
            this.Avatar = site.icon;
            this.Background = site.appearance.image;
            this.Bio = site.description;
            this.Id = site.site_id;
            this.Intro = "";

            if (site.following > 0)
            {
                Intro += "关注 " + site.following;
            }

            if (site.followers > 0 && site.following > 0)
            {
                Intro += " / 粉丝 " + site.followers;
            }
            else if (site.followers > 0)
            {
                Intro += "粉丝 " + site.followers;
            }

            if (site.location.Length > 0 && (site.followers > 0 || site.following > 0))
            {
                Intro += " / " + site.location;
            }
            else if (site.location.Length > 0)
            {
                Intro += site.location;
            }
        }
    }

    public class TagsSearchViewModel
    {
        public string TagName { get; set; }
        public string Cover { get; set; }
        public string Url { get; set; }

        public TagsSearchViewModel(Models.TagsSearchResult.Tag_List tag)
        {
            this.TagName = tag.tag_name;
            this.Cover = tag.cover_image.source.m;
            this.Url = tag.url;
        }
    }

    public class PostsSearchViewModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Cover { get; set; }
        public int Count { get; set; }
        public string AuthorId { get; set; }
        public string PostId { get; set; }
        public Models.PostsSearchResult.Post_List PostRaw { get; set; }

        public PostsSearchViewModel(Models.PostsSearchResult.Post_List post)
        {
            this.Title = post.title;
            this.Author = post.site.name;
            this.Cover = post.images[0].source.mr;//post.title_image.url;
            this.Count = post.image_count;
            this.AuthorId = post.site.site_id;
            this.PostId = post.post_id;
            this.PostRaw = post;
        }
    }
}
