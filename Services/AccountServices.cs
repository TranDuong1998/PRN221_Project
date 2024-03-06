using PRN211_Project.Models;

namespace PRN211_Project.Services
{
    public class AccountServices
    {
        public List<Account> GetAllAccount()
        {
            try
            {
                using (var context = new Prn211ProjectContext())
                {
                    var listAccounts = context.Accounts.Where(a => a.Role.ToLower() != "admin").ToList();
                    return listAccounts;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Account GetAcountByID(int? AcountId)
        {
            try
            {
                using (var context = new Prn211ProjectContext())
                {
                    var account = context.Accounts.Where(a => a.AccountId == AcountId).FirstOrDefault();
                    return account;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Account GetAcountByEmailAndPass(string email, string pass)
        {
            try
            {
                using (var context = new Prn211ProjectContext())
                {
                    var account = context.Accounts.Where(a => (a.Email == email && a.Password == pass) ||
                                                          (a.UserName == email && a.Password == pass)).FirstOrDefault();
                    return account;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int AddNewAccount(Account account)
        {
            try
            {
                using (var context = new Prn211ProjectContext())
                {
                    context.Accounts.Add(account);
                    if (context.SaveChanges() > 0)
                        return 1;
                    else
                        return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int UpdateAccount(Account account)
        {
            try
            {
                using (var context = new Prn211ProjectContext())
                {
                    context.Accounts.Update(account);
                    if (context.SaveChanges() > 0)
                        return 1;
                    else
                        return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int DeleteAccount(Account account)
        {
            try
            {
                using(var context = new Prn211ProjectContext())
                {
                    context.Accounts.Remove(account);
                    if (context.SaveChanges() > 0)
                        return 1;
                    else
                        return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
