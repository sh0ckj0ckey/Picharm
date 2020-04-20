using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 图虫
{
    public class CommentViewModel
    {
        public string UserName { get; set; }
        public string UserPhoto { get; set; }
        public string CommentContent { get; set; }
        public string CommentDate { get; set; }
        public ObservableCollection<ReplyComment> Replies { get; set; }
        public bool InfoComplete { get; set; }
        public bool HasMore { get; set; }

        public CommentViewModel(Models.Comments.Commentlist commentlist)
        {
            this.InfoComplete = false;
            try
            {
                this.UserName = commentlist.author.name;
                this.UserPhoto = commentlist.author.icon;
                this.CommentContent = commentlist.content;
                this.CommentDate = commentlist.created_at;
                this.InfoComplete = true;

                if (commentlist.images.Length > 0)
                {
                    this.CommentContent += "[图片评论]";
                }

                this.Replies = new ObservableCollection<ReplyComment>();
                foreach (var reply in commentlist.sub_notes)
                {
                    ReplyComment replyComment = new ReplyComment();

                    //合并回复的人的名字（因为可能回复了多个人，那就把他们的名字拼接起来）
                    replyComment.ReplyTo = "";
                    for (int i = 0; i < reply.reply_to.Length; i++)
                    {
                        if (i == reply.reply_to.Length - 1)
                        {
                            replyComment.ReplyTo += (reply.reply_to[i].name);
                        }
                        else
                        {
                            replyComment.ReplyTo += (reply.reply_to[i].name + "、");
                        }
                    }

                    replyComment.ReplyContent = reply.content.Replace("<br />", "");
                    replyComment.ReplyDate = reply.created_at;
                    replyComment.UserName = reply.author.name;
                    replyComment.UserPhoto = reply.author.icon;

                    this.Replies.Add(replyComment);
                }
            }
            catch
            {
                this.InfoComplete = false;
            }
        }
    }

    public class ReplyComment
    {
        public string UserName { get; set; }
        public string UserPhoto { get; set; }
        public string ReplyDate { get; set; }
        public string ReplyContent { get; set; }
        public string ReplyTo { get; set; }
    }
}
