using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DAO
{
    public class UserDAO
    {
        QuanLyBanHangEntities db = null;
        public UserDAO()
        {
            db = new QuanLyBanHangEntities();
        }

        public User GetUserByID(string userName)
        {
            return db.Users.FirstOrDefault(x => x.UserName == userName);
        }

        public int Login(string userName, string passWord, bool isLoginAdmin = false)
        {
            var result = db.Users.FirstOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;// tài khoản không tồn tại
            }
            else
            {
                if (result.Status == false)
                {
                    return -1;//tài khoản bị khóa
                }
                else
                {
                    if (result.Password == passWord)
                        return 1;//đăng nhập ok
                    else
                        return -2;// mật khẩu sai
                }
            }
        }

        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public long InsertForFacebook(User entity)
        {
            var user = db.Users.FirstOrDefault(x => x.UserName == entity.UserName);
            if (user == null)
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            else
            {
                return user.ID;
            }
            
        }

        public bool ChangeStatus(int id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();

            return (bool)user.Status;
        }

        public bool CheckUserName(string userName)
        {
            return db.Users.Count(x => x.UserName == userName) > 0 ;//>0:true ; =0:false
        }

        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;//>0:true ; =0:false
        }
    }
}