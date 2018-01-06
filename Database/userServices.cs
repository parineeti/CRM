using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Database
{
    public class userServices
    {
        public tbllogin GetLoginUser(int userId)
        {
            tbllogin _profile = new tbllogin();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                _profile = _idc.tbllogins.Where(a => a.UserId == userId).FirstOrDefault();
                if(string.IsNullOrEmpty(_profile.Image))
                {
                    _profile.Image = "~/assets/images/icon/staff.png";
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _profile;
        }

        public string UpdateProfile(tbllogin _profile,string Image= null)
        {
            string msg = "Profile Updated Succesfully";
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var person = _idc.tbllogins.Where(a => a.UserId == _profile.UserId).FirstOrDefault();
                if(!string.IsNullOrEmpty(Image))
                {
                    person.Image = Image;
                }
                person.FirstName = _profile.FirstName;
                person.LastName = _profile.LastName;
                person.Address = _profile.Address;
                person.Company = _profile.Company; 
                person.Email = _profile.Email;
                person.IsActive ="Y";
                person.Occupation = _profile.Occupation;
                person.Password = _profile.Password ;
                person.PhoneNumber = _profile.PhoneNumber;
                _idc.SubmitChanges();
            }
            catch (Exception ex)
            {
                msg = "Error " + ex.Message;
            }
            finally
            {
                _idc.Dispose();
            }
            return msg;
        }


        public profile getProfileByUserId(int userId)
        {
            profile _profile = new Models.profile();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var p = _idc.tbllogins.Where(a => a.UserId == userId).FirstOrDefault();
                _profile.Email = p.Email;
                _profile.image = p.Image;
                _profile.name = p.FirstName + " " + p.LastName; 
            }
            catch (Exception ex)
            { 
            }
            finally
            {
                _idc.Dispose(); 
            }
            return _profile;
        }

    }
}