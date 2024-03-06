using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using PRN211_Project.Models;
using System.Text.Json;

namespace PRN211_Project.Services
{
    public static class GetSessionExtensions
    {
        private static ISession _session;
        private static Account Account;
        private static Teacher Teacher;
        public static void SetObject(this ISession session, string key, Object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }


        public static Account GetAmdinObject(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value != null)
            {
                Account = JsonSerializer.Deserialize<Account>(value);
            }
            return Account;
        }

        public static Teacher GetTeacherObject(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value != null)
            {
                Teacher = JsonSerializer.Deserialize<Teacher>(value);
            }
            return Teacher;
        }

        public static Account RemoveAccountSession(this ISession session, string key)
        {
            session.Remove(key);
            session.Clear();
            return Account = null;
        }

        public static Teacher RemoveTeacherSession(this ISession session, string key)
        {
            session.Remove(key);
            session.Clear();
            return Teacher = null;
        }

    }

    public class GetSession
    {
        private ISession _session;
        private Account Account;

        public void SetObject(ISession session, string key, Account value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }


        public Account GetObject(ISession session, string key)
        {
            var value = session.GetString(key);
            if (value != null)
            {
                Account = JsonSerializer.Deserialize<Account>(value);
            }
            return Account;
        }
    }
}
