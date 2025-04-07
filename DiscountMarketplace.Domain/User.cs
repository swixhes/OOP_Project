using System;
using System.Text.RegularExpressions;

namespace DiscountMarketplace.Domain
{
    public abstract class User
    {
        public int ID { get; private set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public event EventHandler<string> Notification;

        protected User(int id, string email, string firstName, string lastName, string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public abstract bool Login(string email, string password);
        public virtual void Logout()
        {
            throw new NotImplementedException();
        }
    }
}


