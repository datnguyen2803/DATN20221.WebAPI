﻿using DataAPI.Common;
using DataAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;

namespace DataAPI.Controller
{
    public class UserController : ApiController
    {

        private string EncodeTo64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        private string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        private bool CheckNameExisted(UserModel checkUser)
        {
            UserModel retUser = new UserModel();
            using (var myEntity = new DATN2022DBEntities())
            {
                retUser = myEntity.UserTables.Include("Id")
                .Where(acc => acc.Name == checkUser.Name)
                .Select(acc => new UserModel()
                {
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
            checkUser.Password = EncodeTo64(checkUser.Password);

            using (var myEntity = new DATN2022DBEntities())
            {
                retUser = myEntity.UserTables.Include("Id")
                .Where(acc => (acc.Name == checkUser.Name) && (acc.Password == checkUser.Password))
                .Select(acc => new UserModel()
                {
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

        [HttpGet]
        [ActionName ("GetAdmin")]
        public IHttpActionResult Admin()
        {
            UserModel retUser = new UserModel();

            using (var myEntity = new DATN2022DBEntities())
            {
                retUser = myEntity.UserTables.Include("Id")
                    .Where(acc => acc.Name == "admin")
                    .Select(acc => new UserModel()
                    {
                        Name = acc.Name,
                        Password = acc.Password
                    }).FirstOrDefault<UserModel>();
            }
            if (retUser == null)
            {
                return Ok(new ResponseModel
                {
                    Code = ConstantHelper.APIResponseCode.CODE_FAIL,
                    Message = ConstantHelper.APIResponseMessage.MESSAGE_USER_WRONG_NAME,
                    Data = null
                });
            }

            return Ok(new ResponseModel
            {
                Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                Data = retUser
            });
        }

        [HttpGet]
        [ActionName("Info")]
        public IHttpActionResult Info(string userName)
        {
            UserModel retUser = new UserModel();

            using (var myEntity = new DATN2022DBEntities())
            {
                retUser = myEntity.UserTables.Include("Id")
                    .Where(acc => acc.Name == userName)
                    .Select(acc => new UserModel()
                    {
                        Name = acc.Name,
                        Password = acc.Password
                    }).FirstOrDefault<UserModel>();
            }
            if (retUser == null)
            {
                return Ok(new ResponseModel
                {
                    Code = ConstantHelper.APIResponseCode.CODE_FAIL,
                    Message = ConstantHelper.APIResponseMessage.MESSAGE_USER_WRONG_NAME,
                    Data = null
                });
            }

            return Ok(new ResponseModel
            {
                Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                Data = retUser
            });
        }

        [HttpPost]
        [ActionName ("Register")]
        public IHttpActionResult Register([FromBody] UserModel newUser)
        {
            UserTable newUserTable = newUser.ToUserTable();
            newUserTable.Password = EncodeTo64(newUserTable.Password);

            if (CheckNameExisted(newUser) == false)
            {
                var myEntity = new DATN2022DBEntities();
                myEntity.UserTables.Add(newUserTable);
                myEntity.SaveChanges();
                Debug.WriteLine("Account registed successfully");
                return Ok(new ResponseModel
                {
                    Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                    Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                    Data = newUserTable
                });
            }
            else
            {
                Debug.WriteLine("Account existed!");
                return Ok(new ResponseModel
                {
                    Code = ConstantHelper.APIResponseCode.CODE_RESOURCE_DUPLICATE,
                    Message = ConstantHelper.APIResponseMessage.MESSAGE_USER_DUPLICATE,
                    Data = null
                });
            }
        }

        [HttpPost]
        [ActionName("Login")]
        public IHttpActionResult Login([FromBody] UserModel checkUser)
        {
            if(CheckNameExisted(checkUser) == false)
            {
                return Ok(new ResponseModel
                {
                    Code = ConstantHelper.APIResponseCode.CODE_FAIL,
                    Message = ConstantHelper.APIResponseMessage.MESSAGE_USER_WRONG_NAME,
                    Data = null
                });
            }

            if (CheckAccExisted(checkUser) == true)
            {
                return Ok(new ResponseModel
                {
                    Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                    Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                    Data = null
                });
            }
            else
            {
                return Ok(new ResponseModel
                {
                    Code = ConstantHelper.APIResponseCode.CODE_FAIL,
                    Message = ConstantHelper.APIResponseMessage.MESSAGE_USER_WRONG_PASS,
                    Data = null
                });
            }
        }

        [HttpPut]
        [ActionName("Edit")]
        public IHttpActionResult Edit([FromBody] UserModel checkUser)
        {
            using (var myEntity = new DATN2022DBEntities())
            {
                var oldUser = myEntity.UserTables
                    .Where(acc => acc.Name == checkUser.Name)
                    .FirstOrDefault<UserTable>();

                if (oldUser != null)
                {
                    oldUser.Password = checkUser.Password;

                    myEntity.SaveChanges();
                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                        Data = null
                    });
                }
                else
                {
                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_RESOURCE_NOT_FOUND,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_USER_WRONG_NAME,
                        Data = null
                    });
                }
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete([FromBody] UserModel deleteUser)
        {
            using (var myEntity = new DATN2022DBEntities())
            {
                var oldUser = myEntity.UserTables
                    .Where(acc => acc.Name == deleteUser.Name)
                    .FirstOrDefault<UserTable>();

                if (oldUser != null)
                {
                    myEntity.Entry(oldUser).State = System.Data.Entity.EntityState.Deleted;
                    myEntity.SaveChanges();
                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                        Data = null
                    });
                }
                else
                {
                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_FAIL,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_USER_WRONG_PASS,
                        Data = null
                    });
                }
            }

        }
    }
}
