using Common;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DAO
{
    public class ContentDAO
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        public Content GetByID(int id)
        {
            return db.Contents.Find(id);
        }

        public int Create(Content content)
        {
            //xu ly alias
            if(string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            db.Contents.Add(content);
            db.SaveChanges();

            //xu ly tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                string[] tags = content.Tags.Split(',');
                foreach(var tag in tags)
                {
                    var tagID = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagID);

                    //insert to tag table
                    if(!existedTag)
                    {
                        this.InsertTag(tagID, tag);
                    }

                    //insert to content tag
                    this.InsertContentTag(content.ID, tagID);

                }
            }
            return content.ID;
        }

        public void InsertTag(string id, string name)
        {
            Tag tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }

        public void InsertContentTag(int contentID, string tagID)
        {
            ContentTag contentTag = new ContentTag();
            contentTag.NewsID = contentID;
            contentTag.TagID = tagID;
            db.ContentTags.Add(contentTag);
            db.SaveChanges();
        }

        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }
    }
}