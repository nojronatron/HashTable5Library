using HashTable5;
using System;

namespace HashTable5Tests
{
    public class Person : IEquatable<Person>, IIdable
    {
        public int Id { get; set; }   //  for or from DB ie Entity Framework
        internal string FirstName { get; set; }
        internal string LastName { get; set; }
        internal string Email { get; set; }
        public Person() { }
        public Person(int id, string firstName, string lastName, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
        public override int GetHashCode()
        {
            return this.FirstName.GetHashCode() +
                    this.LastName.GetHashCode() +
                    this.Email.GetHashCode();
        }
        bool IEquatable<Person>.Equals(Person other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return (this.FirstName == other.FirstName &&
                    this.LastName == other.LastName &&
                    this.Email == other.Email);
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<Person>)this).Equals(obj as Person);
        }

        public override string ToString()
        {
            return $"{ Id } { FirstName } { LastName } { Email }";
        }
    }
}
