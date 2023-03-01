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
            UserTable retUser = new UserTable();

            using (var myEntity = new DATNDBEntities())
            {
                retUser = myEntity.UserTables.Include("Id")
                    .Where(acc => acc.Name == "admin")
                    .Select(acc => new UserTable()
                    {
                        Id = acc.Id,
                        Name = acc.Name,
                        Password = acc.Password
                    }).FirstOrDefault<UserTable>();
            }
            if (retUser == null)
            {
                return NotFound();
            }

            return Ok(retUser);
        }

        private bool CheckNameExisted(UserTable checkUser)
        {
            UserTable retUser = new UserTable();
            using (var myEntity = new DATNDBEntities())
            {
                retUser = myEntity.UserTables.Include("Id")
                .Where(acc => acc.Name == checkUser.Name)
                .Select(acc => new UserTable()
                {
                    Id = acc.Id,
                    Name = acc.Name,
                    Password = acc.Password
                }).FirstOrDefault<UserTable>();
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

        private bool CheckAccExisted(UserTable checkUser)
        {
            UserTable retUser = new UserTable();
            using (var myEntity = new DATNDBEntities())
            {
                retUser = myEntity.UserTables.Include("Id")
                .Where(acc => (acc.Name == checkUser.Name) && (acc.Password == checkUser.Password))
                .Select(acc => new UserTable()
                {
                    Id = acc.Id,
                    Name = acc.Name,
                    Password = acc.Password
                }).FirstOrDefault<UserTable>();
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
        public IHttpActionResult Register([FromBody] UserTable newUser)
        {

            if (CheckNameExisted(newUser) == false)
            {
                var myEntity = new DATNDBEntities();
                myEntity.UserTables.Add(newUser);
                myEntity.SaveChanges();
                Debug.WriteLine("Account registed successfully");
                return Ok();
            }
            else
            {
                Debug.WriteLine("Account existed!");
                return NotFound();
            }

            //UserTable retUser = new UserTable();
            //using (var myEntity = new DATNDBEntities())
            //{
            //    retUser = myEntity.UserTables.Include("Id")
            //    .Where(acc => (acc.Name == newUser.Name))
            //    .Select(acc => new UserTable()
            //    {
            //        Id = acc.Id,
            //        Name = acc.Name,
            //        Password = acc.Password
            //    }).FirstOrDefault<UserTable>();

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
        public IHttpActionResult Login([FromBody] UserTable checkUser)
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
        public IHttpActionResult Edit([FromBody] UserTable checkUser)
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
        public IHttpActionResult Delete(UserTable deleteUser)
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
