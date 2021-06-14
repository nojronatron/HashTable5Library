using HashTable5;
using System;

namespace HashTable5Tests
{
    public class TestObject : IEquatable<TestObject>, IIdable
    {
        public int Id { get; set; }
        internal Object Payload { get; set; }
        public TestObject() { }
        public TestObject(int id, object payload)
        {
            Id = id;
            Payload = payload;
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<TestObject>)this).Equals(obj as TestObject);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"Object ID: { this.Id }; Payload: { this.Payload.ToString() }";
        }

        bool IEquatable<TestObject>.Equals(TestObject other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return this.Id == other.Id;
        }

    }
}
