using DataAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataAPI.Controller
{
    public class UserController : ApiController
    {
        /**
         * GET methods
         */
        public IHttpActionResult GetUser([FromBody] string _username )
        {
            UserModel currentUser = new UserModel();

            using (var myEntity = new DBDATNEntities())
            {
                currentUser = myEntity.UserTables.Include("Id")
                    .Where(acc => acc.Name == _username)
                    .Select(acc => new UserModel()
                    {
                        Id = acc.Id,
                        Name = acc.Name,
                        Password = acc.Password
                    }).FirstOrDefault<UserModel>();
            }

            if (currentUser == null)
            {
                return NotFound();
            }

            return Ok(currentUser);
        }

        //public IHttpActionResult GetAllPumps()
        //{
        //    IList<PumpModel> pumps = null;

        //    using (var myEntity = new DBDATNEntities())
        //    {
        //        pumps = myEntity.PumpTables.Include("Id")
        //            .Select(p => new PumpModel()
        //            {
        //                Id = p.Id,
        //                Area = p.Area,
        //                Code = p.Code,
        //                State = p.State
        //            }).ToList<PumpModel>();
        //    }

        //    if (pumps.Count == 0)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(pumps);

        //}

        /**
         * POST methods
         */
        public IHttpActionResult PostNewUser(UserModel _user)
        {
            using (var myEntity = new DBDATNEntities())
            {
                myEntity.UserTables.Add(new UserTable()
                {
                    Id = _user.Id,
                    Name = _user.Name,
                    Password = _user.Password
                });

                myEntity.SaveChanges();
            }

            return Ok();
        }

        /**
         * PUT methods
         */
        public IHttpActionResult PutPump(UserModel _user)
        {
            using (var myEntity = new DBDATNEntities())
            {
                var oldUser = myEntity.UserTables
                    .Where(acc => acc.Name == _user.Name)
                    .FirstOrDefault<UserTable>();

                if (oldUser != null)
                {
                    oldUser.Password = _user.Password;

                    myEntity.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        /**
         * DELETE methods
         */
        public IHttpActionResult DeletePump(UserModel _user)
        {
            using (var myEntity = new DBDATNEntities())
            {
                var oldUser = myEntity.UserTables
                    .Where(acc => acc.Name == _user.Name)
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
