using System;
using System.Text.RegularExpressions;

namespace DiscountMarketplace.Domain
{
    public abstract class User
    {
        public int ID { get; private set; }
        private string email;
        private string firstName;
        private string lastName;
        private string phoneNumber;
        internal int Id;

        public event EventHandler<string> Notification;

        public string Email
        {
            get => email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("Невірний формат електронної пошти.");
                email = value;
            }
        }

        public string FirstName
        {
            get => firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ім'я не може бути порожнім.");
                firstName = value;
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Прізвище не може бути порожнім.");
                lastName = value;
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^\+38\(0\d{2}\)-\d{7}$"))
                    throw new ArgumentException("Невірний формат номера телефону. Очікується формат: +38(0XX)-XXXXXXX");
                phoneNumber = value;
            }
        }

        protected User(int id, string email, string firstName, string lastName, string phoneNumber)
        {
            if (id <= 0)
                throw new ArgumentException("ID має бути більше 0.");
            ID = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }

        public abstract bool Login(string email, string password);

        public virtual void Logout()
        {
            Notification?.Invoke(this, $"{FirstName} {LastName} вийшов з системи.");

        }
    }
}

