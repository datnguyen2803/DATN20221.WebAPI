using DataAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataAPI.Controller
{
    public class UserController : ApiController
    {

        [HttpGet]
        [ActionName ("GetAdmin")]
        public IHttpActionResult Admin()
        {
            UserModel retUser = new UserModel();

            using (var myEntity = new DATNDBEntities())
            {
                retUser = myEntity.UserTables.Include("Id")
                    .Where(acc => acc.Name == "admin")
                    .Select(acc => new UserModel()
                    {
                        Id = acc.Id,
                        Name = acc.Name,
                        Password = acc.Password
                    }).FirstOrDefault<UserModel>();
            }
            if (retUser == null)
            {
                return NotFound();
            }

            return Ok(retUser);
        }

        private bool CheckNameExisted(UserModel checkUser)
        {
            UserModel retUser = new UserModel();
            using (var myEntity = new DATNDBEntities())
            {
                retUser = myEntity.UserTables.Include("Id")
                .Where(acc => acc.Name == checkUser.Name)
                .Select(acc => new UserModel()
                {
                    Id = acc.Id,
                    Name = acc.Name,
                    Password = acc.Password
                }).FirstOrDefault<UserModel>();
            }

            if (retUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckAccExisted(UserModel checkUser)
        {
            UserModel retUser = new UserModel();
            using (var myEntity = new DATNDBEntities())
            {
                retUser = myEntity.UserTables.Include("Id")
                .Where(acc => (acc.Name == checkUser.Name) && (acc.Password == checkUser.Password))
                .Select(acc => new UserModel()
                {
                    Id = acc.Id,
                    Name = acc.Name,
                    Password = acc.Password
                }).FirstOrDefault<UserModel>();
            }

            if (retUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [HttpPost]
        [ActionName ("Register")]
        public IHttpActionResult Register([FromBody] UserModel newUser)
        {
            var newUserTable = new UserTable()
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Password = newUser.Password
            };

            if (CheckNameExisted(newUser) == false)
            {
                var myEntity = new DATNDBEntities();
                myEntity.UserTables.Add(newUserTable);
                myEntity.SaveChanges();
                Debug.WriteLine("Account registed successfully");
                return Ok();
            }
            else
            {
                Debug.WriteLine("Account existed!");
                return NotFound();
            }

            //UserModel retUser = new UserModel();
            //using (var myEntity = new DATNDBEntities())
            //{
            //    retUser = myEntity.UserTables.Include("Id")
            //    .Where(acc => (acc.Name == newUser.Name))
            //    .Select(acc => new UserModel()
            //    {
            //        Id = acc.Id,
            //        Name = acc.Name,
            //        Password = acc.Password
            //    }).FirstOrDefault<UserModel>();

            //    // acc existed
            //    if (retUser != null)
            //    {
            //        myEntity.UserTables.Add(retUser);
            //        return Ok(retUser);
            //    }
            //    else
            //    {
            //        return NotFound();
            //    }
            //}
        }

        [HttpPost]
        [ActionName("Login")]
        public IHttpActionResult Login([FromBody] UserModel checkUser)
        {
            if (CheckAccExisted(checkUser) == true)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [ActionName("Edit")]
        public IHttpActionResult Edit([FromBody] UserModel checkUser)
        {

            using (var myEntity = new DATNDBEntities())
            {
                var oldUser = myEntity.UserTables
                    .Where(acc => acc.Name == checkUser.Name)
                    .FirstOrDefault<UserTable>();

                if (oldUser != null)
                {
                    oldUser.Password = checkUser.Password;

                    myEntity.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpDelete]
        [ActionName("Delete")]
        public IHttpActionResult Delete(UserModel deleteUser)
        {
            using (var myEntity = new DATNDBEntities())
            {
                var oldUser = myEntity.UserTables
                    .Where(acc => acc.Name == deleteUser.Name)
                    .FirstOrDefault<UserTable>();

                if (oldUser != null)
                {
                    myEntity.Entry(oldUser).State = System.Data.Entity.EntityState.Deleted;

                    myEntity.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }
    }
}
